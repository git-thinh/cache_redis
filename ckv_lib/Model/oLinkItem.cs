using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ckv_lib
{
    public class oLinkItem
    {
        public string id { set; get; }
        public string href { set; get; }
        public string text { set; get; }
        public string html { set; get; }

        public oLinkItem() {
            this.id = Guid.NewGuid().ToString();
            this.href = string.Empty;
            this.text = string.Empty;
            this.html = string.Empty;
        }

        public override string ToString()
        {
            return " [A] " + text + " || " + href  + " [/A] ";
        }
    }
}
