using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComTerm
{
    public class MainViewmodel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string comName = "";
        public string ComName { get { return comName; } set { comName = value; OnChanged(nameof(ComName)); } }

        public List<string> ComNames { get; set; } = new List<string>();

        private DeviceManager devMgr;

        public void Initialize(DeviceManager mgr)
        {
            devMgr = mgr;
        }

        public void RefreshDeviceList()
        {
            ComNames.Clear();
            ComNames.AddRange(devMgr.FriendlyNames);
        }
    }
}
