using System.Security.Cryptography;
using System.Text;

namespace ApiGestionStock.Helpers
{
    public static class HelperCryptography
    {
        private static IConfiguration configuration;
        private static string keyCifrado;
        public static void Initialize(IConfiguration config)
        {
            configuration = config;
            keyCifrado = configuration.GetValue<string>("ApiOAuth:CryptoKey");
        }

        public static string EncryptString(String dato)
        {
            var saltconf = configuration.GetValue<string>("Crypto:Salt");
            var bucleconf = configuration.GetValue<string>("Crypto:Iterate");
            string password = configuration.GetValue<string>("Crypto:Key");
            byte[] saltpassword = EncriptarPasswordSalt
                (password, saltconf, int.Parse(bucleconf));
            String res = EncryptString(saltpassword, dato);
            return res;
        }

        public static string DecryptString(String dato)
        {
            var saltconf = configuration.GetValue<string>("Crypto:Salt");
            var bucleconf = configuration.GetValue<string>("Crypto:Iterate");
            string password = configuration.GetValue<string>("Crypto:Key");
            byte[] saltpassword = EncriptarPasswordSalt
                (password, saltconf, int.Parse(bucleconf));
            String res = DecryptString(saltpassword, dato);
            return res;
        }

        private static string EncryptString(byte[] key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private static byte[] EncriptarPasswordSalt(string contenido
            , string salt, int numhash)
        {
            //REALIZAMOS LA COMBINACION DE ENCRIPTADO
            //CON SU SALT
            string textocompleto = contenido + salt;
            //DECLARAMOS EL OBJETO SHA256
            //SHA256Managed objsha = new SHA256Managed();
            SHA256 objsha = SHA256.Create();
            byte[] bytesalida = null;

            try
            {
                //CONVERTIMOS EL TEXTO A BYTES
                bytesalida =
                    Encoding.UTF8.GetBytes(textocompleto);
                //Convert.FromBase64String(textocompleto);
                //ENCRIPTAMOS EL TEXTO 1000 VECES
                for (int i = 0; i < numhash; i++)
                    bytesalida = objsha.ComputeHash(bytesalida);
            }
            finally
            {
                objsha.Clear();
            }
            //DEVOLVEMOS LOS BYTES DE SALIDA
            return bytesalida;
        }

        private static string DecryptString(byte[] key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
