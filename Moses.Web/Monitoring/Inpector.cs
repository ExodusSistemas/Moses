using Moses.Web.Mvc.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Moses.Web
{
    /// <summary>
    /// Entidade que inspeciona o acesso dos usuários a páginas e loga no banco de dados para realização de estatísticas
    /// </summary>
    public static class Inspector
    {
        private static void LogAsyncHandle(object objEntry)
        {
            if (Configuration.HasLogEntryTask)
            {
                MosesLogEntry entry = objEntry as MosesLogEntry;
                if (entry != null)
                    Configuration.ExecuteLogEntry(entry);
            }
        }

        public static void Log(this MosesLogEntry entry)
        {

            var a = new Action<object>(LogAsyncHandle);
            Task t = new Task(a, entry);
            t.Start();
            //LogAsyncHandle(entry);
        }

        public static MosesLogEntry CreateLogEntry(this MembershipContext context, MosesLogEntryOptions type)
        {
            return CreateLogEntry(context, type, "");
        }

        public static MosesLogEntry CreateLogEntry(this MembershipContext context, MosesLogEntryOptions type, string description)
        {
            Guid? userId = null;
            int? contractId = null;

            if (context.Contract != null)
            {
                contractId = context.Contract.Id;
            }

            if (context.User != null)
            {
                userId = context.User.Id;
            }

            MosesLogEntry entry = new MosesLogEntry()
            {
                ContractId = contractId,
                UserId = userId,
                EntryType = type,
                Description = description
            };

            return entry;
        }



        public static void SubmitLogEntry(this MembershipContext context, MosesLogEntryOptions type)
        {
            context.SubmitLogEntry(type, "");
        }

        public static void SubmitLogEntry(this MembershipContext context, MosesLogEntryOptions type, string description)
        {
            context.CreateLogEntry(type, description).Log();
        }


        public static void WriteLog(MosesController context, ExceptionContext filterContext)
        {
            HttpContextBase ctx = context.HttpContext;

            Exception exception = filterContext.Exception;
            string userName, contractName = "";

            try
            {
                if (exception == null) return;
                //filtra bugs conhecidos para não entupir a caixa de e-mails
                if (exception is System.Web.UI.ViewStateException) return;
                if (exception.Message.Contains("was not found")) return;


                if( context.MembershipContext.User != null)
                    userName = context.MembershipContext.User.FullName.ToString();

                if ( context.MembershipContext.Contract != null)
                    contractName = context.MembershipContext.Contract.Name.ToString();
            }
            catch
            {

            }

            System.Net.Mail.MailMessage mail = null;
            try
            {
                mail = Moses.Web.ApplicationMail.CreateErrorMail(ctx.Request.Url.ToString(), exception, context.HttpContext.Session["UserName"].ToString(), contractName);
                Moses.Web.ApplicationMail.SendMail(mail);
            }
            catch
            {

            }

            try
            {
                //insere nos eventos do windows
                // checks if the event source is registered
                if (!System.Diagnostics.EventLog.SourceExists("Exodus Financas 1.0.1.1"))
                {
                    System.Diagnostics.EventLog.CreateEventSource("Exodus Financas 1.0.1.1", "Application");
                }

                //log the details of the error occured
                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
                log.Source = "Exodus Financas 1.0.0.x";
                log.WriteEntry(String.Format("\r\n\r\nApplication Error\r\n\r\n" +
                                                "MESSAGE: {0}\r\n" +
                                                "SOURCE: {1}\r\n" +
                                                "FORM: {2}\r\n" +
                                                "QUERYSTRING: {3}\r\n" +
                                                "TARGETSITE: {4}\r\n" +
                                                "STACKTRACE: {5}\r\n",
                                                exception.Message,
                                                exception.Source,
                                                context.HttpContext.Request.Form.ToString(),
                                                context.HttpContext.Request.QueryString.ToString(),
                                                exception.TargetSite,
                                                exception.StackTrace),
                                                System.Diagnostics.EventLogEntryType.Error);


            }
            catch
            {

            }
        }

            
    }

    public class MosesLogEntry
    {
        public Guid? UserId { get; set; }
        public int? ContractId { get; set; }

        public MosesLogEntryOptions EntryType { get; set; }
        public DateTime LogDate { get { return DateTime.Now; } }
        public string Description { get; set; }
    }

    public enum MosesLogEntryOptions
    {
        NA,
        Erro
    }
}
