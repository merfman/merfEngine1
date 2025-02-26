using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Assets;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;



namespace OpenGLGameEngine.Rendering;
/// <summary>
/// Handles all the rendering. Not static so it can support multiple windows. 
/// </summary>
internal class Render
{
    /// <summary>
    /// List of RenderComponents that are rendered
    /// </summary>
    List<RenderComponent> RenderList = new List<RenderComponent>();

    public void AddToRenderList(ref RenderComponent gameObject) => RenderList.Add(gameObject);
    public void RemoveFromRenderList(ref RenderComponent gameObject) => RenderList.Remove(gameObject);

    public Render()
    {
        GL.ClearColor(0.2f, 0.3f, 0.3f, 0.0f);

        GL.Enable(EnableCap.DepthTest);

        GL.Enable(EnableCap.CullFace);

        GL.CullFace(CullFaceMode.Back);
    }

    public void RenderFrame(Camera camera)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        foreach (var component in RenderList)
            if (component.RenderLayer != RenderingLayer.Hidden)
            {
                foreach (WeakReference<Asset> weakRefAsset in component.RenderObject.Assets)
                    if (weakRefAsset.TryGetTarget(out Asset? asset))
                    {

                    }
            }
    }








}
