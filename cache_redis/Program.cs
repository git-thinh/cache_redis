using ChakraHost.Hosting;
using Fleck2;
using Fleck2.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace cache_redis
{
    class Program
    {
        static readonly string ROOT_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static readonly string ROOT_PATH_UI = Path.Combine(ROOT_PATH, "ui");

        static oConfig m_config;
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(Program).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }

        #region [ MEMORY CACHE ]

        static bool b_01 = false;
        static bool b_02 = false;
        static bool b_03 = false;
        static bool b_04 = false;
        static bool b_05 = false;
        static bool b_06 = false;
        static bool b_07 = false;
        static bool b_08 = false;
        static bool b_09 = false;
        static bool b_10 = false;
        static bool b_11 = false;
        static bool b_12 = false;
        static bool b_13 = false;
        static bool b_14 = false;
        static bool b_15 = false;
        static bool b_16 = false;
        static bool b_17 = false;
        static bool b_18 = false;
        static bool b_19 = false;
        static bool b_20 = false;
        static bool b_21 = false;
        static bool b_22 = false;
        static bool b_23 = false;
        static bool b_24 = false;
        static bool b_25 = false;
        static bool b_26 = false;
        static bool b_27 = false;
        static bool b_28 = false;
        static bool b_29 = false;
        static bool b_30 = false;
        static bool b_31 = false;
        static bool b_32 = false;
        static bool b_33 = false;
        static bool b_34 = false;
        static bool b_35 = false;
        static bool b_36 = false;
        static bool b_37 = false;
        static bool b_38 = false;
        static bool b_39 = false;
        static bool b_40 = false;
        static bool b_41 = false;
        static bool b_42 = false;
        static bool b_43 = false;
        static bool b_44 = false;
        static bool b_45 = false;
        static bool b_46 = false;
        static bool b_47 = false;
        static bool b_48 = false;
        static bool b_49 = false;
        static bool b_50 = false;
        static bool b_51 = false;
        static bool b_52 = false;
        static bool b_53 = false;
        static bool b_54 = false;
        static bool b_55 = false;
        static bool b_56 = false;
        static bool b_57 = false;
        static bool b_58 = false;
        static bool b_59 = false;
        static bool b_60 = false;
        static bool b_61 = false;
        static bool b_62 = false;
        static bool b_63 = false;
        static bool b_64 = false;
        static bool b_65 = false;
        static bool b_66 = false;
        static bool b_67 = false;
        static bool b_68 = false;
        static bool b_69 = false;
        static bool b_70 = false;
        static bool b_71 = false;
        static bool b_72 = false;
        static bool b_73 = false;
        static bool b_74 = false;
        static bool b_75 = false;
        static bool b_76 = false;
        static bool b_77 = false;
        static bool b_78 = false;
        static bool b_79 = false;
        static bool b_80 = false;
        static bool b_81 = false;
        static bool b_82 = false;
        static bool b_83 = false;
        static bool b_84 = false;
        static bool b_85 = false;
        static bool b_86 = false;
        static bool b_87 = false;
        static bool b_88 = false;
        static bool b_89 = false;
        static bool b_90 = false;
        static bool b_91 = false;
        static bool b_92 = false;
        static bool b_93 = false;
        static bool b_94 = false;
        static bool b_95 = false;
        static bool b_96 = false;
        static bool b_97 = false;
        static bool b_98 = false;
        static bool b_99 = false;

        static ConcurrentDictionary<string, string> m_01 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_02 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_03 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_04 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_05 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_06 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_07 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_08 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_09 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_10 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_11 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_12 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_13 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_14 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_15 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_16 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_17 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_18 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_19 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_20 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_21 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_22 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_23 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_24 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_25 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_26 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_27 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_28 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_29 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_30 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_31 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_32 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_33 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_34 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_35 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_36 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_37 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_38 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_39 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_40 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_41 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_42 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_43 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_44 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_45 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_46 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_47 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_48 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_49 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_50 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_51 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_52 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_53 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_54 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_55 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_56 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_57 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_58 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_59 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_60 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_61 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_62 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_63 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_64 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_65 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_66 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_67 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_68 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_69 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_70 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_71 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_72 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_73 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_74 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_75 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_76 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_77 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_78 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_79 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_80 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_81 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_82 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_83 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_84 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_85 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_86 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_87 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_88 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_89 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_90 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_91 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_92 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_93 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_94 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_95 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_96 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_97 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_98 = new ConcurrentDictionary<string, string>();
        static ConcurrentDictionary<string, string> m_99 = new ConcurrentDictionary<string, string>();

        static readonly string[] m_names = new string[99];

        static bool m___check(string cache_name)
        {
            int index = -1;
            if (string.IsNullOrEmpty(cache_name)) return false;

            for (int i = 0; i < m_names.Length; i++) if (m_names[i] == cache_name) { index = i; break; }

            return index != -1;
        }

        static ConcurrentDictionary<string, string> m___get(string cache_name)
        {
            int index = -1;
            if (string.IsNullOrEmpty(cache_name)) return null;

            for (int i = 0; i < m_names.Length; i++) if (m_names[i] == cache_name) { index = i; break; }
            if (index == -1) return null;

            switch (index)
            {
                case 1: return m_01;
                case 2: return m_02;
                case 3: return m_03;
                case 4: return m_04;
                case 5: return m_05;
                case 6: return m_06;
                case 7: return m_07;
                case 8: return m_08;
                case 9: return m_09;
                case 10: return m_10;
                case 11: return m_11;
                case 12: return m_12;
                case 13: return m_13;
                case 14: return m_14;
                case 15: return m_15;
                case 16: return m_16;
                case 17: return m_17;
                case 18: return m_18;
                case 19: return m_19;
                case 20: return m_20;
                case 21: return m_21;
                case 22: return m_22;
                case 23: return m_23;
                case 24: return m_24;
                case 25: return m_25;
                case 26: return m_26;
                case 27: return m_27;
                case 28: return m_28;
                case 29: return m_29;
                case 30: return m_30;
                case 31: return m_31;
                case 32: return m_32;
                case 33: return m_33;
                case 34: return m_34;
                case 35: return m_35;
                case 36: return m_36;
                case 37: return m_37;
                case 38: return m_38;
                case 39: return m_39;
                case 40: return m_40;
                case 41: return m_41;
                case 42: return m_42;
                case 43: return m_43;
                case 44: return m_44;
                case 45: return m_45;
                case 46: return m_46;
                case 47: return m_47;
                case 48: return m_48;
                case 49: return m_49;
                case 50: return m_50;
                case 51: return m_51;
                case 52: return m_52;
                case 53: return m_53;
                case 54: return m_54;
                case 55: return m_55;
                case 56: return m_56;
                case 57: return m_57;
                case 58: return m_58;
                case 59: return m_59;
                case 60: return m_60;
                case 61: return m_61;
                case 62: return m_62;
                case 63: return m_63;
                case 64: return m_64;
                case 65: return m_65;
                case 66: return m_66;
                case 67: return m_67;
                case 68: return m_68;
                case 69: return m_69;
                case 70: return m_70;
                case 71: return m_71;
                case 72: return m_72;
                case 73: return m_73;
                case 74: return m_74;
                case 75: return m_75;
                case 76: return m_76;
                case 77: return m_77;
                case 78: return m_78;
                case 79: return m_79;
                case 80: return m_80;
                case 81: return m_81;
                case 82: return m_82;
                case 83: return m_83;
                case 84: return m_84;
                case 85: return m_85;
                case 86: return m_86;
                case 87: return m_87;
                case 88: return m_88;
                case 89: return m_89;
                case 90: return m_90;
                case 91: return m_91;
                case 92: return m_92;
                case 93: return m_93;
                case 94: return m_94;
                case 95: return m_95;
                case 96: return m_96;
                case 97: return m_97;
                case 98: return m_98;
                case 99: return m_99;
            }

            return null;
        }

        static void m___free_memory()
        {
            m_01.Clear();
            m_02.Clear();
            m_03.Clear();
            m_04.Clear();
            m_05.Clear();
            m_06.Clear();
            m_07.Clear();
            m_08.Clear();
            m_09.Clear();
            m_10.Clear();
            m_11.Clear();
            m_12.Clear();
            m_13.Clear();
            m_14.Clear();
            m_15.Clear();
            m_16.Clear();
            m_17.Clear();
            m_18.Clear();
            m_19.Clear();
            m_20.Clear();
            m_21.Clear();
            m_22.Clear();
            m_23.Clear();
            m_24.Clear();
            m_25.Clear();
            m_26.Clear();
            m_27.Clear();
            m_28.Clear();
            m_29.Clear();
            m_30.Clear();
            m_31.Clear();
            m_32.Clear();
            m_33.Clear();
            m_34.Clear();
            m_35.Clear();
            m_36.Clear();
            m_37.Clear();
            m_38.Clear();
            m_39.Clear();
            m_40.Clear();
            m_41.Clear();
            m_42.Clear();
            m_43.Clear();
            m_44.Clear();
            m_45.Clear();
            m_46.Clear();
            m_47.Clear();
            m_48.Clear();
            m_49.Clear();
            m_50.Clear();
            m_51.Clear();
            m_52.Clear();
            m_53.Clear();
            m_54.Clear();
            m_55.Clear();
            m_56.Clear();
            m_57.Clear();
            m_58.Clear();
            m_59.Clear();
            m_60.Clear();
            m_61.Clear();
            m_62.Clear();
            m_63.Clear();
            m_64.Clear();
            m_65.Clear();
            m_66.Clear();
            m_67.Clear();
            m_68.Clear();
            m_69.Clear();
            m_70.Clear();
            m_71.Clear();
            m_72.Clear();
            m_73.Clear();
            m_74.Clear();
            m_75.Clear();
            m_76.Clear();
            m_77.Clear();
            m_78.Clear();
            m_79.Clear();
            m_80.Clear();
            m_81.Clear();
            m_82.Clear();
            m_83.Clear();
            m_84.Clear();
            m_85.Clear();
            m_86.Clear();
            m_87.Clear();
            m_88.Clear();
            m_89.Clear();
            m_90.Clear();
            m_91.Clear();
            m_92.Clear();
            m_93.Clear();
            m_94.Clear();
            m_95.Clear();
            m_96.Clear();
            m_97.Clear();
            m_98.Clear();
            m_99.Clear();
        }

        static bool busy___get(string cache_name)
        {
            int index = -1;
            if (string.IsNullOrEmpty(cache_name)) return true;

            for (int i = 0; i < 100; i++) if (m_names[i] == cache_name) { index = i; break; }
            if (index == -1) return true;

            switch (index)
            {
                case 1: return b_01;
                case 2: return b_02;
                case 3: return b_03;
                case 4: return b_04;
                case 5: return b_05;
                case 6: return b_06;
                case 7: return b_07;
                case 8: return b_08;
                case 9: return b_09;
                case 10: return b_10;
                case 11: return b_11;
                case 12: return b_12;
                case 13: return b_13;
                case 14: return b_14;
                case 15: return b_15;
                case 16: return b_16;
                case 17: return b_17;
                case 18: return b_18;
                case 19: return b_19;
                case 20: return b_20;
                case 21: return b_21;
                case 22: return b_22;
                case 23: return b_23;
                case 24: return b_24;
                case 25: return b_25;
                case 26: return b_26;
                case 27: return b_27;
                case 28: return b_28;
                case 29: return b_29;
                case 30: return b_30;
                case 31: return b_31;
                case 32: return b_32;
                case 33: return b_33;
                case 34: return b_34;
                case 35: return b_35;
                case 36: return b_36;
                case 37: return b_37;
                case 38: return b_38;
                case 39: return b_39;
                case 40: return b_40;
                case 41: return b_41;
                case 42: return b_42;
                case 43: return b_43;
                case 44: return b_44;
                case 45: return b_45;
                case 46: return b_46;
                case 47: return b_47;
                case 48: return b_48;
                case 49: return b_49;
                case 50: return b_50;
                case 51: return b_51;
                case 52: return b_52;
                case 53: return b_53;
                case 54: return b_54;
                case 55: return b_55;
                case 56: return b_56;
                case 57: return b_57;
                case 58: return b_58;
                case 59: return b_59;
                case 60: return b_60;
                case 61: return b_61;
                case 62: return b_62;
                case 63: return b_63;
                case 64: return b_64;
                case 65: return b_65;
                case 66: return b_66;
                case 67: return b_67;
                case 68: return b_68;
                case 69: return b_69;
                case 70: return b_70;
                case 71: return b_71;
                case 72: return b_72;
                case 73: return b_73;
                case 74: return b_74;
                case 75: return b_75;
                case 76: return b_76;
                case 77: return b_77;
                case 78: return b_78;
                case 79: return b_79;
                case 80: return b_80;
                case 81: return b_81;
                case 82: return b_82;
                case 83: return b_83;
                case 84: return b_84;
                case 85: return b_85;
                case 86: return b_86;
                case 87: return b_87;
                case 88: return b_88;
                case 89: return b_89;
                case 90: return b_90;
                case 91: return b_91;
                case 92: return b_92;
                case 93: return b_93;
                case 94: return b_94;
                case 95: return b_95;
                case 96: return b_96;
                case 97: return b_97;
                case 98: return b_98;
                case 99: return b_99;
            }

            return true;
        }

        static void busy___set(string cache_name, bool busy_ = true)
        {
            int index = -1;
            if (string.IsNullOrEmpty(cache_name)) return;

            for (int i = 0; i < 100; i++) if (m_names[i] == cache_name) { index = i; break; }
            if (index == -1) return;

            switch (index)
            {
                case 1: b_01 = busy_; break;
                case 2: b_02 = busy_; break;
                case 3: b_03 = busy_; break;
                case 4: b_04 = busy_; break;
                case 5: b_05 = busy_; break;
                case 6: b_06 = busy_; break;
                case 7: b_07 = busy_; break;
                case 8: b_08 = busy_; break;
                case 9: b_09 = busy_; break;
                case 10: b_10 = busy_; break;
                case 11: b_11 = busy_; break;
                case 12: b_12 = busy_; break;
                case 13: b_13 = busy_; break;
                case 14: b_14 = busy_; break;
                case 15: b_15 = busy_; break;
                case 16: b_16 = busy_; break;
                case 17: b_17 = busy_; break;
                case 18: b_18 = busy_; break;
                case 19: b_19 = busy_; break;
                case 20: b_20 = busy_; break;
                case 21: b_21 = busy_; break;
                case 22: b_22 = busy_; break;
                case 23: b_23 = busy_; break;
                case 24: b_24 = busy_; break;
                case 25: b_25 = busy_; break;
                case 26: b_26 = busy_; break;
                case 27: b_27 = busy_; break;
                case 28: b_28 = busy_; break;
                case 29: b_29 = busy_; break;
                case 30: b_30 = busy_; break;
                case 31: b_31 = busy_; break;
                case 32: b_32 = busy_; break;
                case 33: b_33 = busy_; break;
                case 34: b_34 = busy_; break;
                case 35: b_35 = busy_; break;
                case 36: b_36 = busy_; break;
                case 37: b_37 = busy_; break;
                case 38: b_38 = busy_; break;
                case 39: b_39 = busy_; break;
                case 40: b_40 = busy_; break;
                case 41: b_41 = busy_; break;
                case 42: b_42 = busy_; break;
                case 43: b_43 = busy_; break;
                case 44: b_44 = busy_; break;
                case 45: b_45 = busy_; break;
                case 46: b_46 = busy_; break;
                case 47: b_47 = busy_; break;
                case 48: b_48 = busy_; break;
                case 49: b_49 = busy_; break;
                case 50: b_50 = busy_; break;
                case 51: b_51 = busy_; break;
                case 52: b_52 = busy_; break;
                case 53: b_53 = busy_; break;
                case 54: b_54 = busy_; break;
                case 55: b_55 = busy_; break;
                case 56: b_56 = busy_; break;
                case 57: b_57 = busy_; break;
                case 58: b_58 = busy_; break;
                case 59: b_59 = busy_; break;
                case 60: b_60 = busy_; break;
                case 61: b_61 = busy_; break;
                case 62: b_62 = busy_; break;
                case 63: b_63 = busy_; break;
                case 64: b_64 = busy_; break;
                case 65: b_65 = busy_; break;
                case 66: b_66 = busy_; break;
                case 67: b_67 = busy_; break;
                case 68: b_68 = busy_; break;
                case 69: b_69 = busy_; break;
                case 70: b_70 = busy_; break;
                case 71: b_71 = busy_; break;
                case 72: b_72 = busy_; break;
                case 73: b_73 = busy_; break;
                case 74: b_74 = busy_; break;
                case 75: b_75 = busy_; break;
                case 76: b_76 = busy_; break;
                case 77: b_77 = busy_; break;
                case 78: b_78 = busy_; break;
                case 79: b_79 = busy_; break;
                case 80: b_80 = busy_; break;
                case 81: b_81 = busy_; break;
                case 82: b_82 = busy_; break;
                case 83: b_83 = busy_; break;
                case 84: b_84 = busy_; break;
                case 85: b_85 = busy_; break;
                case 86: b_86 = busy_; break;
                case 87: b_87 = busy_; break;
                case 88: b_88 = busy_; break;
                case 89: b_89 = busy_; break;
                case 90: b_90 = busy_; break;
                case 91: b_91 = busy_; break;
                case 92: b_92 = busy_; break;
                case 93: b_93 = busy_; break;
                case 94: b_94 = busy_; break;
                case 95: b_95 = busy_; break;
                case 96: b_96 = busy_; break;
                case 97: b_97 = busy_; break;
                case 98: b_98 = busy_; break;
                case 99: b_99 = busy_; break;
            }
        }

        #endregion

        #region [ JS SEARCH - INDEX ]

        static JavaScriptRuntime js_runtime;
        static JavaScriptContext js_context;
        static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        static bool js___connected = false;
        static string js___libs_text = string.Empty;

        static void js___init()
        {
            try
            {
                js___connected = true;

                Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out js_runtime);
                Native.JsCreateContext(js_runtime, out js_context);
                Native.JsSetCurrentContext(js_context);

                if (File.Exists("lib.js")) js___libs_text = File.ReadAllText("lib.js");
                if (js___libs_text.Length > 0)
                    Native.JsRunScript(js___libs_text, currentSourceContext++, "", out JavaScriptValue r1);
            }
            catch { }
        }

        static string js___index(string cache_name, string json)
        {
            try
            {
                if (!js___connected) js___init();
                using (new JavaScriptContext.Scope(js_context))
                {
                    JavaScriptValue result;
                    result = JavaScriptContext.RunScript("(()=>{ var o = " + json + "; \r\n return ___index(\'" + cache_name + "\', o); })()", currentSourceContext++, string.Empty);
                    string v = result.ConvertToString().ToString();
                    return v;
                }
            }
            catch (Exception e)
            {
            }
            return null;
        }

        static oSearchResult js___search(string cache_name, string fn_conditions)
        {
            List<long> ls = new List<long>() { };
            List<string> errs = new List<string>() { };
            try
            {
                if (!js___connected) js___init();

                var m_cache = m___get(cache_name);
                var redis = m_redis[cache_name];

                if (m_cache.Count > 0)
                {
                    string[] a = redis.Keys;

                    if (string.IsNullOrEmpty(fn_conditions)) fn_conditions = " return true; ";
                    else fn_conditions = " return " + fn_conditions;

                    string f1, sf1;

                    using (new JavaScriptContext.Scope(js_context))
                    {
                        f1 = "___" + Guid.NewGuid().ToString().Replace('-', '_');
                        sf1 = @" ___fn." + f1 + @" = function(o){ try { " + fn_conditions + @" }catch(e){ return { ok: false, code: 1585035351111, id: o.id, message: e.message }; } };";
                        JavaScriptContext.RunScript(sf1, currentSourceContext++, string.Empty);
                        string item = string.Empty;
                        for (var i = 0; i < a.Length; i++)
                        {
                            item = m_cache[a[i]];

                            try
                            {
                                string js_exe =
@"(()=>{ 
    try { 
        var o = " + item + @"; 
        var ok = ___fn." + f1 + @"(o); 
        if(ok == true) 
            return o.id; 
        else if(ok == false) 
            return -1; 
        else 
            return JSON.stringify(ok); 
    } catch(e) { 
        return JSON.stringify({ ok: false, code: 1585035452039, id: o.id, message: e.message }); 
    } 
})()";
                                var result = JavaScriptContext.RunScript(js_exe, currentSourceContext++, string.Empty);
                                string v = result.ConvertToString().ToString();
                                if (v.Length > 20)
                                    errs.Add(v);
                                else
                                {
                                    long id = -1;
                                    long.TryParse(v, out id);
                                    if (id != -1)
                                        ls.Add(id);
                                }
                            }
                            catch (Exception e)
                            {
                                errs.Add(oError.getJson(e.Message, 60189311));
                            }
                        }

                        JavaScriptContext.RunScript(" delete ___fn." + f1, currentSourceContext++, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                errs.Add(oError.getJson(ex.Message, 60189399));
            }

            return new oSearchResult() { Keys = ls.ToArray(), Errors = errs.ToArray() };
        }

        static void js___free_memory()
        {
            try
            {
                js___connected = false;
                // Dispose runtime
                Native.JsSetCurrentContext(JavaScriptContext.Invalid);
                Native.JsDisposeRuntime(js_runtime);
            }
            catch { }
        }

        static string js___reset()
        {
            try
            {
                js___free_memory();
                js___init();

                using (new JavaScriptContext.Scope(js_context))
                {
                    JavaScriptValue result;
                    result = JavaScriptContext.RunScript("(()=>{ return ___ping(); })()", currentSourceContext++, "");
                    string v = result.ConvertToString().ToString();
                    return v;
                }
            }
            catch (Exception e)
            {
                js___connected = false;
                return e.Message;
            }
        }

        #endregion

        #region [ SCHEMA ]

        static ConcurrentDictionary<string, Dictionary<string, object>> m_schema
            = new ConcurrentDictionary<string, Dictionary<string, object>>() { };
        static void schema___reset()
        {
            m_schema.Clear();
            if (Directory.Exists("schema"))
            {
                var fs = Directory.GetFiles("schema", "*.json");
                foreach (var f in fs)
                {
                    try
                    {
                        string name = Path.GetFileName(f);
                        name = name.Substring(0, name.Length - 5).ToUpper();
                        string json = File.ReadAllText(f);
                        var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                        m_schema.TryAdd(name, dic);
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region [ VALID ]

        static ConcurrentDictionary<string, string> m_valid_add
            = new ConcurrentDictionary<string, string>() { };
        static string m_valid_text_js = string.Empty;

        static void valid__reset()
        {
            m_valid_add.Clear();

            if (File.Exists("valid.js")) {
                string js = File.ReadAllText("valid.js");
                int pos = js.IndexOf("/*[START]*/");
                if (pos != -1) js = js.Substring(pos, js.Length - pos);
                m_valid_text_js = Environment.NewLine + js;
            }

            if (Directory.Exists("valid_add"))
            {
                var fs = Directory.GetFiles("valid_add", "*.json");
                foreach (var f in fs)
                {
                    try
                    {
                        string name = Path.GetFileName(f);
                        name = name.Substring(0, name.Length - 5).ToUpper();
                        string s = File.ReadAllText(f);
                        m_valid_add.TryAdd(name, s);
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region [ API BASE ]

        static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (Directory.Exists(path) == false) return new string[] { };

            if (string.IsNullOrEmpty(searchPattern) || searchPattern == "*.*")
                return Directory.GetFiles(path, "*.*", searchOption).Select(x => x.ToLower()).ToArray();

            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(Directory.GetFiles(path, sp, searchOption).Select(x => x.ToLower()));
            files.Sort();
            return files.ToArray();
        }

        static Stream api___stream_string(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void api___response_stream(string _extension, Stream input, HttpListenerContext context)
        {
            try
            {
                //Stream input = new FileStream(filename, FileMode.Open);
                //Stream input = api___stream_string(data);

                //Adding permanent http response headers
                string contentType = HTTPServerUI.GetContentType(_extension);
                context.Response.ContentType = contentType;
                context.Response.ContentLength64 = input.Length;
                //context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                //context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));

                byte[] buffer = new byte[1024 * 16];
                int nbytes;
                while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    context.Response.OutputStream.Write(buffer, 0, nbytes);
                input.Close();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch (Exception ex)
            {
                context.Response.StatusDescription = ex.Message;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }

        static void api___response_json_error(HttpListenerContext context, string message, int code = 0)
        {
            try
            {
                api___response_json_text_raw(context, oError.getJson(message, code));
                api___close(context);
            }
            catch { }
        }

        static void api___response_json_text_raw(HttpListenerContext context, string json)
        {
            try { api___response_stream(".json", api___stream_string(json), context); } catch { }
        }

        static void api___response_json_text_body(HttpListenerContext context, bool ok, string json = "null"
            , int total = 0, int count = 0, int page_number = 0, int page_size = 0)
        {
            try { api___response_stream(".json", api___stream_string(@"{""ok"":" + (ok ? "true" : "false") + @",""total"":" + total + @",""count"":" + count + @",""page_number"":" + page_number + @",""page_size"":" + page_size + @",""data"":" + json + "}"), context); } catch { }
        }

        #endregion

        #region [ API PROCESS ]

        static void api___close(HttpListenerContext context) { try { context.Response.OutputStream.Close(); } catch { } }

        static bool api___redis_check_ready(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api.Length == 0)
            {
                api___response_json_error(context, "Uri must be / " + (a.Length > 0 ? a[1] : "[ACTION_NAME]") + "/[CACHE_NAME]");
                return false;
            }

            if (m___check(api) == false)
            {
                api___response_json_error(context, "Api: " + api + " is not exist");
                return false;
            }

            if (m_redis.ContainsKey(api) == false)
            {
                api___response_json_error(context, "Cache engine " + api + " not exist");
                return false;
            }

            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            if (cf == null)
            {
                api___response_json_error(context, "Config " + api + " not exist");
                return false;
            }

            if (m_config.db_connect == null || m_config.db_connect.Count == 0 || m_config.db_connect.ContainsKey(cf.scope) == false)
            {
                api___response_json_error(context, "db_connect not contain connectString " + cf.scope);
                return false;
            }

            if (busy___get(cf.name))
            {
                api___response_json_error(context, "Cache Engine " + cf.name + " is busy");
                return false;
            }

            return true;
        }

        static int ___id = 100;
        
        static long api___get_uuid()
        {
            if (___id > 999) ___id = 100;
            return long.Parse(DateTime.Now.ToString("yyMMddHHmmss") + ___id.ToString());
        }

        static bool api___get_uuid(HttpListenerContext context = null)
        {
            string[] a = context.Request.Url.Segments;
            string id = a.Length > 2 ? a[2].ToUpper() : "";
            int k = 0;
            int.TryParse(id, out k);
            if (k < 1)
                api___response_json_error(context, "Url must be /uuid/[1..]");
            else
            {
                if (k > 999) k = 999;
                List<long> ids = new List<long>() { };
                string time = DateTime.Now.ToString("yyMMddHHmmss");
                for (int i = 0; i < k; i++)
                {
                    if (___id > 999)
                    {
                        ___id = 100;
                        time = DateTime.Now.ToString("yyMMddHHmmss");
                    }
                    ids.Add(long.Parse(time + ___id));
                    //___id++;
                    Interlocked.Increment(ref ___id);
                }
                api___response_json_text_body(context, true, JsonConvert.SerializeObject(ids), ids.Count, ids.Count);
            }
            api___close(context);
            return true;
        }

        static bool api___redis_reset(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            api___response_json_text_body(context, true);
            api___close(context);
            return true;
        }

        static bool api___ram_search(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;

            string fn_conditions = "";
            string fn_map = "";
            if (context.Request.HttpMethod == "GET")
            {
                string url = context.Request.Url.ToString();
                if (url.Contains("fn_conditions"))
                    fn_conditions = url.Split(new string[] { "fn_conditions" }, StringSplitOptions.None)[1].Trim();
                if (fn_conditions.Length > 0 && fn_conditions[0] == '=')
                    fn_conditions = fn_conditions.Substring(1).Trim();

                fn_map = context.Request.QueryString["fn_map"];
            }

            var m = js___search(api, fn_conditions);

            long[] ids = m.Keys;
            string err = string.Join(Environment.NewLine, m.Errors);
            if (string.IsNullOrEmpty(err))
            {
                if (ids.Length > 0)
                {
                    string s_page_number = context.Request.QueryString["page_number"];
                    string s_page_size = context.Request.QueryString["page_size"];
                    int page_number = 1, page_size = 15;
                    int.TryParse(s_page_number, out page_number);
                    int.TryParse(s_page_size, out page_size);
                    if (page_number < 1) page_number = 1;
                    if (page_size < 1) page_size = 15;

                    int min = (page_number - 1) * page_size;
                    int max = page_number * page_size;

                    if (max > ids.Length) max = ids.Length;
                    if (min > max) min = max - page_size;
                    if (min < 0) min = 0;

                    var m_cache = m___get(api);
                    string[] ps = new string[max - min];
                    int k = 0;
                    for (int i = min; i < max; i++)
                    {
                        try { ps[k] = m_cache[ids[i].ToString()]; }
                        catch (Exception ex1) { ps[k] = oError.getJson(ex1.Message, 60086326); }
                        k++;
                    }

                    if (!string.IsNullOrEmpty(fn_map))
                    {
                        try
                        {
                            fn_map = " return " + fn_map;
                            using (new JavaScriptContext.Scope(js_context))
                            {
                                string f2 = "___" + Guid.NewGuid().ToString().Replace('-', '_');
                                string sf2 = @" ___fn." + f2 + @" = function(o){ try { " + fn_map + @" }catch(e){ return { ok: false, code: 8603536, id: o.id, message: e.message }; } };";
                                JavaScriptContext.RunScript(sf2, currentSourceContext++, string.Empty);
                                string item = string.Empty;
                                for (var i = 0; i < ps.Length; i++)
                                {
                                    item = ps[i];
                                    if (item[0] != '{') continue;

                                    try
                                    {
                                        string js_exe =
    @"(()=>{ 
    try { 
        var o = " + item + @"; 
        var o2 = ___fn." + f2 + @"(o); 
        return JSON.stringify(o2); 
    } catch(e) { 
        return JSON.stringify({ ok: false, code: 59512726, id: o.id, message: e.message }); 
    } 
})()";
                                        var result = JavaScriptContext.RunScript(js_exe, currentSourceContext++, string.Empty);
                                        ps[i] = result.ConvertToString().ToString();
                                    }
                                    catch (Exception e123) { ps[i] = oError.getJson(e123.Message, 59835926); }
                                }

                                JavaScriptContext.RunScript(" delete ___fn." + f2, currentSourceContext++, string.Empty);
                            }
                        }
                        catch { }
                    }

                    api___response_json_text_body(context, true, "[" + string.Join(",", ps) + "]"
                        , m_cache.Count, ids.Length, page_number, page_size);
                }
            }
            else api___response_json_error(context, err);
            api___close(context);
            return true;
        }

        static bool api___redis_get_top(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            var m_cache = m___get(cf.name);

            string[] keys = redis.Keys;
            int max = keys.Length < 10 ? keys.Length : 10;
            string[] rs = new string[max];

            for (var i = 0; i < max; i++)
                //rs[i] = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));
                if (m_cache.ContainsKey(keys[i])) rs[i] = m_cache[keys[i]];

            string json = "[" + string.Join(",", rs) + "]";

            Stream input = api___stream_string(json);
            api___response_stream(".json", input, context);
            api___close(context);
            return true;
        }

        static bool api___redis_push_ram(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            var m_cache = m___get(cf.name);
            m_cache.Clear();

            string[] keys = redis.Keys;
            int max = keys.Length;
            string[] rs = new string[max];

            string json, ix;
            for (var i = 0; i < max; i++)
            {
                json = ASCIIEncoding.UTF8.GetString(redis.Get(keys[i]));
                ix = js___index(cf.name, json);
                if (!string.IsNullOrEmpty(ix)) json = ix;
                m_cache.TryAdd(keys[i], json);
            }

            api___response_json_text_body(context, true);
            api___close(context);
            return true;
        }

        static bool api___redis_bgsave(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.BackgroundSave();
            api___response_json_text_body(context, true);
            api___close(context);
            return true;
        }

        static bool api___redis_save(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            redis.Save();
            api___response_json_text_body(context, true);
            api___close(context);
            return true;
        }

        static bool api___redis_clean_all(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];
            var m_cache = m___get(cf.name);

            m_cache.Clear();
            redis.FlushDb();

            api___response_json_text_body(context, true);
            api___close(context);
            return true;
        }

        static bool api___redis_reload_db(HttpListenerContext context)
        {
            string[] a = context.Request.Url.Segments;
            string api = a.Length > 2 ? a[2].ToUpper() : "";
            if (api___redis_check_ready(context) == false) return false;
            var cf = m_config.list_cache.Where(x => x.name == api).Take(1).SingleOrDefault();
            var redis = m_redis[api];

            try
            {
                string file_sql = Path.Combine(ROOT_PATH, "config\\sql\\" + cf.name + ".sql");
                if (File.Exists(file_sql) == false)
                {
                    api___response_json_error(context, "File " + file_sql + " not exist");
                    return false;
                }
                string sql_select = File.ReadAllText(file_sql);

                redis.FlushDb();

                string json;
                bool existID = false;

                Dictionary<string, string> rows = new Dictionary<string, string>() { };
                var m_cache = m___get(cf.name);
                m_cache.Clear();
                busy___set(cf.name, true);

                using (var cn = new SqlConnection(m_config.db_connect[cf.scope]))
                {
                    cn.Open();
                    var cm = cn.CreateCommand();
                    cm.CommandText = sql_select;

                    using (SqlDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var columns = new string[reader.FieldCount];
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                columns[i] = reader.GetName(i).ToLower();
                                if (columns[i] == "id") existID = true;
                            }

                            int k = 0;
                            long id = 0;
                            while (reader.Read())
                            {
                                var dic = new Dictionary<string, object>();
                                for (var i = 0; i < reader.FieldCount; i++)
                                    dic.Add(columns[i], reader.GetValue(i));

                                id = k;
                                if (existID) long.TryParse(dic["id"].ToString(), out id);
                                json = JsonConvert.SerializeObject(dic);

                                string ix = js___index(cf.name, json);
                                if (!string.IsNullOrEmpty(ix)) json = ix;

                                rows.Add(id.ToString(), json);
                                if (m_cache != null) m_cache.TryAdd(id.ToString(), json);

                                k++;
                            }
                        }
                    }
                    cn.Close();
                }

                redis.Set(rows);
                rows.Clear();
                busy___set(cf.name, false);

                api___response_json_text_body(context, true);
                api___close(context);
                return true;
            }
            catch (Exception e111)
            {
                api___response_json_error(context, e111.Message);
            }
            return false;
        }

        static oPostItem[] api___biz_valid(HttpListenerContext context, string tran_id, string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                api___response_json_error(context, "Data is null or empty");
                return null;
            }

            if (string.IsNullOrEmpty(tran_id) || !data.Contains(tran_id))
            {
                api___response_json_error(context, "URL must be host/___tid and Data not contain ___tid: " + tran_id);
                return null;
            }

            if (data[0] != '[' || data[data.Length - 1] != ']'
                || !data.Contains("___tid")
                || !data.Contains("___api")
                || !data.Contains("___dbs")
                || !data.Contains("___do")
                || !data.Contains("id"))
            {
                api___response_json_error(context, "Data must be json {___dbs:...,___tid:...,___api:'USER|...',___do:'ADD|UPDATE|REMOVE', id:...}; ___tid is transaction ID");
                return null;
            }

            long ___tid = 0;
            if (long.TryParse(tran_id, out ___tid) == false || ___tid < 1)
            {
                api___response_json_error(context, "URL must be host/___tid and Data not contain ___tid: " + tran_id + ", ___tid must be number > 0");
                return null;
            }

            List<oPostItem> ls = new List<oPostItem>() { };

            try
            {
                var aj = JsonConvert.DeserializeObject<JObject[]>(data);
                var dt = DateTime.Now;
                JObject it;
                for (int i = 0; i < aj.Length; i++)
                {
                    it = aj[i];
                    JToken j_id, j_tid, j_do, j_api, j_dbs;

                    it.TryGetValue("id", out j_id);
                    it.TryGetValue("___tid", out j_tid);
                    it.TryGetValue("___api", out j_api);
                    it.TryGetValue("___do", out j_do);
                    it.TryGetValue("___dbs", out j_dbs);

                    if (j_id == null)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] missing field id", 65199001);
                        return null;
                    }

                    if (j_tid == null)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] missing field ___tid", 65199002);
                        return null;
                    }

                    if (j_do == null)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] missing field ___do: ADD|UPDATE|REMOVE", 65199003);
                        return null;
                    }

                    if (j_api == null)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] missing field ___api", 65199004);
                        return null;
                    }

                    if (j_dbs == null)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] missing field ___dbs", 65199004);
                        return null;
                    }

                    long id = 0, tid = 0;
                    string api = j_api.ToString().ToUpper(),
                        dbs = j_dbs.ToString(),
                        _do = j_do.ToString().ToUpper();
                    long.TryParse(j_id.ToString(), out id);
                    long.TryParse(j_tid.ToString(), out tid);

                    if (___tid != tid)
                    {
                        api___response_json_error(context, "On URL and Data then ___tid must be same");
                        return null;
                    }

                    if (_do != "ADD" && _do != "UPDATE" && _do != "REMOVE")
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] field ___do must be ADD|UPDATE|REMOVE");
                        return null;
                    }

                    if (m___check(api) == false)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] field ___api: " + api + " is not exist");
                        return null;
                    }

                    if (m_schema.ContainsKey(api) == false)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] field ___api: " + api + " is not exist schema");
                        return null;
                    }

                    string file_dbs = Path.Combine(ROOT_PATH, "dbs\\" + dbs + ".sql");
                    if (File.Exists(file_dbs)==false)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] field ___dbs: " + dbs + " is not find file " + file_dbs);
                        return null;
                    }


                    try
                    {
                        string j = JsonConvert.SerializeObject(it);
                        string ix = js___index(api, j);
                        if (string.IsNullOrEmpty(ix))
                        {
                            api___response_json_error(context, "Cannot make index [" + i.ToString() + "] for update item: " + id.ToString());
                            return null;
                        }
                        j = ix;

                        var input = JsonConvert.DeserializeObject<Dictionary<string, object>>(j);
                        input.Remove("___do");
                        input.Remove("___api");
                        input.Remove("___dbs");
                        input.Remove("___tid");

                        var output = JsonConvert.DeserializeObject<Dictionary<string, object>>(j);
                        output.Remove("___do");
                        output.Remove("___api");
                        output.Remove("___dbs");
                        output.Remove("___tid");

                        var schema = m_schema[api];
                        foreach (var kv in schema) if (!output.ContainsKey(kv.Key)) output.Add(kv.Key, kv.Value);
                        foreach (var kv in output)
                        {
                            string v = output[kv.Key] == null ? string.Empty : output[kv.Key].ToString();
                            switch (v)
                            {
                                case "KEY_IDENTITY":
                                    output[kv.Key] = api___get_uuid();
                                    break;
                                case "hhmmss":
                                    output[kv.Key] = int.Parse(dt.ToString("hhmmss"));
                                    break;
                                case "yyMMdd":
                                    output[kv.Key] = int.Parse(dt.ToString("yyMMdd"));
                                    break;
                                case "yyyyMMdd":
                                    output[kv.Key] = int.Parse(dt.ToString("yyyyMMdd"));
                                    break;
                                case "yyMMddhhmmss":
                                    output[kv.Key] = int.Parse(dt.ToString("yyMMddhhmmss"));
                                    break;
                                case "yyyyMMddhhmmss":
                                    output[kv.Key] = long.Parse(dt.ToString("yyyyMMddhhmmss"));
                                    break;
                                case "-1|hhmmss":
                                case "-1|yyMMdd":
                                case "-1|yyyyMMdd":
                                case "-1|yyMMddhhmmss":
                                case "-1|yyyyMMddhhmmss":
                                    output[kv.Key] = -1;
                                    break;
                            }
                        }

                        oPostItem pi = new oPostItem()
                        {
                            input = input,
                            ouput = output,
                            id = id,
                            ___api = api,
                            ___do = _do,
                            ___dbs = dbs,
                            ___tid = tid
                        };
                        ls.Add(pi);
                    }
                    catch (Exception e22)
                    {
                        api___response_json_error(context, "Command[" + i.ToString() + "] convert json error: " + e22.Message);
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                api___response_json_error(context, e.Message, 65199229);
                return null;
            }

            return ls.ToArray();
        }

        static bool api___post_update(HttpListenerContext context, string tran_id, string data)
        {
            oPostItem[] items = api___biz_valid(context, tran_id, data);
            if (items == null) return false;

            oPostItem po;
            ConcurrentDictionary<string, string> cache;
            Redis redis;
            bool has = false;
            string json, ix;

            for (int i = 0; i < items.Length; i++) {
                po = items[i];
                json = JsonConvert.SerializeObject(po.ouput);

                switch (po.___do)
                {
                    case "ADD":
                        has = true;
                        break;
                    case "UPDATE":
                        has = true;
                        break;
                    case "REMOVE":
                        has = true;
                        break;
                }

                if (has)
                {
                    cache = m___get(po.___api);
                    redis = m_redis[po.___api];
                }
            }

            string s = JsonConvert.SerializeObject(items, Formatting.Indented);
            api___response_json_text_body(context, true, s);
            api___close(context);
            return true;
        }

        static readonly Func<HttpListenerContext, bool> HTTP_API_PROCESS = (context) =>
        {
            string method = context.Request.HttpMethod,
                path = context.Request.Url.AbsolutePath,
                text = string.Empty, filename = path.Substring(1);
            string[] files, dirs;
            Stream input;
            string[] a = context.Request.Url.Segments;
            string cmd = a.Length > 0 ? a[1] : "";

            switch (method)
            {
                #region [ GET ]
                case "GET":
                    filename = path.Substring(1);
                    input = api___stream_string("Cannot found the file: " + path);

                    switch (filename)
                    {
                        case "list":
                            #region
                            dirs = Directory.GetDirectories(ROOT_PATH_UI);
                            files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);

                            files = files.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();
                            dirs = dirs.Select(x => string.Format(@"<a href=""{0}"" target=_blank>{0}</a></br>", Path.GetFileName(x))).ToArray();

                            text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files) +
                                //@"<a href=""/config"">config</a></br>" +
                                @"<a href=""/config"" target=_blank>config</a></br>";

                            input = api___stream_string(text);
                            filename = "list.html";
                            break;
                        #endregion
                        case "config":
                            #region
                            text = JsonConvert.SerializeObject(m_config);
                            input = api___stream_string(text);
                            api___response_stream(".json", input, context);
                            break;
                        #endregion
                        case "lib/reset":
                            #region
                            text = js___reset();
                            if (text.Length > 0 && text[0] != '{')
                                api___response_json_error(context, text);
                            else
                                api___response_json_text_raw(context, text);
                            break;
                        #endregion
                        case "schema":
                            #region
                            text = JsonConvert.SerializeObject(m_schema, Formatting.Indented);
                            api___response_json_text_raw(context, text);
                            break;
                            #endregion
                        case "schema/reset":
                            #region
                            schema___reset();
                            text = JsonConvert.SerializeObject(m_schema, Formatting.Indented);
                            api___response_json_text_raw(context, text);
                            break;
                        #endregion
                        case "valid":
                            #region
                            text = JsonConvert.SerializeObject(m_valid_add, Formatting.Indented);
                            api___response_json_text_raw(context, text);
                            break;
                            #endregion
                        case "valid/reset":
                            #region
                            valid__reset();
                            text = JsonConvert.SerializeObject(m_valid_add, Formatting.Indented);
                            api___response_json_text_raw(context, text);
                            break;
                            #endregion
                        default:
                            switch (cmd)
                            {
                                case "reload_db/": return api___redis_reload_db(context);
                                case "reset/": return api___redis_reset(context);
                                case "bgsave/": return api___redis_bgsave(context);
                                case "save/": return api___redis_save(context);
                                case "clean_all/": return api___redis_clean_all(context);
                                case "top/": return api___redis_get_top(context);
                                case "redis-push-ram/": return api___redis_push_ram(context);
                                case "search/": return api___ram_search(context);
                                case "uuid/": return api___get_uuid(context);
                            }
                            #region

                            if (string.IsNullOrEmpty(filename))
                            {
                                files = GetFiles(ROOT_PATH_UI, "*.*", SearchOption.TopDirectoryOnly);
                                string fileIndex = files.Where(x => x.EndsWith("index.htm") || x.EndsWith("index.html")).SingleOrDefault();
                                if (fileIndex != null) filename = Path.GetFileName(fileIndex);
                            }

                            filename = Path.Combine(ROOT_PATH_UI, filename);
                            if (File.Exists(filename))
                                input = new FileStream(filename, FileMode.Open);
                            else
                            {
                                if (Directory.Exists(filename))
                                {
                                    dirs = Directory.GetDirectories(filename);
                                    files = GetFiles(filename, "*.*", SearchOption.TopDirectoryOnly);

                                    files = files.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();
                                    dirs = dirs.Select(x => string.Format(@"<a href=""{0}/{1}"">{1}</a></br>", path, Path.GetFileName(x))).ToArray();

                                    text = string.Join(string.Empty, dirs) + string.Join(string.Empty, files);
                                    input = api___stream_string(text);
                                    filename = ".html";
                                }
                                else
                                {
                                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                                    context.Response.OutputStream.Close();
                                    return false;
                                }
                            }
                            #endregion
                            break;
                    }

                    //Console.WriteLine(path);
                    api___response_stream(Path.GetExtension(filename), input, context);

                    break;
                #endregion

                #region [ POST ]

                case "POST":
                    using (StreamReader reader = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                        text = reader.ReadToEnd();
                    return api___post_update(context, cmd, text);

                    #endregion
            }
            return false;
        };

        #endregion

        #region [ FREE RESOURCE ]

        static int tcp___get_free_port()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        static HTTPServerUI http_api;
        static WebSocketServer server_ws;
        static List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        static ConcurrentDictionary<string, Redis> m_redis
            = new ConcurrentDictionary<string, Redis>() { };
        static void app___free_resouce()
        {
            try
            {
                http_api.Stop();

                m_config.list_cache.ForEach(cf =>
                {
                    try { m_redis[cf.name].Dispose(); } catch (Exception e1) { }
                    try { cf.process.Kill(); } catch (Exception e2) { }
                });
                m_redis.Clear();

                allSockets.Clear();
                server_ws.Dispose();

                m___free_memory();
                js___free_memory();
            }
            catch (Exception err) { }
        }

        #endregion

        static void Main(string[] args)
        {
            schema___reset();
            valid__reset();

            #region [ CONFIG.JSON ]

            string file_config = Path.Combine(ROOT_PATH, "config.json");
            if (!File.Exists(file_config))
            {
                Console.WriteLine("Cannot find config.json");
                return;
            }

            try
            {
                string sconfig = File.ReadAllText(file_config);
                m_config = JsonConvert.DeserializeObject<oConfig>(sconfig);
            }
            catch (Exception e1)
            {
                Console.WriteLine("Error format JSON file config.json = ", e1.Message);
                return;
            }

            #endregion

            #region [ REDIS CACHE ]

            if (m_config != null && m_config.list_cache != null)
            {
                string file_conf_template = Path.Combine(ROOT_PATH, "redis.conf");
                if (File.Exists(file_conf_template) == false)
                {
                    Console.WriteLine("Cannot found the file: " + file_conf_template);
                    return;
                }
                string temp_conf = File.ReadAllText(file_conf_template);

                string path_conf = Path.Combine(ROOT_PATH, "config");
                if (Directory.Exists(path_conf) == false) Directory.CreateDirectory(path_conf);

                string PATH_DATA = Path.Combine(ROOT_PATH, "data");
                if (Directory.Exists(PATH_DATA) == false) Directory.CreateDirectory(PATH_DATA);

                m_config.busy = true;
                m_config.list_cache.ForEach(cf =>
                {
                    if (cf.id < 100 && cf.id > -1) m_names[cf.id] = cf.name;

                    if (cf.enable)
                    {
                        int port = tcp___get_free_port();
                        string file_redis = Path.Combine(ROOT_PATH, "redis-server.exe");
                        if (File.Exists(file_redis))
                        {
                            string file_conf = Path.Combine(path_conf, cf.name + ".conf");
                            if (File.Exists(file_conf)) File.Delete(file_conf);

                            string conf = temp_conf
                                .Replace("[IP]", "127.0.0.1")
                                .Replace("[PORT]", port.ToString())
                                .Replace("[DATA_FILE]", cf.name)
                                .Replace("[DATA_PATH]", PATH_DATA.Replace('\\', '/'));
                            File.WriteAllText(file_conf, conf);

                            Process p = new Process();
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.RedirectStandardInput = true;
                            p.StartInfo.FileName = file_redis;
                            //string argument = @" """ + file_conf + @""" --port " + port.ToString();
                            string argument = @" """ + file_conf + @"""";
                            p.StartInfo.Arguments = argument;
                            p.Start();

                            cf.process = p;
                            cf.port = port;
                            cf.ready = true;

                            //RedisDataAccessProvider redis = new RedisDataAccessProvider();
                            //redis.Configuration = new TeamDevRedis.LanguageItems.Configuration() { Host = "127.0.0.1", Port = port };
                            //redis.Connect();
                            var redis = new Redis("127.0.0.1", port);

                            if (m_redis.ContainsKey(cf.name))
                                m_redis.TryAdd(cf.name, redis);
                            else
                                m_redis[cf.name] = redis;

                            Console.WriteLine(" -> " + cf.name + " " + cf.port.ToString());
                        }
                    }
                });
                m_config.busy = false;
            }

            #endregion

            #region [ WEB_SOCKET ]

            int port_ws = tcp___get_free_port();
            FleckLog.Level = LogLevel.Error;
            server_ws = new WebSocketServer("ws://127.0.0.1:" + port_ws);
            server_ws.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
            m_config.port_ws = port_ws;

            #endregion

            #region [ TCP_PUSH ]


            #endregion

            #region [ HTTP_API ]

            if (!Directory.Exists(ROOT_PATH_UI)) Directory.CreateDirectory(ROOT_PATH_UI);
            http_api = new HTTPServerUI(ROOT_PATH_UI, m_config.port_api, HTTP_API_PROCESS);

            #endregion

            File.WriteAllText(file_config, JsonConvert.SerializeObject(m_config, Formatting.Indented));

            #region [ READ LINE ]

            string line = Console.ReadLine();
            while (line != "exit")
            {
                //foreach (var socket in allSockets.ToList()) socket.Send(line);
                switch (line)
                {
                    case "cls":
                    case "clean":
                    case "clear":
                        Console.Clear();
                        break;
                }
                line = Console.ReadLine();
            }

            Console.WriteLine("Program is closing... ");
            app___free_resouce();

            //string file_node = Path.Combine(ROOT_PATH, "node.exe");
            //if (File.Exists(file_node))
            //{
            //    Process node = new Process();
            //    node.StartInfo.UseShellExecute = false;
            //    node.StartInfo.RedirectStandardOutput = true;
            //    node.StartInfo.RedirectStandardError = true;
            //    node.StartInfo.RedirectStandardInput = true;
            //    node.StartInfo.FileName = file_node;
            //    string argument = @" --max-old-space-size=4096 app.js";
            //    node.StartInfo.Arguments = argument;
            //    node.Start();
            //}

            #endregion
        }
    }
}
