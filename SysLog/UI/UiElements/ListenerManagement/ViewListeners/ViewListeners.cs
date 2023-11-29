using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysLog.Listeners;
using SysLog.UI;
using SysLog.UI.UiElements.ListenerManagement.ViewListeners.ViewSpecificProtocols;

namespace SysLog.UI
{
  internal class ViewListeners:NavClass
  {
    private static ViewListeners instance;
    public static ViewListeners Create()
    {
      if (instance == null)
      {
        instance = new ViewListeners();
      }
      return instance;
    }

    public ViewListeners()
    {
      _subElements = new List<StringFunctionModel>
            {
              new StringFunctionModel("TCP",Tcp.Create().Load),
              new StringFunctionModel("UDP", Udp.Create().Load),
              new StringFunctionModel("Exit",Exit)
            };
    }
    }
}
