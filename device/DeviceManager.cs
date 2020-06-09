using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static ComTerm.NativeAPI;
using static ComTerm.NativeDeviceInterfaceAPI;
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

        private Tuple<string, string> GetDeviceName(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA interfaceData)
        {
            uint requiredSize = 0;
            string deviceName = null;
            string friendlyName = null;
            SP_DEVINFO_DATA infoData = new SP_DEVINFO_DATA();
            infoData.cbSize = Marshal.SizeOf(infoData);

            //get size first
            SetupDiGetDeviceInterfaceDetail(hDevInfo, ref interfaceData, IntPtr.Zero, 0, ref requiredSize, ref infoData /*infoData*/);
            IntPtr detailDataBuffer = Marshal.AllocHGlobal((int)requiredSize);
            if (detailDataBuffer != IntPtr.Zero)
            {
                // get dev name now.
                SP_DEVICE_INTERFACE_DETAIL_DATA detailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                detailData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                Marshal.StructureToPtr(detailData, detailDataBuffer, false);

                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref interfaceData, detailDataBuffer, requiredSize, ref requiredSize, ref infoData))
                {
                    IntPtr pdevicePathName = (IntPtr)((int)detailDataBuffer + 4);
                    deviceName = Marshal.PtrToStringAuto(pdevicePathName);

                    uint regDataType = 0;

                    // get size for friendly name
                    SetupDiGetDeviceRegistryProperty(hDevInfo, ref infoData, SPDRP_FRIENDLYNAME,
                        ref regDataType, IntPtr.Zero, 0, ref requiredSize);
                    IntPtr nameBuffer = Marshal.AllocHGlobal((int)requiredSize);
                    if (SetupDiGetDeviceRegistryProperty(hDevInfo, ref infoData, SPDRP_FRIENDLYNAME,
                        ref regDataType, nameBuffer, requiredSize, ref requiredSize))
                    {
                        friendlyName = Marshal.PtrToStringAuto(nameBuffer);
                    }
                    Marshal.FreeHGlobal(nameBuffer);
                    nameBuffer = IntPtr.Zero;
                }
                Marshal.FreeHGlobal(detailDataBuffer);
                detailDataBuffer = IntPtr.Zero;
            }
            return new Tuple<string, string>(deviceName, friendlyName);
        }

        protected uint EnumerateDevice(Guid DeviceGuid)
        {
            DeviceNames.Clear();
            FriendlyNames.Clear();

            uint HandleIndex = 0;

            // We start at the "root" of the device tree and look for all
            // devices that match the interface GUID of a disk
            var hDevInfo = SetupDiGetClassDevs(ref DeviceGuid, IntPtr.Zero, IntPtr.Zero, DIGCF_PRESENT | DIGCF_DEVICEINTERFACE);

            if (hDevInfo != IntPtr.Zero)
            {
                // create a Device Interface Data structure
                SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
                DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData);

                // start the enumeration 
                while (SetupDiEnumDeviceInterfaces(hDevInfo,
                    IntPtr.Zero, ref DeviceGuid, HandleIndex, ref DeviceInterfaceData))
                {
                    var names = GetDeviceName(hDevInfo, ref DeviceInterfaceData);
                    if (string.IsNullOrEmpty(names.Item1))
                    {
                        break;
                    }
                    else
                    {
                        DeviceNames.Add(names.Item1);
                        FriendlyNames.Add(names.Item2);
                        HandleIndex++;
                    }
                }
                SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            return HandleIndex;
        }
    }
}
