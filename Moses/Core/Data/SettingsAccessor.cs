using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moses.Extensions;

namespace Moses
{

    /// <summary>
    /// Classe utilizada para acessar o qualquer item com ProfileId que guarda uma chave de padrão string
    /// </summary>
    public class SettingsAccessor
    {
        Dictionary<string, string> _items = new Dictionary<string, string>();
        ISettingsValuesContainer _settings;

        public SettingsAccessor(ISettingsValuesContainer settings)
        {
            _settings = settings;
            
            //não vai se usar settings names
            string[] sts = (settings.SettingsValues ?? "").Split(';');

            foreach (var items in sts)
            {
                string key = null;
                string value = null;
                try
                {
                    if (!string.IsNullOrEmpty(items) ){ 
                        var tuple = items.Split(':');
                        key = tuple[0];
                        value = tuple[1].Decypher();//constroi o valor no padrão
                        _items.Add(key, value);
                    }
                }
                catch { continue; }
                
            }
        }

        public string this[string item]
        {
            get
            {
                if (this._items.ContainsKey(item))
                    return _items[item];
                return null;
            }
            set
            {
                if (this._items.ContainsKey(item))
                    _items[item] = value;
                else
                    _items.Add(item, value);
            }
        }

        public void Insert(string key, string value)
        {
            _items.Add(key, value);
        }

        public void Remove(string key)
        {
            _items.Remove(key);
        }

        public void SyncTo(ISettingsValuesContainer mem = null)
        {
            if (mem == null) mem = _settings;
            StringBuilder bdr = new StringBuilder();
            foreach (var item in _items)
            {
                bdr.AppendFormat("{0}:{1};", item.Key, item.Value.Cypher());
            }

            mem.SettingsValues = bdr.ToString();
        }

        public void Sync()
        {
            SyncTo();
        }

    }
}
