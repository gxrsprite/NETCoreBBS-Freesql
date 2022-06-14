using NetCoreBBS.Entities;
using NetCoreBBS.Infrastructure;
using System.Configuration;

namespace DatabaseInitializer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connstr = ConfigurationManager.ConnectionStrings["NetCoreBBS"].ConnectionString;
            bool isexisted = CreateDatabaseHelper.Create(connstr, FreeSql.DataType.SqlServer);
            var types = ReflexHelper.GetEntityTypes(typeof(IEntity));
            FreeSqlFactory.Init(connstr);
            FreeSqlFactory.Fsql.CodeFirst.SyncStructure(types.ToArray());
            FreeSqlFactory.Fsql.CodeFirst.SyncStructure(typeof(User));
            FreeSqlFactory.Fsql.CodeFirst.SyncStructure(typeof(Role));
            FreeSqlFactory.Fsql.CodeFirst.SyncStructure(typeof(UserClaim));
            FreeSqlFactory.Fsql.CodeFirst.SyncStructure(typeof(UserRole));
            Console.WriteLine("Finish");
            Console.ReadLine();
        }
    }
}