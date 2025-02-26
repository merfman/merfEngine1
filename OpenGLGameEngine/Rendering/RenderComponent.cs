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
public class RenderComponent
{
    /// <summary>
    /// A <see cref="RenderingLayer"/> that determines when or if the <see cref="GameObject"/> is rendered.
    /// </summary>
    /// <remarks> RenderLayer as an enum came from a suggestion by ChatGPT. </remarks>
    public RenderingLayer RenderLayer;

    public RenderComponent()
    {
        RenderLayer = RenderingLayer.Default;
    }
}
