using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ComTerm.Util
{
    public class Disposable : IDisposable
    {
        private bool isDisposed = false;

        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            //
            // FIXME: double check the memory allocation, and enable below code.
            //

            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                DisposeResource(disposing);
                isDisposed = true;
            }
        }

        protected virtual void DisposeResource(bool disposing)
        {
        }
    }

    public delegate void ThreadDelegate();
    public class ComThread
    {
        public ThreadDelegate proc;
        private Thread theThread = null;

        private void ThreadProc()
        {
            //while (true)
            {
                proc?.Invoke();
            }
        }

        public ComThread(ThreadDelegate f=null)
        {
            proc = f;
            theThread = new Thread(new ThreadStart(ThreadProc));
        }

        public void Run()
        {
            theThread.Start();
        }
    }


    public class IoOverlapped : System.Threading.Overlapped
    {
        public IntPtr hEvent;

        public IoOverlapped()
        {
            OffsetLow = 0;
            OffsetHigh = 0;
            EventHandleIntPtr = hEvent;
        }
    }
}

