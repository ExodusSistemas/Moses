using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Moses.Extensions
{
    public static class ParseHelper
    {
        #region CPF

        /// <summary>
        /// Prepares a CPF entry to be set into the DataSource
        /// </summary>
        /// <remarks>Assume-se que a sequência de entrada é valida</remarks>
        /// <param name="s"></param>
        public static ulong? ParseCPF(this string s)
        {
            if (s == null) return null;

            string cpfNumber = s.GetNumbers();

            if (cpfNumber.Length == 11)
            {
                return ulong.Parse(cpfNumber);
            }
            else
            {
                throw new MosesFormatException($"Invalid CPF Entry: {s}");
            }

        }

        /// <summary>
        /// Prepara a sequência de entrada para ser armazenada no datasource.
        /// </summary>
        /// <remarks>Assume-se que a sequência de entrada é valida</remarks>
        /// <param name="s"></param>
        public static ulong? ParseCPF(this object s)
        {
            string j = s as string;
            if (j == null) return null;

            return j.ParseCPF();
        }

        #endregion

        #region Search
        /// <summary>
        /// Prepara uma string para ser utilizada em uma busca
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ParseSearch(this string key)
        {
            key = key.Trim().ToLowerInvariant();
            return key.StripAccents();
        }

        public static string StripSlashes(this string s)
        {
            return s.Replace("/", "-");
        }

        public static string StripAccents(this string s)
        {
            string normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string ToBase64String(this string s)
        {
            return Cypher(s);
        }

        public static string FromBase64String(this string s)
        {
            return Decypher(s);
        }

        public static string Cypher(this string s)
        {
            if (s == null) return null;
            string cyphered = Convert.ToBase64String( UTF8Encoding.Default.GetBytes( s ) );

            return cyphered;
        }

        public static string Decypher(this string s)
        {
            if (s == null) return null;
            string decyphered = UTF8Encoding.Default.GetString( Convert.FromBase64String(s) );
            return decyphered;
        }

        #endregion

        #region Conversões Úteis

         
        public static string ToCsvString(this IEnumerable<long> bytes)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.AppendFormat( "{0},",b);
            }

            return builder.ToString(0, builder.Length - 1);
        }

        public static string ToCsvStringOrNull(this IEnumerable<long> bytes)
        {
            if (bytes == null) return null;
            if (bytes.Count() == 0) return null;

            StringBuilder builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.AppendFormat("{0},", b);
            }


            return builder.ToString(0, builder.Length - 1);
        }

        public static void BindFromCsvToLong(this List<long> list, string csvString)
        {
            if (string.IsNullOrEmpty( csvString ) ) return;

            string[] array = csvString.Split(',');
            
            foreach (var a in array)
            {
                string val = a.Trim();
                if (val.IsNullOrEmpty()) continue;
                list.Add( long.Parse( val ) );
            }

        }

        /// <summary>
        /// Converte uma array de bytes para uma string em hexadecimal
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <author>http://www.nathanm.com/csharp-convert-hex-string-tofrom-byte-array-fast/</author>
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            string hexAlphabet = "0123456789ABCDEF";

            foreach (byte b in bytes)
            {
                result.Append(hexAlphabet[(int)(b >> 4)]);
                result.Append(hexAlphabet[(int)(b & 0xF)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Converte uma string em hexadecimal em uma array em bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        /// <author>http://www.nathanm.com/csharp-convert-hex-string-tofrom-byte-array-fast/</author>
        public static byte[] HexStringToByteArray(this string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            int[] hexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
                                 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D,
                                 0x0E, 0x0F };

            for (int x = 0, i = 0; i < hex.Length; i += 2, x += 1)
            {
                bytes[x] = (byte)(hexValue[Char.ToUpper(hex[i + 0]) - '0'] << 4 |
                                  hexValue[Char.ToUpper(hex[i + 1]) - '0']);
            }

            return bytes;
        }

        public static string GetPhoneAreaCode(this string phone)
        {
            return phone.GetNumbers().Substring(0, 2);
        }

        public static string GetPhoneNumber(this string phone)
        {
            return phone.GetNumbers().Substring(2);
        }

        #endregion

        #region Remove Accents

        public static bool Vazia(this String strCorrente)
        {
            return String.IsNullOrEmpty(strCorrente) || (String.IsNullOrWhiteSpace(strCorrente));

        }

        public static String Igualar(this String strCorrente)
        {
            return strCorrente.Trim().RemoveAccents().ToUpper();
        }

        /// <summary>
        /// Remove a acentuação de uma String
        /// </summary>
        /// <param name="text"></param>
        /// <returns>String</returns>
        public static string RemoveAccents(this string text)
        {
            if (text != null)
            {
                StringBuilder sbReturn = new StringBuilder();
                var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();

                foreach (char letter in arrayText)
                {
                    if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                        sbReturn.Append(letter);
                }
                return sbReturn.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// Verifica se um objeto do banco é igual ao objeto informado pelo usuário.Ignorando acentuação letras maiúsculas ou minúsculas
        /// </summary>
        /// <param name="objeto">Objeto do banco</param>
        /// <param name="strComparar">Valor informado pelo usuário</param>
        /// <param name="removerAcentos">Valor que define se serão removidas as acentuações</param>
        /// <returns>Booleano</returns>
        public static bool CompareInsensitive(object objeto, string strComparar, bool removerAcentos = false)
        {
            if (!strComparar.Vazia())
            {
                if (objeto != null && Convert.ToString(objeto).Vazia())
                    return false;

                if (objeto == null)
                    return false;

                strComparar = strComparar.Igualar();

                return removerAcentos
                    ? (objeto ?? strComparar).ToString().Igualar().Contains(strComparar)
                    : (objeto ?? strComparar).ToString().Trim().ToUpper().Contains(strComparar);

            }

            return false;
        }

        #endregion
    }
}
