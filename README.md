# HD.EFCore.Extensions
efcore扩展(UnitOfWork，EntityCache，DbContextPool增强)

解决如下3个问题：  
1.官方的services.AddDbContextPool方法不能注入两个DbContext实例（调用两次），对应场景是DB主从分离的开发模式。现通过自定义方法services.AddDbContextPoolEnhance解决它。  
2.UnitOfWork机制实现（不使用IRepositoty方式，依然保留ef默认的开发模式），支持无限嵌套调用，仍保证在一个事务中。  
3.EntityCache模式的实现：当你通过一个主键值或一个主键值的集合去读取实体（或viewmodel）的时候，你将不用关系数据来自于缓存还是db，因为当缓存没有的时候会自动从db加载（可选的map to viewmodel），而当db.SaveChanges()的时候会自动delete相关的缓存。  

基本用法（详情可查看Test项目）：  
1.//自己扩展：使用DbContextPool的方式注入读写分离dbcontext  
  services.AddDbContextPoolEnhance<MasterDbContext>(q => q.UseMySql<MasterDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));  
  services.AddDbContextPoolEnhance<SlaveDbContext>(q => q.UseMySql<SlaveDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));  

  //原生ef注入读写分离dbcontext的方式（缺点是不能使用DbContextPool的方式注入）   
  //services.AddDbContext<MasterDbContext>(q => ((DbContextOptionsBuilder<MasterDbContext>)q).UseMySql<MasterHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));  
  //services.AddDbContext<SlaveDbContext>(q => ((DbContextOptionsBuilder<SlaveDbContext>)q).UseMySql<SlaveHDDbContext>("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));  
2.services.AddUnitOfWork();  
  using (var uow = uowMgr.Begin(db))  
  {  
      try  
      {  



          //todo...  

          uow.Commit();  
      }
      catch (Exception ex)  
      {
          Console.WriteLine(ex.ToString());  
          uow.Rollback();  
      }  
  }       
3.services.AddEntityCache(options => {})。通过options可配置为不同的缓存（如redis）  
