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
        foreach (RenderComponent component in RenderList)
            if (component.RenderLayer != RenderingLayer.Hidden)
            {
                foreach (WeakReference<Asset> weakRefAsset in component.RenderObject.Assets)
                    if (weakRefAsset.TryGetTarget(out Asset? asset))
                    {
                        if (asset is Mesh mesh)
                        {
                            GL.BindVertexArray(mesh.VAO);
                            if (mesh.Material.TryGetTarget(out Material? material))
                                if (material.Shader.TryGetTarget(out Shader? shader))
                                {
                                    shader.SetMatrix4("view", camera.GetViewMatrix());

                                    shader.SetMatrix4("projection", camera.GetProjectionMatrix()); 

                                    shader.SetVector3("viewPos", camera.Transform.Position);

                                    shader.SetInt("material.diffuse", 0);
                                    shader.SetInt("material.specular", 1);
                                    shader.SetInt("material.normal", 2);
                                    shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                                    shader.Use();

                                    Matrix4 model = Matrix4.CreateTranslation(new Vector3(0.0f));
                                    model *= Matrix4.CreateFromQuaternion(component.RenderObject.Transform.Rotation);
                                    model *= Matrix4.CreateScale(component.RenderObject.Transform.Scale);
                                    model *= Matrix4.CreateTranslation(component.RenderObject.Transform.Position);
                                    shader.SetMatrix4("model", model);

                                    GL.DrawArrays(PrimitiveType.Triangles, 0, mesh.VertexCount);

                                }
                        }
                    }
            }
    }








}
