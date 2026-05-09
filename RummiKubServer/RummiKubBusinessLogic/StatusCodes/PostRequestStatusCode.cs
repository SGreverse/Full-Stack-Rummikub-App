using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.StatusCodes
{
    public class PostRequestStatusCode
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public PostRequestStatusCode(int code)
        {
            this.Code = code;
            this.Message = StatusMessages.GetMessage(code);
        }
        public PostRequestStatusCode()
        {
            this.Code = 0;
            this.Message = StatusMessages.GetMessage(0);
        }
    }
}
