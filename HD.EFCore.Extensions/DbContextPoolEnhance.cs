﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace HD.EFCore.Extensions
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class DbContextPoolEnhance<TContext> : IDbContextPool, IDisposable
        where TContext : DbContext
    {
        private const int DefaultPoolSize = 32;

        private readonly ConcurrentQueue<TContext> _pool = new ConcurrentQueue<TContext>();

        private readonly Func<TContext> _activator;

        private int _maxSize;
        private int _count;

        private DbContextPoolConfigurationSnapshot _configurationSnapshot;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        // ReSharper disable once SuggestBaseTypeForParameter
        public DbContextPoolEnhance(DbContextOptions<TContext> options)
        {
            _maxSize = options.FindExtension<CoreOptionsExtension>()?.MaxPoolSize ?? DefaultPoolSize;

            options.Freeze();

            _activator = CreateActivator(options);

            if (_activator == null)
            {
                throw new InvalidOperationException(
                    CoreStrings.PoolingContextCtorError(typeof(TContext).ShortDisplayName()));
            }
        }

        private static Func<TContext> CreateActivator(DbContextOptions<TContext> options)
        {
            var ctors 
                = typeof(TContext).GetTypeInfo().DeclaredConstructors
                    .Where(c => !c.IsStatic && c.IsPublic)
                    .ToArray();

            if (ctors.Length == 1)
            {
                var parameters = ctors[0].GetParameters();

                if (parameters.Length == 1
                    && (parameters[0].ParameterType == typeof(DbContextOptions) 
                        || parameters[0].ParameterType == typeof(DbContextOptions<TContext>)))
                {
                    return
                        Expression.Lambda<Func<TContext>>(
                                Expression.New(ctors[0], Expression.Constant(options)))
                            .Compile();
                }
            }

            return null;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual TContext Rent()
        {
            TContext context;

            if (_pool.TryDequeue(out context))
            {
                Interlocked.Decrement(ref _count);

                Debug.Assert(_count >= 0);

                ((IDbContextPoolable)context).Resurrect(_configurationSnapshot);

                return context;
            }

            context = _activator();

            NonCapturingLazyInitializer
                .EnsureInitialized(
                    ref _configurationSnapshot,
                    (IDbContextPoolable)context,
                    c => c.SnapshotConfiguration());

            ((IDbContextPoolable)context).SetPool(this);

            return context;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool Return(TContext context)
        {
            if (Interlocked.Increment(ref _count) <= _maxSize)
            {
                ((IDbContextPoolable)context).ResetState();

                _pool.Enqueue(context);

                return true;
            }

            Interlocked.Decrement(ref _count);

            Debug.Assert(_maxSize == 0 || _pool.Count <= _maxSize);

            return false;
        }

        DbContext IDbContextPool.Rent() => Rent();

        bool IDbContextPool.Return(DbContext context) => Return((TContext)context);

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual void Dispose()
        {
            _maxSize = 0;

            TContext context;
            while (_pool.TryDequeue(out context))
            {
                context.Dispose();
            }
        }
    }
}
