using System;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Management;
using IWshRuntimeLibrary;
using OBSWebsocketDotNet;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Microsoft.Win32.TaskScheduler;

namespace OBS_Supporter
{
    public partial class frmOBSSupporter : Form
    {
        //Variables---------------------------------------------------------------------------------------------------------------
        private readonly Main main;
        FileSystemWatcher OBS_RBDWatcher;
        public ManagementEventWatcher startWatch;
        private OBSScene[] sceneNames;
        public string[] sceneNamesStr;
        private bool connected = false;
        public string[][] allSceneGames = new string[0][];
        public sceneConfigControlLine controlLineList;
        public string missingGames = "Couldn't find the following Games:\n";

        //Initializing---------------------------------------------------------------------------------------------------------------
        public frmOBSSupporter()
        {
            InitializeComponent();

            controlLineList = new Empty(this);
            main = new Main(this);
            nfiTrayIcon.Visible = true;

            // Watch started Processes
            watchOut();

            // Load Settings
            loadSettings();

            // Set PasswordTextBox
            tbxPassword.setPasswordChar('*');
        }
    
        //distributes Event-Watchers
        public void watchOut()
        {   startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(main.startWatch_EventArrived);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(main.stopWatch_EventArrived);
            stopWatch.Start();
        }

        /// <summary>
        /// Fills Form based on connection state.
        /// </summary>
        /// <param name="connectionState"> Connection-State </param>
        private void fillForm(bool connectionState)
        {
            // enables/disables Scene-Configuration
            pnlSceneConfig.Enabled = connectionState;
            if (connectionState)
            {
                // fills ComboBoxes with SceneNames
                fillComboBoxes();

                // shows Connection State
                lblConnection.Text = "*connected";
                lblConnection.ForeColor = System.Drawing.Color.Green;

                // refreshes shown Recording Path
                getRecordingPath();

                // transform connect Button to disconnect Button
                btnOpenConnect.Text = "Disconnect";
                btnOpenConnect.Click -= btnOpenConnect_Click;
                btnOpenConnect.Click += btnDisconnect_Click;
            }
            else
            {
                // shows Connection State
                lblConnection.Text = "*not connected";
                lblConnection.ForeColor = System.Drawing.Color.Red;

                // transform disconnect Button to connect Button
                btnOpenConnect.Text = "Connect to OBS";
                btnOpenConnect.Click -= btnDisconnect_Click;
                btnOpenConnect.Click += btnOpenConnect_Click;
            }
        }

        /// <summary>
        /// Loads saved Settings.
        /// </summary>
        private void loadSettings()
        {
            // Load Scene-Configurations
            allSceneGames = Properties.Settings.Default.savedSceneGames;
            if (allSceneGames != null)
            {
                sceneNamesStr = new string[allSceneGames.Length];
                for (int i = 0; i < allSceneGames.Length; i++)
                {
                    sceneNamesStr[i] = allSceneGames[i][0];
                }
                for (int i = 0; i < allSceneGames.Length; i++)
                {
                    addSceneConfig(allSceneGames[i]);
                }
                if (missingGames != "Couldn't find the following Games:\n")
                {
                    MessageBox.Show(missingGames);
                }
            } 
            else
            {
                allSceneGames = new string[1][];
                allSceneGames[0] = new string[0];
            }

            // Load Utility-Applications
            System.Collections.ArrayList utilityApplications = Properties.Settings.Default.savedUtilityProcesses;
            if (utilityApplications != null) controlLineList.loadAllUtilityApplications(utilityApplications, 0);

            // Load Notification Sound-Setting
            cbxNotificationSound.Checked = Properties.Settings.Default.savedNotificationSound;

            // Load Start-On-Boot-Setting
            string taskPath = Properties.Settings.Default.savedTaskPath;
            TaskService ts = new TaskService();
            cbxStartOnBoot.Checked = ts.RootFolder.Tasks.Exists(taskPath);

            // Load OBS-Path
            tbxObsPath.setText(main.obsPath);

            // Load Port and Password
            tbxPort.setText(Properties.Settings.Default.savedPort.ToString());
            tbxPassword.setText(Properties.Settings.Default.savedPassword);
            cbxSavePassword.Checked = !tbxPassword.getText().Equals("");

            btnOK.Enabled = false;
            btnApply.Enabled = false;
        }

        //form-Events---------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Hides the Form on launch and shows the Tray-Icon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOBSSupporter_Shown(object sender, EventArgs e)
        {
            loadSettings();

            // check for Commandline-Arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1].Equals("-min"))
            {
                Hide();
            } else
            {
                fillForm(false);
            }
        }

        /// <summary>
        /// Shows Form on Tray-Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nfiTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Hides Form on minimize / Loads Settings on Normalize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOBSSupporter_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
            if (WindowState == FormWindowState.Normal)
            {
                loadSettings();
                btnOK.Enabled = false;
                btnApply.Enabled = false;
            }
        }

        //Settings Changed
        private void tbxObsPath_TextChanged(object sender, EventArgs e)
        {
            if ((tbxObsPath.getText() == "") || (!tbxObsPath.getText().Contains("obs64.exe")))
            {
                lblInvalid.Visible = true;
            }
            else if (tbxObsPath.getText() != main.obsPath)
            {
                btnOK.Enabled = true;
                btnApply.Enabled = true;
            }
        }
        private void tbxPort_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(tbxPort.getText(), out _))
            {
                btnOK.Enabled = true;
                btnApply.Enabled = true;
                lblInvalidPort.Visible = false;
            }
            else
            {
                lblInvalidPort.Visible = true;
            }
        }
        private void tbxPassword_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }
        private void cbxShowConsoleOnBoot_CheckStateChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }
        private void cbxSavePassword_CheckStateChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }
        public void pnlControlRemoved(object sender, ControlEventArgs e)
        {
            btnOK.Enabled = false;
        }
        public void pnlControlAdded(object sender, ControlEventArgs e)
        {
            btnOK.Enabled = true;
        }
        public void cobxSelectedValueChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

        /// <summary>
        /// Manually opens and connects OBS.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenConnect_Click(object sender, EventArgs e)
        {
            main.obsProcess.Start();
        }

        /// <summary>
        /// Stops the Replay-Buffer and closes OBS.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            main.stopReplayBufferAndCloseOBS();
        }
        
        /// <summary>
        /// Saves all Settings.
        /// </summary>
        private void applySettings()
        {
            lblInvalid.Visible = false;
            lblInvalidPort.Visible = false;
            main.obsPath = Path.GetFullPath(tbxObsPath.getText());
            main.removeObsLnk();
            main.createObsLnk();
            main.createObsProcess();
            btnOpenConnect.Enabled = true;

            Properties.Settings.Default.savedSceneGames = controlLineList.getAllSceneGames();
            Properties.Settings.Default.savedOBSPath = main.obsPath;
            Properties.Settings.Default.savedUtilityProcesses = controlLineList.getAllUtilityApplications();
            Properties.Settings.Default.savedNotificationSound = cbxNotificationSound.Checked;
            Properties.Settings.Default.savedPort = int.Parse(tbxPort.getText());
            Properties.Settings.Default.savedPassword = tbxPassword.getText();

            if (cbxSavePassword.Checked)
            {
                Properties.Settings.Default.savedPassword = tbxPassword.getText();
            }
            else
            {
                Properties.Settings.Default.savedPassword = "";
            }

            Properties.Settings.Default.Save();

            if (cbxStartOnBoot.Checked)
            {
                removeScheduleTask();
                createScheduleTask();
            }
            else
            {
                removeScheduleTask();
            }

            btnOK.Enabled = false;
            btnApply.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            applySettings();
            Hide();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            applySettings();
        }

        private void btnAddSceneConfig_Click(object sender, EventArgs e)
        {
            addSceneConfig(null);
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }

        private void btnAddSceneConfig_LocationChanged(object sender, EventArgs e)
        {
            pnlSceneConfig.Size = new Size(pnlSceneConfig.Size.Width, 52 + 37 * controlLineList.getLength());
        }

        //Scene-Configuration--------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Fills ComboBoxes with Scene-Names
        /// </summary>
        private void fillComboBoxes()
        {
            controlLineList.fillComboboxes(sceneNamesStr);
        }

        public T[] addArrayElement<T> (T[] input, T nextElement)
        {
            T[] output = new T[input.Length + 1];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = input[i];
            }
            output[output.Length - 1] = nextElement;
            return output;
        }

        public static bool arrayContains<T> (T[] array, T element)
        {
            bool found = false;
            for (int i = 0; i < array.Length && !found; i++)
            {
                found = array[i].Equals(element);
            }
            return found;
        }

        public void addSceneConfig(string[] games)
        {            
            controlLineList = controlLineList.addControlLine(games);
            pnlSceneConfig.Controls.AddRange(controlLineList.draw(controlLineList.getLength()));
            
            btnAddSceneConfig.Location = new Point(6, 22 + 37 * controlLineList.getLength());

            controlLineList.fillComboboxes(sceneNamesStr);
        }

        public Panel getPnlSceneConfig()
        {
            return pnlSceneConfig;
        }

        public void setSceneAfterAdd(string scene)
        {
            main.gameStarted(scene);
        }

        //WebSocket-Events--------------------------------------------------------------------------------------------------------------
        
        /// <summary>
        /// sets up the Form when connected
        /// </summary>
        public void onConnect()
        {
            // retrieving Scene-Names from OBS
            for (int i = 0; i < 3; i++)
            {
                sceneNames = main._obs.ListScenes().ToArray();
            }
            sceneNamesStr = new string[sceneNames.Length];
            for (int i = 0; i < sceneNames.Length; i++)
            {
                sceneNamesStr[i] = sceneNames[i].Name;
            }

            // enables Connection Based GUI Functionality
            if (WindowState == FormWindowState.Normal)
            {
                connected = true;
                fillForm(true);
            }
            else
            {
                connected = true; //TODO: nötig? vtl fillform(false);
                controlLineList.fillComboboxes(sceneNamesStr);
            }

        } //Todo contorllinelist laden reichschaeun

        /// <summary>
        /// Disables Connection-Based GUI-Features
        /// </summary>
        public void onDisconnect()
        {
            connected = false;
            fillForm(false);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                RestoreDirectory = false
            };
            if (oFDialog.ShowDialog() == DialogResult.OK)
            {
                tbxObsPath.setText(oFDialog.FileName);
            }
        }

        private void makeSettingsVisible(Panel panel)
        {
            pnlGeneral.Visible = false;
            pnlConsole.Visible = false;
            pnlFixes.Visible = false;
            pnlSceneConfig.Visible = false;

            panel.Visible = true;
        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlGeneral);
            btnGeneral.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnConsole.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnSceneConfig.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnFixes.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);

            btnGeneral.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnConsole.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnSceneConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnFixes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlConsole);
            btnConsole.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnSceneConfig.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnFixes.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);

            btnConsole.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnSceneConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnFixes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);

        }

        private void btnSceneConfig_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlSceneConfig);
            btnSceneConfig.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnConsole.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnFixes.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);

            btnSceneConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnConsole.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnFixes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
        }

        private void btnFixes_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlFixes);
            btnFixes.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnConsole.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);
            btnSceneConfig.BackColor = System.Drawing.Color.FromArgb(31, 30, 31);

            btnFixes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btnGeneral.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnConsole.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            btnSceneConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
        }

        private void btnRefreshPath_Click(object sender, EventArgs e)
        {
            getRecordingPath();
        }

        private void getRecordingPath()
        {
            if (connected)
            {
                string recordingPath = @main._obs.GetRecordingFolder().Replace("/", "\\");
                lblRecordingPath.Text = "Recording Path:   " + recordingPath;
                OBS_RBDWatcher = new FileSystemWatcher(recordingPath)
                {
                    EnableRaisingEvents = true
                };
                OBS_RBDWatcher.Changed += main.RBDChanged;
            }
        }

        private void cbxStartOnBoot_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }

        private void llblTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            main.playNotificationSound();
        }

        private void cbxNotificationSound_CheckedChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = true;
            btnApply.Enabled = true;
        }

        public void enableBtnOpenConnect(bool enable)
        {
            btnOpenConnect.Enabled = enable;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }

        // Application Methods-------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Creates a Task in the Task Scheduler which will launch the App on every login.
        /// </summary>
        private void createScheduleTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "launches OBS Supporter with logon";
                td.Triggers.Add(new LogonTrigger());
                td.Actions.Add(new ExecAction(System.Reflection.Assembly.GetEntryAssembly().Location, "-min", null));
                td.Principal.RunLevel = TaskRunLevel.Highest;
                ts.RootFolder.RegisterTaskDefinition(@"OBS Supporter", td);
            }
            Properties.Settings.Default.savedTaskPath = "OBS Supporter";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Removes the Task from the Task Scheduler which launches the App on System Boot.
        /// </summary>
        private void removeScheduleTask()
        {
            string taskPath = Properties.Settings.Default.savedTaskPath;
            if (taskPath != "")
            {
                try
                {
                    using (TaskService ts = new TaskService())
                    {
                        ts.RootFolder.DeleteTask(taskPath);
                    }
                    Properties.Settings.Default.savedTaskPath = "";
                    Properties.Settings.Default.Save();
                }
                catch { }
            }
        }

        /// <summary>
        /// Writes a Message into the Application Console with the specified Color
        /// </summary>
        /// <param name="color"> The Color of the Message </param>
        /// <param name="message"> The Output Message </param>
        public void writeInConsole(System.Drawing.Color color, string message)
        {
            Invoke(new MethodInvoker(delegate {
                rtbxConsole.SelectionStart = rtbxConsole.TextLength;
                rtbxConsole.SelectionLength = 0;

                rtbxConsole.SelectionColor = color;
                rtbxConsole.AppendText(message + Environment.NewLine);
                rtbxConsole.ScrollToCaret();
                rtbxConsole.SelectionColor = System.Drawing.Color.White;
            }));
        }

        public int getPort()
        {
            return int.Parse(tbxPort.getText());
        }

        public string getPassword()
        {
            return tbxPassword.getText();
        }
    }

    public class Main
    {
        public OBSWebsocket _obs = new OBSWebsocket();
        private string isScene;
        private string isProfile;
        private string wantScene = "";
        private readonly string wantProfile = "";
        private bool replayBufferState = false;
        private readonly frmOBSSupporter supporterForm;
        public WshShell shell1;
        public IWshShortcut shortcut1;
        public Process obsProcess;
        UInt32 obsProcessID;
        public string obsPath = Properties.Settings.Default.savedOBSPath;
        private bool onConnectTriggered = false;
        private bool opened = false;
        private bool launchInit = false;
        private bool closingOBS = false;
        private UInt32 currentAppID = 0;
        private Thread thread;
        private int connectionAttempts;


        public Main(frmOBSSupporter e)
        {
            supporterForm = e;
            _obs.Connected += onConnect;
            _obs.Disconnected += onDisconnect;
            _obs.SceneChanged += onSceneChange;
            if (obsPath != "")
            {
                createObsLnk();
                createObsProcess();
                supporterForm.Enabled = true;
            }
        }

        //Watcher-Events-------------------------------------------------------------------------------------------------------------
        public void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string process = (string)e.NewEvent.Properties["ProcessName"].Value;
            string scene = supporterForm.controlLineList.getScene(process);
            supporterForm.writeInConsole(System.Drawing.Color.White, process);

            if (scene != null)
            {
                currentAppID = (UInt32)e.NewEvent.Properties["ProcessID"].Value;
                supporterForm.writeInConsole(System.Drawing.Color.White, "currentGame: " + process + "(" + currentAppID + ")");
                gameStarted(scene);
                supporterForm.controlLineList.startUtilityApplications();
            }

            if ((string)e.NewEvent.Properties["ProcessName"].Value == "obs64.exe")
            {
                opened = true;
                obsProcessID = (UInt32)obsProcess.Id;
                supporterForm.startWatch.Stop();// TODO: nessacery?
                supporterForm.startWatch.Start();// TODO: nessacery?
                connectionAttempts = 0;
                thread = new Thread(new ThreadStart(connect));
                thread.Start();
            }
        }

        public void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            UInt32 Process = (UInt32)e.NewEvent.Properties["ProcessID"].Value;

            if (Process == obsProcessID)
            {
                opened = false;
                onConnectTriggered = false;
                launchInit = false;
            }
            if (Process == currentAppID && onConnectTriggered)
            {
                stopReplayBufferAndCloseOBS();
                currentAppID = 0;
                supporterForm.controlLineList.closeUtilityApplications();
            }
        }

        //Watcher-Methods---------------------------------------------------------------------------------------------------------------
        public void gameStarted(string scene)
        {
            wantScene = scene;
            if (onConnectTriggered)
            {
                setScene();
                setProfile();
            }
            else
            {
                if (!launchInit)
                {
                    launchInit = true;

                    obsProcess.Start();

                    MediaPlayer mPlayer = new MediaPlayer();
                    mPlayer.Open(new Uri(@"Y:\Users\Fnolikopternator\Musik\Non-music\TTS\OBS has launched.mp3"));
                    mPlayer.Volume = 0.2;
                    mPlayer.Play();
                }
            }
        }

        //_obs-Methods---------------------------------------------------------------------------------------------------------------
        private void setScene()
        {
            refreshSceneName();
            if (isScene != wantScene)
            {
                if (wantScene != "")
                {
                    _obs.SetCurrentScene(wantScene);
                }
            }
        }

        private void setProfile()
        {
            refreshProfileName();
            if (isProfile != wantProfile && wantProfile != "")
            {
                _obs.StopReplayBuffer();
                _obs.SetCurrentProfile(wantProfile);
            }
            if (!replayBufferState)
            {
                _obs.StartReplayBuffer();
                replayBufferState = true;
            }
        }

        private void refreshSceneName()
        {
            isScene = _obs.GetCurrentScene().Name;
        }

        private void refreshProfileName()
        {
            isProfile = _obs.GetCurrentProfile();
        }

        private void replayBufferToggled(object sender, EventArgs e)
        {
            replayBufferState = !replayBufferState;
        }

        public void createObsLnk()
        {
            try
            {
                shell1 = new WshShell();
                string directory = obsPath.Substring(0, obsPath.LastIndexOf("\\"));
                shortcut1 = (IWshShortcut)shell1.CreateShortcut(directory + "OBS_escape.lnk");
                shortcut1.TargetPath = obsPath;
                shortcut1.WorkingDirectory = directory;
                shortcut1.Save();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                supporterForm.enableBtnOpenConnect(false);
            }
        }

        public void removeObsLnk()
        {
            if (shortcut1 != null)
            {
                FileInfo f = new FileInfo(shortcut1.FullName);
                if (f.Exists) f.Delete();
            }
        }

        public void createObsProcess()
        {
            obsProcess = new Process();
            obsProcess.StartInfo.FileName = shortcut1.FullName;
            supporterForm.enableBtnOpenConnect(true);
        }

        /// <summary>
        /// Disconnects from Websocket and Closes OBS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="outputState"></param>
        private void disconnectAndCloseOBS(OBSWebsocket sender, OutputState outputState)
        {
            _obs.ReplayBufferStateChanged -= disconnectAndCloseOBS;
            Thread.Sleep(3000);
            //_obs.Disconnect();
            closingOBS = true;
            supporterForm.writeInConsole(System.Drawing.Color.Red, "CLOSING OBS...");
            obsProcess.CloseMainWindow();
        }

        /// <summary>
        /// Tries to connect to an OBS Websocket.
        /// </summary>
        private void connect()
        {
            if(!onConnectTriggered && opened)
            {
                onConnectTriggered = false;
                supporterForm.writeInConsole(System.Drawing.Color.Yellow, "CONNECTING...");
                try
                {
                    //_obs.Connect("ws://127.0.0.1:4444", "");
                    _obs.Connect("ws://127.0.0.1:" + supporterForm.getPort(), supporterForm.getPassword());
                }
                catch (Exception e)
                {
                    supporterForm.writeInConsole(System.Drawing.Color.Red, "Failed Connecting: " + e.Message);
                }
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// Stops the Replay Buffer and Closes OBS
        /// </summary>
        public void stopReplayBufferAndCloseOBS()
        {
            _obs.ReplayBufferStateChanged += disconnectAndCloseOBS;
            _obs.StopReplayBuffer();
            replayBufferState = false;
        }

        //_obs-Events---------------------------------------------------------------------------------------------------------------
        private void onSceneChange(OBSWebsocket sender, string newSceneName)
        {
            //if (sceneChangedTo != newSceneName)
            //{
            //    //sound for scenechange
            //}
        }

        public void RBDChanged(object sender, FileSystemEventArgs e)
        {
            if (Properties.Settings.Default.savedNotificationSound)
            {
                playNotificationSound();
            }
        }

        public void playNotificationSound()
        {
            System.Media.SoundPlayer sPlayer = new System.Media.SoundPlayer
            {
                Stream = Properties.Resources.AirPlanDing30Percent
            };
            sPlayer.Play();
        }

        /// <summary>
        /// Retrieves Scenes and Profiles on a new Websocket Connection, if no connection already exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onConnect(object sender, EventArgs e)
        {
            if (!onConnectTriggered)
            {
                onConnectTriggered = true;
                supporterForm.writeInConsole(System.Drawing.Color.LightGreen, "CONNECTED");
                setScene();
                setProfile();
                supporterForm.Invoke(new MethodInvoker(delegate { supporterForm.onConnect(); }));
                launchInit = false;
            }
        }

        /// <summary>
        /// Marks OBS as closed and disconnected. If the Disconnect happened without obs being closed, it will reconnect to the Websocket.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onDisconnect(object sender, EventArgs e)
        {
            onConnectTriggered = false;
            if (closingOBS)
            {
                opened = false;
                closingOBS = false;
                supporterForm.writeInConsole(System.Drawing.Color.Red, "DISCONNECTED FROM WEBSOCKET");
                supporterForm.Invoke(new MethodInvoker(delegate { supporterForm.onDisconnect(); }));
                replayBufferState = false;
            } 
            else
            {
                supporterForm.writeInConsole(System.Drawing.Color.Orange, "CONNECTION FAILED");
                if (obsProcess.HasExited)
                {
                    supporterForm.Invoke(new MethodInvoker(delegate { supporterForm.onDisconnect(); }));
                    replayBufferState = false;
                }
                else if (connectionAttempts < 5)
                {
                    connectionAttempts++;
                    thread = new Thread(new ThreadStart(connect));
                    thread.Start();
                }
            }
        }
    }
}