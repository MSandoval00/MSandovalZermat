using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Result
    {
        public List<object> Objects { get; set; }
        public object Object { get; set; }
        public bool Correct { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Ex { get; set; }
    }
}
