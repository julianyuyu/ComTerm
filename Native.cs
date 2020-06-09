using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ComTerm
{
    /// <summary>
    /// native interface for COMPORT
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NativeAPI
    {
        // {86E0D1E0-8089-11D0-9CE4-08003E301F73}
        public static Guid GUID_DEVINTERFACE_COMPORT = new Guid("86E0D1E0-8089-11D0-9CE4-08003E301F73");

    }

    /// <summary>
    /// native API for device interface
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NativeDeviceInterfaceAPI
    {
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr deviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            IntPtr deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            ref uint requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData);

        //Get device, return true
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            IntPtr devInfo,
            ref Guid interfaceClassGuid,
            uint memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInfo(
            IntPtr hDevInfo,
            uint memberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr HwndParent, uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceRegistryProperty(
            IntPtr hDevInfo,
            ref SP_DEVINFO_DATA devInfoData,
            uint Property,
            ref uint PropertyRegDataType,
            IntPtr PropertyBuffer,
            uint PropertyBufferSize,
            ref uint RequiredSize);

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public class SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
            public short devicePath;
        }

        //[StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;// = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DATA));
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SP_DEVINFO_DATA
        {
            public int cbSize;// = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            public Guid classGuid;// = Guid.Empty; // temp
            public int devInst;// = 0; // dumy
            public int reserved;// = 0;
        }

        //
        // Device Interface Get Class Flag
        // used in SetupDiGetClassDevsW
        // https://docs.microsoft.com/en-us/windows/desktop/api/setupapi/nf-setupapi-setupdigetclassdevsw
        //

        public static readonly uint DIGCF_DEFAULT = 0x1;
        public static readonly uint DIGCF_PRESENT = 0x2;
        public static readonly uint DIGCF_ALLCLASSES = 0x4;
        public static readonly uint DIGCF_PROFILE = 0x8;
        public static readonly uint DIGCF_DEVICEINTERFACE = 0x10;

        //
        public static readonly uint SPDRP_FRIENDLYNAME = 0x0000000C;  // FriendlyName (R/W)
    }
}
