using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moses.Web
{

    //CREATE TABLE AppSettings(
    //    Id varchar(200) not null ,
	//    Brand varchar(20) not null,
	//    SettingValue varchar(512) not null default '',
	//    primary key(Id, Brand)
    //)
    public interface ISettingsService
    {
        void Initialize( string brand);
        List<ISettingsServiceItem> GetConfiguration(string brand);
        string Get(string id);
    }

    public interface ISettingsServiceItem
    {
        string Id { get; set; }
        string Module { get; set; }
        string Brand { get; set; }
        string SettingValue { get; set; }
    }
}
