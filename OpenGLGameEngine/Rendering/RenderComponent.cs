using OpenGLGameEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Rendering;
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

/// <summary>
/// 
/// </summary>
//TODO: maybe make this derive from component, or have a shared ancestry or something
public class RenderComponent
{
    /// <summary>
    /// A <see cref="RenderingLayer"/> that determines when or if the <see cref="GameObject"/> is rendered.
    /// </summary>
    /// <remarks> RenderLayer as an enum came from a suggestion by ChatGPT. </remarks>
    public RenderingLayer RenderLayer;

    public RenderComponent(RenderingLayer renderLayer = RenderingLayer.Default)
    {
        RenderLayer = renderLayer;
    }
}
