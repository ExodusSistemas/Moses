using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses
{
    public enum TextMode
    {
        None,
        Rg,
        NumberOnly,
        Cpf,
        Cnpj,
        Cep,
        Phone,
        Percent,
        Decimal,
        Password, 
        Currency,
        Date
    }

    public enum GenderOptions
    {
        Male,
        Female
    }

    public enum MessageTypeOptions
    {
        InvalidCpf,
        InvalidCnpj,
        ConfirmExclusion,
        UpdateSuccessfull,
        UseCurrentRegistryInfo,
        MsgInformacao,
        MsgConfirmacao,
        MsgContasReceber,
        MsgContasPagar,
        EnsurePrintQuote,
        Error
    }

    public enum InputFormMode
    {
        Edit,
        Insert,
        ReadOnly
    }

    public enum DataAction
    {
        Create,
        Edit,
        Delete,
        Read,
        Print,
        Filter
    }

    //public enum EnumTipoConsulta
    //{
    //    TpcFornecedor,
    //    TpcClientes,
    //    TpcVendedores,
    //    TpcProdutos,
    //    TpcAlmoxarifado,
    //    TpcFormPag,
    //}

    //public enum EnumTipoAcaoConsulta
    //{
    //    TpcInsert,
    //    TpcSelect

    //}

    public enum DialogFilterOptions
    {
        Xml,
        Images,
        Txt
    }
}
