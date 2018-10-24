using System;
using System.Runtime.InteropServices;

namespace ConsoleAppCallWinAPI
{
    internal sealed class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CreateToolhelp32Snapshot(int dwFlags, uint th32ProcessID);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool Process32First(IntPtr intPtr, ref PROCESSENTRY32 str);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool Process32Next(IntPtr intPtr, ref PROCESSENTRY32 str);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseHandle(IntPtr intPtr); 

        static void Main(string[] args)
        {
            IntPtr tasklist = CreateToolhelp32Snapshot(0x00000002, 0);

            PROCESSENTRY32 mystrct = new PROCESSENTRY32();

            mystrct.dwSize = (uint)Marshal.SizeOf(mystrct);
           
            Process32First(tasklist, ref mystrct);

            Console.WriteLine("Processes");
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("ProcessID\t Name\t\t\t Threads\t");
            do
            {
                Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t", mystrct.th32ProcessID, mystrct.szExeFile, mystrct.cntThreads);
            } while (Process32Next(tasklist, ref mystrct));

            CloseHandle(tasklist);

            Console.ReadKey();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    }
}
