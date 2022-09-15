using System;
using System.Diagnostics;
using System.Reflection;

namespace WindowsRuntimeSecurity.MiscMethods
{
	// Token: 0x02000005 RID: 5
	internal class Persistence
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002548 File Offset: 0x00000748
		public static bool AlreadyRunning()
		{
			Process[] processes = Process.GetProcesses();
			Process currentProcess = Process.GetCurrentProcess();
			foreach (Process process in processes)
			{
				try
				{
					if (process.Modules[0].FileName == Assembly.GetExecutingAssembly().Location && currentProcess.Id != process.Id)
					{
						return true;
					}
				}
				catch (Exception)
				{
				}
			}
			return false;
		}
	}
}
