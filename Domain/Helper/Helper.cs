using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helper
{
    public static class Helper
    {
        public static void SetSuccessRespose<T>(ResponseObject<T> response)
        {
            response.Success = true;
            response.Message = response.Message == "" ? ResposneCode.Failuer.ToString() : response.Message;
        }

        public static void SetFailuerRespose<T>(ResponseObject<T> response, Exception exception = null)
        {
            response.Success = false;
            response.Message = response.Message == "" ? ResposneCode.Failuer.ToString() : response.Message;
        }
    }
}
