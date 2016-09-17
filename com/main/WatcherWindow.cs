using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using MarkupWatchtower.com.main;

namespace HamlWatchtowerApp
{
    public partial class WatcherWindow : Form
    {
        public static int IdCounter = 0;
        public static List<MarkupObject> mObjects = new List<MarkupObject>();
        public static List<MarkupWatcher> watcherObjects = new List<MarkupWatcher>();
        public static RichTextBox txtLog;

        private readonly string jsonPath = Environment.CurrentDirectory + "\\watchers.json";
        private readonly string listPath = Environment.CurrentDirectory + "\\markupList.json";

        public WatcherWindow()
        {
            validateFiles();
            InitializeComponent();
            InitLogger();
            LoadWatchers();
        }

        private void validateFiles()
        {
            if (!File.Exists(listPath))
            {
                new LangListGen();
                MessageBox.Show("Could not find 'markupList.json'. A new file was generated.",
                    "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!File.Exists(jsonPath))
            {
                string s = @"{""watchers"": {}}";
                File.WriteAllText(jsonPath, s);
            }
            LoadLang();
        }
        private void LoadLang()
        {
            string mainJson = File.ReadAllText(listPath);
            JObject jo = JObject.Parse(mainJson);
            JObject lang = jo["markup"] as JObject;
            foreach (var key in lang.Children())
            {
                MarkupObject mo = new MarkupObject();
                mo.Name = ((JProperty)key).Name;
                foreach (var p in key.Children())
                {
                    mo.Output = (string)((JObject)p).Property("output").Value;
                    mo.Command = (string)((JObject)p).Property("command").Value;
                    if (((JObject)p).Property("input").Value.Type == JTokenType.Array)
                    {
                        var arr = ((JArray)(((JObject)p).Property("input").Value));
                        for (int i = 0; i < arr.Count; i++)
                        {
                            mo.Input.Add((string)arr[i]);
                        }
                        continue;
                    }
                    mo.Input.Add((string)((JObject)p).Property("input").Value);
                }
                mObjects.Add(mo);
            }
        }

        private void LoadWatchers()
        {

            string mainJson = File.ReadAllText(jsonPath);
            JObject jo = JObject.Parse(mainJson);
            JObject watchers = jo["watchers"] as JObject;
            int i = 0;
            while (watchers[i.ToString()] != null)
            {
                var w = watchers[i.ToString()];
                MarkupPanel mp = new MarkupPanel();
                string s = ((string)w["path"]).Replace("/", "\\");
                List<string> input = new List<string>();
                if (w["input"].Type == JTokenType.Array)
                    input = ((JArray)w["input"]).ToObject<List<string>>();
                else input.Add((string)w["input"]);

                mp.updateComboBox((string)w["name"]);
                mp.setDirText(s);
                mp.getWatcher().setID(i);
                mp.getWatcher().setFolderPath(s);
                mp.getWatcher().setName((string)w["name"]);
                mp.getWatcher().setCommand((string)w["command"]);
                mp.getWatcher().setInput(input);
                mp.getWatcher().setOutput((string)w["output"]);
                mp.getWatcher().update();

                this.layoutMainTop.Controls.Add(mp.getPanel());
                int j = this.layoutMainTop.Controls.GetChildIndex(btnAdd);
                this.layoutMainTop.Controls.SetChildIndex(btnAdd, j + 1);
                
                i++;
            }
        }

        private void InitLogger()
        {
            // 
            // txtLog
            // 
            txtLog = new RichTextBox();
            txtLog.ForeColor = System.Drawing.Color.FromArgb(160, 215, 163);
            txtLog.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            txtLog.BorderStyle = BorderStyle.None;
            txtLog.Dock = DockStyle.Fill;
            txtLog.HideSelection = false;
            txtLog.Location = new System.Drawing.Point(3, 3);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Size = new System.Drawing.Size(694, 191);
            txtLog.Text = "";
            txtLog.WordWrap = false;
            txtLog.TabStop = false;

            layoutPanelBottom.Controls.Add(txtLog);
        }

        public static MarkupObject getMObject(string name)
        {
            foreach (MarkupObject o in mObjects)
            {
                if (o.Name.Equals(name))
                    return o;
            }
            return null;
        }

        public static void addWatcher(MarkupWatcher watcher)
        {
            watcherObjects.Add(watcher);
        }

        public static void removeAndClean(int ID, MarkupWatcher watcher)
        {
            watcherObjects.Remove(watcher);
            for (int i = ID, j = 0; i < watcherObjects.Count; i++, j++)
            {
                watcherObjects[i].setID(i + j);
            }
        }

        private void WatcherWindow_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MarkupPanel mp = new MarkupPanel();
            this.layoutMainTop.Controls.Add(mp.getPanel());
            int i = this.layoutMainTop.Controls.GetChildIndex(btnAdd);
            this.layoutMainTop.Controls.SetChildIndex(btnAdd, i + 1);
        }
    }
}
