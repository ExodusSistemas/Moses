using System;
using System.Text;
using Moses.Extensions;

namespace Moses.Web.Mvc.Html
{
    using System.Web.Mvc;
    using System.Linq.Expressions;
    using System.Web.Caching;
    using System.Web;

    // Definição da Propriedade de Máscara:
    public enum MaskTypes
    {
        None,
        Cpf,
        Cnpj,
        Phone,
        InternationalPhone,
        Date,
        HourMinute,
        Decimal,
        Integer,
        UnsignedInteger,
        Currency,
        Cep,
        MathExpression
    }

    public static class MosesInputExtensions
    {

        #region CPF Inputs

        public static string InputCpf(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, "", MaskTypes.Cpf, false, "Cpf Inválido");
        }

        public static string InputCpf(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, "", MaskTypes.Cpf, true, validationMessage);
        }

        public static string InputCpf(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCpf(), MaskTypes.Cpf, false, "Cpf Inválido");
        }

        public static string InputCpf(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCpf(), MaskTypes.Cpf, true, validationMessage);
        }

        #endregion

        #region DateInputs

        public static string InputDate(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            DateTime dt;
            if (htmlHelper.ViewData[name] != null && DateTime.TryParse(htmlHelper.ViewData[name].ToString(), out dt))
            {
                //bem sucedido
                return MosesTextBox(htmlHelper, name, dt.ToString("dd/MM/yyyy"), MaskTypes.Date, false, "");
            }

            return MosesTextBox(htmlHelper, name, null, MaskTypes.Date, false, "");
        }

        public static string InputDate(this System.Web.Mvc.HtmlHelper htmlHelper, string name, DateTime value)
        {
            return MosesTextBox(htmlHelper, name, value.ToString("dd/MM/yyyy"), MaskTypes.Date, false, "");
        }

        public static string InputDate(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            DateTime dt;
            if (htmlHelper.ViewData[name] != null && DateTime.TryParse(htmlHelper.ViewData[name].ToString(), out dt))
            {
                //bem sucedido
                return MosesTextBox(htmlHelper, name, dt.ToString("dd/MM/yyyy"), MaskTypes.Date, true, validationMessage);
            }

            return MosesTextBox(htmlHelper, name, null, MaskTypes.Date, true, validationMessage);
        }

        public static string InputDate(this System.Web.Mvc.HtmlHelper htmlHelper, string name, DateTime value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.ToString("dd/MM/yyyy"), MaskTypes.Date, true, validationMessage);
        }

        #endregion

        #region Cep Inputs

        public static string InputCep(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Cep, false, "");
        }

        public static string InputCep(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCep(), MaskTypes.Cep, false, "");
        }

        public static string InputCep(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Cep, true, validationMessage);
        }

        public static string InputCep(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCep() , MaskTypes.Cep, true, validationMessage);
        }

        #endregion

        #region PhoneInputs

        public static string InputPhone(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Phone, false, "");
        }

        public static string InputPhone(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value)
        {
            return MosesTextBox(htmlHelper, name, value.FormatPhone(), MaskTypes.Phone, false, "");
        }

        public static string InputPhone(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Phone, true, validationMessage);
        }

        public static string InputPhone(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.FormatPhone(), MaskTypes.Phone, true, validationMessage);
        }

        #endregion


        #region CnpjInputs

        public static string InputCnpj(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Cnpj, false, "");
        }

        public static string InputCnpj(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCnpj(), MaskTypes.Cnpj, false, "");
        }

        public static string InputCnpj(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Cnpj, true, validationMessage);
        }

        public static string InputCnpj(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCnpj(), MaskTypes.Cnpj, true, validationMessage);
        }

        #endregion

        #region CurrencyInputs

        public static string InputCurrency(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Currency, false, "");
        }

        public static string InputCurrency(this System.Web.Mvc.HtmlHelper htmlHelper, string name, decimal value)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCurrency(), MaskTypes.Currency, false, "");
        }

        public static string InputCurrency(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Currency, true, validationMessage);
        }

        public static string InputCurrency(this System.Web.Mvc.HtmlHelper htmlHelper, string name, decimal value, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, value.FormatCurrency(), MaskTypes.Currency, true, validationMessage);
        }

        #endregion

        #region IntegerInputs

        public static string InputInteger(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Integer, false, "");
        }

        public static string InputInteger(this System.Web.Mvc.HtmlHelper htmlHelper, string name, int value)
        {
            return MosesTextBox(htmlHelper, name, value, MaskTypes.Integer, false, "");
        }

        public static string InputInteger(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string validationMessage)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.Integer, true, validationMessage);
        }

        public static string InputInteger(this System.Web.Mvc.HtmlHelper htmlHelper, string name, int value, string validationMessage, bool acceptNegative)
        {
            if (acceptNegative)
            {
                return MosesTextBox(htmlHelper, name, value, MaskTypes.UnsignedInteger, true, validationMessage);
            }
            else{
                return MosesTextBox(htmlHelper, name, value, MaskTypes.Integer, true, validationMessage);
            }
        }

        #endregion

        #region MathExpressionInputs

        public static string InputMathExpression(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return InputMathExpression(htmlHelper, name,"","");
        }

        public static string InputMathExpression(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string value)
        {
           return InputMathExpression(htmlHelper, name,value,"");
        }

        public static string InputMathExpression(this System.Web.Mvc.HtmlHelper htmlHelper, string name, string value, string validationMessage)
        {
            StringBuilder bdr = new StringBuilder();
            bdr.Append(MosesTextBox(htmlHelper, name, value, MaskTypes.MathExpression, true, validationMessage));
            bdr.AppendFormat("<input type=\"hidden\" name=\"{0}__formula\" id=\"{1}__formula\" value=\"{2}\" />", name, name.Replace(".","-") , value ?? "");
            bdr.AppendScript(@"$(document).ready( function() {  MathExpressionInput_PrepareControls('"+name+"'); } );");
            return bdr.ToString();
            
        }

        #endregion

        public static string MosesTextBox(this System.Web.Mvc.HtmlHelper htmlHelper, string name)
        {
            return MosesTextBox(htmlHelper, name, null, MaskTypes.None, false, "*");
        }

        public static string MosesTextBox(this System.Web.Mvc.HtmlHelper htmlHelper, string name, MaskTypes maskType, bool validateInput, string validateMessage)
        {
            return MosesTextBox(htmlHelper, name, null, maskType, validateInput, validateMessage);
        }

        public static string MosesTextBox(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, MaskTypes maskType, bool validateInput, string validateMessage)
        {
            if (value == null)
            {
                value = htmlHelper.ViewData[name];
            }

            StringBuilder attributesBuilder = new StringBuilder();
            attributesBuilder.AddAttributesToRender(maskType, validateInput, validateMessage);
            
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\" value=\"{2}\" {3} />",
                    name,
                    name.Replace(".", "_"),
                    value,
                    attributesBuilder.ToString()
                );

            return builder.ToString();
        }

        public static MvcHtmlString MosesTextBoxFor<TModel, TProperty>(this System.Web.Mvc.HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MaskTypes maskType, object htmlAttributes)
        {
            StringBuilder attributesBuilder = new StringBuilder();
            attributesBuilder.AddAttributesToRender(maskType, false, "");
            //htmlAttributes
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"text\" name=\"{0}\"id=\"{1}\" value=\"{2}\" {3} />",
                    expression.ToString(),
                    "",
                    "",
                    attributesBuilder.ToString()
                );


            return MvcHtmlString.Create(builder.ToString());
        }

        public static string MosesTextBox(this System.Web.Mvc.HtmlHelper htmlHelper, string name, object value, MaskTypes maskType, bool validateInput, string validateMessage,string className)
        {
            if (value == null)
            {
                value = htmlHelper.ViewData[name];
            }

            StringBuilder attributesBuilder = new StringBuilder();
            attributesBuilder.AddAttributesToRender(maskType, validateInput, validateMessage);
            attributesBuilder.AddAttribute("class", className);
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"text\" name=\"{0}\" id=\"{1}\" value=\"{2}\" {3} />",
                    name,
                    name.Replace(".", "_"),
                    value,
                    attributesBuilder.ToString()
                );



            return builder.ToString();

        }
 

        private static void AddAttributesToRender(this StringBuilder builder, MaskTypes maskType,  bool validateInput, string validateMessage)
        {
            
            switch (maskType)
            {
                // None
                case MaskTypes.None:
                    // Não faz nada!
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // CPF
                case MaskTypes.Cpf:
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraCPF(this); } else { return false; }");
                    builder.AddAttribute("maxlength", "14");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; } else if (!ValidaCPF(this.value)) { alert('CPF Inválido!'); return false; }");
                    }
                    break;
                // CNPJ
                case MaskTypes.Cnpj:
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraCNPJ(this, event); } else { return false; }");
                    builder.AddAttribute("maxlength", "18");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; } else if (!ValidaCNPJ(this.value)) { alert('CNPJ Inválido!'); return false; }");
                    }
                    break;
                // Telefone Nacional
                case MaskTypes.Phone:
                    builder.AddAttribute("maxlength", "13");
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, ' ', '')) { MascaraTelefoneNacional(this); } else { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // TelefoneInternacional
                case MaskTypes.InternationalPhone:
                    builder.AddAttribute("maxlength", "30");
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, ' ', '-')) { MascaraTelefoneInternacional(this); } else { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // DataDigitar
                case MaskTypes.Date:
                    builder.AddAttribute("maxlength", "10");
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraData(this); } else { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { return false; } else if (!ValidaData(this.value)) { return false; }");
                        //Com a mensgem de validação, q ainda não funciona:
                        //writer.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('"+ValidarMensagem+"'); return false; } else if (!ValidaData(this.value)) { return false; }");
                    }
                    break;
                // HoraMinuto
                case MaskTypes.HourMinute:
                    builder.AddAttribute("maxlength", "5");
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event, '', '')) { MascaraHoraMinuto(this); } else { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; } else if (!ValidaHoraMinuto(this.value)) { return false; }");
                    }
                    break;
                // NumeroDecimal
                case MaskTypes.Decimal:
                    builder.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', ',')) { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // NumeroInteiro
                case MaskTypes.Integer:
                    builder.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', '-')) { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // NumeroInteiroPositivo
                case MaskTypes.UnsignedInteger:
                    builder.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', '')) { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                //expressões matemáticas (=2+3)
                case MaskTypes.MathExpression:
                    builder.AddAttribute("OnKeyPress", @"javascript: 
                        if (!SomenteExpressao(event,this.value )) 
                            { return false; }");
                    if (validateInput)
                    {
                        //builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // Moeda
                case MaskTypes.Currency:
                    builder.AddAttribute("OnKeyPress", "javascript: if (!SomenteNumero(event, '', '-')) { return false; }");
                    builder.AddAttribute("OnKeyUp", "javascript: MascaraMoeda(event, this);");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
                // CEP
                case MaskTypes.Cep:
                    builder.AddAttribute("maxlength", "9");
                    builder.AddAttribute("OnKeyPress", "javascript: if (SomenteNumero(event,'','')) { MascaraCEP(this); } else { return false; }");
                    if (validateInput)
                    {
                        builder.AddAttribute("OnBlur", "javascript: if (this.value.trim()=='') { alert('" + validateMessage + "'); return false; }");
                    }
                    break;
            }

        }

   }
}
