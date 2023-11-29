using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace SysLog.UI.Data.DelimitedMessageModel
{
  /// <summary>
  ///   Defines concrete attributes in their own fields to allow the user to interact with them while filtering in a significantly easier manner.
  /// </summary>
  public class DelimitedMessageModel
  {
    private const string DATE_TIME_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffK";
    private const string MESSAGE_REGEX = @"^(?:<(?<severity>\d+)>)?(?:<>)?(?<version>\d+) (?<dateTime>.{24}) (?<program>.*?)( - - - - |$)(?<message>.*)";
    public IPEndPoint Src;
    public byte Severity;
    public byte Version;
    public DateTime Timestamp;
    public string Message;
    public byte SeverityCalc;
    public string Original;
    public bool IsTcp;

    /// <summary>
    ///  Define behaviour when casting to string.
    /// </summary>
    /// <param name="m">
    ///   Model instance subject to coercion.
    /// </param>
    public static implicit operator string(DelimitedMessageModel m)
    {
      return m.Original;
    }
    /// <summary>
    ///   separates all individual attributes of each message to be further used for filtring by the user.
    /// </summary>
    /// <param name="sourceMessage">
    ///   original message to be separated
    /// </param>
    /// <param name="ip">
    ///   ip endpoint of where the message origin
    /// </param>
    public DelimitedMessageModel(string sourceMessage,IPEndPoint ip,bool isTcp)
    {
      Match m = Regex.Match(sourceMessage, MESSAGE_REGEX);
      string sev = m.Groups["severity"].Value;
      Severity = Convert.ToByte(sev == ""? "0" : sev);
      Version = Convert.ToByte(m.Groups["version"].Value);
      DateTime.TryParseExact(m.Groups["DateTime"].Value, DATE_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out Timestamp);
      Message = m.Groups["message"].Value;
      Src = ip;
      Original = sourceMessage;
      SeverityCalc = Convert.ToByte(Severity % 8);
      this.IsTcp = isTcp;
    }
  }
}
