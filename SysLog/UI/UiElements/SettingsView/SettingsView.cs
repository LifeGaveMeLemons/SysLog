using SysLog.UI.UiElements.SetFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.UiElements.SettingsView
{
  internal class SettingsView : NavClass
  {
    private static SettingsView instance;

    public static SettingsView Create()
    {
      if (instance == null)
      {
        instance = new SettingsView();
      }
      return instance;
    }

    public SettingsView()
    {
      subElements = new List<StringFunctionModel>()
      {
        new StringFunctionModel("Colours",ColourDefinitionView.Create().Load),
        new StringFunctionModel("Filtering",FilterDefinition.Create().Load),
        new StringFunctionModel("exit",Exit)
      };
    }
  }
}
