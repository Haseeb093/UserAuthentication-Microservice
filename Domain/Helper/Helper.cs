using Serilog;
using Domain.Enum;
using Domain.CustomModels;

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

    }
}
