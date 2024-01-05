using Serilog;
using Domain.Enum;
using Domain.CustomModels;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Domain.Helper
{
    public static class Helper
    {
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
                userNmae = claimsPrincipal?.Identities.FirstOrDefault()?.Claims.FirstOrDefault(s => s.Type == JwtRegisteredClaimNames.Name).Value;
            }
            return userNmae;
        }

    }
}
