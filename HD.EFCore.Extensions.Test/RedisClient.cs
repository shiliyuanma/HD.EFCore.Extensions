using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace HD.EFCore.Extensions.Test
{
    public class RedisClient : IDisposable
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> _connections;
        private string _defaultConStr;
        public RedisClient(string defaultConStr)
        {
            _defaultConStr = defaultConStr;
            _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        }

        public IDatabase Database
        {
            get { return GetDatabase(); }
        }

        public IServer Server
        {
            get { return GetServer(); }
        }

        public ISubscriber Subscriber
        {
            get { return GetSubscriber(); }
        }

        public int Timeout
        {
            get { return GetConnect(null).TimeoutMilliseconds; }
        }

        private ConnectionMultiplexer GetConnect(string conStr)
        {
            if (string.IsNullOrWhiteSpace(conStr))
            {
                conStr = _defaultConStr;
            }
            return _connections.GetOrAdd(conStr, str => ConnectionMultiplexer.Connect(str));
        }

        public IDatabase GetDatabase(string conStr = null, int db = 0)
        {
            return GetConnect(conStr).GetDatabase(db);
        }

        public IServer GetServer(string conStr = null, int endPointsIndex = 0)
        {
            if (string.IsNullOrWhiteSpace(conStr))
            {
                conStr = _defaultConStr;
            }
            var confOption = ConfigurationOptions.Parse((string)conStr);
            return GetConnect(conStr).GetServer(confOption.EndPoints[endPointsIndex]);
        }

        public EndPoint[] GetEndPoints(string conStr = null)
        {
            return GetConnect(conStr).GetEndPoints();
        }

        public List<IServer> GetMasterServers(string conStr = null)
        {
            var servers = new List<IServer>();
            var eps = GetEndPoints(conStr);
            for (int i = 0; i < eps.Length; i++)
            {
                var server = GetServer(conStr, i);
                if (!server.IsSlave)
                {
                    servers.Add(server);
                }
            }
            return servers;
        }

        public ISubscriber GetSubscriber(string conStr = null)
        {
            return GetConnect(conStr).GetSubscriber();
        }

        public void Dispose()
        {
            if (_connections != null && _connections.Count > 0)
            {
                foreach (var item in _connections.Values)
                {
                    item.Close();
                }
            }
        }
    }
}