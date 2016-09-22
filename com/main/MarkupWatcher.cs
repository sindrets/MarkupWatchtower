using HamlWatchtowerApp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MarkupWatchtower.com.main
{
    public class MarkupWatcher
    {
        private FileSystemWatcher watcher;
        private string pathToFolder = "";

        private MarkupObject mObject;
        private int ID;
        private string name;
        private string output;
        private string command;
        private bool subdirectories = true;
        private List<string> input = new List<string>();

        public MarkupWatcher()
        {
            ID = WatcherWindow.IdCounter;
            WatcherWindow.IdCounter++;
            setMObject(WatcherWindow.getMObject("HAML"));
            watcher = new FileSystemWatcher();
            WatcherWindow.AddWatcher(this);
        }

        private void readMObject()
        {
            name = mObject.Name;
            output = mObject.Output;
            command = mObject.Command;
            input = mObject.Input;
        }

        public void update()
        {
            watcher.IncludeSubdirectories = true;
            watcher.Filter = "*.*";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            //Add event handlers
            watcher.Created += new FileSystemEventHandler(WatcherChanged);
            watcher.Changed += new FileSystemEventHandler(WatcherChanged);
            watcher.Deleted += new FileSystemEventHandler(WatcherChanged);
            watcher.Renamed += new RenamedEventHandler(WatcherChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
            Log();
        }

        public void stopWatcher()
        {
            if (watcher.EnableRaisingEvents == false)
            {
                WatcherWindow.IdCounter--;
                Output("Sucessfully removed watcher!\n");
                return;
            }

            watcher.EnableRaisingEvents = false;

            string jsonPath = Environment.CurrentDirectory + "\\watchers.json";
            string mainJson = File.ReadAllText(jsonPath);

            JObject jo = JObject.Parse(mainJson);
            JObject watchers = jo["watchers"] as JObject;
            //Remove watcher from json
            watchers.Property(ID.ToString()).Remove();
            WatcherWindow.IdCounter--;
            //Move all newer watchers up if necessary
            int i = 1;
            while (watchers[(ID + i).ToString()] != null)
            {
                string name = (ID + i).ToString();
                JObject jo2 = watchers[name] as JObject;
                JToken jt = (JObject)jo2.DeepClone();
                watchers.Property(name).Remove();
                watchers.Add((ID + i - 1).ToString(), jt);
                i++;
            }
            File.WriteAllText(jsonPath, jo.ToString());

            WatcherWindow.RemoveAndClean(ID, this);

            Output("Sucessfully shut down and removed watcher!\n");
        }
        public void setMObject(MarkupObject o)
        {
            mObject = o;
            readMObject();
        }
        public void setFolderPath(string path)
        {
            this.pathToFolder = path;
            watcher.Path = path;
        }
        public void setID(int id)
        {
            this.ID = id;
        }
        public void setName(string name)
        {
            this.name = name;
        }
        public void setInput(List<string> input)
        {
            this.input = input;
        }
        public void setOutput(string output)
        {
            this.output = output;
        }
        public void setSubdirectries(bool boolean)
        {
            this.subdirectories = boolean;
        }
        public void setCommand(string command)
        {
            this.command = command;
        }
        public void setIncludeSubdirectories(bool b)
        {
            watcher.IncludeSubdirectories = b;
            subdirectories = b;
            if (b) Output("All subdirectories included\n");
            else Output("All subdirectories excluded\n");
        }
        public void Log()
        {
            string files = "";
            foreach (string str in input)
            {
                files += "'" + str + "', ";
            }
            int i = files.LastIndexOf(",");
            int j = files.LastIndexOf(",", i - 1);
            if (j != -1)
            {
                files = files.Remove(j, 2);
                files = files.Insert(j, " and ");
            }
            files = files.Remove(files.Length-2, 2);
            Console.WriteLine("Listening for changes to " + files + " files in: " + pathToFolder);
            Output("Listening for changes to " + files + " files in: " + pathToFolder + "\n");
        }

        public void saveToJson()
        {
            string jsonPath = Environment.CurrentDirectory + "\\watchers.json";
            string mainJson = File.ReadAllText(jsonPath);
            string s = pathToFolder.Replace("\\", "/");

            JObject jo = JObject.Parse(mainJson);
            if (jo["watchers"][ID.ToString()] == null)
            {
                string inp = "[";
                foreach (string str in input)
                {
                    inp += @"""" + str + @""",";
                }
                inp = inp.Remove(inp.Length - 1) + "],";

                string subDir = subdirectories.ToString().ToLower();
                Console.WriteLine(subDir);
                JObject watchers = jo["watchers"] as JObject;
                watchers.Add(ID.ToString(), JToken.Parse(
                    @"{""name"":" + @"""" + name + @""","
                    + @"""path"":" + @"""" + s + @""","
                    + @"""subdirectories"":" + subDir + "}"));
            } else
            {
                JObject watchers = jo["watchers"][ID.ToString()] as JObject;
                watchers["name"] = name;
                watchers["path"] = s;
                watchers["subdirectories"] = subdirectories;
            }
            File.WriteAllText(jsonPath, jo.ToString());
        }

        private void WatcherChanged(object source, FileSystemEventArgs e)
        {
            string extension = Path.GetExtension(e.FullPath);
            if (!input.Contains(extension))
                return;

            string name = e.Name.Remove(0, e.Name.LastIndexOf("\\") + 1);
            int nameLength = name.Length;
            string filePath = @e.FullPath;
            filePath = "\"" + filePath.Remove(filePath.Length - nameLength) + "\"";
            int i = name.IndexOf(".");
            string newName = name.Remove(i, nameLength - i) + output;
            filePath = filePath.Remove(filePath.Length - 1) + newName + "\"";

            Console.WriteLine("\n" + "File \'" + name + "\' was modified.");
            Console.WriteLine("Compiling to " + filePath + ".");
            string s1 = "\n" + "File \'" + name + "\' was modified.";
            string s2 = "\n" + "Compiling to " + filePath + ".\n";
            Output(s1);
            Output(s2);

            string c = command;
            int j = c.IndexOf("!input!");
            if (j != -1)
            {
                c = c.Remove(j, 7);
                c = c.Insert(j, " \"" + e.FullPath + "\"");
            }
            j = c.IndexOf("!output!");
            if (j != -1)
            {
                c = c.Remove(j, 8);
                c = c.Insert(j, filePath);
            }
            ExecuteCommand(c);
        }
        delegate void SetTextCallback(string text);

        private void Output(string text)
        {
            if (WatcherWindow.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Output);
                WatcherWindow.txtLog.Invoke(d, new object[] { text });
            }
            else
            {
                if (text.Substring(0,1).Equals("\n"))
                {
                    text = text.Substring(1);
                    WatcherWindow.txtLog.AppendText("\n");
                }
                WatcherWindow.txtLog.SelectionColor = System.Drawing.Color.FromArgb(86,156,214);
                WatcherWindow.txtLog.AppendText("Watcher "+ID+": ");
                WatcherWindow.txtLog.SelectionColor = WatcherWindow.txtLog.ForeColor;
                WatcherWindow.txtLog.AppendText(text);
            }
        }
        private void Output(string text, System.Drawing.Color color)
        {
            if (WatcherWindow.txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Output);
                WatcherWindow.txtLog.Invoke(d, new object[] { text });
            }
            else
            {
                WatcherWindow.txtLog.SelectionColor = color;
                WatcherWindow.txtLog.AppendText(text);
                WatcherWindow.txtLog.SelectionColor = WatcherWindow.txtLog.ForeColor;
            }
        }

        public static string ExecuteCommand(string command)
        {

            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/C" + command)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;
                proc.Start();

                string output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                {
                    output = proc.StandardError.ReadToEnd();
                }
                return output;
            }
        }

    }
}
