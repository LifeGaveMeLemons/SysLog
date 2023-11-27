﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
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
    private static ListenerCollection instance;

    /// <summary>
    /// Static create Method returns an active ListenerCollection object. If no such objects exist, creates a new one.
    /// </summary>
    /// <param name="callback">Callback for any messages recieved</param>
    /// <returns></returns>
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


    /// <summary>
    ///   Returns a new list wil all the UdpListeners currently active.
    /// </summary>
    /// <returns>A List of all active UdpListeners</returns>
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
    /// <summary>
    ///   Returns a new list wil all the InboundConnectionListeners currently active.
    /// </summary>
    /// <returns>A List of all active InboundConnectionListeners </returns>
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

    /// <summary>
    ///   Checks all InboundConnectionlisteners for new connections.
    /// </summary>
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

    /// <summary>
    ///   Removes specified Listener.
    /// </summary>
    /// <param name="l">TcpSessionlistener to be removed.</param>
    public void RemoveClient(TcpSessionListener l)
    {
      lock(key) 
      {
        l.Dispose();
        Clients.Remove(l);
      }
    }
    /// <summary>
    ///   Removes specified Listener.
    /// </summary>
    /// <param name="l">UdpListener to be removed.</param>
    public void RemoveClient(UdpListener l)
    {
      lock (key)
      { 
        l.Dispose();
        Clients.Remove(l);
        UDPlisteners.Remove(l);
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
        listenerList.Remove(l);
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
        listenerList.Add(new InboundConnectionListener(port));
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

    /// <summary>
    ///   Adds UdpListener.
    /// </summary>
    /// <param name="listener">Listener to be added.</param>
    public void Add(UdpListener listener)
    {
      lock (key)
      {
        Clients.Add(listener);
        UDPlisteners.Add(listener); 
      }
    }

    /// <summary>
    /// Creates a new ListenerCollection
    /// </summary>
    /// <param name="callback"> Callback to be added</param>
    private ListenerCollection(Action<SyslogIpModel>? callback)
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
