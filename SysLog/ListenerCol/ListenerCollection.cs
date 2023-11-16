using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SysLog.Listeners;

namespace SysLog.ListenerCol
{
  internal class ListenerCollection
  {


    List<Listener> li;
    Thread listenerThread;

    Thread inboundConnectionListenerThread;
    List<NewInboundConnectionListener> listenerList;
    bool isListeningTCP = true;

    private OnDataRecieved? callback;

    public OnDataRecieved OnRecieve { set { callback = value == null ? (string val) => { } : value; } }

    private object key = new object();
    private object keyConInitializer = new object();
    public ListenerCollection(OnDataRecieved? callback)
    {
          li = new List<Listener>();
      listenerThread = new Thread(CheckForMessages);
      listenerThread.Start();
      listenerList = new List<NewInboundConnectionListener>();  
      inboundConnectionListenerThread = new Thread(CheckForInboundConnections);
      inboundConnectionListenerThread.Start();
      this.callback
    }
    private void CheckForInboundConnections()
    {
      while (isListeningTCP)
      {
        lock (keyConInitializer)
        {
          TcpClient tcpClient = null;
          foreach (var listener in listenerList)
          {
            tcpClient = listener.CheckForConnections();
            if (tcpClient != null)
            {
              Add(new TcpClientListener(tcpClient, callback, RemoveElement));
            }
          }
        }
      }
    }
    public void RemoveElement(TcpClientListener l)
    {
      lock(key) 
      {
        li.Remove(l);
      }
    }
    public void AddListener(int port)
    {
      lock(keyConInitializer)
      {
        listenerList.Add(new NewInboundConnectionListener(port));
      }
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
