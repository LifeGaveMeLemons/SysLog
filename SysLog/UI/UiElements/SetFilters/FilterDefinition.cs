using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SysLog.UI.Data;

namespace SysLog.UI.UiElements.SetFilters
{
  internal class FilterDefinition : NavClass
  {
    string codeToCompile;
    ScriptOptions scriptOptions;

    Script<bool> script;

    public void DefineScript()
    {
      Console.WriteLine("enter ocntision");
      string v = Console.ReadLine();
      string codeToBeCompiled = $"return {v}";
      script = CSharpScript.Create<bool>(codeToBeCompiled, scriptOptions,null);
      ImmutableArray<Diagnostic> a = script.Compile();
      foreach(Diagnostic e = indexer 
      

    }
    public FilterDefinition()
    {
      scriptOptions = ScriptOptions.Default.AddImports("System").AddReferences(typeof(DelimitedMessageModel).Assembly);
    }
  }
}
