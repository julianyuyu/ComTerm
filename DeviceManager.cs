using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static ComTerm.NativeAPI;

namespace ComTerm
{
    public class DeviceManager
    {
        public List<string> DeviceNames = new List<string>();
        public List<string> FriendlyNames = new List<string>();

        public DeviceManager()
        {
        }

        public bool OpenDevice()
        {
            EnumerateDevice(GUID_DEVINTERFACE_COMPORT);
            return true;
        }

        protected void EnumerateDevice(Guid DeviceGuid)
        {
            DeviceNames.Clear();
            FriendlyNames.Clear();
        }
    }
}
