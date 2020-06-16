using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ComTerm.Util;
using Microsoft.Win32.SafeHandles;
using static ComTerm.NativeWindef;

namespace ComTerm
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

        protected virtual bool Read(
            IntPtr outBuffer,
            uint bytesToRead,
            ref uint bytesRead)
        {
            return ReadFile(deviceHandle, outBuffer, bytesToRead, ref bytesRead, IntPtr.Zero);
        }

#if false
        protected virtual bool ReadAsync(
            IntPtr outBuffer,
            uint bytesToRead,
            ref uint bytesRead,
            IntPtr lpOverlapped)
        {
            EventObject = CreateEvent(IntPtr.Zero, true, true, "");
            int lastError = Marshal.GetLastWin32Error();

            var HIDOverlapped = new System.Threading.Overlapped();
            HIDOverlapped.OffsetLow = 0;
            HIDOverlapped.OffsetHigh = 0;
            HIDOverlapped.EventHandleIntPtr = EventObject;

            c_overlapped(bool manual, bool sigaled)

            {
                Internal = 0;
                InternalHigh = 0;
                Offset = 0;
                OffsetHigh = 0;
                hEvent = ::CreateEvent(nullptr, manual, sigaled ? TRUE : FALSE, nullptr);
            }
            ~c_overlapped()

            {

                ::CloseHandle(hEvent);
            }
            //if (deviceHandle == -1)
                //Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            // The last parameter of the FileStream constructor (isAsync) will make the class use async I/O
            using (var stream = new FileStream(deviceHandle as SafeFileHandle, FileAccess.ReadWrite, 4096, true))
            {
                var buffer = new byte[4096];
                // Asynchronously read 4kb
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            }

            return ReadFile(deviceHandle, outBuffer, bytesToRead, ref bytesRead, &overlap);
        }
#endif
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
