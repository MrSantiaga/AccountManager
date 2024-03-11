using System.Security.Cryptography;
using System.Text;

namespace AccountManager
{
	internal class Encryptor : IDisposable
	{
		private Aes _aes;
		private byte[] _vector;

		public Encryptor() : this("ho-ho-ho")
		{

		}
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

		public string Encrypt(string password, string login)
		{
			byte[] loginBytes = GetLogin(login);
			_aes.Key = loginBytes;
			ICryptoTransform encryptor = _aes.CreateEncryptor();
			using MemoryStream ms = new MemoryStream();
			using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
			using (StreamWriter sw = new StreamWriter(cs))
			{
				sw.Write(password);
			}
			byte[] encryptedPassword = ms.ToArray();
			return Convert.ToBase64String(encryptedPassword);

		}
		private byte[] GetLogin(string login)
		{
			byte[] loginBytes = Encoding.UTF8.GetBytes(login);
			using SHA256 sha256 = SHA256.Create();
			return sha256.ComputeHash(loginBytes);
		}
		private byte[] GetVector(string vector)
		{
			byte[] keyBytes = Encoding.UTF8.GetBytes(vector);
			using MD5 mD5 = MD5.Create();
			return mD5.ComputeHash(keyBytes);
		}
	}
}









