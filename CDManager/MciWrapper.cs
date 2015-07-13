using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CDManager
{
    class MciWrapper
    {
        [DllImport("winmm", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
            string command, StringBuilder buffer,
            int bufferSize, IntPtr hwndCallback);
    }
}
