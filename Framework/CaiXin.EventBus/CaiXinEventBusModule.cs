using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace CaiXin.EventBus
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public class CaiXinEventBusModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddCap(options =>
            {
                //订阅者所属的默认消费者组
                options.DefaultGroupName = "caixin.nima.dotnet.cap";
                //成功的消息被删除的过期时间/秒
                options.SucceedMessageExpiredAfter = 10;
                //失败最大重试次数
                options.FailedRetryCount = 10;
                //失败重试间隔时间	/ 秒
                options.FailedRetryInterval = 20;
                //执行失败消息时的回调函数
                options.FailedThresholdCallback = null;

                //options.FailedMessageExpiredAfter = 10;

                //配置RabbitMQ
                options.UseRabbitMQ(rabbitMqOptions =>
                {
                    //宿主地址
                    rabbitMqOptions.HostName = "host";
                    //用户名
                    rabbitMqOptions.UserName = "rabbitmq";
                    //密码
                    rabbitMqOptions.Password = "rabbitmq";
                    //虚拟主机
                    // rabbitMqOptions.VirtualHost = "/";
                    //端口号
                    rabbitMqOptions.Port = 5672;
                    //CAP默认Exchange名称
                    rabbitMqOptions.ExchangeName = "caixin.nima.dotnet.cap";

                    //RabbitMQ连接超时时间
                    /*   rabbitMqOptions.RequestedConnectionTimeout = 30000;
                       //RabbitMQ消息读取超时时间
                       rabbitMqOptions.SocketReadTimeout = 30000;
                       //RabbitMQ消息写入超时时间
                       rabbitMqOptions.SocketWriteTimeout = 30000;
                       //队列中消息自动删除时间 (10天) 毫秒
                       rabbitMqOptions.QueueMessageExpires = 864000000;*/
                });
                //配置SQLServer
                options.UseSqlServer(sqlServerOptions =>
                {
                    //数据库连接字符串
                    sqlServerOptions.ConnectionString = "server=host;uid=sa;pwd=sa@12345;database=CaiXin_Db;Encrypt=True;TrustServerCertificate=True;";
                    //Cap表架构
                    sqlServerOptions.Schema = "Cap";
                });
                //启用面板
                options.UseDashboard();
            });

            // CAP配置
            // ConfigureCAP(context);

            // 取消原模块 RabbitMQ 配置处理
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // 取消原模块 RabbitMQ 初始化处理
        }

        /// <summary>
        /// CAP配置
        /// </summary>
        /// <param name="context"></param>
        protected void ConfigureCAP(ServiceConfigurationContext context)
        {
            context.Services.AddCap(x =>
            {
                /*
                 * 发送和消费消息的过程中失败会立即重试 3 次，在 3 次以后将进入重试轮询，此时 FailedRetryInterval 配置才会生效。
                 * 在默认情况下，重试将在发送和消费消息失败的 4分钟后开始
                 */

                // 重试间隔（秒）
                x.FailedRetryInterval = 10; //EgmpEnvironment.Instance.CAPRetryInterval;
                // 重试次数
                x.FailedRetryCount = 3; //EgmpEnvironment.Instance.CAPRetryCount;
                // 重试阈值的失败回调
                x.FailedThresholdCallback = (failedInfo) =>
                {
                    Log.Error("CAP消息重试失败！{MessageType},{@Message}", failedInfo.MessageType, failedInfo.Message);
                };
                // 失败消息的过期时间（天）
                x.FailedMessageExpiredAfter = 3600 * 24 * 1; //EgmpEnvironment.Instance.CAPFailedMessageExpired;
                                                             // 每个消费者组都会根据 ConsumerThreadCount 设置的值创建单独的线程进行处理
                                                             // x.UseDispatchingPerGroup = true;
                                                             // 路由Key前缀
                x.TopicNamePrefix = "";
                // 版本号
                x.Version = "v1";
                // 使用 RabbitMQ
                x.UseRabbitMQ(options =>
                {
                    options.HostName = "192.168.100.9"; //EgmpEnvironment.Instance.MqHost;
                    options.VirtualHost = "/"; //EgmpEnvironment.Instance.MqVirtualHost;
                    options.Port = 5672;// EgmpEnvironment.Instance.MqPort;
                    options.UserName = "rabbitmq";// EgmpEnvironment.Instance.MqUserName;
                    options.Password = "rabbitmq";// EgmpEnvironment.Instance.MqPassword;
                    options.ExchangeName = "CaiXin.Cap"; //EgmpEnvironment.Instance.CAPExchangeName;
                });
                // 使用仪表板，http://localhost:xxx/cap
                x.UseDashboard(options =>
                {
                    options.PathMatch = "/cap"; //EgmpEnvironment.Instance.CAPDashboardPath;
                });

                //  数据库类型
                //var databaseType = (EnumDataBaseType)EgmpEnvironment.Instance.DatabaseType;
                //if (databaseType == EnumDataBaseType.SqlServer)
                //{
                x.UseSqlServer("server=host;uid=sa;pwd=sa@12345;database=CaiXin_Db;Encrypt=True;TrustServerCertificate=True;");
                //}
                //else if (databaseType == EnumDataBaseType.Mysql)
                //{
                //    x.UseMySql(EgmpEnvironment.Instance.DatabaseConnectionString);
                //}
                //else if (databaseType == EnumDataBaseType.PostgreSql)
                //{
                //    x.UsePostgreSql(EgmpEnvironment.Instance.DatabaseConnectionString);
                //}
                //else if (databaseType == EnumDataBaseType.Sqlite)
                //{
                //    x.UseInMemoryStorage();
                //}
            });
        }
    }
}