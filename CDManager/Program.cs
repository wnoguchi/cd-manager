using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CDManager
{
    class Program
    {
        [DllImport("winmm", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
            string command, StringBuilder buffer,
            int bufferSize, IntPtr hwndCallback);

        private enum Command
        {
            List,
            Open,
            Close,
            None,
        };

        static void Main(string[] args)
        {
            var cmd = Command.None;
            string targetDrive = null;

            if (args.Length > 0 && args[0] == "list")
            {
                cmd = Command.List;
            }
            else if (args.Length > 0 && args[0] == "open")
            {
                cmd = Command.Open;

                if (args.Length > 1)
                    targetDrive = args[1].ToLower()[0].ToString();
            }
            else if (args.Length > 0 && args[0] == "close")
            {
                cmd = Command.Close;

                if (args.Length > 1)
                    targetDrive = args[1].ToLower()[0].ToString();
            }
            else
            {
                cmd = Command.None;
            }

            switch (cmd)
            {
                case Command.List:
                    foreach (string drive in from drive in Environment.GetLogicalDrives() let di = new DriveInfo(drive) where di.DriveType == DriveType.CDRom select drive)
                    {
                        Console.WriteLine(drive.Replace(":\\", string.Empty));
                    }
                    break;
                case Command.Open:
                    foreach (string drive in from drive in Environment.GetLogicalDrives() let di = new DriveInfo(drive) where di.DriveType == DriveType.CDRom && (targetDrive == drive.Replace(":\\", string.Empty) || targetDrive == null) select drive)
                    {
                        mciSendString("open " + drive + " type cdaudio alias orator", null, 0, IntPtr.Zero);
                        int canEject = mciSendString("capability orator can eject", null, 0, IntPtr.Zero);
                        Console.WriteLine("{0}はイジェクト{1}です。", drive, (canEject == 0) ? "可能" : "不可");

                        Console.WriteLine("{0}ドライブを開きます。", drive);
                        mciSendString("set orator door open", null, 0, IntPtr.Zero);
                        break;
                    }
                    break;
                case Command.Close:
                    foreach (string drive in from drive in Environment.GetLogicalDrives() let di = new DriveInfo(drive) where di.DriveType == DriveType.CDRom && (targetDrive == drive.Replace(":\\", string.Empty) || targetDrive == null) select drive)
                    {
                        mciSendString("open " + drive + " type cdaudio alias orator", null, 0, IntPtr.Zero);
                        int canEject = mciSendString("capability orator can eject", null, 0, IntPtr.Zero);
                        Console.WriteLine("{0}はイジェクト{1}です。", drive, (canEject == 0) ? "可能" : "不可");

                        Console.WriteLine("{0}ドライブを閉じます。", drive);
                        mciSendString("set orator door closed", null, 0, IntPtr.Zero);
                        mciSendString("close orator", null, 0, IntPtr.Zero);
                        break;
                    }
                    break;
                case Command.None:
                    Console.WriteLine("Usage: CDManager command (drive letter)");
                    Console.WriteLine("  command: list");
                    Console.WriteLine("  command: open: drive letter required.");
                    Console.WriteLine("  command: close: drive letter required.");
                    break;
                default:
                    Console.WriteLine("Usage: CDManager command (drive letter)");
                    Console.WriteLine("  command: list");
                    Console.WriteLine("  command: open: drive letter required.");
                    Console.WriteLine("  command: close: drive letter required.");
                    break;
            }


        }
    }
}
