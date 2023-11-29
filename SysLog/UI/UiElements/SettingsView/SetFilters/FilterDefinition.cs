using System.Collections.Immutable;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data.DelimitedMessageModel;
using static System.Net.Mime.MediaTypeNames;

namespace SysLog.UI.UiElements.SetFilters
{
	internal class FilterDefinition : NavClass
	{
		private static FilterDefinition s_instance;
		private Script<bool> _script;
		private static string _regex = "^[a-zA-Z0-9.<>= |&!?:+-]+$";
		private ScriptOptions _scriptOptions;

		/// <summary>
		/// Creates an instance of the FilterDefinition class using the Singleton pattern.
		/// </summary>
		/// <returns>The instance of FilterDefinition.</returns>
		public static FilterDefinition Create()
		{
			if (s_instance == null)
			{
				s_instance = new FilterDefinition();
			}
			return s_instance;
		}

		/// <summary>
		/// Starts the navigation within the FilterDefinition.
		/// </summary>
		internal override void StartNavigation()
		{
			DefineScript();
		}

		/// <summary>
		/// Defines a filtering script based on user input.
		/// </summary>
		async public void DefineScript()
		{
			while (true)
			{
				Console.CursorVisible = true;
				Console.WriteLine("Please enter your filtering pattern, leave empty to disable the filter");
				string userInput = Console.ReadLine();
				if (!string.IsNullOrEmpty(userInput))
				{
					// Validate user input against the regex
					if (!Regex.IsMatch(userInput, _regex))
					{
						Console.WriteLine("Input not valid. Use alphanumeric characters, numbers, <, >, =, -, +, ?, :, &, |, or an IP address.");
						continue;
					}

					// Automatically encapsulate IP address patterns with IPAddress.Parse()
					string modifiedInput = Regex.Replace(userInput, @"\b\d{1,3}(\.\d{1,3}){3}\b", m => $"IPAddress.Parse(\"{m.Value}\")");

					string codeToCompile = $"return {modifiedInput};";

					_script = CSharpScript.Create<bool>(codeToCompile, _scriptOptions, typeof(DelimitedMessageModel));
					ImmutableArray<Diagnostic> diagnostics = _script.Compile();
					try
					{
						ScriptState<bool> s = _script.RunAsync(new DelimitedMessageModel("<17>1 2023-11-06T10:11:12.381Z Park Air Systems LTD, test app. - - - - VAU3303G", new System.Net.IPEndPoint(IPAddress.Any, 514), true)).Result;
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						continue;
					}
					if (diagnostics.Length > 0)
					{
						foreach (Diagnostic msg in diagnostics)
						{
							Console.WriteLine(msg);
						}
						continue;
					}
					ListeningView.Create().Filter = _script;
				}
				else
				{
					ListeningView.Create().Filter = null;
				}
				Exit();
				Console.CursorVisible = false;
				return;
			}
		}


		private FilterDefinition()
		{
			_scriptOptions = ScriptOptions.Default.AddImports("System", "SysLog.UI.Data.DelimitedMessageModel","System.Net").AddImports().AddReferences(typeof(DelimitedMessageModel).Assembly);
		}
	}
}
