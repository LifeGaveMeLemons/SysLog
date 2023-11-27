using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data;

namespace SysLog.UI.UiElements.SetFilters
{
  internal class FilterDefinition : NavClass
  {
    private static string regex = "^[a-zA-Z0-9.<>=|&!?:+-]+$";
    ScriptOptions scriptOptions;

    Script<bool> script;

    public void DefineScript()
    {
      while (true)
      {
        try
        {
          Console.WriteLine("please enter your filtering pattern");
          string userInput = Console.ReadLine();  
          if (!Regex.IsMatch(userInput, regex))
          {
            Console.WriteLine("you can only use alphanumeric characters, numbers and <,>,=,-,+,?,:,&,|");
            continue;
          }
          string codeToCompile = $"return{userInput};";
          script = CSharpScript.Create<bool>(codeToCompile, scriptOptions, null);
          ImmutableArray<Diagnostic> a = script.Compile();
          if (a.Length > 0)
          {
            foreach (Diagnostic msg in a)
            {
              Console.WriteLine(msg);
            }
            continue;
          }
          return;
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
        }
      }
      

    }
    public FilterDefinition()
    {
      scriptOptions = ScriptOptions.Default.AddImports("System").AddReferences(typeof(DelimitedMessageModel).Assembly);
    }
  }
}
