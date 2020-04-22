using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ckv_lib
{
    public class oLinkItem
    {
        public string href { set; get; }
        public string text { set; get; }
        public string html { set; get; }

        public oLinkItem() {
            this.href = string.Empty;
            this.text = string.Empty;
            this.html = string.Empty;
        }

        public override string ToString()
        {
            return href + "\n\t" + text;
        }
    }
}
