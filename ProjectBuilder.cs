using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Conventions;


namespace dotCreate
{
	public class ProjectBuilder
	{
		public void BuildProject(Dictionary<string, string> userChocies,CommandLineApplication app)
		{

			var subject = app.Option("-s|--subject <SUBJECT>", "The subject", CommandOptionType.SingleValue);
			subject.DefaultValue = "world";

			var repeat = app.Option<int>("-n|--count <N>", "Repeat", CommandOptionType.SingleValue);
			repeat.DefaultValue = 1;

			app.OnExecute(() =>
			{
				for (var i = 0; i < repeat.ParsedValue; i++)
				{
					Console.WriteLine($"Hello {subject.Value()}!");
				}
			});
		}
	}
}
