using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Asset;
public abstract class Asset : BaseObject
{
    public string Path { get; private set; }
    public bool IsLoaded { get; private set; }

    public abstract void LoadFromFile(string path);
}
