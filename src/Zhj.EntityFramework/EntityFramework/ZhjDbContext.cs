using System.Data.Common;
using System.Data.Entity;
using Abp.Zero.EntityFramework;
using MyCompanyName.AbpZeroTemplate.Authorization.Roles;
using MyCompanyName.AbpZeroTemplate.Authorization.Users;
using MyCompanyName.AbpZeroTemplate.MultiTenancy;
using MyCompanyName.AbpZeroTemplate.Storage;
using MyCompanyName.AbpZeroTemplate.Navigation;
using MyCompanyName.AbpZeroTemplate.EntityModel;

namespace MyCompanyName.AbpZeroTemplate.EntityFramework
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ZhjDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
        /* Define an IDbSet for each entity of the application */
        
        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }
        public virtual IDbSet<BaseNavigation> BaseNavigation { get; set; }
        
        public virtual IDbSet<Customers> Customer { get; set; }
        public virtual IDbSet<Point> Point { get; set; }

        public virtual IDbSet<IcCard> IcCard { get; set; }

        public virtual IDbSet<OrderPayList> OrderPayList { get; set; }
        public virtual IDbSet<Orders> Order { get; set; }
        public virtual IDbSet<RechargerRecord> RechargerRecord { get; set; }
       // public virtual IDbSet<Users> User { get; set; }

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public ZhjDbContext()
            : base("Default")
        {

        }

        /* This constructor is used by ABP to pass connection string defined in AbpZeroTemplateDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of AbpZeroTemplateDbContext since ABP automatically handles it.
         */
        public ZhjDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public ZhjDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {

        }
    }
}
