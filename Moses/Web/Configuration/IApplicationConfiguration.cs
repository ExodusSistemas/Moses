using Moses.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Web
{
    public interface IApplicationConfiguration
    {
        string Brand { get; set; }
        string ApplicationTitle { get; set; }
        string NoRowsMessage { get; set; }
        string SystemEmail { get; set; }
        string SystemEmailSenderName { get; set; }
        string SystemEmailSenderNotificationTag { get; set; }
        string SupportEmail { get; set; }
        string SystemEmailChangePasswordTag { get; set; }
        string SystemEmailErrorTag { get; set; }

        void RegisterConfiguration();
        ISettingsService CreateSettingsService();
        void Start(ISettingsService configuration);
        List<IFeature> GetApplicationFeatures();
    }
}
