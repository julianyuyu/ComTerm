using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ComTerm.Util;
using static ComTerm.NativeWindef;

namespace ComTerm.device
{
    public class ComDevice : Disposable
    {
        protected string displayName;
        protected string deviceName;
        protected IntPtr deviceHandle = IntPtr.Zero;

        public ComDevice(string DeviceName) : base()
        {
            this.deviceName = DeviceName;
        }

        protected override void DisposeResource(bool disposing)
        {
            if (disposing == true)
            {
                // managed resource
                GC.Collect();
            }
            Close();
        }

        public virtual bool IsOpened()
        {
            return (IntPtr.Zero != deviceHandle);
        }

        public virtual IntPtr Open()
        {
            if (deviceHandle == IntPtr.Zero)
            {
                deviceHandle = CreateFile(deviceName,
                    (GENERIC_READ | GENERIC_WRITE),
                    0,
                    IntPtr.Zero,
                    OPEN_EXISTING,
                    FILE_FLAG_OVERLAPPED,
                    IntPtr.Zero);
            }

            return deviceHandle;
        }

        // don't call Close directly, but call Dispose instead.
        protected virtual bool Close()
        {
            bool result = false;

            if (deviceHandle != IntPtr.Zero)
            {
                result = CloseHandle(deviceHandle);
                deviceHandle = IntPtr.Zero;
            }

            return result;
        }

        protected virtual bool IoControl(
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            ref uint lpBytesReturned,
            IntPtr lpOverlapped)
        {
            return DeviceIoControl(deviceHandle, dwIoControlCode, lpInBuffer, nInBufferSize,
                lpOutBuffer, nOutBufferSize, ref lpBytesReturned, lpOverlapped);
        }
    }
}
