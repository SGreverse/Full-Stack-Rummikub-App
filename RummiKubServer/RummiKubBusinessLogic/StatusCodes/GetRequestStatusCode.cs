using RummiKubClasses.GamePieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RummiKubClasses.StatusCodes
{
    public class GetRequestStatusCode<T>:PostRequestStatusCode
    {

        public T ReturnedObject {  get; set; }


        public GetRequestStatusCode(int code):base(code)
        { 
        }
        public GetRequestStatusCode():base()
        {
        }

        public GetRequestStatusCode(int Code,T obj):this(Code)
        {
            this.ReturnedObject = obj;
        }

    }
}
