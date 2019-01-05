using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace cryptServCS
{
	public class AES_CxDecipher
	{
		private static readonly byte[] S_BOX = new byte[] {
			///  0     1     2     3     4     5     6     7     8     9     A     B     C     D     E     F   /
			0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
			0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
			0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
			0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
			0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
			0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
			0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
			0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
			0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
			0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
			0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
			0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
			0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
			0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
			0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
			0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
		};

		private static readonly byte[] invS_BOX = new byte[] {
			///  0     1     2     3     4     5     6     7     8     9     A     B     C     D     E     F   /
			0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
			0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
			0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
			0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
			0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
			0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
			0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
			0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
			0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
			0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
			0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
			0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
			0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
			0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
			0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
			0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d
		};

		private static readonly byte[] R_CON = new byte[] {
			0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36 ///required R_CON values for AES implementation/
		};
		private static readonly byte[] verify = new byte[] {
			0x1e, 0x6f, 0x24, 0xb8, 0xce, 0xf4, 0x91, 0x4b, 0x0d, 0x14, 0x28, 0xe2, 0x63, 0x8f, 0x70, 0xdc
		};

		private short Nr;
		private short Nk;

		public const byte AES_128 = 0x80;
		public const byte AES_192 = 0xC0;
		public const byte AES_256 = 0x10;

		public static float percentCompleted;
		public static bool incorrectKey = false;

		public AES_CxDecipher (byte bitMode)
		{
			if (bitMode == AES_CxDecipher.AES_128) {
				this.Nk = 4;
				this.Nr = 10;
			} else if (bitMode == AES_CxDecipher.AES_192) {
				this.Nk = 6;
				this.Nr = 12;
			} else if (bitMode == AES_CxDecipher.AES_256){
				this.Nk = 8; 
				this.Nr = 14; 
			}
		}

		//Miscellaneous Routines/
		static void swap(ref byte x, ref byte y)
		{
			byte temp = x;
			x = y;
			y = temp;
		}

		//Wrapper Routines/
		public void EncryptFile(string inputFilePath, string cipherFileDirectory, byte[] key)
		{
			ProgressBar.ProgressRadialBehaviour.encryptionActive = true;
			FileStream inputFile = new FileStream (inputFilePath, FileMode.Open, FileAccess.Read);
			long inputFileSize = new FileInfo (inputFilePath).Length;

			string fileName = Path.GetFileName (Path.ChangeExtension (inputFilePath, ".cipher"));
			string cipherPath = Path.Combine (cipherFileDirectory, fileName);

			FileStream cipherFile = new FileStream(cipherPath, FileMode.OpenOrCreate, FileAccess.Write);

			byte[] extensionBytes = new byte[16];

			string extension = Path.GetExtension (inputFilePath);
			byte[] store = Encoding.ASCII.GetBytes (extension);

			for (int i = 0; i < store.Length; i++)
				extensionBytes [i] = store [i];

			byte[] input = new byte[16]; 
			byte[] output = new byte[16]; 
			byte[] xorVector = new byte[16];

			byte[] keySchedule = new byte[16 * (this.Nr + 1)];

			this.keyExpansion(key, keySchedule);
			this.generateInitVector(xorVector);

			for (ushort i = 0; i < 16; i++)
				input [i] = AES_CxDecipher.verify [i];

			CBC_Encryption(input, output, xorVector, keySchedule);

			cipherFile.Write (xorVector, 0, 16);
			cipherFile.Write (output, 0, 16);
			cipherFile.Write (extensionBytes, 0, 16);

			ulong iterations = (ulong)inputFileSize / 16;

			for (ulong i = 0; i < iterations; i++)
			{
				inputFile.Read (input, 0, 16);
				percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
				CBC_Encryption (input, output, xorVector, keySchedule);

				for (int j = 0; j < 16; j++)
					xorVector[j] = output[j];

				cipherFile.Write(output, 0, 16);
			}

			if(inputFileSize % 16 != 0)
			{
				inputFile.Read(input, 0, 16);
				percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
				this.padBlock(inputFileSize, input);
				CBC_Encryption(input, output, xorVector, keySchedule);
				cipherFile.Write(output, 0, 16);
			}
			percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
			inputFile.Close();
			cipherFile.Close();
		}

		public void DecryptFile(string cipherFilePath, string outputFileDir, byte[] key)
		{
			ProgressBar.ProgressRadialBehaviour.encryptionActive = false;
			FileStream inputFile = new FileStream (cipherFilePath, FileMode.Open, FileAccess.Read);
			long inputFileSize = new FileInfo (cipherFilePath).Length;

			byte[] input = new byte[16];
			byte[] output = new byte[16];
			byte[] xorVector = new byte[16];
			byte[] extensionBytes = new byte[16];

			byte[] keySchedule =  new byte[16 * (this.Nr + 1)];

			this.keyExpansion(key, keySchedule);
			inputFile.Read (xorVector, 0, 16);
			inputFile.Read (input, 0, 16);
			CBC_Decryption(input, output, xorVector, keySchedule);

			for (ushort i = 0; i < 16; i++)
				if (output [i] != AES_CxDecipher.verify [i]) {
					incorrectKey = true;
					inputFile.Close ();
					return;
				}
			incorrectKey = false;

			inputFile.Read (extensionBytes, 0, 16);
			percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
			string deciphName = Path.GetFileNameWithoutExtension (cipherFilePath);
			string ext = Encoding.ASCII.GetString (extensionBytes);

			int garbageOffset = 0;
			for (int i = 0; i < 16; i++)
			{
				if (extensionBytes [i] != 0x00)
					garbageOffset += 1;
				else
					break;
			}

			ext = ext.Remove (garbageOffset, 16 - garbageOffset);
			deciphName = string.Concat (deciphName, ext);
			string outputFile = Path.Combine (outputFileDir, deciphName);

			FileStream decipherFile = new FileStream (outputFile, FileMode.OpenOrCreate, FileAccess.Write);

			ulong iterations = (ulong) inputFileSize / 16;

			for(ulong i = 0; i < iterations - 3; i++)
			{
				if(i == iterations - 4)
				{
					inputFile.Read(input, 0, 16);
					percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
					CBC_Decryption(input, output, xorVector, keySchedule);

					for(ushort j = 0; j < 16; j++)
					{
						if (output [j] != 0x80)
							decipherFile.WriteByte (output [j]);
						else
							break;
					}
					break;
				}

				inputFile.Read(input, 0, 16);
				percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
				CBC_Decryption(input, output, xorVector, keySchedule);

				for (ushort j = 0; j < 16; j++)
					xorVector [j] = input [j];

				decipherFile.Write(output, 0, 16);
			}

			percentCompleted = ((float)inputFile.Position / (float)inputFileSize) * 100;
			inputFile.Close();
			decipherFile.Close();
		}

		//ROUTINES FOR AES OPERATION IN CBC MODE/
		void CBC_Encryption(byte[] plainText, byte[] outputCipherText, byte[] xorVector, byte[] keySchedule)
		{
			for (ushort i = 0; i < 16; i++)
				plainText [i] ^= xorVector [i];

			AES_CIPHER (plainText, outputCipherText, keySchedule);
		}

		void CBC_Decryption(byte[] cipherText, byte[] outputPlainText, byte[] xorVector, byte[] keySchedule)
		{
			AES_INVERSE_CIPHER (cipherText, outputPlainText, keySchedule);

			for (ushort i = 0; i < 16; i++)
				outputPlainText [i] ^= xorVector [i];
		}

		///COMMON ROUTINES FOR CIPHER AND DECIPHER/
		static byte byteProduct(byte x, byte y) ///returns product for GF(2^8)
		{
			byte result = 0x00;
			byte temp;

			while(x != 0)
			{
				if((x & 1) != 0)
					result ^= y;

				temp = (byte)(y & 0x80);
				y = (byte)(y << 1);

				if (temp != 0)
					y = (byte)(y ^ 0x1b);

				x = (byte)(x >> 1);
			}

			return result;
		}

		void keyExpansion(byte[] key, byte[] keySchedule)
		{
			byte[] temp = new byte[4];
			ushort i = 0;

			for(; i < this.Nk; i++)
			{
				for(ushort j = 0; j < 4; j++)
				{
					keySchedule [4 * i + j] = key [4 * i + j];
				}
			}

			i = (ushort)this.Nk;

			while(i < (4 * (this.Nr + 1)))
			{
				for( ushort j = 0; j < 4; j++)
				{
					temp[j] = keySchedule[4 * (i - 1) + j];
				}

				if(i % this.Nk == 0)
				{
					for(ushort j = 0; j < 3; j++) ///RotWord SubRoutine/
					{
						AES_CxDecipher.swap (ref temp [j], ref temp [j + 1]);
					}

					for(ushort j = 0; j < 4; j++) ///SubWord SubRoutine/
					{
						temp[j] = get_SBOX_value(temp[j]);
					}

					for(ushort j = 0; j < 4; j++)
					{
						if(j == 0)
							temp[j] ^= R_CON[(i / this.Nk) - 1];

						else
							temp[j] ^= 0x00;
					}

				}

				else if((this.Nk > 6) && (i % this.Nk == 4))
				{
					for(ushort j = 0; j < 4; j++) ///SubWord SubRoutine/
					{
						temp[j] = get_SBOX_value(temp[j]);
					}
				}

				for(ushort j = 0; j < 4; j++)
				{
					keySchedule [(4 * i) + j] = (byte)(keySchedule [(4 * (i - this.Nk)) + j] ^ temp [j]);
				}

				i += 1;
			}
		}

		void addRoundKey(byte [][] state, byte[] keySchedule, int round)
		{
			for(ushort i = 0; i < 4; i++)
				for(ushort j = 0; j < 4; j++)
				{
					state[j][i] ^= keySchedule[4 * (round * 4 + i) + j];
				}
		}

		///CIPHER SUBROUTINES/
		static byte get_SBOX_value(byte byteIndex)
		{
			return(S_BOX[byteIndex]);
		}

		void subBytes(byte [][] state)
		{
			for(ushort i = 0; i < 4; i++)
			{
				for(ushort j = 0; j < 4; j++)
				{
					state[i][j] = AES_CxDecipher.get_SBOX_value(state [i][j]);
				}
			}
		}

		void shiftRows(byte [][] state)
		{
			///no shift row is to be applied on row 0/

			///shift row operation on row 1/
			for(ushort i = 0; i < 3; i++)
			{
				AES_CxDecipher.swap (ref state[1][i], ref state[1][i + 1]);
			}

			///shift row operation on row 2/
			for(ushort i = 0; i < 2; i++)
			{
				for (ushort j = 0; j < 3; j++)
				{
					AES_CxDecipher.swap (ref state [2] [j], ref state [2] [j + 1]);
				}
			}

			///shift row operation on row 3/
			for(ushort i = 0; i < 3; i++)
			{
				for(ushort j = 0; j < 3; j++)
				{
					AES_CxDecipher.swap (ref state [3] [j], ref state [3] [j + 1]);
				}
			}
		}

		void mixColumns(byte [][] state)
		{
			byte[] temp = new byte[4];

			for(ushort i = 0; i < 4; i++)
			{
				for(ushort j = 0; j < 4; j++)
				{
					temp [j] = state [j] [i];
				}

				temp [0] = (byte)(byteProduct (0x02, state [0] [i]) ^ byteProduct (0x03, state [1] [i]) ^ state [2] [i] ^ state [3] [i]);
				temp [1] = (byte)(state [0] [i] ^ byteProduct (0x02, state [1] [i]) ^ byteProduct (0x03, state [2] [i]) ^ state [3] [i]);
				temp [2] = (byte)(state [0] [i] ^ state [1] [i] ^ byteProduct (0x02, state [2] [i]) ^ byteProduct (0x03, state [3] [i]));
				temp [3] = (byte)(byteProduct (0x03, state [0] [i]) ^ state [1] [i] ^ state [2] [i] ^ byteProduct (0x02, state [3] [i]));

				for(ushort j = 0; j < 4; j++)
				{
					state [j] [i] = temp [j];
				}
			}
		}

		///DECIPHER SUBROUTINES/
		static byte get_invSBOX_value(byte byteIndex)
		{
			return(invS_BOX [byteIndex]);
		}

		void invSubBytes(byte [][] inv_state)
		{
			for(ushort i = 0; i < 4; i++)
			{
				for(ushort j = 0; j < 4; j++)
				{
					inv_state [i] [j] = AES_CxDecipher.get_invSBOX_value (inv_state [i] [j]);
				}
			}
		}

		void invShiftRows(byte [][] inv_state)
		{
			///no inverse shift row is to be applied on row 0/

			///inverse shift row operation on row 1/
			for (ushort i = 3; i > 0; i--)
				AES_CxDecipher.swap (ref inv_state [1] [i], ref inv_state [1] [i - 1]);

			///inverse shift row operation on row 2/
			for(ushort i = 0; i < 2; i++)
			{
				for(ushort j = 3; j > 0; j--)
				{
					AES_CxDecipher.swap (ref inv_state [2] [j], ref inv_state [2] [j - 1]);
				}
			}

			///inverse shift row operation on row 3/
			for(ushort i = 0; i < 3; i++)
			{
				for(ushort j = 3; j > 0; j--)
				{
					AES_CxDecipher.swap (ref inv_state [3] [j], ref inv_state [3] [j - 1]);
				}
			}
		}

		void invMixColumns(byte [][] inv_state)
		{
			byte[] res = new byte[4];

			for (ushort i = 0; i < 4; i++)
			{
				for (ushort j = 0; j < 4; j++)
				{
					res [j] = inv_state [j] [i];
				}

				res [0] = (byte)((((byteProduct (inv_state [0] [i], 0x0e) ^ byteProduct (inv_state [1] [i], 0x0b)) ^ byteProduct (inv_state [2] [i], 0x0d)) ^ byteProduct (inv_state [3] [i], 0x09)));
				res [1] = (byte)((((byteProduct (inv_state [0] [i], 0x09) ^ byteProduct (inv_state [1] [i], 0x0e)) ^ byteProduct (inv_state [2] [i], 0x0b)) ^ byteProduct (inv_state [3] [i], 0x0d)));
				res [2] = (byte)((((byteProduct (inv_state [0] [i], 0x0d) ^ byteProduct (inv_state [1] [i], 0x09)) ^ byteProduct (inv_state [2] [i], 0x0e)) ^ byteProduct (inv_state [3] [i], 0x0b)));
				res [3] = (byte)((((byteProduct (inv_state [0] [i], 0x0b) ^ byteProduct (inv_state [1] [i], 0x0d)) ^ byteProduct (inv_state [2] [i], 0x09)) ^ byteProduct (inv_state [3] [i], 0x0e)));


				for(ushort j = 0; j < 4; j++)
				{
					inv_state [j] [i] = res [j];
				}
			}
		}

		///CIPHER/DECIPHER ROUTINES/
		void AES_CIPHER(byte[] plainText, byte[] cipherText, byte[] keySchedule)
		{
			byte[][] state = new byte[4][];
			state [0] = new byte[4];
			state [1] = new byte[4];
			state [2] = new byte[4];
			state [3] = new byte[4];

			/// state = plainText /
			for (ushort r = 0; r < 4; r++)
				for (ushort c = 0; c < 4; c++)
				{
					state [r] [c] = plainText [r + 4 * c];
				}

			this.addRoundKey(state, keySchedule, 0);

			for (ushort round = 1; round <= this.Nr - 1; round++)
			{
				this.subBytes (state);
				this.shiftRows (state);
				this.mixColumns (state);
				this.addRoundKey (state, keySchedule, round);
			}

			this.subBytes (state);
			this.shiftRows (state);
			this.addRoundKey (state, keySchedule, this.Nr);

			/// cipherText = state /
			for (ushort r = 0; r < 4; r++)
				for (ushort c = 0; c < 4; c++)
				{
					cipherText [r + 4 * c] = state [r] [c];
				}
		}

		void AES_INVERSE_CIPHER(byte[] cipherText, byte[] plainText, byte[] keySchedule)
		{
			byte[][] state = new byte[4][];
			state [0] = new byte[4];
			state [1] = new byte[4];
			state [2] = new byte[4];
			state [3] = new byte[4];


			/// state = cipherText /
			for (ushort r = 0; r < 4; r++)
				for (ushort c = 0; c < 4; c++)
				{
					state [r] [c] = cipherText [r + 4 * c];
				}

			this.addRoundKey (state, keySchedule, this.Nr);

			for (ushort round = (ushort)(this.Nr - 1); round >= 1; round--)
			{
				this.invShiftRows (state);
				this.invSubBytes (state);
				this.addRoundKey (state, keySchedule, round);
				this.invMixColumns (state);
			}

			this.invShiftRows (state);
			this.invSubBytes (state);
			this.addRoundKey (state, keySchedule, 0);

			/// plainText = state/
			for (ushort r = 0; r < 4; r++)
				for (ushort c = 0; c < 4; c++)
				{
					plainText [r + 4 * c] = state [r][c];
				}
		}

		///BLOCK CIPHER MODE OF OPERATION SUBROUTINES/
		void padBlock(long fileSize, byte[] inputBlock)
		{
			ushort padInitPos = (ushort)(fileSize % 16);
			inputBlock[padInitPos] = 0x80;

			for (padInitPos += 1; padInitPos < 16; padInitPos++)
			{
				inputBlock [padInitPos] = 0x00;
			}
		}

		void generateInitVector(byte[] IV_buff)
		{
			System.Random randomBytes = new System.Random();
			randomBytes.NextBytes(IV_buff);
		}

	}
		
}