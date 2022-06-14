namespace NetCoreBBS.Infrastructure
{
    public class FreeSqlFactory
    {
        public static void Init(string config)
        {
            Fsql = new FreeSql.FreeSqlBuilder()
                 .UseConnectionString(FreeSql.DataType.SqlServer, config)
                .UseAutoSyncStructure(false) //自动同步实体结构到数据库
                .UseMonitorCommand((cmd) => { cmd.CommandTimeout = 300000; })
                .Build(); //请务必定义成 Singleton 单例模式
        }

        public static IFreeSql Fsql;
    }
}
