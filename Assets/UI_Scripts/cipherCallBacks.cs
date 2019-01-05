using cryptServCS;
using System.Text;
using System.IO;

public class cipherCallBacks {

	public static void __CIPHER(string filePath, string cipherFileDir, string password, string keyFilePath, int AES_MODE)
	{
		int length = AES_MODE / 8;
		byte[] key = new byte[length];

		if (keyFilePath.Equals ("_EMPTY")) {
			byte[] store = Encoding.ASCII.GetBytes (password);
			for (ushort i = 0; i < key.Length; i++) {
				if (i < store.Length)
					key [i] = store [i];
				else
					key [i] = 0x00;
			}
				
		} else if (password.Equals ("_EMPTY"))
		{
			FileStream keyInput = new FileStream (keyFilePath, FileMode.Open, FileAccess.Read);
			keyInput.Read (key, 0, key.Length);
			keyInput.Close ();
		}
			
		if(AES_MODE == 128)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_128);
			encrypt.EncryptFile (filePath, cipherFileDir, key);
		}else if(AES_MODE == 192)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_192);
			encrypt.EncryptFile (filePath, cipherFileDir, key);
		}else if(AES_MODE == 256)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_256);
			encrypt.EncryptFile (filePath, cipherFileDir, key);
		}
	}

	public static void __INV_CIPHER(string filePath, string cipherFileDir, string password, string keyFilePath, int AES_MODE)
	{
		int length = AES_MODE / 8;
		byte[] key = new byte[length];

		if (keyFilePath.Equals ("_EMPTY")) {
			byte[] store = Encoding.ASCII.GetBytes (password);
			for (ushort i = 0; i < key.Length; i++) {
				if (i < store.Length)
					key [i] = store [i];
				else
					key [i] = 0x00;
			}

		} else if (password.Equals ("_EMPTY"))
		{
			FileStream keyInput = new FileStream (keyFilePath, FileMode.Open, FileAccess.Read);
			keyInput.Read (key, 0, key.Length);
			keyInput.Close ();
		}

		if(AES_MODE == 128)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_128);
			encrypt.DecryptFile (filePath, cipherFileDir, key);
		}else if(AES_MODE == 192)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_192);
			encrypt.DecryptFile (filePath, cipherFileDir, key);
		}else if(AES_MODE == 256)
		{
			AES_CxDecipher encrypt = new AES_CxDecipher (AES_CxDecipher.AES_256);
			encrypt.DecryptFile (filePath, cipherFileDir, key);
		}

	}
}