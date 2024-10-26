using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace IntegrationWithCzech
{
    public static class Logger
    {
        private static NLog.Logger log;

        public static void ConfigLog()
        {
            var config = new LoggingConfiguration();

            string appDirectory = Directory.GetCurrentDirectory();
            string configFilePath = Path.Combine(appDirectory, "appsettings.json");
            var config1 = new ConfigurationBuilder()
                  .AddJsonFile(configFilePath)
                  .Build();
            var connectionstringsection = config1.GetSection("AppSettings:ConnectionString");

            string connectionString = connectionstringsection.Value;

            // Czech Database Target
            var LogDbTarget = new DatabaseTarget("logDBTarget")
            {
                ConnectionString = connectionString,
                CommandText = "INSERT INTO CDN.CzechLogTable(Date, Level, Message, Exception) VALUES(@date, @level, @message, @exception)"
            };
            LogDbTarget.Parameters.Add(new DatabaseParameterInfo("@date", "${longdate}"));
            LogDbTarget.Parameters.Add(new DatabaseParameterInfo("@level", "${level}"));
            LogDbTarget.Parameters.Add(new DatabaseParameterInfo("@message", "${message}"));
            LogDbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", "${exception}"));


            var logconsole = new ConsoleTarget("logconsole")
            {
                Layout = "${longdate}  ${message} ${exception}"
            };



            // Adding Rules for Poland Logging
            config.AddRuleForOneLevel(NLog.LogLevel.Error, LogDbTarget, "PolandLogger");
            config.AddRuleForAllLevels(logconsole, "PolandLogger");

            LogManager.Configuration = config;
            log = LogManager.GetLogger("CzechLogger");
            
        }

        public static void LogInfo(string message)
        {
            log?.Info(message);
        }

    

        public static void Write2CzechLogError(string message, Exception ex = null)
        {
            if (ex is null)
            {
                log?.Error(message);
            }
            else
            {
                log?.Error(ex, message);
            }
        }

    }
}
