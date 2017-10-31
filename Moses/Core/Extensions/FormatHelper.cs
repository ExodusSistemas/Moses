using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Extensions
{
    public static class FormatHelper
    {

        #region Generic Format


        public static string SafeObjectFormat(this object o, Converter<string, string> stringOperation, Converter<decimal?, string> decimalOperation)
        {
            string a = o as string;
            decimal? b = null;

            if (a == null)
            {
                b = o as decimal?;
                if (b == null)
                {
                    return "";
                }
                else{
                    decimalOperation(((decimal?)o));
                }
            }
            else{
                return stringOperation(a);
            }

            throw new System.Exception("Entrada Inválida");
        }

        private static string SafeDecimalFormat(this decimal? input, string formatString)
        {
            if ((!input.HasValue) || (input.Value == 0))
                return "";
            else if (input.Value > 0)
                return input.Value.ToString(formatString);
            else 
                throw new FormatException("A Formatação é inválida");
        }

        /// <summary>
        /// Método padrão genérico para formatação de entradas de dados.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="formatString"></param>
        /// <returns></returns>
        private static string SafeStringFormat(this string input, TypeString typeStr, Predicate<string> recursiveChecker)
        {
            //se a expressão contém algo além de números, então é possível que 
            //ela já esteja formatada, então o ideal eh fazer o Parse antes.
            if (string.IsNullOrEmpty(input)) return "";

            if (!input.IsNumber())
            {
                //se dentro desse caso for string vazia ou nula, retorna logo
                if (recursiveChecker != null)
                {
                    if (recursiveChecker(input))
                        return input;
                }

                throw new FormatException("Conversão Inválida");
            }

            return FormatString(input, typeStr);
        }


        #endregion

        #region CPF

        const string cpfFormatStr = @"000\.000\.000-00";
        public static string CpfFormatString { get { return cpfFormatStr; } }

        const string cpfMask = "###.###.###-##";
        public static string CpfMask { get { return cpfMask; } }

        public static string FormatCpf(this object cpf)
        {
            return cpf.SafeObjectFormat(FormatCpf, FormatCpf);
        }

        public static string FormatCpf(this string cpf)
        {
            return cpf.SafeStringFormat(TypeString.CPF , Validators.IsFormatedCpf);
        }

        public static string FormatCpf(this decimal? cpf)
        {
            return cpf.SafeDecimalFormat(CpfFormatString);
        }

        #endregion

        #region Cnpj

        const string cnpjFormat = @"00\.000\.000/0000-00";
        public static string CnpjFormatString { get { return cnpjFormat; } }

        const string cnpjMask = @"##.###.###/####-##";
        public static string CnpjMask { get { return cnpjMask; } }

        public static string FormatCnpj(this object cnpj)
        {
            return cnpj.SafeObjectFormat(FormatCnpj, FormatCnpj);
        }

        public static string FormatCnpj(this decimal? cnpj)
        {
            return cnpj.SafeDecimalFormat(CnpjFormatString);
        }

        public static string FormatCnpj(this string cnpj)
        {
            return cnpj.SafeStringFormat(TypeString.CNPJ, Validators.IsFormatedCnpj);
        }

        public static string FormatCpfOrCnpj(this string cpfCnpj)
        {
            try
            {
                if (string.IsNullOrEmpty(cpfCnpj)) return "";

                if (cpfCnpj.GetNumbers().Length == 11)
                {
                    return cpfCnpj.SafeStringFormat(TypeString.CPF, Validators.IsFormatedCpf);
                }
                return cpfCnpj.SafeStringFormat(TypeString.CNPJ, Validators.IsFormatedCnpj);
            }
            catch
            {
                throw new Moses.MosesFormatException("Erro ao formatar "+cpfCnpj +" como CPF/CNPJ");
            }
        }

        #endregion

        #region Phone

        const string phoneFormat = @"(99)99999-9999";
        public static string PhoneFormatString { get { return phoneFormat; } }

        const string phoneMask = @"(##)99###-####";
        public static string PhoneMask { get { return phoneMask; } }

        public static string FormatPhone(this object phone)
        {
            return phone.SafeObjectFormat(FormatPhone, FormatPhone);
        }

        public static string FormatPhone(this decimal? phone)
        {
            return phone.SafeDecimalFormat(PhoneFormatString);
        }

        public static string FormatPhone(this string phone)
        {
            return phone.SafeStringFormat(TypeString.Phone, null);
        }

        #endregion

        #region Cep

        const string cepFormat = @"00000-000";
        public static string CepFormatString { get { return cepFormat; } }

        const string cepMask = @"00000-000";
        public static string CepMask { get { return cepMask; } }

        public static string FormatCep(this object cep)
        {
            var str = cep as string;
            decimal? num = null;
            if (str == null)
            {
                num = cep as decimal?;

                if (num == null)
                {
                    return "";
                }
                else
                {
                    return num.FormatCep();
                }
            }
            else
            {
                return str.FormatCep();
            }

            throw new FormatException("Entrada não Suportada");
        }

        public static string FormatCep(this decimal? cep)
        {
            return cep.SafeDecimalFormat(CepFormatString);
        }

        public static string FormatCep(this string cep)
        {
            //se a expressão contém algo além de números, então é possível que 
            //ela já esteja formatada, então o ideal eh fazer o Parse antes.
            if (!cep.IsNumber())
            {
                //se dentro desse caso for string vazia ou nula, retorna logo
                if (string.IsNullOrEmpty(cep))
                    return "";
                else if (cep.IsFormatedCep())
                    return cep;

                throw new FormatException("Conversão Inválida");
            }

            return decimal.Parse(cep).FormatCep();
        }

        #endregion

        #region Currency

        const string currencyFormat = @"###,###,###.00";
        public static string CurrencyFormatString { get { return currencyFormat; } }

        const string currencyMask = @"###,###,###.00";
        public static string CurrencyMask { get { return currencyMask; } }

        public static string FormatCurrency(this decimal? currency)
        {
            return currency.SafeDecimalFormat(CurrencyFormatString);
        }

        public static string FormatCurrency(this decimal currency)
        {
            return SafeDecimalFormat(currency, CurrencyFormatString);
        }

        public static string FormatCurrency(this string currency)
        {
            return currency.SafeStringFormat(TypeString.Currency, null);
        }

        /// <summary>
        /// Escreve o valor por extenso
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string SpellCurrencyOut(this decimal currency)
        {
            if (currency <= 0 | currency >= 1000000000000000)
                return "Valor não suportado pelo sistema.";
            else
            {
                string strValor = currency.ToString("000000000000000.00");
                string valor_por_extenso = string.Empty;

                for (int i = 0; i <= 15; i += 3)
                {
                    valor_por_extenso += SpellCurrencyOutPart(Convert.ToDecimal(strValor.Substring(i, 3)));
                    if (i == 0 &  !valor_por_extenso.IsNullOrEmpty() )
                    {
                        if (Convert.ToInt32(strValor.Substring(0, 3)) == 1)
                            valor_por_extenso += " TRILHÃO" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(0, 3)) > 1)
                            valor_por_extenso += " TRILHÕES" + ((Convert.ToDecimal(strValor.Substring(3, 12)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 3 & !valor_por_extenso.IsNullOrEmpty())
                    {
                        if (Convert.ToInt32(strValor.Substring(3, 3)) == 1)
                            valor_por_extenso += " BILHÃO" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(3, 3)) > 1)
                            valor_por_extenso += " BILHÕES" + ((Convert.ToDecimal(strValor.Substring(6, 9)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 6 & !valor_por_extenso.IsNullOrEmpty())
                    {
                        if (Convert.ToInt32(strValor.Substring(6, 3)) == 1)
                            valor_por_extenso += " MILHÃO" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                        else if (Convert.ToInt32(strValor.Substring(6, 3)) > 1)
                            valor_por_extenso += " MILHÕES" + ((Convert.ToDecimal(strValor.Substring(9, 6)) > 0) ? " E " : string.Empty);
                    }
                    else if (i == 9 & !valor_por_extenso.IsNullOrEmpty())
                        if (Convert.ToInt32(strValor.Substring(9, 3)) > 0)
                            valor_por_extenso += " MIL" + ((Convert.ToDecimal(strValor.Substring(12, 3)) > 0) ? " E " : string.Empty);

                    if (i == 12)
                    {
                        if (valor_por_extenso.Length > 8)
                            if (valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "BILHÃO" | valor_por_extenso.Substring(valor_por_extenso.Length - 6, 6) == "MILHÃO")
                                valor_por_extenso += " DE";
                            else
                                if (valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "BILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 7, 7) == "MILHÕES" | valor_por_extenso.Substring(valor_por_extenso.Length - 8, 7) == "TRILHÕES")
                                    valor_por_extenso += " DE";
                                else
                                    if (valor_por_extenso.Substring(valor_por_extenso.Length - 8, 8) == "TRILHÕES")
                                        valor_por_extenso += " DE";

                        if (Convert.ToInt64(strValor.Substring(0, 15)) == 1)
                            valor_por_extenso += " REAL";
                        else if (Convert.ToInt64(strValor.Substring(0, 15)) > 1)
                            valor_por_extenso += " REAIS";

                        if (Convert.ToInt32(strValor.Substring(16, 2)) > 0 && !valor_por_extenso.IsNullOrEmpty())
                            valor_por_extenso += " E ";
                    }

                    if (i == 15)
                        if (Convert.ToInt32(strValor.Substring(16, 2)) == 1)
                            valor_por_extenso += " CENTAVO";
                        else if (Convert.ToInt32(strValor.Substring(16, 2)) > 1)
                            valor_por_extenso += " CENTAVOS";
                }
                return valor_por_extenso;
            }
        }

        private static string SpellCurrencyOutPart(decimal valor)
        {
            if (valor <= 0)
                return string.Empty;
            else
            {
                string montagem = string.Empty;
                if (valor > 0 & valor < 1)
                {
                    valor *= 100;
                }
                string strValor = valor.ToString("000");
                int a = Convert.ToInt32(strValor.Substring(0, 1));
                int b = Convert.ToInt32(strValor.Substring(1, 1));
                int c = Convert.ToInt32(strValor.Substring(2, 1));

                if (a == 1) montagem += (b + c == 0) ? "CEM" : "CENTO";
                else if (a == 2) montagem += "DUZENTOS";
                else if (a == 3) montagem += "TREZENTOS";
                else if (a == 4) montagem += "QUATROCENTOS";
                else if (a == 5) montagem += "QUINHENTOS";
                else if (a == 6) montagem += "SEISCENTOS";
                else if (a == 7) montagem += "SETECENTOS";
                else if (a == 8) montagem += "OITOCENTOS";
                else if (a == 9) montagem += "NOVECENTOS";

                if (b == 1)
                {
                    if (c == 0) montagem += ((a > 0) ? " E " : string.Empty) + "DEZ";
                    else if (c == 1) montagem += ((a > 0) ? " E " : string.Empty) + "ONZE";
                    else if (c == 2) montagem += ((a > 0) ? " E " : string.Empty) + "DOZE";
                    else if (c == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TREZE";
                    else if (c == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUATORZE";
                    else if (c == 5) montagem += ((a > 0) ? " E " : string.Empty) + "QUINZE";
                    else if (c == 6) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSEIS";
                    else if (c == 7) montagem += ((a > 0) ? " E " : string.Empty) + "DEZESSETE";
                    else if (c == 8) montagem += ((a > 0) ? " E " : string.Empty) + "DEZOITO";
                    else if (c == 9) montagem += ((a > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
                else if (b == 2) montagem += ((a > 0) ? " E " : string.Empty) + "VINTE";
                else if (b == 3) montagem += ((a > 0) ? " E " : string.Empty) + "TRINTA";
                else if (b == 4) montagem += ((a > 0) ? " E " : string.Empty) + "QUARENTA";
                else if (b == 5) montagem += ((a > 0) ? " E " : string.Empty) + "CINQUENTA";
                else if (b == 6) montagem += ((a > 0) ? " E " : string.Empty) + "SESSENTA";
                else if (b == 7) montagem += ((a > 0) ? " E " : string.Empty) + "SETENTA";
                else if (b == 8) montagem += ((a > 0) ? " E " : string.Empty) + "OITENTA";
                else if (b == 9) montagem += ((a > 0) ? " E " : string.Empty) + "NOVENTA";

                if (strValor.Substring(1, 1) != "1" & c != 0 & !montagem.IsNullOrEmpty()) montagem += " E ";

                if (strValor.Substring(1, 1) != "1")
                    if (c == 1) montagem += "UM";
                    else if (c == 2) montagem += "DOIS";
                    else if (c == 3) montagem += "TRÊS";
                    else if (c == 4) montagem += "QUATRO";
                    else if (c == 5) montagem += "CINCO";
                    else if (c == 6) montagem += "SEIS";
                    else if (c == 7) montagem += "SETE";
                    else if (c == 8) montagem += "OITO";
                    else if (c == 9) montagem += "NOVE";

                return montagem;
            }
        }

        #endregion


        public static string FormatString(string value, TypeString tType)
        {
            try
            {
                switch (tType)
                {
                    case TypeString.CNPJ:
                        {
                            int s = -1;
                            if (value.Length == 14) s = 0;//para formatar com o zero inicial
                            if ( value.Length == 15) s = 1;//para formatar com o zero inicial
                            if (s == -1) return "";
                            return string.Format("{0}.{1}.{2}/{3}-{4}", value.Substring(0, 2+s), value.Substring(2+s, 3), value.Substring(5+s, 3), value.Substring(8+s, 4), value.Substring(12+s, 2));
                        }
                    case TypeString.CPF:
                        return string.Format("{0}.{1}.{2}-{3}", value.Substring(0, 3), value.Substring(3, 3), value.Substring(6, 3), value.Substring(9, 2));
                    case TypeString.Date:
                        if (Convert.ToDateTime(value) == Convert.ToDateTime("1/1/1900"))
                            return string.Empty;
                        else
                            return Convert.ToDateTime(value).ToString("dd/MM/yyyy");
                    case TypeString.Numeric:
                        return Convert.ToDouble(value).ToString("#,##0.00");
                    case TypeString.Int:
                        return Convert.ToInt64(value).ToString("#,##0");
                    case TypeString.Text:
                        return value;
                    case TypeString.CEP:
                        return string.Format("{0}.{1}-{2}", value.Substring(0, 2), value.Substring(2, 3), value.Substring(5, 3));
                    case TypeString.Phone:
                        value = value.Replace("-", "").Replace(" ", "").Replace(".", "");
                        return string.Format("{0}-{1}", value.Substring(0, value.Length - 4), value.Substring(value.Length - 4, 4));
                    case TypeString.Currency:
                        return Convert.ToDouble(value).ToString("C");
                    default:
                        return value;

                }

            }

            catch
            {

                return value;

            }

        }
    }

    public enum TypeString
    {

        Text,

        Numeric,

        CNPJ,

        CPF,

        Date,

        Int,

        CEP,

        Phone,

        Currency

    }
}
