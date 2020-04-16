using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;
using TeamDevRedis;
using TeamDevRedis.LanguageItems;
using Newtonsoft.Json;
using System.Threading;
using System.Collections.Generic;

namespace studio_redis
{
    public partial class fMainUI : Form
    {
        #region [ UI ]

        static fMainUI()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(fMainUI).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
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

        const int BAR_HEIGHT = 5;
        const int MAIN_HEIGHT_NORMAL = 590;
        const int MAIN_WIDTH_NORMAL = 1024;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void BoxUI_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public fMainUI()
        {
            InitializeComponent();
            this.panelHeader.MouseDown += BoxUI_MouseDown;
            this.lblMessage.MouseDown += BoxUI_MouseDown;
        }


        #region [ Disable Expand Node After dblClick ]

        private bool isDoubleClick = false;

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Collapse)
                e.Cancel = true;
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (isDoubleClick && e.Action == TreeViewAction.Expand)
                e.Cancel = true;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            isDoubleClick = e.Clicks > 1;
        }

        void f_treeKey_Disable_Expand_Nodes_After_dblClick()
        {
            treeKeys.BeforeCollapse += treeView1_BeforeCollapse;
            treeKeys.BeforeExpand += treeView1_BeforeExpand;
            treeKeys.MouseDown += treeView1_MouseDown;
        }

        #endregion

        private void fMainUI_Load(object sender, EventArgs e)
        {
            f_treeKey_Disable_Expand_Nodes_After_dblClick();

            f_ui_init();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.Height == MAIN_HEIGHT_NORMAL)
            {
                //this.WindowState = FormWindowState.Maximized;
                this.Top = 0;
                this.Left = 0;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height - BAR_HEIGHT;
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            }
            else
            {
                //this.WindowState = FormWindowState.Normal;
                this.Top = ((Screen.PrimaryScreen.WorkingArea.Height - BAR_HEIGHT) - MAIN_HEIGHT_NORMAL) / 2;
                this.Left = (Screen.PrimaryScreen.WorkingArea.Width - MAIN_WIDTH_NORMAL) / 2;
                this.Height = MAIN_HEIGHT_NORMAL;
                this.Width = MAIN_WIDTH_NORMAL;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            f_clean_all();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            f_load();
        }

        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            f_export_file();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            f_search();
        }

        private void btnKeysReload_Click(object sender, EventArgs e)
        {
            f_load(true);
            //f_key_selected(treeKeys.SelectedNode);
            //if (!string.IsNullOrEmpty(txtSearch.Text.ToLower().Trim()))
            f_search();
        }

        private void btnLogItemSaveFile_Click(object sender, EventArgs e)
        {
            f_editor_save_file();
        }

        private void btnEditorSearch_Click(object sender, EventArgs e)
        {
            f_editor_search();
        }

        private void btnEditorFontBigger_Click(object sender, EventArgs e)
        {
            f_editor_font_bigger();
        }

        private void btnEditorFontSmaller_Click(object sender, EventArgs e)
        {
            f_editor_font_smaller();
        }

        private void labelKey_Selected_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            treeKeys.ExpandAll();
        }

        private void labelKey_Selected_Click(object sender, EventArgs e)
        {
            treeKeys.ExpandAll();
        }

        private void btnKeyDEL_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
                f_key_del();
        }

        private void btnLogItem_DEL_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this item ??",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
                f_key_item_del();
        }

        private void button_key_del_all_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this all items ??",
                                        "Confirm Delete!!",
                                        MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
                f_key_item_del_all();
        }

        private void txtSearch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtSearch.Text = "";
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) f_search();
        }

        #endregion

        #region [ Methods ]

        void f_ui_init()
        {
            treeKeys.NodeMouseDoubleClick += (s1, e1) => f_key_selected(treeKeys.SelectedNode);
            listKeys.SelectedIndexChanged += (s1, e1) => f_key_ids_selected(listKeys.SelectedIndex == -1 ? null : listKeys.Items[listKeys.SelectedIndex].ToString());
            this.Top = 0;
            this.Left = 0;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height - BAR_HEIGHT;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;

            txtEditor.Font = new Font("Consolas", 14f, FontStyle.Regular);

            f_load();
        }

        void f_message(string text = "") => lblMessage.Text = text;

        void f_dialog_save_file()
        {

        }


        static RedisDataAccessProvider m_redis;
        //static string[] m_keys = new string[] { };
        static oKey[] m_keys = new oKey[] { };
        static oKey m_key_selected;
        static string[] m_key_ids = new string[] { };

        void f_load(bool cache_selected = false)
        {
            f_message();

            treeKeys.Nodes.Clear();
            listKeys.Items.Clear();
            lblKeyID_Selected.Text = "";
            lblKeyID_Counter.Text = "";

            string key_full_selected = "";
            if (cache_selected && m_key_selected != null) key_full_selected = m_key_selected.key_full;

            try
            {
                m_redis = new RedisDataAccessProvider();
                m_redis.Configuration = new Configuration() { Host = txtIP.Text.Trim(), Port = Convert.ToInt16(txtPort.Text.Trim()) };
                m_redis.Connect();

                string[] keys = m_redis.ReadMultiString(m_redis.SendCommand(RedisCommand.KEYS, "*"));
                m_keys = keys.Select(x => new oKey(m_redis, x)).ToArray();

                var keys_0 = keys.Select(x => x.Split(new char[] { '#', '.' }))
                    .Select(x => x[0]).Distinct().OrderBy(x => x).ToArray();

                TreeNode[] nodes_0 = new TreeNode[keys_0.Length];
                for (var i = 0; i < keys_0.Length; i++)
                {
                    string key_0 = keys_0[i];
                    nodes_0[i] = new TreeNode(key_0);

                    var okeys_1 = m_keys.Where(x => x.key_full.StartsWith(key_0 + "#")
                        || x.key_full.StartsWith(key_0 + ".")).OrderBy(x => x.key_full).ToArray();
                    string[] keys_1 = okeys_1.Select(x => x.key_full.Split(new char[] { '#', '.' })[1]).Distinct().OrderBy(x => x).ToArray();
                    if (keys_1.Length == 0)
                    {
                        nodes_0[i].Tag = new oKey() { key0 = key_0, level = 0 };
                    }
                    else
                    {
                        for (var i1 = 0; i1 < keys_1.Length; i1++)
                        {
                            string key_1 = keys_1[i1];
                            var nodes_1 = new TreeNode(key_1);
                            nodes_0[i].Nodes.Add(nodes_1);

                            var okeys_2 = m_keys.Where(x => (x.key_full.StartsWith(key_0 + "#") || x.key_full.StartsWith(key_0 + "."))
                                    && (x.key_full.Contains(key_1 + "#") || x.key_full.Contains(key_1 + "."))).OrderBy(x => x.key_full).ToArray();
                            string[] keys_2 = okeys_2.Select(x => x.key_full.Split(new char[] { '#', '.' })[2]).Distinct().OrderBy(x => x).ToArray();
                            if (keys_2.Length == 0)
                            {
                                nodes_1.Tag = new oKey() { key0 = key_0, key1 = key_1, level = 1 };
                            }
                            else
                            {
                                for (var i2 = 0; i2 < keys_2.Length; i2++)
                                {
                                    string key_2 = keys_2[i2];
                                    var nodes_2 = new TreeNode(key_2) { Tag = new oKey() { key0 = key_0, key1 = key_1, key2 = key_2, level = 2 } };
                                    nodes_1.Nodes.Add(nodes_2);
                                }
                            }
                        }
                    }
                }
                treeKeys.Nodes.AddRange(nodes_0);
                //f_message("OK");
                //Thread.Sleep(100);
                treeKeys.ExpandAll();
                //Thread.Sleep(100);
                if (key_full_selected.Length > 0) CallRecursive(key_full_selected);
            }
            catch (Exception err)
            {
                f_message("FAIL: " + err.Message);
            }
        }

        void PrintRecursive(TreeNode treeNode, string key_full)
        {
            oKey key = treeNode.Tag as oKey;
            if (key != null)
            {
                var ok = key.findKeys(m_keys).Where(x => x.key_full == key_full).Count() == 1;
                if (ok)
                {
                    f_key_selected(treeNode);
                    return;
                }
            }

            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn, key_full);
            }
        }

        private void CallRecursive(string key_full)
        {
            foreach (TreeNode n in treeKeys.Nodes)
                PrintRecursive(n, key_full);
        }

        void f_key_selected(TreeNode node)
        {
            txtEditor.Text = "";
            listKeys.Items.Clear();
            m_key_selected = null;
            m_key_ids = new string[] { };
            listKeys.Items.Clear();
            lblKeyID_Selected.Text = "";
            lblKeyID_Counter.Text = "";
            labelKey_Selected.Text = "";

            if (node != null)
            {
                if (node.Tag == null)
                {
                    string err = "ERR: " + node.Text;
                    f_message(err);
                    //MessageBox.Show(err);
                }
                else
                {
                    labelKey_Selected.Text = node.Text;

                    oKey key = node.Tag as oKey;
                    lblKeySelected_Path.Text = key.getPath();
                    lblKeySelected_Time.Text = "";

                    var keys = key.findKeys(m_keys);
                    if (keys.Length == 1)
                    {
                        m_key_selected = keys[0];
                        m_key_ids = m_key_selected.keys_last.Reverse().ToArray();
                        listKeys.Items.AddRange(m_key_ids);
                        lblKeyID_Counter.Text = "(" + m_key_ids.Length.ToString() + ")";
                    }
                }
            }
        }

        void f_key_ids_selected(string id)
        {
            lblKeySelected_Time.Text = "";
            lblKeyID_Selected.Text = "";
            txtEditor.Text = "";

            if (id != null && m_key_ids.Length > 0 && m_key_selected != null)
            {
                //string id = m_key_ids[index];
                lblKeySelected_Time.Text = id;
                lblKeyID_Selected.Text = id;

                string data = "";
                try
                {
                    data = m_redis.ReadString(m_redis.SendCommand(RedisCommand.HGET, m_key_selected.key_full, id));
                    var o = JsonConvert.DeserializeObject(data);
                    string json = JsonConvert.SerializeObject(o, Formatting.Indented);
                    txtEditor.Text = json;
                }
                catch (Exception e)
                {
                    txtEditor.Text = data;
                }
            }
        }

        void f_key_del()
        {
            if (m_key_selected != null)
            {
                m_redis.SendCommand(RedisCommand.DEL, m_key_selected.key_full);
                f_key_selected(null);
                f_load();
            }
        }

        void f_key_item_del()
        {
            if (m_key_selected != null && lblKeyID_Selected.Text.Length > 0)
            {
                m_redis.SendCommand(RedisCommand.HDEL, m_key_selected.key_full, lblKeyID_Selected.Text);
                f_key_ids_selected(null);
                f_search();
            }
            else
            {
                f_search();
            }
        }

        void f_key_item_del_all()
        {
            if (txtSearch.Text.Trim().Length == 0)
                f_key_del();
            else
            {
                if (m_key_selected != null && m_key_ids.Length > 0)
                {
                    var ls = new List<string>();
                    ls.Add(m_key_selected.key_full);
                    ls.AddRange(listKeys.Items.Cast<string>().ToArray());
                    m_redis.SendCommand(RedisCommand.HDEL, ls.ToArray());
                    f_key_ids_selected(null);
                    f_search();
                }
            }
        }

        void f_search()
        {
            txtEditor.Text = "";
            //listKeys.Items.Clear();
            //m_key_selected = null;
            //m_key_ids = new string[] { };
            listKeys.Items.Clear();
            lblKeyID_Selected.Text = "";
            lblKeyID_Counter.Text = "";

            //lblKeySelected_Path.Text = key.getPath();
            lblKeySelected_Time.Text = "";

            f_load(true);

            if (m_key_ids.Length > 0)
            {
                listKeys.Items.Clear();

                string kw = txtSearch.Text.ToLower().Trim();
                var a = m_key_ids.Where(x => x.Contains(kw)).ToArray();
                listKeys.Items.AddRange(a);
                lblKeyID_Counter.Text = "[ " + a.Length.ToString() + " | " + m_key_ids.Length.ToString() + " ]";
            }
        }

        void f_clean_all()
        {
            //string s = m_redis.ReadString(m_redis.SendCommand(RedisCommand.FLUSHDB));
            string s = "";
            m_redis.SendCommand(RedisCommand.FLUSHDB);
            f_message(s);
            treeKeys.Nodes.Clear();
            listKeys.Items.Clear();
            //f_load();
        }

        void f_export_file()
        {

        }

        void f_editor_save_file()
        {

        }

        void f_editor_search()
        {

        }

        void f_editor_font_bigger()
        {
            float fs = txtEditor.Font.Size + 1;
            txtEditor.Font = new Font("Consolas", fs, FontStyle.Regular);
        }

        void f_editor_font_smaller()
        {
            float fs = txtEditor.Font.Size - 1;
            txtEditor.Font = new Font("Consolas", fs, FontStyle.Regular);
        }

        #endregion

    }
}
