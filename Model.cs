using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ComTerm.Util;

namespace ComTerm
{
    class Model
    {
        private DeviceManager devMgr;
        //private ComThread WriteThread;
        private ComThread ReadThread;
        private ComDevice Device;

        public void Initialize(DeviceManager mgr)
        {
            devMgr = mgr;
            CreateThreads();
        }

        public void CreateThreads()
        {
            ReadThread = new ComThread(ReadProc);
        }

        private void ReadProc()
        {
            //Console.WriteLine("hello");
            //Thread.Sleep(1000);
            while (true)
            {
                //Device?.Read();
            }
        }

        public void Run(int devId)
        {
            Device = devMgr.OpenDevice(devId);
            Device.Open();
            ReadThread.Run();
        }

    }
}
