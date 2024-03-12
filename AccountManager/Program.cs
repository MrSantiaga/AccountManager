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
							UserData credentials = new UserData();
							using Encryptor first = new Encryptor();
							Console.WriteLine("Add your dates to sign. \nAdd your login");
							string login = Console.ReadLine();
							Console.WriteLine("Add your password");
							string password = Console.ReadLine();
							Console.WriteLine("Confirm your password, please");
							string passwordRepeat = Console.ReadLine();
							if (password == passwordRepeat)
							{
								Console.WriteLine("Success");
								credentials.Login = login;
								credentials.Password = first.Encrypt(password, login);
								Console.WriteLine($"User login is  {credentials.Login}  user password is {credentials.Password}");
								GetUserCreditJson(credentials.Login, credentials.Password);
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
					case 3:
						{
							GetSearch();
							Console.WriteLine("Press key to continue");
							Console.ReadKey();
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


		public static void GetUserCreditJson(string login, string encryptedPassword)
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

		public static void GetSearch()
		{
			Console.WriteLine("Enter your login");
			string userLogin = Console.ReadLine();
			foreach (UserData user in dataBase.AllUserData)
			{
				if (user.Login == userLogin)
				{
					Console.WriteLine("This login is exist");
					Console.WriteLine($"your login is {user.Login} password is {user.Password}");
					return;
				}
			}
			Console.WriteLine("This login is not founded");
		}


	}
}