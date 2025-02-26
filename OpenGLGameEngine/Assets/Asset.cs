using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Assets;

public abstract class Asset : BaseObject
{
    public string Path { get; private set; }
    public bool IsLoaded { get; private set; }

    public virtual void LoadFromFile(string path)
    {

    }
}
