using Moses.Web.Formatters;
using Moses.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moses.Extensions;
using Moses.Web.Mvc.Patterns;
using System.Web;
using System.Net.Http.Formatting;

namespace Moses.Web
{
    public class Configuration
        
    {
        public static IApplicationConfiguration ApplicationConfiguration { get; set; } = null;
        public static ISettingsService ApplicationSettings { get; set; }
        public static IJsonService Json { get; set; }

        static Configuration()
        {
            ApplicationExceptionType = typeof(MosesApplicationException);
        }

        public static void Setup<TApplicationConfiguration>(TApplicationConfiguration app,  System.Net.Http.Formatting.MediaTypeFormatterCollection mediaTypeFormatterCollection, System.Web.Mvc.ValueProviderFactoryCollection valueProviderFactoryCollection, System.Web.Mvc.ModelBinderDictionary modelBinderDictionary)
            where TApplicationConfiguration : class, IApplicationConfiguration
        {
            ApplicationConfiguration = app;
            mediaTypeFormatterCollection.RemoveAt(0);//remove o Json
            MediaTypeFormatter t = new JsonNetFormatter();
            mediaTypeFormatterCollection.Insert(0, t );

            valueProviderFactoryCollection.Remove(valueProviderFactoryCollection.OfType<JsonValueProviderFactory>().FirstOrDefault());
            valueProviderFactoryCollection.Add(new JsonNetValueProviderFactory());

            modelBinderDictionary.Add(typeof(decimal), new DecimalModelBinder());
            modelBinderDictionary.Add(typeof(decimal?), new DecimalModelBinder());
            modelBinderDictionary.Add(typeof(DateTime?), new DateTimePtBrModelBinder());
            modelBinderDictionary.Add(typeof(DateTime), new DateTimePtBrModelBinder());

            //Settings from Application Configuration
            ApplicationSettings = app.CreateSettingsService();
            ApplicationSettings.Initialize(app.Brand);
            HomeUrl = ApplicationSettings.Get(nameof(HomeUrl)) ?? "~/Home/Login";
            LoginUrl = ApplicationSettings.Get(nameof(LoginUrl)) ?? "~/Home/Login";

            ApplicationConfiguration.Start(ApplicationSettings);
        }


        #region SmtpClientActions 

        private static Func<SmtpClient> _configuratioSmtpClientFunction = null;

        public static void SetupMailClient(Func<SmtpClient> mailClient)
        {
            _configuratioSmtpClientFunction = mailClient;
        }

        public static SmtpClient GetMailClient()
        {
            return _configuratioSmtpClientFunction();
        }

        #endregion

        #region LogEntry Actions

        private static Action<MosesLogEntry> _configuratioLogEntryTaskFunction = null;

        public static void SetupLogEntryTask(Action<MosesLogEntry> logEntryTask)
        {
            _configuratioLogEntryTaskFunction = logEntryTask;
        }
        public static void ExecuteLogEntry(MosesLogEntry entry)
        {
            _configuratioLogEntryTaskFunction(entry);
        }

        public static bool HasLogEntryTask { get { return _configuratioLogEntryTaskFunction != null; } }

        #endregion

        public static string _bugTrackingEmail = null;
        public static string BugTrackingMail
        {
            get
            {
                return _bugTrackingEmail;
            }
            set
            {
                if (!_bugTrackingEmail.IsValidEmailAddress())
                {
                    throw new Moses.MosesConfigurationException("O E-mail de bugtracking é inválido. Verifique o Setup");
                }
                _bugTrackingEmail = value;
            }
        }

        public static string DefaultTitle { get; set; }

        public static string HomeUrl { get; set; }

        public static string LoginUrl { get; set; }

        public static Type ApplicationExceptionType { get; set; }

        #region User Initializer

        private static Func<HttpContextBase, HttpSessionStateBase, IUser> _configuratioUserInitializeTask = null;

        public static void SetupUserInitializer(Func<HttpContextBase, HttpSessionStateBase, IUser> logEntryTask)
        {
            _configuratioUserInitializeTask = logEntryTask;
        }
        
        internal static IUser ExecuteUserInitializer(System.Web.HttpContextBase httpContextBase, System.Web.HttpSessionStateBase httpSessionStateBase)
        {
            return _configuratioUserInitializeTask(httpContextBase, httpSessionStateBase);
        }

        public static bool HasUserInitializer { get { return _configuratioUserInitializeTask != null; } }

        #endregion

        #region Contract Initializer

        private static Func<HttpContextBase, HttpSessionStateBase, IContract> _configuratioContractInitializeTask = null;

        public static void SetupContractInitializer(Func<HttpContextBase, HttpSessionStateBase, IContract> logEntryTask)
        {
            _configuratioContractInitializeTask = logEntryTask;
        }

        internal static IContract ExecuteContractInitializer(System.Web.HttpContextBase httpContextBase, System.Web.HttpSessionStateBase httpSessionStateBase)
        {
            return _configuratioContractInitializeTask(httpContextBase, httpSessionStateBase);
        }

        public static bool HasContractInitializer { get { return _configuratioContractInitializeTask != null; } }

        #endregion

        #region Info Initializer

        private static Func<HttpContextBase, HttpSessionStateBase, object> _configuratioInfoInitializeTask = null;

        public static void SetupInfoInitializer(Func<HttpContextBase, HttpSessionStateBase, object> logEntryTask)
        {
            _configuratioInfoInitializeTask = logEntryTask;
        }

        internal static object ExecuteInfoInitializer(System.Web.HttpContextBase httpContextBase, System.Web.HttpSessionStateBase httpSessionStateBase)
        {
            return _configuratioInfoInitializeTask(httpContextBase, httpSessionStateBase);
        }

        public static bool HasInfoInitializer { get { return _configuratioInfoInitializeTask != null; } }

        #endregion

        

    }
}
