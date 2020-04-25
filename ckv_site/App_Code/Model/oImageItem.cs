using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ckv_site
{
    public class oImageItem
    {
        public Dictionary<string,string> attrs { set; get; }
        public string html { set; get; }
        public string id { set; get; }

        public oImageItem()
        {
            this.id = Guid.NewGuid().ToString();
            this.html = string.Empty;
            this.attrs = new Dictionary<string, string>();
        }

    }
}
