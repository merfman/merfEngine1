using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Rendering;
using OpenTK;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using static System.Net.Mime.MediaTypeNames;

namespace OpenGLGameEngine;
internal class Window : GameWindow
{
    private Render _renderer;

    public Camera ActiveCamera;

    public Shader _testShader;


    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        _testShader = new Shader( PathH.GetRelative(@"Resources\Shaders\test.vert"), PathH.GetRelative(@"Resources\Shaders\test.frag"));


        _renderer = new Render();
    }
    protected override void OnUnload()
    {
        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _renderer.RenderFrame(ActiveCamera);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
}
