using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using OBSWebsocketDotNet;

namespace OBS_Supporter
{
    public class Empty : sceneConfigControlLine
    {
        frmOBSSupporter parent;

        public Empty(frmOBSSupporter par)
        {
            parent = par;
        }

        public int getLength()
        {
            return 0;
        }

        public sceneConfigControlLine addControlLine(String[] sceneGames)
        {
            ControlLine line = new ControlLine(parent, sceneGames, this);

            return line;
        }
        
        public void fillComboboxes(String[] scenes) { }

        public Control[] draw(int index)
        {
            return null;
        }

        public String getAllSelectedItems()
        {
            return "";
        }

        public String[][] getAllSceneGames()
        {
            return new String[0][];
        }

        public String getScene(String process)
        {
            return null;
        }

        public sceneConfigControlLine removeControlLine(int index){ return this; }

        public void startUtilityApplications() { }
        public void closeUtilityApplications() { }

        public System.Collections.ArrayList getAllUtilityApplications()
        {
            return new System.Collections.ArrayList();
        }

        public void loadAllUtilityApplications(System.Collections.ArrayList savedUA, int index) { }
    }
}