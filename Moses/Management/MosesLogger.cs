using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Moses.Management.Extensions
{
    

    /// <summary>
    /// 
    /// </summary>
    /// <permission cref="Autor:Ashraf Mohamed"></permission>
    public static class MosesLogHelper
    {
        /// <summary>
        /// Construtor estático
        /// </summary>
        static MosesLogHelper()
        {
            AlwaysLogToWindows = true;
        }

        private static StreamWriter sw = null;

        public static bool LoggingEnabled { get; set; }
        public static bool AlwaysLogToWindows { get; set; } 

        /// <summary>
        /// Setting LogFile path. If the logfile path is 
        /// null then it will update error info into LogFile.txt under
        /// application directory.
        /// </summary>
        public static string LogFilePath { set; get; }
        public static string ErrorFilePath { set; get; }
        

        /// <summary>
        /// Write error log entry for window event if the bLogType is true.
        /// Otherwise, write the log entry to
        /// customized text-based text file
        /// </summary>
        /// <param name="bLogType"></param>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static bool RaiseWindowsEvent(this Exception objException)
        {
            try
            {
                //Don't process more if the logging is disabled
                if (false == LoggingEnabled)
                    return true;

                //Write to Windows event log
                string EventLogName = "ErrorSample";

                if (!EventLog.SourceExists(EventLogName))
                    EventLog.CreateEventSource(objException.Message,
                    EventLogName);

                // Inserting into event log
                EventLog Log = new EventLog();
                Log.Source = EventLogName;
                Log.WriteEntry(objException.Message,
                         EventLogEntryType.Error);

                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Write error log entry to selected erro file path custom routine
        /// </summary>
        /// <param name="bLogType"></param>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static bool RaiseErrorToLogFile(this Exception e)
        {
            try
            {
                //Don't process more if the logging is disabled
                if (false == LoggingEnabled) return true;

                //Custom text-based event log
                sw = new StreamWriter(ErrorFilePath);
                sw.WriteLine("--------------------------------------");
                sw.WriteLine(e.Message);
                sw.WriteLine("--------------------------------------");

                if (AlwaysLogToWindows)
                {
                    e.RaiseWindowsEvent();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Escreve para um log uma 
        /// </summary>
        /// <param name="bLogType"></param>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static void Log(string e)
        {
            //Don't process more if the logging is disabled
            if (false == LoggingEnabled) return;

            string applicationName = AppDomain.CurrentDomain.ApplicationIdentity.FullName;
            //Custom text-based event log
            sw = new StreamWriter(ErrorFilePath);
            sw.Write(applicationName);
            sw.Write(":");
            sw.Write(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
            sw.Write(" ==> ");
            sw.Write(e);
            sw.Write("\r\n");
        }

        static Action<string> logIddleHandler = (string e) => { Log(e); };
        

        /// <summary>
        /// Escreve para um log uma 
        /// </summary>
        /// <param name="bLogType"></param>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static void BeginLog(string e)
        {
            logIddleHandler.BeginInvoke(e,null,null);
        }


        /// <summary>
        /// Write error log entry for custom routine
        /// </summary>
        /// <param name="bLogType"></param>
        /// <param name="objException"></param>
        /// <returns>false if the problem persists</returns>
        public static bool RaiseToCustom(this Exception e, Action<Exception> customErrorRoutine)
        {
            try
            {
                //Don't process more if the logging is disabled
                if (false == LoggingEnabled) return true;
                
                //Custom text-based event log
                customErrorRoutine(e);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
