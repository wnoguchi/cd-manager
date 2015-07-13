using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace CDManager
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            bool closeFlag = false;
            if (args.Length > 0 && args[0] == "close")
            {
                closeFlag = true;
            }

            foreach (var drive in Environment.GetLogicalDrives())
            {
                DriveInfo di = new DriveInfo(drive);
                if (di.DriveType == DriveType.CDRom)
                {
                    mciSendString("open " + drive + " type cdaudio alias orator", null, 0, IntPtr.Zero);
                    int canEject = mciSendString("capability orator can eject", null, 0, IntPtr.Zero);
                    Console.WriteLine("{0}はイジェクト{1}です。", drive, (canEject == 0) ? "可能" : "不可");

                    if (!closeFlag)
                    {
                        Console.WriteLine("{0}ドライブを開きます。", drive);
                        mciSendString("set orator door open", null, 0, IntPtr.Zero);
                    }
                    else
                    {
                        Console.WriteLine("{0}ドライブを閉じます。", drive);
                        mciSendString("set orator door closed", null, 0, IntPtr.Zero);
                        mciSendString("close orator", null, 0, IntPtr.Zero);
                    }

                }
            }

        }
    }
}
