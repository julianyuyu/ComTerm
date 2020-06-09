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
}
