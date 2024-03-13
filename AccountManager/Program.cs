using Newtonsoft.Json;

namespace AccountManager
{
	internal class Program
	{
		static DataBase dataBase;
		static string path = "C:\\Users\\Alexa\\Desktop\\test\\Dates.json";
		static void Main(string[] args)
		{
			dataBase = InitData();
			Menu();
		}

		static public void Menu()
		{
			while (true)
			{
				PrintMainMenu();

				uint choice;
				while (!uint.TryParse(Console.ReadLine(), out choice) || choice > 5)
				{
					Console.WriteLine("Invalid value. Try again");
				}

				switch (choice)
				{
					case 1:
						{
							Console.WriteLine("Add your dates to sign. \nAdd your login");
							string login = Console.ReadLine();
							foreach (UserData user in dataBase.AllUserData)
							{
								while (user.Login == login)
								{
									Console.ForegroundColor = ConsoleColor.Yellow;
									Console.WriteLine("This login is used");
									Console.ResetColor();
									Console.WriteLine("Try again");
									login = Console.ReadLine();
								}
							}
							Console.WriteLine("Add your password");
							string password = Console.ReadLine();
							Console.WriteLine("Add your key");
							string key = Console.ReadLine();
							Console.WriteLine("Confirm your key, please");
							string keyCofirm = Console.ReadLine();
							if (key == keyCofirm)
							{
								AddUser(login, password, key);
								Console.WriteLine("Success");
							}
							else
							{
								Console.WriteLine("Password mismatch");
							}

							PrintReturnMainMenu();
							bool tryParse2 = int.TryParse(Console.ReadLine(), out int answerRetutn);

							while (!tryParse2)
							{
								Console.WriteLine("Uncorrect dates");
								PrintReturnMainMenu();
								tryParse2 = int.TryParse(Console.ReadLine(), out answerRetutn);
							}
							if (answerRetutn == 1)
							{
								continue;
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
							string userLogin = Console.ReadLine();
							GetSearch(userLogin);
							Console.WriteLine("Press key to continue");
							Console.ReadKey();
							break;
						}
					case 4:
						{
							GetDelete();
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
			Console.WriteLine("1. - Add new credentails ");
			Console.WriteLine("2. - List all credentiles");
			Console.WriteLine("3. - Find and show credentiles");
			Console.WriteLine("4. - Delete credentiles");
			Console.WriteLine("0. - Exit");
		}

		private static void PrintReturnMainMenu()
		{
			Console.WriteLine("Do you want to return to menu?\n 1. - Yes\n 2. - No");
		}


		public static void AddUserDatesJson(string login, string encryptedPassword)
		{
			UserData user = new UserData();
			user.Login = login;
			user.Password = encryptedPassword;
			dataBase.AllUserData.Add(user);
			string resultTry = JsonConvert.SerializeObject(dataBase);
			File.WriteAllText(path, resultTry);
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

		public static void GetSearch(string userLogin)
		{
			foreach (UserData user in dataBase.AllUserData)
			{
				if (user.Login == userLogin)
				{
					Console.WriteLine("This login is exist");
					Console.WriteLine($"your login is {user.Login} password is {user.Password}");
					Console.WriteLine("Do you want to decrypt your password? \n1.Yes \n2.No");
					int answer = int.Parse(Console.ReadLine());
					if (answer == 1)
					{
						using Encryptor first = new Encryptor();
						Console.WriteLine("Enter your key");
						string key = Console.ReadLine();
						try
						{
							string toDecrypt = first.Decrypt(user.Password, key);
							Console.WriteLine($"Your password is {toDecrypt}");
						}
						catch (Exception)
						{
							Console.WriteLine("Your code is wrong. Try again");
						}
					}
					return;
				}
			}
			Console.WriteLine("This login is not founded");
		}

		public static void GetDelete()
		{
			Console.WriteLine("Enter your login");
			string userLogin = Console.ReadLine();
			foreach (UserData user in dataBase.AllUserData)
			{
				if (user.Login == userLogin)
				{
					Console.WriteLine("This login is exist");
					//Console.WriteLine($"your login is {user.Login} password is {user.Password}");
					Console.WriteLine($"Are you sure want to delete {user.Login}? \n1.Yes \n2.No");
					int answer = int.Parse(Console.ReadLine());
					if (answer == 1)
					{
						using Encryptor first = new Encryptor();
						Console.WriteLine("Enter your key");
						string key = Console.ReadLine();
						try
						{
							string toDecrypt = first.Decrypt(user.Password, key);
							dataBase.AllUserData.Remove(user);
							string resultTry = JsonConvert.SerializeObject(dataBase);
							File.WriteAllText(path, resultTry);
							Console.WriteLine($"This user is deleted");
						}
						catch (Exception)
						{
							Console.WriteLine("Your code is wrong. Try again");
						}
					}
					return;
				}
			}
			Console.WriteLine("This login is not founded");
		}


		public static void AddUser(string login, string password, string key)
		{
			UserData user1 = new UserData();
			using Encryptor first = new Encryptor();
			user1.Login = login;
			user1.Password = first.Encrypt(password, key);
			Console.WriteLine($"User login is  {user1.Login}  user password is {user1.Password}");
			AddUserDatesJson(user1.Login, user1.Password);
		}

	}
}