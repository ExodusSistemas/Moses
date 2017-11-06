using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moses.Services
{
    public class LogService 
    {
        public LogService(ILogger log)
        {
            Log = log;
        }

        public static ILogger Log { get; set; }

        private static ILoggerFactory _Factory = null;

        public static Action<ILoggerFactory> ConfigureLoggerAction;

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                if (_Factory == null)
                {
                    _Factory = new LoggerFactory();
                    if (ConfigureLoggerAction != null)
                        ConfigureLoggerAction(_Factory);
                }
                return _Factory;
            }
            set { _Factory = value; }
        }

        public static ILogger CreateLogger(string category) => LoggerFactory.CreateLogger(category);
    }

}
