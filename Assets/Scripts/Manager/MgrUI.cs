using System.Collections.Generic;
using UI;

namespace Manager
{
    public class MgrUI
    {
        public List<UIBase> AllUI;

        public MgrUI()
        {
            AllUI = new List<UIBase>();
        }

        public void AddUI(UIBase ui)
        {
            AllUI.Add(ui);
        }

        public void RemoveUI(UIBase ui)
        {
            AllUI.Remove(ui);
        }
        
        public void Update()
        {
            foreach (var ui in AllUI)
            {
                ui.Update();
            }
        }
    }
}