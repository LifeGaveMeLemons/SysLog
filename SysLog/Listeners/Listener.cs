namespace SysLog.Listeners
{
  abstract class Listener: IDisposable
  {
    abstract public void Dispose();
    abstract public string GetDescription();
    abstract public void CheckForMessages();
  }
}
