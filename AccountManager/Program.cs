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
			Console.Clear();
			Console.WriteLine("Welcome to Account Manager");
			Console.WriteLine("Menu: \n1. - Add new credentails \n2. - List all credentiles \n3. - Find and show credentiles \n4. - Delete credentiles \n0. - Exit");

			bool tryParse = int.TryParse(Console.ReadLine(), out int choose);
			while ((!tryParse) || (choose < 0 || choose > 5))
			{
				Console.WriteLine("Unknown value. Try again");
				int.TryParse(Console.ReadLine(), out choose);
			}

			switch (choose)
			{
				case 1:
					{
						Dates user = new Dates();
						Encryptor first = new Encryptor();
						Console.WriteLine("Add your dates to sign. \nAdd your login");
						string login = Console.ReadLine();
						Console.WriteLine("Add your password");
						string password = Console.ReadLine();
						Console.WriteLine("Repeat your password, please");
						string passwordRepeat = Console.ReadLine();
						if (password == passwordRepeat)
						{
							Console.WriteLine("Success");
							user.Login = login;
							user.Password = first.Encrypt(password, login);
							Console.WriteLine($"User login is  {user.Login}  user password is {user.Password}");
						}
						else
						{
							Console.WriteLine("Password mismatch");
						}
						Console.WriteLine("Do you want to return to menu?\n 1. - Yes\n 2. - No");
						int answerRetutn = int.Parse(Console.ReadLine());
						if (answerRetutn == 1)
						{
							Menu();
						}
						break;
					}
				case 2:
					{
						break;
					}
				case 3:
					{
						break;
					}
				case 4:
					{
						break;

					}
				case 0:
					{
						break;
					}

			}
		}
	}
}
