using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysLog.Listeners;

namespace SysLog.ListenerCol
{
  internal class ListenerCollection
  {
    List<Listener> li;
    public ListenerCollection()
    {
          li = new List<Listener>();
    }

    public void Add(Listener listener)
    {
      li.Add(listener);
    }
  }
}
