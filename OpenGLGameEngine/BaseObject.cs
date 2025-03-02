using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
public class BaseObject
{
    public string Name;
    public override string ToString()
    {
        if (Name == null)
            return base.ToString();
        else 
            return Name;
    }
}
