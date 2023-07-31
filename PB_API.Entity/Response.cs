using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB_API.Entity
{
    public class Response
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<ResponseData> Data { get; set; }
    }
    public class ResponseData
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
}
