using System.Net;
using System.Net.Sockets;
using SysLog.Listeners;
using SysLog.UI.Data;

namespace SysLog.ListenerCol
{
  /// <summary>
  /// Manages all of the lsiteners in the program
  /// </summary>
  internal class ListenerCollection
  {
    /// <summary>
    /// Singleton class, static field to store current instance.
    /// </summary>
    private static ListenerCollection s_instance;

    /// <summary>
    /// Static create Method returns an active ListenerCollection object. If no such objects exist, creates a new one.
    /// </summary>
    /// <param name="callback">Callback for any messages recieved</param>
    /// <returns></returns>
    public static ListenerCollection Create(Action<SyslogIpModel>? callback)
    {
      if (s_instance == null) 
      {
        s_instance= new ListenerCollection(callback);
      }
      return s_instance;
    }


    private List<UdpListener> _udpListeners = new List<UdpListener>();
    private object _udpListKey = new object();

    private List<Listener> _clients;
    private Thread _connectionHandlerThread;

    private Thread _newConnections;


    private List<InboundConnectionListener> _listenerList;
    private object keyConInitializer = new object();
    private bool _isListeningTCP = true;

    private Action<SyslogIpModel>? _dataRecievedCallback;

    //will not update current connecctions
    public Action<SyslogIpModel> OnRecieve { set { _dataRecievedCallback = value == null ? (SyslogIpModel val) => { } : value; } }

    
    private object _key = new object();


    /// <summary>
    ///   Returns a new list wil all the UdpListeners currently active.
    /// </summary>
    /// <returns>A List of all active UdpListeners</returns>
    
    public void ChangeIp(IPAddress address)
    {
      foreach(InboundConnectionListener listener in _listenerList)
      {
        listener.ChangeIp(address);
      }
      foreach(UdpListener listener in _udpListeners)
      {
        listener?.ChangeIp(address);
      }
    }
    public List<UdpListener> GetUdpList()
    {
      List<UdpListener> li = new List<UdpListener>();
      lock(_udpListKey)
      {
        foreach(UdpListener c in _udpListeners)
        {
          li.Add(c);
        }
      }
      return li;
    }
    /// <summary>
    ///   Returns a new list wil all the InboundConnectionListeners currently active.
    /// </summary>
    /// <returns>A List of all active InboundConnectionListeners </returns>
    public List<InboundConnectionListener> GetTcpListeners()
    {
      List<InboundConnectionListener> li = new List<InboundConnectionListener>();
      lock (keyConInitializer)
      {
        foreach (InboundConnectionListener c in _listenerList)
        {
          li.Add(c);
        }
      }
      return li;
    }

    /// <summary>
    ///   Checks all InboundConnectionlisteners for new connections.
    /// </summary>
    private void CheckForInboundConnections()
    {
      while (_isListeningTCP)
      {
        InboundConnectionListener[] listeners;
        lock (keyConInitializer)
        {
          listeners = _listenerList.ToArray();
        }
        TcpClient tcpClient = null;
        foreach (var listener in listeners)
        {
          tcpClient = listener.CheckForConnections();
          if (tcpClient != null)
          {
            lock (_key)
            {
              _clients.Add(new Listeners.TcpSessionListener(tcpClient, _dataRecievedCallback, RemoveClient));
            }
          }

        }
      }
    }

    /// <summary>
    ///   Removes specified Listener.
    /// </summary>
    /// <param name="l">TcpSessionlistener to be removed.</param>
    public void RemoveClient(TcpSessionListener l)
    {
      lock(_key) 
      {
        l.Dispose();
        _clients.Remove(l);
      }
    }
    /// <summary>
    ///   Removes specified Listener.
    /// </summary>
    /// <param name="l">UdpListener to be removed.</param>
    public void RemoveClient(UdpListener l)
    {
      lock (_key)
      { 
        l.Dispose();
        _clients.Remove(l);
        _udpListeners.Remove(l);
      }
    }

    /// <summary>
    ///   Removes specified Listener.
    /// </summary>
    /// <param name="l">Removes specified InboundConnectionListener to be removed.</param>
    public void RemoveTcpListener(InboundConnectionListener l)
    {
      lock(keyConInitializer)
      {
        _listenerList.Remove(l);
      }
    }

    /// <summary>
    ///   Adds InboundConnectionListener on specified port.
    /// </summary>
    /// <param name="port"> Port to be added on.</param>
    public void AddIncomingConnectionListener(int port)
    {
      lock(keyConInitializer)
      {
        _listenerList.Add(new InboundConnectionListener(port));
      }
    }

    /// <summary>
    ///   Checks for inbound messages across all listeners
    /// </summary>
    private void CheckForMessages()
    {
      while (true)
      {
        try
        {
          Listener[] l;
          if (_clients.Count == 0) continue;
          lock (_key)
          {
            l = _clients.ToArray();
          }
          foreach (Listener element in l)
          {
            element.CheckForMessages();
          }
        }
        catch (Exception e) { }
                

      }


        }

    /// <summary>
    ///   Adds UdpListener.
    /// </summary>
    /// <param name="listener">Listener to be added.</param>
    public void Add(UdpListener listener)
    {
      lock (_key)
      {
        _clients.Add(listener);
        _udpListeners.Add(listener); 
      }
    }

    /// <summary>
    /// Creates a new ListenerCollection
    /// </summary>
    /// <param name="callback"> Callback to be added</param>
    private ListenerCollection(Action<SyslogIpModel>? callback)
    {
      _clients = new List<Listener>();
      _listenerList = new List<InboundConnectionListener>();


      _connectionHandlerThread = new Thread(CheckForMessages);
      _connectionHandlerThread.Start();


      _newConnections = new Thread(CheckForInboundConnections);
      _newConnections.Start();


      this._dataRecievedCallback = callback;
    }

  }
}
