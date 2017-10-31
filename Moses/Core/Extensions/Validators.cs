using System.IO;
using System.Text.RegularExpressions;
using System;

namespace Moses.Extensions
{
    /// <summary>
    /// Classe criada para realizar validação de dados convencionados pela Exodus
    /// </summary>
    /// 
    public static partial class Validators
    {

        #region Phone

        static Regex phoneFormat = new Regex(@"^((\+\d?\d)?\([1-9][0-9]\))?([1-9]{1})?[1-9]{1}\d{3}\-\d{4}$", RegexOptions.Compiled);

        public static bool IsPhoneFormat(this string phone)
        {
            return phoneFormat.IsMatch(phone);
        }

        #endregion

        /// <summary>
        /// Diz se a seqûencia é um CNPJ ou CPF válido
        /// </summary>
        /// <param name="cpfCnpj"></param>
        /// <returns></returns>
        public static bool IsValidCpfCnpj(this string cpfCnpj)
        {
            return cpfCnpj.IsValidCpf() || cpfCnpj.IsValidCnpj();
        }

        #region CPF

        static Regex cpfFmtRegex = new Regex(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$",RegexOptions.Compiled);

        public static bool IsFormatedCpf(this string cpf)
        {
            return cpfFmtRegex.IsMatch(cpf);
        }

        /// <summary>
        /// Diz se a sequência em questão é um CPF válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns>Retorna verdadeiro se o CPF for válido ou a string for nula ou vazia</returns>
        public static bool IsValidCpf(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return true;

            int soma, resto, i = 0;
            int[] digitos;

            //faz uma limpeza antes, e armazena numa nova string no mesmo ponteiro
            cpf = cpf.Replace("-", "").Replace(".", "");

            using (StringReader cpfReader = new StringReader(cpf))
            {

                if (cpf.Length == 0) return (true);
                if (cpf.Length != 11) return (false);

                //pequeno melhoramento do código original
                digitos = new int[11];
                char c;
                int tempC;
                while ((tempC = cpfReader.Read()) != -1)
                {
                    c = (char)tempC;
                    switch (c)
                    {
                        case '0': digitos[i] = 0; break;
                        case '1': digitos[i] = 1; break;
                        case '2': digitos[i] = 2; break;
                        case '3': digitos[i] = 3; break;
                        case '4': digitos[i] = 4; break;
                        case '5': digitos[i] = 5; break;
                        case '6': digitos[i] = 6; break;
                        case '7': digitos[i] = 7; break;
                        case '8': digitos[i] = 8; break;
                        case '9': digitos[i] = 9; break;
                    }
                    i++;
                }



                soma = 0;
                for (i = 0; i < 9; i++)
                    soma = soma + (digitos[i] * (10 - i));

                resto = 11 - (soma - ((soma / 11) * 11));
                if ((resto == 10) || (resto == 11)) resto = 0;

                if (resto != digitos[9])
                    return (false);

                soma = 0;
                for (i = 0; i < 10; i++)
                    soma = soma + (digitos[i] * (11 - i));

                resto = 11 - (soma - ((soma / 11) * 11));
                if ((resto == 10) || (resto == 11)) resto = 0;

                if (resto != digitos[10])
                    return (false);

                return (true);
            }
        }


        /// <summary>
        /// Diz se a sequência em questão é um CPF válido
        /// </summary>
        /// <author>Olavo Rocha Neto</author>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsValidCpf(this decimal cpf)
        {
            return IsValidCpf(cpf.ToString());
        }

        public static void ValidateCpf(this string cpf)
        {
            if (!cpf.IsValidCpf()) throw new ArgumentException("Cpf Inválido", cpf);
        }


        #endregion

        #region Email

        //static Regex emailRegex = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[a-z]{2}|com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|mobi|name|aero|jobs)\b$", (RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace));
        static Regex emailRegex = new Regex(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]*?\.)+(?:[a-z]{2}|eti|com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|mobi|name|aero|jobs)$", (RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace));
        //static Regex emailRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"; 

        /// <summary>
        /// Determina se uma sequência representa um e-mail válido. 
        /// </summary>
        /// <remarks>Strings vazias ou nulas são declaradas válidas</remarks>
        /// <author>Olavo Neto</author>
        /// <param name="sEmail">sequência de entrada</param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string sEmail)
        {
            if (!string.IsNullOrEmpty(sEmail))
            {
                return emailRegex.IsMatch(sEmail);
            }
            
            return true;
        }

        public static void ValidateEmailAddress(this string sEmail)
        {
            if (!sEmail.IsValidEmailAddress()) throw new ArgumentException("Endereço de e-mail Inválido", sEmail);
        }


        public static bool IsValidEmailAddressList(this string sEmail)
        {
            if (!string.IsNullOrEmpty(sEmail))
            {
                bool output = true;

                var ss = sEmail.Split(',');
                foreach (var s in ss)
                {
                    if (string.IsNullOrWhiteSpace(s))
                        continue;

                    output = emailRegex.IsMatch(s.Trim());
                    if (!output) return false;
                }
                return output;
            }

            return true;
        }

        public static void ValidateEmailAddressList(this string sEmail)
        {
            if (!sEmail.IsValidEmailAddressList()) throw new ArgumentException("Endereço(s) de e-mail(s) Inválido(s)", sEmail);
        }


        #endregion

        #region CNPJ

        static Regex cnpjFmtRegex = new Regex(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", RegexOptions.Compiled);

        public static bool IsFormatedCnpj(this string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj) ) return false;
            return cnpjFmtRegex.IsMatch(cnpj);
        }

        public static bool IsValidCnpj(this string cnpj) //CGC 
        {
            if (string.IsNullOrEmpty(cnpj)) return true;

            cnpj = cnpj.Replace("/", "").Replace("-", "").Replace(".", "");

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;
            for (nrDig = 0; nrDig < 14; nrDig++)
            {
                digitos[nrDig] = int.Parse(cnpj.Substring(nrDig, 1));
                if (nrDig <= 11)
                    soma[0] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig + 1, 1)));
                if (nrDig <= 12)
                    soma[1] += (digitos[nrDig] * int.Parse(ftmt.Substring(nrDig, 1)));
            }

            for (nrDig = 0; nrDig < 2; nrDig++)
            {
                resultado[nrDig] = (soma[nrDig] % 11);
                if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                    CNPJOk[nrDig] = (digitos[12 + nrDig] == 0);
                else
                    CNPJOk[nrDig] = (digitos[12 + nrDig] == (11 - resultado[nrDig]));
            }
            return (CNPJOk[0] && CNPJOk[1]);
        }

        public static void ValidateCnpj(this string cnpj)
        {
            if (!cnpj.IsValidCnpj()) throw new ArgumentException("Cnpj Inválido",cnpj);
        }

        #endregion

        #region Cep

        static Regex cepFmtRegex = new Regex(@"^\d{5}-\d{3}$", RegexOptions.Compiled);

        public static bool IsFormatedCep(this string cep)
        {
            return cepFmtRegex.IsMatch(cep);
        }

        #endregion

        #region String

        private const string _stringRequiredErrorMessage = "Parâmetro não pode ser vazio.";

        /// <summary>
        /// Valida se a string é não-nula ou vazia
        /// </summary>
        /// <param name="value">Valor do Parâmetro</param>
        /// <param name="parameterName">Nome do parâmetro a ser validado</param>
        /// <exception cref="ArgumentException" ></exception>
        public static void ValidateRequired(this string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException(_stringRequiredErrorMessage, parameterName);
        }

        /// <summary>
        /// Compara duas strings ignorando se as duas são case sensitive
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string a, string b)
        {
            if (a != null) 
                return a.Equals(b, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        #endregion

    }
}
