using System;
using System.Collections.Generic;
using System.Text;
using TeamDevRedis;
using System.Linq;

namespace studio_redis
{
    public class oKey
    {
        public string key_full { set; get; }
        public string key0 { set; get; }
        public string key1 { set; get; }
        public string key2 { set; get; }
        public string key3 { set; get; }
        public int level { set; get; }

        public string[] keys_last { set; get; }

        public oKey() { }

        public oKey(RedisDataAccessProvider redis, string s)
        {
            string[] a = s.Split(new char[] { '#', '.' });

            this.key_full = s;
            this.key0 = a[0];
            this.level = 0;

            if (a.Length > 1)
            {
                this.key1 = a[1];
                this.level = 1;
            }

            if (a.Length > 2)
            {
                this.key2 = a[2];
                this.level = 2;
            }

            if (a.Length > 3)
            {
                this.key3 = a[3];
                this.level = 3;
            }

            try
            {
                string[] ks = redis.ReadMultiString(redis.SendCommand(RedisCommand.HKEYS, s));
                this.keys_last = ks.OrderBy(x => x).ToArray();
            }
            catch (Exception e)
            {

            }
        }

        public string getPath()
        {
            string s = "/ ";
            if (key0 != null) s += key0 + " / ";
            if (key1 != null) s += key1 + " / ";
            if (key2 != null) s += key2 + " / ";
            return s;
        }

        public oKey[] findKeys(oKey[] keys)
        {
            Func<oKey, bool> where = null;
            switch (level)
            {
                case 0:
                    where = x => x.key0 == key0 && x.level == 0;
                    break;
                case 1:
                    where = x => x.key0 == key0 && x.key1 == key1 && x.level == 1;
                    break;
                case 2:
                    where = x => x.key0 == key0 && x.key1 == key1 && x.key2 == key2 && x.level == 2;
                    break;
            }

            if (where != null)
            {
                var a = keys.Where(where).ToArray();
                return a;
            }
            return new oKey[] { };
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.key_full, string.Join(", ", this.keys_last));
        }
    }
}
