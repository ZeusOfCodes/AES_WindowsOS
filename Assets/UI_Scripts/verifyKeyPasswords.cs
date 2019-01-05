using System.IO;

namespace verification{
	public class verifyKeyPasswords {

		public static bool verifyKey(int AESmode, string keyFilePath){
			if (new FileInfo(keyFilePath).Length == (AESmode / 8))
				return true;
			else
				return false;
		}

		public static bool verifyKey(string keyFilePath, ref int keyLength)
		{
			if ((keyLength = (int)new FileInfo (keyFilePath).Length * 8) == 128)
				return true;
			else if ((keyLength = (int)new FileInfo (keyFilePath).Length * 8) == 192)
				return true;
			else if ((keyLength = (int)new FileInfo (keyFilePath).Length * 8) == 256)
				return true;
			else
				return false;
		}

		public static int VerifyPassPin(string passPin, int threshold){
			if (passPin.Length >= threshold && passPin.Length <= 32)
				return 0;
			else if (passPin.Length < threshold)
				return passPin.Length - 32;
			else
				return passPin.Length - 32;
		}
	}
}