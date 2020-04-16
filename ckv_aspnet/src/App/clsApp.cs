using ckv_aspnet.src.Chakra;

namespace ckv_aspnet
{
    public class clsApp
    {
        public const int LOG_PORT = 11111;
        public static string PATH_ROOT = string.Empty;
        static bool _inited = false;
        public static void _init(string path)
        {
            if (_inited == false)
            {
                _inited = true;
                PATH_ROOT = path;
                clsApi._init(path);
                clsRouter._init();
                clsJob._init();
                clsChakra._init();
            }
        }
    }
}