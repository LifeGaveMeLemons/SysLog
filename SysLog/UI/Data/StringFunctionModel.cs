using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysLog.UI
{
  internal class StringFunctionModel
  {

    string str;
    public Action method;

    public static implicit operator string(StringFunctionModel v)
    {
      return v.str;
    }
        public StringFunctionModel(string str, Action method)
        {
          this.str = str;
          this.method = method;
        }
    }
}
