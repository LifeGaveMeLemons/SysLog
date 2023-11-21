using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.MenuItemModel
{
  internal class MenuItemModel
  {
    string s;
    Action a;

    public void Exit()
    {

    }
    public static implicit operator string(MenuItemModel m)
    {

    }
  }
}
