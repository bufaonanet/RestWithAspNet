using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestWithAspNet.Hypermedia
{
    public class HyperMediaLink
    {
        private string _href;
        public string Href
        {
            get
            {
                object _lock = new object();
                lock (_lock)
                {
                    StringBuilder sb = new StringBuilder(_href);
                    return sb.Replace("%2F", "/").ToString();
                }
            }
            set => _href = value;
        }
        public string Rel { get; set; }
        public string Type { get; set; }
        public string Action { get; set; }
    }
}
