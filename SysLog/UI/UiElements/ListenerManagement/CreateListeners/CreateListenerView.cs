using SysLog.ListenerCol;
using SysLog.Listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI.UiElements
{
  internal class CreateListenerView : NavClass
  {
    private static CreateListenerView instance;
    public static CreateListenerView Create()
    {
      if (instance == null)
      {
        instance = new CreateListenerView();
      }
      return instance;
    }
    private void CreateUDPListener()
    {
      ushort portToBeCreatedOn = 514;
      Console.Clear();
      Console.WriteLine("what port do you want to create the UDP listener on?");
      bool isGettinInput = true;
      while (isGettinInput)
      {
        try
        {
          portToBeCreatedOn = Convert.ToUInt16(Console.ReadLine());
          isGettinInput = false;
          Handlers.Handler h = Handlers.Handler.Create();
          UdpListener listener = new UdpListener(h.Enqueue, portToBeCreatedOn);
          ListenerCollection.Create(null).Add(listener);
          return;
        }
        catch (FormatException)
        {
          Console.WriteLine("please enter a valid number");
        }
        catch (OverflowException)
        {
          Console.WriteLine($"please enter a number between 0 and {ushort.MaxValue}");
        }
        catch (SocketException)
        {
          Console.WriteLine("there is already a UDP listener on this socket");
        }
        catch (Exception e) { Console.WriteLine(e); }

      }
    }
    private void CreateTCPListener()
    {
      ushort portToBeCreatedOn = 514;
      Console.Clear();
      Console.WriteLine("what port do you want to create the TCP listener on?");
      bool isGettinInput = true;
      while (isGettinInput)
      {
        try
        {
          portToBeCreatedOn = Convert.ToUInt16(Console.ReadLine());
          isGettinInput = false;
          ListenerCollection.Create(null).AddIncomingConnectionListener(portToBeCreatedOn);
        }
        catch (FormatException)
        {
          Console.WriteLine("please enter a number");
        }
        catch (OverflowException)
        {
          Console.WriteLine($"please enter a number between 0 and {ushort.MaxValue}");
        }
        catch (SocketException)
        {
          Console.WriteLine("there is already a UDP listener on this socket");
        }


      }
    }
    public CreateListenerView()
    {
      subElements = new List<StringFunctionModel>()
            {
              new StringFunctionModel("Create a UDP Listener",this.CreateUDPListener),
              new StringFunctionModel("Create a TCP listener", this.CreateTCPListener),
              new StringFunctionModel("Exit", this.Exit)
            };
      }
    }
}
