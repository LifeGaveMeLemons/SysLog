using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI
{
  /// <summary>
  ///   This model associates a UI element with a method to be invoked upon interaction with it.
  /// </summary>
  internal class StringFunctionModel
  {

    public string Str;
    public Action Method;

    /// <summary>
    ///   Conversion operator defines implict behaviour upon need of casting to string.
    /// </summary>
    /// <param name="v">
    ///   Instance of StringFunctionModel to be converted.
    /// </param>
    public static implicit operator string(StringFunctionModel v)
    {
      return v.Str;
    }

    /// <summary>
    ///   Returns a element whose callback can be invoked and string contents accessed.
    /// </summary>
    /// <param name="str">
    ///   String value t be used
    /// </param>
    /// <param name="method">
    ///   Method to be associated with the string.
    /// </param>
    public StringFunctionModel(string str, Action method)
    {
      this.Str = str;
      this.Method = method;
    }
    }
}
