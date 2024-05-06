using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using dotCreate;
using McMaster.Extensions.CommandLineUtils;

class Program
{
	static int Main(string[] args)
	{
		try
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("*** Welcome to Dotnet Project Builder ***\n");
			Console.ResetColor();
			Thread.Sleep(2000);

			if (args.Length < 1)
			{
				Console.WriteLine("No argument Provided");
			}
			else
			{
				bool isProjectExist = false;
				Dictionary<string, bool> OptionalFeatures = [];
				string ProjectDirectory = null;

				if (args[0].ToLower() == "create-app@latest")
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(@"Enter Project Location(default: C:/DotCreate_Projects)");
					Console.ResetColor();
					var DirectoryPath = Console.ReadLine() ;
					DirectoryPath = string.IsNullOrWhiteSpace(DirectoryPath) ? @"C:\DotCreate_Projects" : $@"C:\{DirectoryPath}";
					DirectoryPath = DirectoryPath.Replace(" ", @"\");

					//Console.WriteLine("Path " + DirectoryPath);
					
					if(!Directory.Exists(DirectoryPath)) 
					{
						Directory.CreateDirectory(DirectoryPath);
					}
					else
					{
						isProjectExist = true;
					}

					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Enter Project Name(default: myProject)");
					Console.ResetColor();
					string projectName = Console.ReadLine();
					projectName = string.IsNullOrEmpty(projectName) ?  isProjectExist ? $"myProject_{DateTime.Now.Ticks.ToString().Substring(0,5)}" : "myProject" : projectName;

					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Enter Project Type(default:mvcapp | options: webapi or mvc)");
					Console.ResetColor();
					var projectType = Console.ReadLine();
					projectType = string.IsNullOrEmpty(projectType) ? "mvc" : projectType;

					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Do you want to install Jquery?(Y/N)");
					Console.ResetColor();
					var t1 = Console.ReadLine();

					if (t1.ToUpper() is "Y" or "TRUE" or "T" or "YES")
					{
						OptionalFeatures.Add("jquery", true);
					}

					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Do you want to install Bootstrap?(Y/N)");
					Console.ResetColor();
					var t2 = Console.ReadLine();

					if (t2.ToUpper() is "Y" or "TRUE" or "T" or "YES")
					{
						OptionalFeatures.Add("bootstrap", true);
					}

					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Do you want to install a ORM?(Y/N)");
					Console.ResetColor();
					var t3 = Console.ReadLine();

					if (t3.ToUpper() is "Y" or "TRUE" or "T" or "YES")
					{
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("Which ORM You want to install: Dapper/Entity (Default: Dapper)");
						Console.ResetColor();
						var temp = Console.ReadLine();

						if (t3.ToLower().Trim() is "dapper")
							OptionalFeatures.Add("dapper", true);
						else if (t3.ToLower().Trim() is "entity")
							OptionalFeatures.Add("entity", true);
						else
							Console.WriteLine("Invalid ORM, Going with default value");
						OptionalFeatures.Add("dapper", true);
					}

					string solutionDirectory = $@"{DirectoryPath}/{projectName}";

					ProjectDirectory = Path.Combine(solutionDirectory, "PM.Portal");
					string[] classLibraryDirectories =
						{
							Path.Combine(solutionDirectory, "PM.Common"),
							Path.Combine(solutionDirectory, "PM.Repositories"),
							Path.Combine(solutionDirectory, "PM.Manager"),
							Path.Combine(solutionDirectory, "PM.Model")
						};

					if (!Directory.Exists(solutionDirectory))
					{
						Directory.CreateDirectory(solutionDirectory);
					}

					Console.Clear();
					Console.ForegroundColor = ConsoleColor.DarkGray;

					Console.WriteLine("Creating solution and projects...");
					DisplayLoader();

					RunCommand($"dotnet new sln -n {projectName} -o \"{solutionDirectory}\"");

					RunCommand($"dotnet new {projectType} -n PM.Portal -o \"{ProjectDirectory}\"");

					foreach (var kvp in OptionalFeatures)
					{
						if (kvp.Value)
							InstallNuGetPackage(kvp.Key, ProjectDirectory);
					}

					foreach (var classLibraryDir in classLibraryDirectories)
					{
						RunCommand($"dotnet new classlib -n {Path.GetFileName(classLibraryDir)} -o \"{classLibraryDir}\"");
					}

					RunCommand($"dotnet sln \"{solutionDirectory}\" add \"{ProjectDirectory}\"");
					foreach (var classLibraryDir in classLibraryDirectories)
					{
						RunCommand($"dotnet sln \"{solutionDirectory}\" add \"{classLibraryDir}\"");
					}

					if (OptionalFeatures.ContainsKey("dapper"))
					{
						Console.WriteLine("Adding Connection string for DB access using Dapper");
						string appSettingsPath = Path.Combine(ProjectDirectory, "appsettings.json");
						string connectionStringTemplate = @"
                                 {
                                   ""ConnectionStrings"": {
                                     ""DefaultConnection"":         ""Server=YourServerName;Database=YourDatabaseName;User=YourUsername;Password=YourPassword;""
                                   }
                                 }";

						File.WriteAllText(appSettingsPath, connectionStringTemplate);

						string programCsPath = Path.Combine(ProjectDirectory, "Program.cs");
						string programCsContent = File.ReadAllText(programCsPath);

						string[] lines = programCsContent.Split(Environment.NewLine);
						int builderIndex = Array.FindIndex(lines, line => line.Trim().StartsWith("var builder"));

						if (builderIndex != -1)
						{

							string connectionStringSetup = @"
                                 // Register Dapper in DI container
                                 builder.Services.AddTransient<IDbConnection>(sp =>
                                 {
                                     var config = sp.GetRequiredService<IConfiguration>();
                                     var connectionString = config.GetConnectionString(""DefaultConnection"");
                                     return new SqlConnection(connectionString);
                                 });
                                 ";

							lines = lines.Take(builderIndex + 1)
										 .Concat(new[] { connectionStringSetup })
										 .Concat(lines.Skip(builderIndex + 1))
										 .ToArray();

							programCsContent = string.Join(Environment.NewLine, lines);

							File.WriteAllText(programCsPath, programCsContent);

							InstallNuGetPackage("Microsoft.Data.SqlClient", ProjectDirectory);
							Console.WriteLine("Program.cs updated successfully.");

						}
					}

					Console.ResetColor();

					Console.Clear();
					Console.WriteLine("Solution and projects created successfully.");
				}

				else if (args[0].ToLower() is  "-h" or "-help")
				{
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Welcome to dotnetCreate!");
					Console.ResetColor();
					Console.WriteLine("This tool helps you quickly scaffold a starter project According to your needs.");
					Console.WriteLine("Usage: dc create-app@latest");
					Console.WriteLine("");
					Console.WriteLine("Available project types:");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("mvc : Create an ASP.NET Core MVC application");
					Console.WriteLine("webapi : Create an ASP.NET Core Web API application");
					Console.WriteLine("classlib : Create a .NET Core class library");
					Console.WriteLine("");
					Console.ResetColor();
					Console.WriteLine("");
				}
				else
				{
				 Console.WriteLine("Invalid argument provided");
					return 0;
				}
			}

			Console.ReadKey();
			return 1;

		}
		catch (Exception e)
		{
			Console.WriteLine($"Error Occured: {e.Message}");
			Console.ReadKey();
			return 0;
		}

		static void RunCommand(string command)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "cmd",
				Arguments = $"/c {command}",
				RedirectStandardOutput = true,
				UseShellExecute = false
			};

			Process process = new Process
			{
				StartInfo = startInfo
			};

			process.Start();
			process.WaitForExit();
		}

		static void InstallNuGetPackage(string packageName, string projectDirectory)
		{
			Console.WriteLine($"Installing {packageName} package in {Path.GetFileName(projectDirectory)} project...");
			DisplayLoader();

			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = "dotnet",
				Arguments = $"add \"{projectDirectory}\" package {packageName}",
				RedirectStandardOutput = true,
				UseShellExecute = false
			};

			Process process = new Process
			{
				StartInfo = startInfo
			};

			process.Start();
			process.WaitForExit();
		}

		static void DisplayLoader()
		{
			Console.Write("Loading");
			for (int i = 0; i < 3; i++)
			{
				Console.Write(".");
				System.Threading.Thread.Sleep(1000); // Simulate delay for demonstration
			}
			Console.WriteLine();
		}

	}
}
