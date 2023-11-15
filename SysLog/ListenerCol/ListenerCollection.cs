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
    Thread listenerThread;

    private object key = new object();
    public ListenerCollection()
    {
          li = new List<Listener>();
      listenerThread = new Thread(CheckForMessages);
      listenerThread.Start();
    }

    private void CheckForMessages()
    {
      while (true)
      { 
        lock (key)
        {
          foreach (Listener l in li)
          {
            l.CheckForMessages();
          }
        }
      }
    

    }
    public void Add(Listener listener)
    {
      lock (key)
      {
        li.Add(listener);
      }
    }
  }
}
