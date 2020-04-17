namespace ckv_lib
{
    public class clsApp
    {
        static bool _inited = false;
        public static void _init()
        {
            if (_inited == false)
            {
                _inited = true;
                clsApi._init();
                clsRouter._init();
                clsJob._init();
                clsChakra._init();
            }
        }
    }
}