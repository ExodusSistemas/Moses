using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Moses.Extensions
{
    /// <summary>
    /// Enumerator com os tipos de classes para criptografia.
    /// </summary>
    public enum CryptProvider
    {
        /// <summary>
        /// Representa a classe base para implementações criptografia dos algoritmos simétricos Rijndael.
        /// </summary>
        Rijndael,
        /// <summary>
        /// Representa a classe base para implementações do algoritmo RC2.
        /// </summary>
        RC2,
        /// <summary>
        /// Representa a classe base para criptografia de dados padrões (DES - Data Encryption Standard).
        /// </summary>
        DES,
        /// <summary>
        /// Representa a classe base (TripleDES - Triple Data Encryption Standard).
        /// </summary>
        TripleDES
    }

    public static class SecurityHelper
    {
        public static string GetMd5String(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            return ByteConverter.GetString(result);
        }

        public static string GetMd5String(this string data)
        {
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            return GetMd5String(ByteConverter.GetBytes(data));
        }

        public static string GetMd5Hex(this byte[] data)
        {
            StringBuilder sText = new StringBuilder();
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] result = md5.ComputeHash(data);

                for (int i = 0; i < result.Length; i++)
                {
                    sText.Append(String.Format("{0,2:x}", result[i]).Replace(" ", "0"));
                }
            }

            return sText.ToString();
        }

        public static string GetMd5Hex(this string data)
        {
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            return GetMd5Hex(ByteConverter.GetBytes(data));
        }

        public static string Encrypt(this string data, bool legacy = false)
        {
            Crypt c = new(legacy);
            return c.Encrypt(data);
        }

        public static string Decrypt(this string data, bool legacy = false)
        {
            Crypt c = new(legacy);
            return c.Decrypt(data);
        }

        /// <summary>
        /// Classe auxiliar com métodos para criptografia de dados.
        /// </summary>
        private class Crypt
        {
            #region Private members
            private string _key = string.Empty;
            private readonly CryptProvider _cryptProvider;
            private readonly SymmetricAlgorithm _algorithm;
            private void SetIV()
            {
                _algorithm.IV = _cryptProvider switch
                {
                    CryptProvider.Rijndael => [0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8, 0x4b, 0x73, 0xcc],
                    _ => [0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9],
                };
            }
            #endregion

            #region Properties
            /// <summary>
            /// Chave secreta para o algoritmo simétrico de criptografia.
            /// </summary>
            public string Key
            {
                get { return _key; }
                set { _key = value; }
            }
            #endregion

            #region Constructors
            /// <summary>
            /// Contrutor padrão da classe, é setado um tipo de criptografia padrão.
            /// </summary>
            public Crypt(bool legacy = false)
            {
                if (!legacy)
                {
                    _algorithm = Aes.Create();
                    _algorithm.Mode = CipherMode.CBC;
                    _cryptProvider = CryptProvider.TripleDES;
                }
                else
                {
                    _algorithm = new RijndaelManaged();
                    _algorithm.Mode = CipherMode.CBC;
                    _cryptProvider = CryptProvider.Rijndael;
                }
            }
            
            #endregion

            #region Public methods
            /// <summary>
            /// Gera a chave de criptografia válida dentro do array.
            /// </summary>
            /// <returns>Chave com array de bytes.</returns>
            public virtual byte[] GetKey()
            {
                string salt = string.Empty;

                // Ajuta o tamanho da chave se necessário e retorna uma chave válida
                if (_algorithm.LegalKeySizes.Length > 0)
                {
                    // Tamanho das chaves em bits
                    int keySize = _key.Length * 8;
                    int minSize = _algorithm.LegalKeySizes[0].MinSize;
                    int maxSize = _algorithm.LegalKeySizes[0].MaxSize;
                    int skipSize = _algorithm.LegalKeySizes[0].SkipSize;

                    if (keySize > maxSize)
                    {
                        // Busca o valor máximo da chave
                        _key = _key[..(maxSize / 8)];
                    }
                    else if (keySize < maxSize)
                    {
                        // Seta um tamanho válido
                        int validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                        if (keySize < validSize)
                        {
                            // Preenche a chave com arterisco para corrigir o tamanho
                            _key = _key.PadRight(validSize / 8, '*');
                        }
                    }
                }
                PasswordDeriveBytes key = new(_key, ASCIIEncoding.ASCII.GetBytes(salt));
                return key.GetBytes(_key.Length);
            }
            /// <summary>
            /// Encripta o dado solicitado.
            /// </summary>
            /// <param name="plainText">Texto a ser criptografado.</param>
            /// <returns>Texto criptografado.</returns>
            public virtual string Encrypt(string plainText)
            {
                byte[] plainByte = ASCIIEncoding.UTF8.GetBytes(plainText);
                byte[] keyByte = GetKey();

                // Seta a chave privada
                _algorithm.Key = keyByte;
                SetIV();

                // Interface de criptografia / Cria objeto de criptografia
                ICryptoTransform cryptoTransform = _algorithm.CreateEncryptor();

                MemoryStream _memoryStream = null;
                
                CryptoStream _cryptoStream = null;

                try
                {
                    _memoryStream = new MemoryStream();

                    _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Write);

                    // Grava os dados criptografados no MemoryStream
                    _cryptoStream.Write(plainByte, 0, plainByte.Length);
                    _cryptoStream.FlushFinalBlock();

                    // Busca o tamanho dos bytes encriptados
                    byte[] cryptoByte = _memoryStream.ToArray();

                    // Converte para a base 64 string para uso posterior em um xml
                    return Convert.ToBase64String(cryptoByte, 0, cryptoByte.GetLength(0));
                }
                catch
                {
                    return null;
                }
                finally
                {
                    _cryptoStream?.Close();

                    _memoryStream?.Dispose();
                }
            }

            /// <summary>
            /// Desencripta o dado solicitado.
            /// </summary>
            /// <param name="cryptoText">Texto a ser descriptografado.</param>
            /// <returns>Texto descriptografado.</returns>
            public virtual string Decrypt(string cryptoText)
            {
                // Converte a base 64 string em num array de bytes
                byte[] cryptoByte = Convert.FromBase64String(cryptoText);
                byte[] keyByte = GetKey();

                // Seta a chave privada
                _algorithm.Key = keyByte;
                SetIV();

                // Interface de criptografia / Cria objeto de descriptografia
                ICryptoTransform cryptoTransform = _algorithm.CreateDecryptor();
                 MemoryStream _memoryStream = null;
                                            
                 CryptoStream _cryptoStream = null;
                try
                {
                    _memoryStream = new MemoryStream(cryptoByte, 0, cryptoByte.Length);

                    _cryptoStream = new CryptoStream(_memoryStream, cryptoTransform, CryptoStreamMode.Read);

                    // Busca resultado do CryptoStream
                    StreamReader _streamReader = new(_cryptoStream);
                    return _streamReader.ReadToEnd();
                }
                catch
                {
                    return null;
                }
                finally
                {
                    _cryptoStream.Close();

                    _memoryStream?.Dispose();
                }
            }

            #endregion
        }
    }
}
