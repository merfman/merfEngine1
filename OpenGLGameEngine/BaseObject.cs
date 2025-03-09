using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
/// <summary>
/// The Base of every Engine object
/// </summary>
public class BaseObject
{
    /// <summary>
    /// 
    /// </summary>
    public string Name;
    public override string ToString()
    {
        if (Name == null)
            return base.ToString();
        else 
            return Name;
    }

    public BaseObject(string? name = null)
    {
        if (name != null)
            Name = name;
    }
}
