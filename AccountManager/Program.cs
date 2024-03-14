using Newtonsoft.Json;

namespace AccountManager
{
	internal class Program
	{
		static DataBase dataBase;
		static string path = "C:\\Users\\Alexa\\Desktop\\test\\Dates.json";
		static Encryptor encryptor = new Encryptor();

		static void Main(string[] args)
		{
			dataBase = InitData();
			Menu();
		}

		public static void Menu()
		{
			while (true)
			{
				PrintMainMenu();

				uint choice;
				while (!uint.TryParse(ReadNotNullableLine(), out choice) || choice > 5)
				{
					Console.WriteLine("Invalid value. Try again");
				}

				switch (choice)
				{
					case 1:
						{
							Console.WriteLine("Add your dates to sign. \nAdd your login");
							string login = ReadNotNullableLine();

							foreach (UserData user in dataBase.AllUserData)
							{
								while (user.Login == login)
								{
									Console.ForegroundColor = ConsoleColor.Yellow;
									Console.WriteLine("This login is used");
									Console.ResetColor();
									Console.WriteLine("Try again");
									login = ReadNotNullableLine();
								}
							}
							Console.WriteLine("Add your password");
							string password = ReadNotNullableLine();
							Console.WriteLine("Add your key");
							string key = ReadNotNullableLine();
							Console.WriteLine("Confirm your key, please");
							string keyCofirm = ReadNotNullableLine();
							if (key == keyCofirm)
							{
								AddUser(login, password, key);
								Console.ForegroundColor = ConsoleColor.Green;
								Console.WriteLine("Success");
								Console.ResetColor();
							}
							else
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Password mismatch");
								Console.ResetColor();
							}

							int answerReturn;
							PrintReturnMainMenu();
							while (!int.TryParse(ReadNotNullableLine(), out answerReturn))
							{
								Console.ForegroundColor = ConsoleColor.Red;
								Console.WriteLine("Uncorrect dates");
								Console.ResetColor();
								PrintReturnMainMenu();
							}
							if (answerReturn != 1)
							{
								return;
							}
							break;
						}
					case 2:
						{
							Console.WriteLine("All logins:");
							foreach (UserData user in dataBase.AllUserData)
							{
								Console.WriteLine(user.Login);
							}
							Console.WriteLine("Press key to continue");
							Console.ReadKey();
							break;
						}
					case 3:
						{
							Console.WriteLine("Enter your login");
							string login = ReadNotNullableLine();
							SearchByLogin(login);
							Console.WriteLine("Press key to continue");
							Console.ReadKey();
							break;
						}
					case 4:
						{
							Console.WriteLine("Enter your login");
							string userLogin = ReadNotNullableLine();
							GetDelete(userLogin);
							break;
						}

				}
			}
		}

		private static void PrintMainMenu()
		{
			Console.Clear();
			Console.WriteLine("Welcome to Account Manager");
			Console.WriteLine("Menu:");
			Console.WriteLine("1. - Add new credentials ");
			Console.WriteLine("2. - List all credentials");
			Console.WriteLine("3. - Find and show credentials");
			Console.WriteLine("4. - Delete credentials");
			Console.WriteLine("0. - Exit");
		}

		private static void PrintReturnMainMenu()
		{
			Console.WriteLine("Do you want to return to menu?\n 1. - Yes\n 2. - No");
			Console.WriteLine("1. - Yes");
			Console.WriteLine("2. - No");
		}

		public static void AddUser(string login, string password, string key)
		{
			UserData user = new UserData();
			user.Login = login;
			user.Password = encryptor.Encrypt(password, key);
			Console.WriteLine($"User login is  {user.Login}  user password is {user.Password}");
			dataBase.AllUserData.Add(user);
			string serializedText = JsonConvert.SerializeObject(dataBase);
			File.WriteAllText(path, serializedText);
		}

		private static DataBase InitData()
		{
			bool isExist = File.Exists(path);
			if (isExist)
			{
				string json = File.ReadAllText(path);
				return JsonConvert.DeserializeObject<DataBase>(json);
			}
			dataBase = new DataBase();
			dataBase.AllUserData = new List<UserData>();
			return dataBase;
		}

		public static void SearchByLogin(string userLogin)
		{
			foreach (UserData user in dataBase.AllUserData)
			{
				if (user.Login == userLogin)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("This login is exist");
					Console.ResetColor();
					Console.WriteLine($"your login is {user.Login} password is {user.Password}");
					Console.WriteLine("Do you want to decrypt your password? \n1.Yes \n2.No");
					int answer = int.Parse(ReadNotNullableLine());
					if (answer == 1)
					{
						Console.WriteLine("Enter your key");
						string key = ReadNotNullableLine();
						try
						{
							string decryptedPassword = encryptor.Decrypt(user.Password, key);
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine($"Your password is {decryptedPassword}");
							Console.ResetColor();
						}
						catch (Exception)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Your code is wrong. Try again");
							Console.ResetColor();
						}
					}
					return;
				}
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("This login is not founded");
			Console.ResetColor();
		}

		public static void GetDelete(string login)
		{
			foreach (UserData user in dataBase.AllUserData)
			{
				if (user.Login == login)
				{
					Console.WriteLine("This login is exist");
					//Console.WriteLine($"your login is {user.Login} password is {user.Password}");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"Are you sure want to delete {user.Login}? \n1.Yes \n2.No");
					Console.ResetColor();
					int answer = int.Parse(ReadNotNullableLine());
					if (answer == 1)
					{
						Console.WriteLine("Enter your key");
						string key = ReadNotNullableLine();
						try
						{
							string toDecrypt = encryptor.Decrypt(user.Password, key);
							dataBase.AllUserData.Remove(user);
							string resultTry = JsonConvert.SerializeObject(dataBase);
							File.WriteAllText(path, resultTry);
							Console.WriteLine($"This user is deleted");
						}
						catch (Exception)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine("Your key is wrong. Try again");
							Console.ResetColor();
						}
					}
					return;
				}
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("This login is not founded");
			Console.ResetColor();
		}

		public static string ReadNotNullableLine()
		{
			string text;
			while (true)
			{
				text = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(text))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Input is empty");
					Console.ResetColor();
					Console.WriteLine("Try again");
				}
				else
				{
					return text;
				}
			}
		}
	}
}

