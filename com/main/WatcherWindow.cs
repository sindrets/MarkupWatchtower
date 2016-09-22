using System;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.Generic;
using MarkupWatchtower.com.main;
using System.Linq;

namespace HamlWatchtowerApp
{
    public partial class WatcherWindow : Form
    {
        public static readonly string VERSION = "1.0.4";
        private string listVersion;
        public static bool needUpdate = false;
        public static int IdCounter = 0;
        public static List<MarkupObject> mObjects = new List<MarkupObject>();
        public static List<MarkupWatcher> watcherObjects = new List<MarkupWatcher>();
        public static RichTextBox txtLog;

        private readonly string jsonPath = Environment.CurrentDirectory + "\\watchers.json";
        private readonly string listPath = Environment.CurrentDirectory + "\\markupList.json";

        public WatcherWindow()
        {
            ValidateFiles();
            RunUpdate();
            InitializeComponent();
            InitLogger();
            LoadWatchers();
        }

        private void RunUpdate()
        {
            if (needUpdate)
            {
                DialogResult result = MessageBox.Show("It seems the application version and the Markup List "
                    +"version are conflicting. The Markup list might have been updated since it was last "
                    +"generated. Do you want Markup Watchtower to replace the Markup List?\n\n"
                    +"Application version: " + VERSION + "\n"
                    +@"markupList.json version: " + listVersion,
                    "Version Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    new LangListGen();
                }
            }
        }

        private void IsUpdateNeeded(string version)
        {
            int[] current = ParseVersionString(VERSION);
            int[] fileVersion = ParseVersionString(version);
            needUpdate = !current.SequenceEqual(fileVersion);
            listVersion = string.Join(".", fileVersion);
        }

        private int[] ParseVersionString(string version)
        {
            int count = version.Count(f => f == '.');
            int[] arr = new int[count + 1];
            int i = 0, j;
            for (int k = 0; k < count; k++)
            {
                j = version.IndexOf(".", i);
                int result = int.Parse(version.Substring(i, j-i));
                arr[k] = result;
                i = j+1;
            }
            arr[count] = int.Parse(version.Substring(i));

            return arr;
        }

        private void ValidateFiles()
        {
            if (!File.Exists(listPath))
            {
                new LangListGen();
                MessageBox.Show("Could not find 'markupList.json'. A new file was generated.",
                    "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (!File.Exists(jsonPath))
            {
                string s = 
@"{
    ""version"": "+@""""+VERSION+@""",
    ""watchers"": {}
}";
                File.WriteAllText(jsonPath, s);
            }
            LoadLang();
        }

        private void LoadLang()
        {
            string mainJson = File.ReadAllText(listPath);
            JObject jo = JObject.Parse(mainJson);
            try {
                string currentVersion = (string)jo.Property("version").Value;
                IsUpdateNeeded(currentVersion);
            } catch (System.NullReferenceException) { needUpdate = true; }
            JObject lang = jo["markup"] as JObject;
            foreach (JToken key in lang.Children())
            {
                MarkupObject mObj = new MarkupObject();
                mObj.Name = ((JProperty)key).Name;
                foreach (JToken token in key.Children())
                {
                    JObject child = (JObject)token;
                    mObj.Output = (string)child.Property("output").Value;
                    mObj.Command = (string)child.Property("command").Value;
                    if ((child).Property("input").Value.Type == JTokenType.Array)
                    {
                        var arr = ((JArray)((child).Property("input").Value));
                        for (int i = 0; i < arr.Count; i++)
                        {
                            mObj.Input.Add((string)arr[i]);
                        }
                        continue;
                    }
                    mObj.Input.Add((string)child.Property("input").Value);
                }
                mObjects.Add(mObj);
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

                string name = (string)w["name"];
                mp.updateComboBox(name);
                mp.setSubDir((bool)w["subdirectories"]);
                mp.setDirText(s);
                mp.getWatcher().setID(i);
                mp.getWatcher().setFolderPath(s);
                mp.getWatcher().setName(name);
                mp.getWatcher().setInput(input);
                mp.getWatcher().setOutput((string)w["output"]);
                mp.getWatcher().setSubdirectries((bool)w["subdirectories"]);
                mp.getWatcher().setCommand(getMObject(name).Command);
                mp.getWatcher().update();

                layoutMainTop.Controls.Add(mp.getPanel());
                int j = layoutMainTop.Controls.GetChildIndex(btnAdd);
                layoutMainTop.Controls.SetChildIndex(btnAdd, j + 1);
                
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
            txtLog.HideSelection = false;
            txtLog.Location = new System.Drawing.Point(3, 3);
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.Dock = DockStyle.Fill;
            txtLog.Text = "";
            txtLog.WordWrap = false;
            txtLog.TabStop = false;

            layoutPanelBottom.Controls.Add(txtLog);
            Text = "Markup Watchtower - v" + VERSION;
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

        public static void AddWatcher(MarkupWatcher watcher)
        {
            watcherObjects.Add(watcher);
        }

        public static void RemoveAndClean(int ID, MarkupWatcher watcher)
        {
            watcherObjects.Remove(watcher);
            for (int i = ID, j = 0; i < watcherObjects.Count; i++, j++)
            {
                watcherObjects[i].setID(i + j);
            }
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