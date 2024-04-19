using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ApiResponse<T>
    {
        
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
    }
}
