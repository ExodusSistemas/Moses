/*
 * 
 *  Framework Moses 3.0.0.0
 *  Este código é propriedade da Exodus. Todos os Direitos reservados.
 * 
 *  Autor: Olavo Rocha Neto olavo@exodus.eti.br 
 * 
 */
using System;   
using System.Configuration;
using System.Collections.Specialized;
using Moses.Management;
using Moses.Data;

namespace Moses
{
    /// <summary>
    /// MosesConfiguration
    /// </summary>
    public sealed class MosesConfiguration : ConfigurationSection  
    {
        //  <moses dbmsType="" >
        //       server="localhost" port="3450" />
        //  </moses>
        private static MosesConfiguration _default;

        /// <summary>
        ///  Default
        /// </summary>
        public static MosesConfiguration Default
        {
            set
            {
                _default = value;
                
                if (OnConfigurationChanged != null)
                    OnConfigurationChanged();
            }
            get
            {
                return _default;
            }
        }

        /// <summary>
        /// MosesConfiguration
        /// </summary>
        public MosesConfiguration(){}

        /// <summary>
        /// Atalho para o o ConnectionStrings Default do arquivo de configuração
        /// </summary>
        string connectionString;
        public String ConnectionString
        {
            get
            { 
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        /// <summary>
        /// DbmsType
        /// </summary>
        [ConfigurationProperty("dbmsType", DefaultValue = DbmsTypeOptions.Postgres, IsRequired = false)]
        public DbmsTypeOptions DbmsType
        {
            get
            { return (DbmsTypeOptions)this["dbmsType"]; }
            set
            { this["dbmsType"] = value; }
        }

        /// <summary>
        /// EventProvider
        /// </summary>
        [ConfigurationProperty("eventProvider", IsRequired=false )]
        public EventProviderConfiguration EventProvider
        {
            get
            { return this["eventProvider"] as EventProviderConfiguration; }
            set
            { this["eventProvider"] = value; }
        }

        /// <summary>
        /// MosesConfigurationChangedHandler
        /// </summary>
        public delegate void MosesConfigurationChangedHandler();

        /// <summary>
        /// OnConfigurationChanged event
        /// </summary>
        public static event MosesConfigurationChangedHandler OnConfigurationChanged;
    }

    public class EventProviderConfiguration : ConfigurationElement
    {
        /// <summary>
        /// EventProviderConfiguration
        /// </summary>
        public EventProviderConfiguration()
        {
        }

        /// <summary>
        /// EventProviderConfiguration
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public EventProviderConfiguration(string server, string port)
        {
            this["server"] = server;
            this["port"] = port;
        }

        /// <summary>
        /// Server
        /// </summary>
        [ConfigurationProperty("server", DefaultValue = "localhost", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string Server
        {
            get
            { return (string)this["server"]; }
            set
            { this["server"] = value; }
        }

        /// <summary>
        /// Port
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = "3450", IsRequired = true)]
        [StringValidator(InvalidCharacters = "[A-Za-z]~!@#$%^&*()[]{}/;'\"|\\", MinLength = 3, MaxLength = 5)]
        public string Port
        {
            get
            { return (String)this["port"]; }
            set
            { this["port"] = value; }
        }

        /// <summary>
        /// Type
        /// </summary>
        [ConfigurationProperty("type", DefaultValue = "Manager", IsRequired = true)]
        public string Type
        {
            get
            { return (String)this["type"]; }
            set
            { this["type"] = value; }
        }

        private void GenerateConfigFile(ref System.Configuration.Configuration configFile)
        {
            //MosesSection section = new MosesSection();
            //section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
            //configFile.Sections.Add("Moses.Configuration", section);
            //configFile.Save(ConfigurationSaveMode.Full);
        }

        private void GenerateConfigFile(ref System.Configuration.Configuration configFile,string fileName)
        {
            //MosesSection section = new MosesSection();
            //section.SectionInformation.AllowExeDefinition = ConfigurationAllowExeDefinition.MachineToLocalUser;
            //configFile.Sections.Add("Moses.Configuration", section);
            //configFile.SaveAs(fileName, ConfigurationSaveMode.Full);
        }
    }
}