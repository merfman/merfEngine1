using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Assets;
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
    public GameObject _testGameObject;
    public RenderComponent _testRenderComponent;
    public Material _testMaterial;
    public Mesh _testMesh;


    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();
        _testGameObject = new GameObject();
        _testRenderComponent = new RenderComponent(_testGameObject);
        _testGameObject.Components.Add(_testRenderComponent);
        _testShader = new Shader( PathH.GetRelative(@"Resources\Shaders\test.vert"), PathH.GetRelative(@"Resources\Shaders\test.frag"));
        _testMaterial = new Material(_testShader);
        _testMesh = new Mesh(PathH.GetRelative(@"Resources\Meshes\Suzanne.obj"));
        _testGameObject.Assets.Add(new WeakReference<Asset>(_testMaterial));
        _testGameObject.Assets.Add(new WeakReference<Asset>(_testMesh));

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
