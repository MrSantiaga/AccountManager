using Newtonsoft.Json;

namespace AccountManager
{
	internal class Program
	{
		static void Main(string[] args)
		{
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
							UserCredentials credentials = new UserCredentials();
							Encryptor first = new Encryptor();
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
			string path = $"C:\\Users\\Alexa\\Desktop\\test\\{login}_{DateTime.Now:dd-MM-yyyy_hh-mm-ss}.json";
			string resultTry = JsonConvert.SerializeObject(encryptedPassword);
			File.WriteAllText(path, $"login is \"{login}\"  password is {resultTry}");
		}

	}
}