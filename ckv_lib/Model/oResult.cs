using System.Collections.Generic;

namespace ckv_lib
{
    public enum DATA_TYPE {
        TEXT_PLAIN = 0,
        JSON_TEXT = 1,
        JSON_RESPONSE = 2,
        HTML_TEXT = 3,
        ARRAY_LIST = 4,
        OBJECT = 5,
        BUFFER = 6
    }

    public class oResult
    {
        public bool ok { set; get; }
        public object data { set; get; }
        public string name { set; get; }
        public string error { set; get; }
        public Dictionary<string, object> request { set; get; }
        public DATA_TYPE type { set; get; }

        public oResult()
        {
            this.ok = false;
            this.data = null;
            this.name = string.Empty;
            this.error = string.Empty;
            this.type = DATA_TYPE.JSON_TEXT;
            this.request = new Dictionary<string, object>();
        }

        public static oResult Ok(object data = null, Dictionary<string, object> request = null, DATA_TYPE type = DATA_TYPE.JSON_TEXT)
        {
            if (request == null) request = new Dictionary<string, object>();
            return new oResult() { ok = true, data = data, type = type, request = request };
        }

        public static oResult Error(string message = "", Dictionary<string, object> request = null)
        {
            if (request == null) request = new Dictionary<string, object>();
            return new oResult() { ok = false, error = message, type = DATA_TYPE.JSON_TEXT };
        }
    }
}