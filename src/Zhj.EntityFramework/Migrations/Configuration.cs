using System.Data.Entity.Migrations;
using MyCompanyName.AbpZeroTemplate.Migrations.Seed;

namespace MyCompanyName.AbpZeroTemplate.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<EntityFramework.ZhjDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AutoSale";
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        protected override void Seed(EntityFramework.ZhjDbContext context)
        {
            new InitialDbBuilder(context).Create();
        }
    }
}
