using Serilog;
using Domain.Enum;
using Domain.CustomModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helper
{
    public static class Helper
    {
        private readonly static string key = "b14ca5898a4e4133bbce2ea2315a1916";
        public static string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
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

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
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

        public static void SetSuccessRespose<T>(ResponseObject<T> response)
        {
            response.Success = true;
            response.Message = response.Message == "" ? ResposneCode.Success.ToString() : response.Message;
        }

        public static void SetFailuerRespose<T>(ResponseObject<T> response, Exception exception = null)
        {
            response.Success = false;
            response.Message = response.Message == "" ? ResposneCode.Failuer.ToString() : response.Message;
            if (exception != null) { LogError(exception); }
        }

        private static void LogError(Exception ex)
        {
            string msg = "\n Log Level: Error \n Exception: " + ex.Message + " \n Stack Trace: " + ex.StackTrace +
                " \n ------------------------------------------------------------------------------------------------ \n";
            Log.Information(msg);
        }

        public static string GetUserFromToken(ClaimsPrincipal claimsPrincipal)
        {
            string userNmae = "";
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                userNmae = DecryptString(claimsPrincipal?.Identities.FirstOrDefault()?.Claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Name).Value);
            }
            return userNmae;
        }

        public static string GetUserIdFromToken(ClaimsPrincipal claimsPrincipal)
        {
            string userId = "";
            if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                userId = DecryptString(claimsPrincipal?.Identities.FirstOrDefault()?.Claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Sid).Value);
            }
            return userId;
        }

    }
}
