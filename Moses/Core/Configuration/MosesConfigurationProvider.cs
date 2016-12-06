using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using Moses.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;
using Moses.Management;

namespace Moses
{
    /// <summary>
    /// Classe respons�vel por realizar a leitura 
    /// </summary>
    public class MosesConfigurationProvider : ProviderBase
    {
       
        public static MosesConfiguration LoadConfiguration() 
        {
            MosesConfiguration config = System.Configuration.ConfigurationManager.GetSection("moses") as MosesConfiguration;

            if (config != null)
            {
                config.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            }

           

            return config;
        }


        /// <summary>
        /// Carrega a configura��o do Moses de Forma Manual
        /// </summary>
        /// <param name="filePath">Caminho do arquivo de configura��o</param>
        /// <returns></returns>
        public static MosesConfiguration LoadConfiguration(string filePath)
        {
            ConfigXmlDocument doc = new System.Configuration.ConfigXmlDocument();
            doc.Load(filePath);
            
            //Carrega a conectionString
            XmlElement connectionStringElem = doc["configuration"]["connectionStrings"];

            string cStr = null;
            DbmsTypeOptions option = DbmsTypeOptions.Postgres;
            EventProviderConfiguration evtProvConfig = new EventProviderConfiguration();
            MosesConfiguration configuration = new MosesConfiguration();

            try
            {
                //pega a connection string com o nome Default
                foreach (XmlNode node in connectionStringElem.ChildNodes)
                {
                    if (node.Name.ToLower() == "add" && node.Attributes["name"].Value.ToLower() == "default")
                    {
                        cStr = node.Attributes["connectionString"].Value;
                    }
                }

                //carrega as outras op��es do Moses
                connectionStringElem = doc["configuration"]["moses"];

                //Tipo de DBMS
                //switch (connectionStringElem.Attributes["dbmsType"].Value )
                //{
                //    case "Postgres": option = DbmsTypeOptions.Postgres; break;
                //    case "SqlServer": option = DbmsTypeOptions.SqlServer; break;
                //    case "Firebird": option = DbmsTypeOptions.Firebird; break;
                //    case "MySql": option = DbmsTypeOptions.MySql; break;
                //    case "OleDb": option = DbmsTypeOptions.OleDb; break;
                //    case "Oracle": option = DbmsTypeOptions.Oracle; break;
                //}

                //Configura��es do Event Provider
                foreach (XmlNode node in connectionStringElem.ChildNodes)
                {
                    if (node.Name == "eventProvider")
                    {
                        evtProvConfig.Port = node.Attributes["port"].Value;
                        evtProvConfig.Type = node.Attributes["type"].Value;
                        evtProvConfig.Server = node.Attributes["server"].Value;
                    }
                }

                //comp�e o Objeto
                configuration.EventProvider = evtProvConfig;
                configuration.DbmsType = option;
                configuration.ConnectionString = cStr;

                return configuration;
            }
            catch
            {
                throw new MosesConfigurationException("N�o foi poss�vel carregar o arquivo de Configura��o. Verifique se a se��o Moses existe no arquivo de configura��o.");
            }
        }

        public static MosesConfiguration CreateConfiguration(string connectionString, DbmsTypeOptions dbmsType, EventProviderConfiguration eventProviderConfig)
        {
            MosesConfiguration configuration = new MosesConfiguration();

            configuration.ConnectionString = connectionString;
            configuration.DbmsType = dbmsType;
            configuration.EventProvider = eventProviderConfig;

            return configuration;
        }


        public static MosesConfiguration CreateConfiguration(string connectionString, DbmsTypeOptions dbmsType, NameValueCollection eventProviderConfigValues)
        {
            if (eventProviderConfigValues != null)
            {
                return CreateConfiguration(connectionString, dbmsType, new EventProviderConfiguration(eventProviderConfigValues["server"], eventProviderConfigValues["port"]));
            }

            return CreateConfiguration(connectionString, dbmsType);
        }

        public static MosesConfiguration CreateConfiguration(string connectionString, DbmsTypeOptions dbmsType)
        {
            return CreateConfiguration(connectionString, dbmsType, null as EventProviderConfiguration);
        }

    }
}
