namespace ckv_aspnet
{
    public class oRouterResult
    {
        public bool ok { set; get; }
        public string data { set; get; }

        public oRouterResult()
        {
            this.ok = false;
            this.data = string.Empty;
        }

        public static oRouterResult Ok(string data = "")
        {
            return new oRouterResult() { ok = true, data = data };
        }

        public static oRouterResult Error(string message = "")
        {
            return new oRouterResult() { ok = false, data = message };
        }
    }
}