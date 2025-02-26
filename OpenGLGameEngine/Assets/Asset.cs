using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Assets;
/// <summary>
/// 
/// </summary>
/// <remarks> RenderLayer came from a suggestion by ChatGPT. </remarks>
public enum RenderingLayer
{
    /// <summary>
    /// Special layer for non-rendered objects
    /// </summary>
    Hidden = -1,
    /// <summary>
    /// 
    /// </summary>
    Default = 0
}
public abstract class Asset : BaseObject
{
    public string Path { get; private set; }
    public bool IsLoaded { get; private set; }

    public abstract void LoadFromFile(string path);
}
