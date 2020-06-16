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

    /// <summary>
    /// native interface for windows calls.
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NativeWindef
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr CreateFile(
            [MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
            uint dwDesireAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool ReadFile(
            IntPtr hFile,
            IntPtr outBuffer,
            uint bytesToRead,
            ref uint bytesRead,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped);

        //Windows define
        [StructLayout(LayoutKind.Sequential)]
        public struct OVERLAPPED
        {
            IntPtr Internal;
            IntPtr InternalHigh;
            IntPtr Pointer;
            IntPtr hEvent;
        };

        public const int ATTACH_PARENT_PROCESS = -1;

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;

        public const int OPEN_EXISTING = 3;
        public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        public const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        public const uint FILE_FLAG_WRITE_THROUGH = 0x80000000;
        public const uint FILE_FLAG_RANDOM_ACCESS = 0x10000000;

        // Define the method codes for how buffers are passed for I/O and FS controls
        public const uint METHOD_BUFFERED = 0;
        public const uint METHOD_IN_DIRECT = 1;
        public const uint METHOD_OUT_DIRECT = 2;
        public const uint METHOD_NEITHER = 3;

        public const uint FILE_ANY_ACCESS = 0;
        public const uint FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;
        public const uint FILE_READ_ACCESS = 0x0001;
        public const uint FILE_WRITE_ACCESS = 0x0002;

        public const uint FILE_READ_DATA = 0x0001;
        public const uint FILE_WRITE_DATA = 0x0002;
        public const uint FILE_READ_WRITE_DATA = (FILE_WRITE_DATA | FILE_READ_DATA);

        // windows device define.
        public const uint FILE_DEVICE_UNKNOWN = 0x00000022;
    }
}
