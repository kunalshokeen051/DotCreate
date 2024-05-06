using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotCreate
{
	public class Menu
	{
		public Dictionary<string, string> UserChoices { get; private set; }

		public Menu()
		{
			UserChoices = [];
		}

		public Dictionary<string,string> ShowMenu()
		{
			Console.Clear();
			Console.WriteLine("Choose a project type:");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("1. ASP.NET 5.0 MVC");
			Console.WriteLine("2. ASP.NET Core MVC");
			Console.WriteLine("3. ASP.NET Core API");

			Console.ResetColor();

			Console.Write("Enter your choice: ");

			string projectType = Console.ReadLine();
			UserChoices["ProjectType"] = projectType;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Do you want to use SCSS styling? (T/F): ");
			Console.ForegroundColor = ConsoleColor.Red;
			string useScss = Console.ReadLine();
			UserChoices["UseScss"] = useScss;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Do you want to add jQuery? (T/F): ");
			Console.ForegroundColor = ConsoleColor.Red;
			string addJQuery = Console.ReadLine();
			UserChoices["AddJQuery"] = addJQuery;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Do you want to add Bootstrap packages? (T/F): ");
			Console.ForegroundColor = ConsoleColor.Red;
			string addBootstrap = Console.ReadLine();
			UserChoices["AddBootstrap"] = addBootstrap;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("Do you want to use class libraries? (T/F): ");
			Console.ForegroundColor = ConsoleColor.Red;
			string useClassLibraries = Console.ReadLine();
			UserChoices["UseClassLibraries"] = useClassLibraries;

			Console.ResetColor();

			return UserChoices;
		}

		public void DisplayUserChoices()
		{
			Console.WriteLine("\nYour choices:");
			foreach (var choice in UserChoices)
			{
				Console.WriteLine($"{choice.Key}: {choice.Value}");
			}
		}
	}
}
