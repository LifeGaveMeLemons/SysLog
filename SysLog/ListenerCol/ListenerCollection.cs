using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SysLog.Listeners;
using SysLog.UI.Data;

namespace SysLog.ListenerCol
{
  internal class ListenerCollection
  {
    private static ListenerCollection instance;

    public static ListenerCollection Create(Action<SyslogIpModel>? callback)
    {
      if (instance == null) 
      {
        instance= new ListenerCollection(callback);
      }
      return instance;
    }


    List<UdpListener> UDPlisteners = new List<UdpListener>();
    object UdpListKey = new object();

    List<Listener> Clients;
    Thread ConnectionHandlerThread;

    Thread newConnections;


    List<InboundConnectionListener> listenerList;
    private object keyConInitializer = new object();
    bool isListeningTCP = true;

    private Action<SyslogIpModel>? dataRecievedCallback;

    //will not update current connecctions
    public Action<SyslogIpModel> OnRecieve { set { dataRecievedCallback = value == null ? (SyslogIpModel val) => { } : value; } }

    
    private object key = new object();


    public List<UdpListener> GetUdpList()
    {
      List<UdpListener> li = new List<UdpListener>();
      lock(UdpListKey)
      {
        foreach(UdpListener c in UDPlisteners)
        {
          li.Add(c);
        }
      }
      return li;
    }
    public List<InboundConnectionListener> GetTcpListeners()
    {
      List<InboundConnectionListener> li = new List<InboundConnectionListener>();
      lock (keyConInitializer)
      {
        foreach (InboundConnectionListener c in listenerList)
        {
          li.Add(c);
        }
      }
      return li;
    }
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
            lock (key)
            {
              Clients.Add(new Listeners.TcpSessionListener(tcpClient, dataRecievedCallback, RemoveClient));
            }
          }

        }
      }
    }
    public void RemoveClient(TcpSessionListener l)
    {
      lock(key) 
      {
        l.Dispose();
        Clients.Remove(l);
      }
    }
    public void RemoveClient(UdpListener l)
    {
      lock (key)
      { 
        l.Dispose();
        Clients.Remove(l);
        UDPlisteners.Remove(l);
      }
    }

    public void RemoveTcpListener(InboundConnectionListener l)
    {
      lock(keyConInitializer)
      {
        listenerList.Remove(l);
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
          if (Clients.Count == 0) continue;
          lock (key)
          {
            l = Clients.ToArray();
          }
          foreach (Listener element in l)
          {
            element.CheckForMessages();
          }
        }
        catch (Exception e) { Console.WriteLine(  e); }
                

      }


        }
    public void Add(UdpListener listener)
    {
      lock (key)
      {
        Clients.Add(listener);
        UDPlisteners.Add(listener); 
      }
    }

    public ListenerCollection(Action<SyslogIpModel>? callback)
    {
      Clients = new List<Listener>();
      listenerList = new List<InboundConnectionListener>();


      ConnectionHandlerThread = new Thread(CheckForMessages);
      ConnectionHandlerThread.Start();


      newConnections = new Thread(CheckForInboundConnections);
      newConnections.Start();


      this.dataRecievedCallback = callback;
    }

  }
}
