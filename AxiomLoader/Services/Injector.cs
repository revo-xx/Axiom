using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AxiomLoader.Services
{
    public static class Injector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        // Privileges
        private const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        private const uint MEM_COMMIT = 0x00001000;
        private const uint MEM_RESERVE = 0x00002000;
        private const uint PAGE_READWRITE = 0x04;

        public static bool Inject(string processName, string dllPath, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length == 0)
                {
                    errorMessage = $"Process '{processName}' not found. Please make sure the game is running.";
                    return false;
                }

                Process targetProcess = processes[0];
                IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, targetProcess.Id);

                if (hProcess == IntPtr.Zero)
                {
                    errorMessage = "Failed to open target process. Try running as Administrator.";
                    return false;
                }

                IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                if (loadLibraryAddr == IntPtr.Zero)
                {
                    errorMessage = "Failed to get LoadLibraryA address.";
                    CloseHandle(hProcess);
                    return false;
                }

                uint size = (uint)((dllPath.Length + 1) * Marshal.SizeOf(typeof(char)));
                IntPtr allocMem = VirtualAllocEx(hProcess, IntPtr.Zero, size, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);

                if (allocMem == IntPtr.Zero)
                {
                    errorMessage = "Failed to allocate memory in target process.";
                    CloseHandle(hProcess);
                    return false;
                }

                byte[] dllPathBytes = Encoding.Default.GetBytes(dllPath);
                if (!WriteProcessMemory(hProcess, allocMem, dllPathBytes, (uint)dllPathBytes.Length, out _))
                {
                    errorMessage = "Failed to write DLL path to target process memory.";
                    CloseHandle(hProcess);
                    return false;
                }

                IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocMem, 0, IntPtr.Zero);
                if (hThread == IntPtr.Zero)
                {
                    errorMessage = "Failed to create remote thread.";
                    CloseHandle(hProcess);
                    return false;
                }

                CloseHandle(hThread);
                CloseHandle(hProcess);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
