using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses
{
    /// <summary>
    /// A classe que deve implementar IComposedItemContainer para 
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public interface IComposedItemContainer<K>
    {
        /// <summary>
        /// Entity que está contida na classe que implementa IComposedItemConainer
        /// </summary>
        K DataItem { get; set; }
    }

    /// <summary>
    /// A classe que implementa IProperyValueHolder deve se responsabilizar em serializar propriedades
    /// para entidades com um campo string representando várias proriedades (formato JSon).
    /// 
    /// </summary>
    /// <remarks>
    /// Geralmente a entidade que implementa IPropertyValueHolder possui um campo chamado PropertyValues
    /// que guarda uma coletânea de campos "customizáveis" de forma serializada
    /// </remarks>
    public interface IPropertyValueHolder
    {
        Dictionary<string, object> PropertyDictionary { get; set; }
    }

    public interface ISettingsValuesContainer
    {
        string SettingsValues { get; set; }
    }

}
