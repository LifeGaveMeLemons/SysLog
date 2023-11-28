using System.Collections.Immutable;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data.DelimitedMessageModel;

namespace SysLog.UI.UiElements.SetFilters
{
  internal class FilterDefinition : NavClass
  {
    
    private static FilterDefinition instance;

    public static FilterDefinition Create()
    {
      if (instance == null) 
      {
        instance = new FilterDefinition();
      }
      return instance;
    }

    private static string regex = "^[a-zA-Z0-9.<>= |&!?:+-]+$";
    ScriptOptions scriptOptions;

    Script<bool> script;
		internal override void StartNavigation()
		{
      DefineScript();
		}
		async public void DefineScript()
    {
      while (true)
      {

          Console.WriteLine("please enter your filtering pattern leave empty in order to disable filter");
        string userInput = Console.ReadLine();
        if (userInput != "")
        {
          if (!Regex.IsMatch(userInput, regex))
          {
            Console.WriteLine("you can only use alphanumeric characters, numbers and <,>,=,-,+,?,:,&,|");
            continue;
          }
          string codeToCompile = $"return {userInput};";
          script = CSharpScript.Create<bool>(codeToCompile, scriptOptions, typeof(DelimitedMessageModel));
          ImmutableArray<Diagnostic> a = script.Compile();
          if (a.Length > 0)
          {
            foreach (Diagnostic msg in a)
            {
              Console.WriteLine(msg);
            }
            continue;
          }
          ListeningView.Create().filter = script;
        }
        else
        {
          ListeningView.Create().filter = null;
        }
        Exit();
        return;
      }
      

    }
    public FilterDefinition()
    {
      scriptOptions = ScriptOptions.Default.AddImports("System", "SysLog.UI.Data.DelimitedMessageModel").AddImports().AddReferences(typeof(DelimitedMessageModel).Assembly);
    }
  }
}
