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
/// <see cref="Component"/> that can be added to a <see cref="GameObject"/> that holds render settings and can be sent to <see cref="Render"/> to be rendered.
/// </summary>
public class RenderComponent : Component
{
    /// <summary>
    /// A <see cref="RenderingLayer"/> that determines when or if the <see cref="GameObject"/> is rendered.
    /// </summary>
    /// <remarks> RenderLayer as an enum came from a suggestion by ChatGPT. </remarks>
    public RenderingLayer RenderLayer;
    //public GameObject RenderObject; // Component has a reference to the GameObject

    public RenderComponent(GameObject renderObject, RenderingLayer renderLayer = RenderingLayer.Default)
    {
        //RenderObject = renderObject;
        this.GameObject = renderObject;
        RenderLayer = renderLayer;
    }
}
