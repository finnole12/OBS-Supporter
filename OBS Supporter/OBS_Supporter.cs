using System;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media;
using System.Management;
using IWshRuntimeLibrary;
using OBSWebsocketDotNet;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using Microsoft.Win32.TaskScheduler;

namespace OBS_Supporter
{
    public partial class frmOBSSupporter : Form
    {
        //Variables---------------------------------------------------------------------------------------------------------------
        private Main main;
        FileSystemWatcher OBS_RBDWatcher;
        public ManagementEventWatcher startWatch;
        private OBSScene[] sceneNames;
        public string[] sceneNamesStr;
        private Boolean consoleVisible;
        private Boolean connected = false;
        public string[][] allSceneGames = new string[0][];
        public sceneConfigControlLine controlLineList;
        public string missingGames = "Couldn't find the following Games:\n";

        //Console Access-------------------------------
        private IntPtr handle;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        //Initializing---------------------------------------------------------------------------------------------------------------
        public frmOBSSupporter()
        {
            handle = GetConsoleWindow();
            if (!Properties.Settings.Default.savedConsoleOnLaunch)
            {
                ShowWindow(handle, SW_HIDE);
                consoleVisible = false;
            }

            InitializeComponent();
            controlLineList = new Empty(this);
            main = new Main(this);
            nfiTrayIcon.Visible = true;
            watchOut();

            allSceneGames = Properties.Settings.Default.savedSceneGames;
            if (allSceneGames != null)
            {
                sceneNamesStr = new String[allSceneGames.Length];
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

            System.Collections.ArrayList utilityApplications = Properties.Settings.Default.savedUtilityProcesses;
            if (utilityApplications != null) controlLineList.loadAllUtilityApplications(utilityApplications, 0);
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
        
            //initiates form filling based on connection state
        private void fillForm(bool connectionState)
        {
            pnlSceneConfig.Enabled = connectionState;
            if (connectionState)
            {
                fillComboBoxes();
                connected = true;
                lblConnection.Text = "*connected";
                lblConnection.ForeColor = System.Drawing.Color.Green;
                getRecordingPath();
            }
            else
            {
                lblConnection.Text = "*not connected";
                lblConnection.ForeColor = System.Drawing.Color.Red;
            }
        }

        //form-Events---------------------------------------------------------------------------------------------------------------

            //hides Form on launch
        private void frmOBSSupporter_Shown(object sender, EventArgs e)
        {
            Hide();
            nfiTrayIcon.Visible = true;
        }

            //shows on Tray-Click
        private void nfiTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

            //hides on Minimize / loads Form on Normalize
        private void frmOBSSupporter_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
            if (WindowState == FormWindowState.Normal)
            {
                cbxShowConsoleOnLaunch.Checked = Properties.Settings.Default.savedConsoleOnLaunch;
                cbxTeamspeakRelaunch.Checked = Properties.Settings.Default.savedTeamSpeakRelaunch;
                cbxStrikeDriverRelaunch.Checked = Properties.Settings.Default.savedStrikeDriverRelaunch;
                cbxSynapseRelaunch.Checked = Properties.Settings.Default.savedSynapseRelaunch;
                cbxNotificationSound.Checked = Properties.Settings.Default.savedNotificationSound;
                string taskPath = Properties.Settings.Default.savedTaskPath;
                //Properties.Settings.Default.savedTaskPath = "";
                //Properties.Settings.Default.Save();
                cbxStartOnBoot.Checked = taskPath != "";
                tbxObsPath.Text = main.obsPath;
                fillForm(connected);
                refreshShowConsoleButton();
                allSceneGames = Properties.Settings.Default.savedSceneGames;
                if (allSceneGames == null)
                {
                    allSceneGames = new String[1][];
                    allSceneGames[0] = new String[0];
                } //on first Time
                btnOK.Enabled = false;
                btnApply.Enabled = false;
            }
        }

        //Settings Changed
        private void tbxObsPath_TextChanged(object sender, EventArgs e)
        {
            if ((tbxObsPath.Text == "") || (!tbxObsPath.Text.Contains("obs64.exe")))
            {
                lblInvalid.Visible = true;
            }
            else if (tbxObsPath.Text != main.obsPath)
            {
                btnOK.Enabled = true;
                btnApply.Enabled = true;
            }
        }
        private void cbxShowConsoleOnBoot_CheckStateChanged(object sender, EventArgs e)
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
        public void cobxSelectedValueChanged(Object sender, EventArgs e)
        {
            btnOK.Enabled = true;
        }

            //manually opens and connects OBS
        private void btnOpenConnect_Click(object sender, EventArgs e)
        {
            main.obsProcess.Start();
        }

            //Console Visiblity
        private void btnShowConsole_Click(object sender, EventArgs e)
        {
            if (consoleVisible)
            {
                ShowWindow(handle, SW_HIDE);
                consoleVisible = false;
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
                consoleVisible = true;
            }
            refreshShowConsoleButton();
        }

            //Saves Settings
        private void applySettings()
        {
            lblInvalid.Visible = false;
            main.obsPath = Path.GetFullPath(tbxObsPath.Text);
            main.removeObsLnk();
            main.createObsLnk();
            main.createObsProcess();
            btnOpenConnect.Enabled = true;

            Properties.Settings.Default.savedSceneGames = controlLineList.getAllSceneGames();
            Properties.Settings.Default.savedOBSPath = main.obsPath;
            Properties.Settings.Default.savedUtilityProcesses = controlLineList.getAllUtilityApplications();
            Properties.Settings.Default.savedConsoleOnLaunch = cbxShowConsoleOnLaunch.Checked;
            Properties.Settings.Default.savedTeamSpeakRelaunch = cbxTeamspeakRelaunch.Checked;
            Properties.Settings.Default.savedStrikeDriverRelaunch = cbxStrikeDriverRelaunch.Checked;
            Properties.Settings.Default.savedSynapseRelaunch = cbxSynapseRelaunch.Checked;
            Properties.Settings.Default.savedNotificationSound = cbxNotificationSound.Checked;

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

            //refreshes ShowConsole-Button Text
        private void refreshShowConsoleButton()
        {
            if (consoleVisible)
            {
                btnShowConsole.Text = "Click to hide Console";
            }
            else
            {
                btnShowConsole.Text = "Click to show Console";
            }
        }

        //WebSocket-Events--------------------------------------------------------------------------------------------------------------
            //triggered by main.onConnect()
        public void onConnect()
        {
            for (int i = 0; i < 3; i++)
            {
                sceneNames = main._obs.ListScenes().ToArray();
            }

            sceneNamesStr = new String[sceneNames.Length];
            for (int i = 0; i < sceneNames.Length; i++)
            {
                sceneNamesStr[i] = sceneNames[i].Name;
            }

            if (WindowState == FormWindowState.Normal)
            {
                fillForm(true);
            }
            else
            {
                connected = true; //TODO: nötig? vtl fillform(false);
            }

            controlLineList.fillComboboxes(sceneNamesStr);
        } //Todo contorllinelist laden reichschaeun

        public void onDisconnect()
        {
            main.writeInConsole(ConsoleColor.Red, "supporterondisconnect");
            lblConnection.Text = "not connected";
            lblConnection.ForeColor = System.Drawing.Color.Red;

            pnlSceneConfig.Enabled = false;
            connected = false;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFDialog = new OpenFileDialog();
            oFDialog.InitialDirectory = "c:\\";
            oFDialog.RestoreDirectory = false;
            if (oFDialog.ShowDialog() == DialogResult.OK)
            {
                tbxObsPath.Text = oFDialog.FileName;
            }
        }

        private void makeSettingsVisible(Panel panel)
        {
            btnGeneral.FlatStyle = FlatStyle.Popup;
            btnConsole.FlatStyle = FlatStyle.Popup;
            btnSceneConfig.FlatStyle = FlatStyle.Popup;
            btnFixes.FlatStyle = FlatStyle.Popup;

            pnlGeneral.Visible = false;
            pnlConsole.Visible = false;
            pnlFixes.Visible = false;
            pnlSceneConfig.Visible = false;

            panel.Visible = true;
        }

        private void btnGeneral_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlGeneral);
            btnGeneral.FlatStyle = FlatStyle.Flat;
        }

        private void btnConsole_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlConsole);
            btnConsole.FlatStyle = FlatStyle.Flat;
            pnlConsole.Visible = true;
        }

        private void btnSceneConfig_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlSceneConfig);
            btnSceneConfig.FlatStyle = FlatStyle.Flat;
        }

        private void btnFixes_Click(object sender, EventArgs e)
        {
            makeSettingsVisible(pnlFixes);
            btnFixes.FlatStyle = FlatStyle.Flat;
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
                OBS_RBDWatcher = new FileSystemWatcher(recordingPath);
                OBS_RBDWatcher.EnableRaisingEvents = true;
                OBS_RBDWatcher.Changed += main.RBDChanged;
            }
        }

        private void createScheduleTask()
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "launches OBS Supporter with logon";
                td.Triggers.Add(new LogonTrigger());
                td.Actions.Add(new ExecAction(System.Reflection.Assembly.GetEntryAssembly().Location, "c:\\test.log", null));
                td.Principal.RunLevel = TaskRunLevel.Highest;
                ts.RootFolder.RegisterTaskDefinition(@"OBS Supporter", td);
            }
            Properties.Settings.Default.savedTaskPath = "OBS Supporter";
            Properties.Settings.Default.Save();
        }

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

        public void enableBtnOpenConnect()
        {
            btnOpenConnect.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
        }
    }

    public class Main
    {
        public OBSWebsocket _obs = new OBSWebsocket();
        private string isScene;
        private string isProfile;
        private string wantScene = "";
        private string wantProfile = "";
        private bool replayBufferState = false;
        private frmOBSSupporter supporterForm;
        public WshShell shell1;
        public IWshShortcut shortcut1;
        public Process obsProcess;
        UInt32 obsProcessID;
        public string obsPath = Properties.Settings.Default.savedOBSPath;
        private Boolean onConnectTriggered = false;
        private Boolean opened = false;
        private Boolean launchInit = false;
        private UInt32 currentAppID = 0;
        private Thread thread;
        private DateTime dateTime;
        private string sceneChangedTo;


        public Main(frmOBSSupporter e)
        {
            supporterForm = e;
            _obs.Connected += onConnect;
            _obs.Disconnected += onDisconnect;
            _obs.SceneChanged += onSceneChange;
            writeInConsole(ConsoleColor.Red, "OBS-events distributed");
            if (obsPath != "")
            {
                createObsLnk();
                createObsProcess();
                supporterForm.Enabled = true;
            }
        }

        //Form-Methods--------------------------------------------------------------------------------------------------------------
        public void writeInConsole(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        //Watcher-Events-------------------------------------------------------------------------------------------------------------
        public void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string process = (string)e.NewEvent.Properties["ProcessName"].Value;
            Console.WriteLine(process);

            string scene = supporterForm.controlLineList.getScene(process);
            //if (process == "TslGame.exe")
            //{
            //    Thread.Sleep(3000);
            //    gameStarted(scene);
            //    if (Properties.Settings.Default.savedTeamSpeakRelaunch) Process.Start(@"Y:\Program Files\TeamSpeak 3 Client\ts3client_win64.exe");
            //    if (Properties.Settings.Default.savedStrikeDriverRelaunch) Process.Start(@"Y:\Program Files\Mad Catz\S.T.R.I.K.E.TE\STRIKE_TE_Profiler.exe");
            //    if (Properties.Settings.Default.savedSynapseRelaunch)
            //    {
            //        Process[] synapse = Process.GetProcessesByName("Razer Synapse 3.exe");
            //        if (synapse.Length == 0) Process.Start(@"C:\Program Files (x86)\Razer\Synapse3\WPFUI\Framework\Razer Synapse 3 Host\Razer Synapse 3.exe");
            //    }
            //    return;
            //}
            if (scene != null)
            {
                currentAppID = (UInt32)e.NewEvent.Properties["ProcessID"].Value;
                writeInConsole(ConsoleColor.Yellow, "currentGame: " + process + "(" + currentAppID + ")");
                gameStarted(scene);
                supporterForm.controlLineList.startUtilityApplications();
            }

            if ((string)e.NewEvent.Properties["ProcessName"].Value == "obs64.exe")
            {
                opened = true;
                obsProcessID = (UInt32)obsProcess.Id;
            }

            if (!onConnectTriggered && opened && DateTime.Now.CompareTo(dateTime) > 0)
            {
                supporterForm.startWatch.Stop();
                supporterForm.startWatch.Start();
                writeInConsole(ConsoleColor.Yellow, "TRY NOW");
                dateTime = DateTime.Now.AddSeconds(3);
                thread = new Thread(new ThreadStart(connect));
                thread.Start();
            }
        }

        public void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            UInt32 Process = (UInt32)e.NewEvent.Properties["ProcessID"].Value;
            Console.WriteLine("Process stopped: {0}" + Process);

            if (Process == obsProcessID)
            {
                opened = false;
                onConnectTriggered = false;
                launchInit = false;
            }
            if (Process == currentAppID && onConnectTriggered)
            {
                _obs.ReplayBufferStateChanged += closeObs;
                _obs.StopReplayBuffer();
                replayBufferState = false;
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
            shell1 = new WshShell();
            string directory = obsPath.Substring(0, obsPath.LastIndexOf("\\"));
            shortcut1 = (IWshShortcut)shell1.CreateShortcut(directory + "OBS_escape.lnk");
            shortcut1.TargetPath = obsPath;
            shortcut1.WorkingDirectory = directory;
            shortcut1.Save();
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
            supporterForm.enableBtnOpenConnect();
        }

        public void closeObs(OBSWebsocket sender, OutputState outputState)
        {
            _obs.ReplayBufferStateChanged -= closeObs; //changed
            writeInConsole(ConsoleColor.Red, "closing now");
            Thread.Sleep(3000);
            obsProcess.CloseMainWindow();
        }

        private void connect()
        {
            onConnectTriggered = false;
            try
            {
                _obs.Connect("ws://127.0.0.1:4444", "");
            }
            catch (Exception e)
            {
                writeInConsole(ConsoleColor.Red, "Failed Connecting!!!!!!!! for: " + e.Message);
            }
            onConnectTriggered = true;
        }

        //_obs-Events---------------------------------------------------------------------------------------------------------------
        private void onSceneChange(OBSWebsocket sender, string newSceneName)
        {
            if (sceneChangedTo != newSceneName)
            {
                //sound for scenechange
            }
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
            System.Media.SoundPlayer sPlayer = new System.Media.SoundPlayer();
            sPlayer.Stream = Properties.Resources.AirPlanDing30Percent;
            sPlayer.Play();
        }

        private void onConnect(object sender, EventArgs e)
        {
            writeInConsole(ConsoleColor.Green, "On-Connect-Event triggered");
            onConnectTriggered = true;
            setScene();
            setProfile();
            supporterForm.Invoke(new MethodInvoker(delegate { supporterForm.onConnect(); }));
            launchInit = false;
        }

        private void onDisconnect(object sender, EventArgs e)
        {
            onConnectTriggered = false;
            if (obsProcess.HasExited) opened = false;
            writeInConsole(ConsoleColor.Red, "Disconnected with Websocket");
            supporterForm.Invoke(new MethodInvoker(delegate { supporterForm.onDisconnect(); }));
            replayBufferState = false;
        }
    }
}