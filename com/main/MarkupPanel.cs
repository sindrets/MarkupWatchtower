using HamlWatchtowerApp;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MarkupWatchtower.com.main
{
    class MarkupPanel
    {
        private MarkupWatcher watcher;
        private const string dirInfo = "Current directory: ";
        private const string btnText = "Change";
        private string dir = "none";
        private object[] comboOptions;

        private readonly Color foreground = Color.FromArgb(215,215,215);
        private readonly Color background = Color.FromArgb(44,44,44);
        private readonly Color borderColor = Color.FromArgb(55,55,55);

        public MarkupPanel()
        {
            readComboOptions();
            watcher = new MarkupWatcher();
            init();
        }

        private void readComboOptions()
        {
            var list = WatcherWindow.mObjects;
            string[] arr = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[i] = list[i].Name;
            }
            comboOptions = arr;
        }

        public Control getPanel()
        {
            return pnlMain;
        }
        public MarkupWatcher getWatcher()
        {
            return watcher;
        }
        public void setDirText(string text)
        {
            dir = text;
            this.lblDir.Text = text;
        }
        public void updateComboBox(string name)
        {
            for (int i = 0; i < options.Items.Count; i++)
            {
                string s = options.Items[i] as string;
                if (s.Equals(name))
                {
                    options.SelectedIndex = i;
                    break;
                }
            }
        }
        public void setSubDir(bool b)
        {
            chkSubDirs.Checked = b;
        }

        private void init()
        {
            options = new ComboBox();
            lblSubDirs = new Label();
            chkSubDirs = new CheckBox();
            btnChange = new Button();
            lblDirInfo = new Label();
            lblDir = new Label();
            btnDelete = new Button();
            pnlMain = new Panel();
            // 
            // options
            // 
            this.options.ForeColor = foreground;
            this.options.BackColor = background;
            this.options.FlatStyle = FlatStyle.Flat;
            this.options.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.options.FormattingEnabled = true;
            this.options.Items.AddRange(comboOptions);
            this.options.Location = new System.Drawing.Point(12, 9);
            this.options.Name = "options";
            this.options.Size = new System.Drawing.Size(117, 24);
            this.options.SelectedIndexChanged += new System.EventHandler(this.options_SelectedIndexChanged);
            this.options.MouseWheel += new MouseEventHandler(this.options_MouseWheel);
            this.options.TabIndex = 0;
            this.options.SelectedIndex = 0;
            //
            // lblSubDirs
            //
            this.lblSubDirs.ForeColor = foreground;
            this.lblSubDirs.AutoSize = true;
            this.lblSubDirs.Location = new System.Drawing.Point(135, 12);
            this.lblSubDirs.Name = "lblSubDirs";
            this.lblSubDirs.Size = new System.Drawing.Size(80, 17);
            this.lblSubDirs.TabIndex = 0;
            this.lblSubDirs.Text = "Subdirectories";
            //
            // chkSubDirs
            //
            this.chkSubDirs.Location = new Point(238, 11);
            this.chkSubDirs.CheckState = CheckState.Checked;
            this.chkSubDirs.CheckStateChanged += new EventHandler(chkSubDirs_StateChanged);
            // 
            // btnChange
            // 
            this.btnChange.ForeColor = foreground;
            this.btnChange.BackColor = background;
            this.btnChange.FlatStyle = FlatStyle.Flat;
            this.btnChange.FlatAppearance.BorderColor = borderColor;
            this.btnChange.Location = new System.Drawing.Point(12, 43);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(117, 29);
            this.btnChange.TabIndex = 1;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // lblDirInfo
            // 
            this.lblDirInfo.ForeColor = foreground;
            this.lblDirInfo.AutoSize = true;
            this.lblDirInfo.Location = new System.Drawing.Point(135, 49);
            this.lblDirInfo.Name = "lblDirInfo";
            this.lblDirInfo.Size = new System.Drawing.Size(120, 17);
            this.lblDirInfo.TabIndex = 0;
            this.lblDirInfo.Text = "Current Directory:";
            // 
            // lblDir
            // 
            this.lblDir.ForeColor = foreground;
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(252, 49);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(40, 17);
            this.lblDir.TabIndex = 1;
            this.lblDir.Text = dir;
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = foreground;
            this.btnDelete.BackColor = background;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderColor = borderColor;
            this.btnDelete.Location = new System.Drawing.Point(542, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 28);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete Watcher";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = background;
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.btnChange);
            this.pnlMain.Controls.Add(this.options);
            this.pnlMain.Controls.Add(this.lblSubDirs);
            this.pnlMain.Controls.Add(this.chkSubDirs);
            this.pnlMain.Controls.Add(this.lblDirInfo);
            this.pnlMain.Controls.Add(this.lblDir);
            this.pnlMain.Controls.Add(this.btnDelete);
            this.pnlMain.Location = new System.Drawing.Point(3, 3);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Size = new System.Drawing.Size(670, 86);
            this.pnlMain.Anchor = (AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right);
            this.pnlMain.TabIndex = 3;
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
                WatcherWindow.txtLog.AppendText(text);
            }
        }

        //OPTIONS
        private void options_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateWatcherArgs(options.GetItemText(options.SelectedItem));
        }
        private void options_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!((ComboBox)sender).DroppedDown)
                ((HandledMouseEventArgs)e).Handled = true;
        }

        //CHKSUBDIRS
        private void chkSubDirs_StateChanged(object sender, EventArgs e)
        {
            watcher.setIncludeSubdirectories(((CheckBox)sender).Checked);
            if (!dir.Equals("none"))
                watcher.saveToJson();
        }

        //CHANGE DIR
        private void btnChange_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                watcher.setFolderPath(fbd.SelectedPath);
                watcher.update();
                lblDir.Text = fbd.SelectedPath;
                dir = fbd.SelectedPath;
                watcher.saveToJson();
            }
        }
        //DELETE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            watcher.stopWatcher();
            pnlMain.Visible = false;
            pnlMain.Enabled = false;
        }

        private void updateWatcherArgs(string markup)
        {
            watcher.setMObject(WatcherWindow.getMObject(markup));
            if (!dir.Equals("none"))
            {
                watcher.Log();
                watcher.saveToJson();
            }
        }

        private Panel pnlMain;
        private ComboBox options;
        private Label lblSubDirs;
        private CheckBox chkSubDirs;
        private Button btnChange;
        private Label lblDirInfo;
        private Label lblDir;
        private Button btnDelete;

    }
}
