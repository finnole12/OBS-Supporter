using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OBSWebsocketDotNet;

namespace OBS_Supporter
{
    public interface sceneConfigControlLine
    {
        int getLength();
        sceneConfigControlLine addControlLine(string[] sceneGames);
        void fillComboboxes(string[] scenes);
        Control[] draw (int index);
        string getAllSelectedItems();
        string[][] getAllSceneGames();
        string getScene(string process);
        sceneConfigControlLine removeControlLine(int index);
        void startUtilityApplications();
        void closeUtilityApplications();
        System.Collections.ArrayList getAllUtilityApplications();
        void loadAllUtilityApplications(System.Collections.ArrayList savedUA, int index);
    }

    
}
