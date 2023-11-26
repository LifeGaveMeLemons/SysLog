using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysLog.UI;
using SysLog.UI.UiElements;
using SysLog.UI.UiElements;

namespace SysLog.UI
{
  internal class ListenerManagement :NavClass
  {
    private static ListenerManagement instance;
    public static ListenerManagement Create()
    {
      if (instance == null)
      {
        instance = new ListenerManagement();
      }
      return instance;
    }
    public static ListenerManagement Instance;


    public ListenerManagement()
    {
      subElements = new List<StringFunctionModel>(){
      new StringFunctionModel("Remove Listeners",ViewListeners.Create().Load ),
      new StringFunctionModel("Crate a listener",CreateListenerView.Create().Load),
      new StringFunctionModel("exit",Exit)
    };
  }




  }
}
