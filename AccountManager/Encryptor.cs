using System.Security.Cryptography;
using System.Text;

namespace AccountManager
{
	internal class Encryptor : IDisposable
	{
		private Aes _aes;
		private byte[] _vector;

		public Encryptor() : this("123Ioliutio98")
		{ }

		public Encryptor(string vector)
		{
			_aes = Aes.Create();
			_vector = GetVector(vector);
			_aes.IV = _vector;
		}

		public void Dispose()
		{
			_aes.Dispose();
		}

		public string Encrypt(string plainText, string key)
		{
			byte[] keyBytes = GetKey(key);
			_aes.Key = keyBytes;
			ICryptoTransform encryptor = _aes.CreateEncryptor();
			using MemoryStream ms = new MemoryStream();
			using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
			using (StreamWriter sw = new StreamWriter(cs))
			{
				sw.Write(plainText);
			}
			byte[] encryptedPassword = ms.ToArray();
			string encryptedPasswordText = Convert.ToBase64String(encryptedPassword);
			return encryptedPasswordText;
		}

		public string Decrypt(string encryptedText, string key)
		{
			byte[] decryptedBytes = Convert.FromBase64String(encryptedText);
			byte[] keyByte = GetKey(key);
			_aes.Key = keyByte;
			ICryptoTransform encryptor = _aes.CreateDecryptor();
			using MemoryStream ms = new MemoryStream(decryptedBytes);
			using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read);
			using StreamReader sr = new StreamReader(cs);
			return sr.ReadToEnd();
		}

		private byte[] GetKey(string key)
		{
			byte[] keyBytes = Encoding.UTF8.GetBytes(key);
			using SHA256 sha256 = SHA256.Create();
			return sha256.ComputeHash(keyBytes);
		}

		private byte[] GetVector(string vector)
		{
			byte[] keyBytes = Encoding.UTF8.GetBytes(vector);
			using MD5 mD5 = MD5.Create();
			return mD5.ComputeHash(keyBytes);
		}
	}
}









