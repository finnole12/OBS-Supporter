using System;
using System.Windows.Forms;
using System.Management;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace OBS_Supporter
{
    public class ControlLine : sceneConfigControlLine
    {
        //Variables---------------------------------------------------------------------------------------------------------------
        public sceneConfigControlLine next;

        private frmOBSSupporter parent;
        private Label lblScene;
        private ComboBox cobxScenes;
        private string cobxSelItem;
        private Panel pnlGames;
        private Button btnAddGames;
        private ImageList imglIcons = new ImageList();
        private string[] sceneGames = new string[0];
        private Label lblNoGames = new Label();
        private Label lblNoScene = new Label();
        private PictureBox[] pbs = new PictureBox[0];
        private Button btnDeleteLine;
        private SortedDictionary<string, StringCollection> utilityApplications = new SortedDictionary<string, StringCollection>();
        private string runningGame = "";
        private List<Process> utilityProcesses = new List<Process>();

        //Initializing---------------------------------------------------------------------------------------------------------------
        public ControlLine(frmOBSSupporter par, string[] sg , sceneConfigControlLine n)
        {
            next = n;

            parent = par;
            lblScene = new Label();
            lblScene.AutoSize = true;
            lblScene.Text = "Scene " + getLength();
            lblScene.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            cobxScenes = new ComboBox();
            cobxScenes.Size = new Size(160, 21);
            cobxScenes.FlatStyle = FlatStyle.Flat;
            cobxScenes.SelectedValueChanged += new EventHandler(cobxScenes_SelectedValueChanged);
            cobxScenes.SelectionChangeCommitted += new EventHandler(cobxScene_SelectionChangeCommitted);
            cobxScenes.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            pnlGames = new Panel();
            pnlGames.Name = "" + getLength();
            pnlGames.Size = new Size(234, 21);
            pnlGames.ControlAdded += new ControlEventHandler(pnlControlAdded);
            pnlGames.ControlRemoved += new ControlEventHandler(pnlControlRemoved);
            pnlGames.Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right;

            btnAddGames = new Button();
            btnAddGames.Size = new Size(75, 23);
            btnAddGames.Text = "Add Games";
            btnAddGames.Click += new EventHandler(btnAddGames_Click);
            btnAddGames.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            if (sg != null)
            {
                cobxSelItem = sg[0];
                for (int i = 1; i < sg.Length; i++)
                {
                    try
                    {
                        addGame(sg[i]);
                    }
                    catch
                    {
                        parent.missingGames += Environment.NewLine + sg[i];
                    }
                }
            }

            lblNoGames.Text = "* no Games are Selected";
            lblNoGames.ForeColor = Color.Red;
            lblNoGames.Size = new Size(105, 13);
            lblNoGames.AutoSize = true;
            lblNoGames.Visible = false;
            lblNoGames.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            lblNoScene.Text = "* no Scene selected";
            lblNoScene.ForeColor = Color.Red;
            lblNoScene.Size = new Size(103, 13);
            lblNoScene.AutoSize = true;
            lblNoScene.Visible = false;
            lblNoScene.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            btnDeleteLine = new Button();
            btnDeleteLine.Text = "remove";
            btnDeleteLine.Size = new Size(55, 23);
            btnDeleteLine.Click += new EventHandler(removeMe);
            btnDeleteLine.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        }
        
        public void fillComboboxes(string[] scenes)
        {
            // clear the comboBox
            cobxScenes.Items.Clear();

            // setup array with unused Scenes
            String[] remaining = new String[scenes.Length -1];


            if (cobxSelItem == null)
            {
                // add all scenes to the comboBox
                cobxScenes.Items.AddRange(scenes);
                remaining = scenes;
            }
            else
            {
                cobxScenes.Items.Add(cobxSelItem);
                cobxScenes.SelectedItem = cobxScenes.Items[0];
                int d = 0;
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (!getAllSelectedItems().Contains(scenes[i]))
                    {
                        cobxScenes.Items.Add(scenes[i]);
                    }
                    if (cobxSelItem.Equals(scenes[i]))
                    {
                        d++;
                    }
                    else
                    {
                        remaining[i - d] = scenes[i];
                    }
                }
            }
            next.fillComboboxes(remaining);
        }

        //Config Line Methods--------------------------------------------------------------------------------------------------------
        public void btnAddGames_Click(Object sender, EventArgs e)
        {
            WindowListForm wlF = new WindowListForm(this);
            wlF.TopMost = true;
            wlF.Show();
        }

        public void setSceneAfterAdd(string scene)
        {
            parent.setSceneAfterAdd(cobxSelItem);
        }

        public void addGame(string path)
        {
            // create new PictureBox
            imglIcons.Images.Add(Icon.ExtractAssociatedIcon(path));
            PictureBox pb = new PictureBox();
            pb.Size = new Size(16, 16);
            pb.Image = imglIcons.Images[imglIcons.Images.Count - 1];
            pb.ContextMenu = buildIconContextMenu(path);

            // add new PictureBox to PictureBox array
            pbs = parent.addArrayElement(pbs, pb);

            // add all PictreBox to the Panel once again
            pnlGames.Controls.Clear();
            pnlGames.Controls.AddRange(pbs);

            // rearange all PictureBoxes in the Panel
            for (int i = 0; i < pnlGames.Controls.Count; i++)
            {
                pnlGames.Controls[i].Location = new Point(20 *i, 0);
            }

            // add the path to the sceneGames array
            sceneGames = parent.addArrayElement(sceneGames, path);
        }

        private void removeGame(string path)
        {
            String[] shortetSceneGames = new String[sceneGames.Length - 1];
            PictureBox[] shortedPbs = new PictureBox[pbs.Length - 1];
            int index = 0;
            int one = 0;
            for (int i = 0; i < sceneGames.Length; i++)
            {
                if (sceneGames[i] == path) 
                {
                    one = 1;
                    index = i;
                }
                else
                {
                    shortetSceneGames[i - one] = sceneGames[i];
                    shortedPbs[i - one] = pbs[i];
                }
            }
            sceneGames = shortetSceneGames;                        //Eintrag aus scenegames entfernen
            pbs = shortedPbs;                                    //PictureBox aus pbs entfernen

            pnlGames.Controls.RemoveAt(index);                   //PictrueBox aus dem Panel entfernen

            for (int i = 0; i < pnlGames.Controls.Count; i++)
            {
                pnlGames.Controls[i].Location = new Point(20 * i, 0);   //rearange Panel
            } 
        }

        private void addUtilityApplication(string path)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                StringCollection stringCollection;
                if (utilityApplications.ContainsKey(path))
                {
                    stringCollection = utilityApplications[path];
                }
                else
                {
                    stringCollection = new StringCollection();
                    utilityApplications.Add(path, stringCollection);
                }
                stringCollection.Add(ofd.FileName);
                utilityApplications[path] = stringCollection;
            }
        }

        private void showUtilityApplications(string path)
        {
            string output = "";
            if (utilityApplications.ContainsKey(path))
            {
                StringCollection stringCollection = utilityApplications[path];
                foreach (string s in stringCollection)
                {
                    output += s + Environment.NewLine;
                }
            }
            MessageBox.Show(output);
        }

        private ContextMenu buildIconContextMenu(string path)
        {
            ContextMenu cMS = new ContextMenu();
            MenuItem open = new MenuItem();
            open.Text = "open " + path.Substring(path.LastIndexOf("\\") + 1);
            open.Click += new EventHandler((sender, e) => Process.Start(path));

            MenuItem remove = new MenuItem();
            remove.Text = "remove";
            remove.Click += new EventHandler((sender, e) => removeGame(path));

            MenuItem addUtility = new MenuItem();
            addUtility.Text = "add Utility Applications";
            addUtility.Click += new EventHandler((sender, e) => addUtilityApplication(path));

            MenuItem showUtility = new MenuItem();
            showUtility.Text = "show Utility Applications";
            showUtility.Click += new EventHandler((sender, e) => showUtilityApplications(path));

            cMS.MenuItems.AddRange(new MenuItem[]
            {
                open,
                remove,
                addUtility,
                showUtility
            });
            return cMS;
        }
        private void removeMe(Object sender, EventArgs e)
        {
            int length = parent.controlLineList.getLength() - getLength();
            String[][] unsavedSceneGames = parent.controlLineList.getAllSceneGames();
            parent.controlLineList = new Empty(parent);
            String[][] newAllSceneGames = new String[unsavedSceneGames.Length - 1][];
            int d = 0;
            for (int i = 0; i < unsavedSceneGames.Length; i++)
            {
                if (i != length)
                {
                    newAllSceneGames[i - d] = unsavedSceneGames[i];
                }
                else
                {
                    d = 1;
                }
            }

            Panel pnlSceneConfig = parent.getPnlSceneConfig();
            Control btnAddGames = parent.Controls.Find("btnAddSceneConfig", true)[0];
            pnlSceneConfig.Controls.Clear();
            pnlSceneConfig.Controls.Add(btnAddGames);
            if(newAllSceneGames.Length == 0)
            {
                btnAddGames.Location = new Point(6, 22);
            }
            for (int i = 0; i < newAllSceneGames.Length; i++)
            {
                parent.addSceneConfig(newAllSceneGames[i]);
            }
        }

        public sceneConfigControlLine removeControlLine(int index)
        {
            if (index > 0)
            {
                next = next.removeControlLine(index - 1);
                return this;
            }
            else
            {
                return next;
            }
        }

        //Events---------------------------------------------------------------------------------------------------------------------
        //ComboBox Events
        public void cobxScenes_SelectedValueChanged(Object comboBox, EventArgs e)
        {
            cobxSelItem = (String)cobxScenes.SelectedItem;
            if (pnlGames.Controls.Count != 0)
            {
                parent.cobxSelectedValueChanged(comboBox, e);
                lblNoGames.Visible = false;
                lblNoScene.Visible = false;
            }
            else
            {
                lblNoGames.Visible = true;
            }
        }
        public void cobxScene_SelectionChangeCommitted(Object comboBox, EventArgs e)
        {
            cobxSelItem = (String)cobxScenes.SelectedItem;
            parent.controlLineList.fillComboboxes(parent.sceneNamesStr);
        }
        //Panel Events
        private void pnlControlRemoved(Object panel, ControlEventArgs e)
        {
            if (pnlGames.Controls.Count == 0)
            {
                parent.pnlControlRemoved(panel, e);
            }
            lblNoGames.Visible = true;
        }
        private void pnlControlAdded(Object panel, ControlEventArgs e)
        {
            if (cobxSelItem != null)
            {
                parent.pnlControlAdded(panel, e);
                lblNoGames.Visible = false;
                lblNoScene.Visible = false;
            }
            else
            {
                lblNoScene.Visible = true;
            }
        }

        //List Methods---------------------------------------------------------------------------------------------------------------
        public string[][] getAllSceneGames()
        {
            string[][] nextsg = next.getAllSceneGames();
            string[][] allSceneGames = new string[nextsg.Length + 1][];
            for(int i = 0; i < nextsg.Length; i++)
            {
                allSceneGames[i +1] = nextsg[i];
            }
            string[] sceneGamesWithScene = new string[sceneGames.Length + 1];
            sceneGamesWithScene[0] = cobxSelItem;
            for (int i = 1; i < sceneGamesWithScene.Length; i++)
            {
                sceneGamesWithScene[i] = sceneGames[i - 1];
            }
            allSceneGames[0] = sceneGamesWithScene;
            return allSceneGames;
        }  //Used only for saving

        public System.Collections.ArrayList getAllUtilityApplications()
        {
            System.Collections.ArrayList returnColl = new System.Collections.ArrayList();
            System.Collections.ArrayList addColl = next.getAllUtilityApplications();
            returnColl.Add("line");
            SortedDictionary<string, StringCollection>.KeyCollection keyColl = utilityApplications.Keys;
            foreach (string key in keyColl)
            {
                StringCollection valColl = utilityApplications[key];
                returnColl.Add("game");
                returnColl.Add(key);
                foreach (string val in valColl)
                {
                    returnColl.Add(val);
                }
            }
            returnColl.AddRange(addColl);
            return returnColl;
        }

        public void loadAllUtilityApplications(System.Collections.ArrayList savedUA, int index)
        {
            bool uApps = false;
            bool first = true;
            bool end = false;
            StringCollection valColl = new StringCollection();
            string key = "";
            int i = 0;
            for (i = index; i < savedUA.Count && !end; i++)
            {
                if ((string)savedUA[i] == "game")
                {
                    if (uApps)
                    {
                        string[] copy = new string[valColl.Count];
                        valColl.CopyTo(copy, 0);
                        StringCollection loaded = new StringCollection();
                        loaded.AddRange(copy);
                        utilityApplications.Add(key, loaded);
                    }
                    valColl = new StringCollection();
                    i++;
                    key = (string)savedUA[i];
                    uApps = true;
                }
                else if ((string)savedUA[i] == "line")
                {
                    first = !first;
                    end = first;
                    if (end) i--;
                }
                else if (uApps)
                {
                    valColl.Add((string)savedUA[i]);
                }
            }
            if (uApps)
            {
                string[] copy = new string[valColl.Count];
                valColl.CopyTo(copy, 0);
                StringCollection loaded = new StringCollection();
                loaded.AddRange(copy);
                utilityApplications.Add(key, loaded);
            }
            next.loadAllUtilityApplications(savedUA, i);
        }

        public string getScene(string process)
        {
            bool contains = false;
            for (int i = 0; i < sceneGames.Length && !contains; i++)
            {
                contains = sceneGames[i].Substring(sceneGames[i].LastIndexOf("\\") + 1).Equals(process);
                runningGame = contains ? sceneGames[i] : "";
            }
            return contains ? cobxSelItem : next.getScene(process);
        }

        public int getLength()
        {
            return 1 + next.getLength();
        }

        public string getAllSelectedItems()
        {
            return ((cobxSelItem == null) ? "" : cobxSelItem + ",") + next.getAllSelectedItems();
        }

        public Control[] draw(int index)
        {
            if (next is ControlLine)
            {
                return next.draw(index);
            }
            else
            {
                lblScene.Text = "Scene " + index;

                lblScene.Location = new Point(8, 22 + 37 * (index - 1));
                cobxScenes.Location = new Point(72, 19 + 37 * (index - 1));
                pnlGames.Location = new Point(238, 21 + 37 * (index - 1));
                btnAddGames.Location = new Point(508, 18 + 37 * (index - 1));
                btnDeleteLine.Location = new Point(583, 18 + 37 * (index - 1));

                lblNoGames.Location = new Point(238, pnlGames.Location.Y + 24);
                lblNoScene.Location = new Point(72, cobxScenes.Location.Y + 23);

                for (int i = 0; i < pnlGames.Controls.Count; i++)
                {
                    pnlGames.Controls[i].Location = new Point(20 * i, 0);
                }

                return new Control[] { lblScene, cobxScenes, pnlGames, btnAddGames, btnDeleteLine };
            }
        }

        public sceneConfigControlLine addControlLine(string[] sg)
        {
            next = next.addControlLine(sg);
            return this;
        }

        public void startUtilityApplications()
        {
            if (utilityApplications.ContainsKey(runningGame))
            {
                StringCollection stringCollection = utilityApplications[runningGame];
                foreach (string s in stringCollection)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = s;
                    utilityProcesses.Add(p);
                    p.Start();
                }
            }
        }

        public void closeUtilityApplications()
        {
            foreach (Process p in utilityProcesses)
            {
                try
                {
                    KillProcessAndChildrens(p.Id);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            utilityProcesses.Clear();
            runningGame = "";
        }

        private static void KillProcessAndChildrens(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }

            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }
    }
}