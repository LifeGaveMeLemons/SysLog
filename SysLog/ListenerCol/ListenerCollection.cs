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


    List<Listener> tcpClients;
    Thread ConnectionHandlerThread;

    Thread newConnections;
    List<InboundConnectionListener> listenerList;
    bool isListeningTCP = true;

    private OnDataRecieved? dataRecievedCallback;

    //will not update current connecctions
    public OnDataRecieved OnRecieve { set { dataRecievedCallback = value == null ? (string val) => { } : value; } }

    private object key = new object();
    private object keyConInitializer = new object();
    private void CheckForInboundConnections()
    {
      while (isListeningTCP)
      {
        InboundConnectionListener[] listeners;
        lock (keyConInitializer)
        {
          listeners = listenerList.ToArray();
        }
          TcpClient tcpClient = null;
          foreach (var listener in listeners)
          {
            tcpClient = listener.CheckForConnections();
            if (tcpClient != null)
            {
            Add(new Listeners.TcpSessionListener(tcpClient, dataRecievedCallback, RemoveClient));
            }
          }
        
      }
    }
    public void RemoveClient(Listener l)
    {
      lock(key) 
      {
        l.Dispose();
        tcpClients.Remove(l);
      }
    }
    public void AddIncomingConnectionListener(int port)
    {
      lock(keyConInitializer)
      {
        listenerList.Add(new InboundConnectionListener(port));
      }
    }
    private void CheckForMessages()
    {
      while (true)
      {
        try
        {
          Listener[] l;
          if (tcpClients.Count == 0) continue;
          lock (key)
          {
            l = tcpClients.ToArray();
          }
          foreach (Listener element in l)
          {
            element.CheckForMessages();
          }
        }
        catch (Exception e) { Console.WriteLine(  e); }
                

      }


        }
    public void Add(Listener listener)
    {
      lock (key)
      {
        tcpClients.Add(listener);
      }
    }

    public ListenerCollection(OnDataRecieved? callback)
    {
      tcpClients = new List<Listener>();
      listenerList = new List<InboundConnectionListener>();


      ConnectionHandlerThread = new Thread(CheckForMessages);
      ConnectionHandlerThread.Start();


      newConnections = new Thread(CheckForInboundConnections);
      newConnections.Start();


      this.dataRecievedCallback = callback;
    }

  }
}
