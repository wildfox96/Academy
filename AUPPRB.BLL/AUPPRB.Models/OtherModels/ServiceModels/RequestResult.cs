using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Models.OtherModels.ServiceModels
{
    public class RequestResult : IEquatable<RequestResult>
    {
        public int RequestCode { get; set; }
        public string RequestMessage { get; set; }

        public RequestResult(int code, string msg)
        {
            RequestCode = code;
            RequestMessage = msg;
        }
        public static RequestResult Ok
        {
            get { return new RequestResult(0, "Ok"); }
        }


        public bool Equals(RequestResult other)
        {
            return RequestCode == other.RequestCode;
        }
    }
}
