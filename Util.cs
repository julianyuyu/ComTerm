using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}

