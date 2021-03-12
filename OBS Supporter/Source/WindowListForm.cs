using System;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OBS_Supporter
{
    public partial class WindowListForm : Form
    {
        ControlLine parent;
        System.Collections.ArrayList PathList = new System.Collections.ArrayList();

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public WindowListForm(ControlLine cL)
        {
            InitializeComponent();

            parent = cL;

            Process[] processList = Process.GetProcesses();
            System.Collections.ArrayList WindowList = new System.Collections.ArrayList();

            foreach (Process p in processList)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle) && Environment.Is64BitProcess)
                {
                    WindowList.Add(p.ProcessName);
                    PathList.Add(GetProcessName(p.Id));
                }
            }

            lbxWindowList.Items.AddRange(WindowList.ToArray());
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string GetProcessName(int pid)
        {
            var processHandle = OpenProcess(0x0400 | 0x0010, false, pid);

            if (processHandle == IntPtr.Zero)
            {
                return null;
            }

            const int lengthSb = 4000;

            var sb = new StringBuilder(lengthSb);

            string result = null;

            if (GetModuleFileNameEx(processHandle, IntPtr.Zero, sb, lengthSb) > 0)
            {
                result = sb.ToString();
            }

            CloseHandle(processHandle);

            return result;
        }

        private void lbxWindowList_DoubleClick(object sender, EventArgs e)
        {
            parent.addGame(PathList[lbxWindowList.SelectedIndex].ToString());
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            parent.addGame(PathList[lbxWindowList.SelectedIndex].ToString());
            parent.setSceneAfterAdd(PathList[lbxWindowList.SelectedIndex].ToString());
            Close();
        }

        private void lbxWindowList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                parent.addGame(ofd.FileName);
            }
            Close();
        }
    }
}
