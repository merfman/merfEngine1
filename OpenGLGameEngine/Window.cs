using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using OpenGLGameEngine.Assets;
using OpenGLGameEngine.Rendering;
using OpenTK;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.Mime.MediaTypeNames;

namespace OpenGLGameEngine;
internal class Window : GameWindow
{
    private (int left, int top) _consoleCurrentLine;

    private Render _renderer;

    public CameraComponent ActiveCamera;
    public GameObject CameraObject;

    public Shader _testShader;
    public GameObject _testGameObject;
    public RenderComponent _testRenderComponent;
    public Material _testMaterial;
    public Mesh _testMesh;
    public Texture _testTexture;

    private bool _firstMove = true;
    private Vector2 _lastPos;

    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
    }
    
    protected override void OnLoad()
    {
        base.OnLoad();

        CameraObject = new GameObject();
        ActiveCamera = CameraObject.AddComponent<CameraComponent>(Size.X / (float)Size.Y);

        _testGameObject = new GameObject();
        _testGameObject.Transform.Yaw = 90;
        _testGameObject.Transform.Pitch = 25;
        _testRenderComponent = new RenderComponent(_testGameObject);
        _testGameObject.Components.Add(_testRenderComponent);
        _testShader = new Shader( PathH.GetRelative(@"Resources\Shaders\test.vert"), PathH.GetRelative(@"Resources\Shaders\test.frag"));
        _testMaterial = new Material(_testShader);
        _testMesh = new Mesh(PathH.GetRelative(@"Resources\Meshes\Suzanne.obj"), _testMaterial);
        _testTexture = Texture.LoadFromFile(PathH.GetRelative(@"Resources\Textures\container2.png"));
        _testMaterial.ColorMap = new WeakReference<Texture>(_testTexture);
        //_testGameObject.Assets.Add(new WeakReference<Asset>(_testMaterial));
        _testGameObject.Assets.Add(new WeakReference<Asset>(_testMesh));

        _renderer = new Render();

        _renderer.AddToRenderList(ref _testRenderComponent);
        
    }
    protected override void OnUnload()
    {
        base.OnUnload();
    }
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _renderer.RenderFrame(ActiveCamera);
        
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (!IsFocused) return;

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape)) Close();

        float cameraSpeed = 1.5f;
        float sensitivity = 0.2f;

        if (input.IsKeyDown(Keys.LeftShift)) cameraSpeed = 3.5f;
        if (input.IsKeyDown(Keys.W)) ActiveCamera.Transform.Position += ActiveCamera.Transform.Front * cameraSpeed * (float)args.Time; // Forward
        if (input.IsKeyDown(Keys.S)) ActiveCamera.Transform.Position -= ActiveCamera.Transform.Front * cameraSpeed * (float)args.Time; // Backwards
        if (input.IsKeyDown(Keys.A)) ActiveCamera.Transform.Position -= ActiveCamera.Transform.Right * cameraSpeed * (float)args.Time; // Left
        if (input.IsKeyDown(Keys.D)) ActiveCamera.Transform.Position += ActiveCamera.Transform.Right * cameraSpeed * (float)args.Time; // Right
        if (input.IsKeyDown(Keys.Space)) ActiveCamera.Transform.Position += new Vector3(0.0f, 1.0f, 0.0f) * cameraSpeed * (float)args.Time; // Up
        if (input.IsKeyDown(Keys.LeftControl)) ActiveCamera.Transform.Position -= new Vector3(0.0f, 1.0f, 0.0f) * cameraSpeed * (float)args.Time; // Down
        if (input.IsKeyDown(Keys.T)) CursorState = CursorState.Grabbed; // Down

        _consoleCurrentLine = Console.GetCursorPosition();
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.WriteLine(ActiveCamera.Transform.Position.ToString());
        Console.SetCursorPosition(0, Console.WindowHeight);
        Console.WriteLine(ActiveCamera.Transform.Rotation.ToString());
        Console.SetCursorPosition(_consoleCurrentLine.left, _consoleCurrentLine.top);


        var mouse = MouseState;

        if (_firstMove)
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            ActiveCamera.Transform.Yaw += deltaX * sensitivity;
            ActiveCamera.Transform.Pitch = MathHelper.Clamp(ActiveCamera.Transform.Pitch - deltaY * sensitivity, -89f, 89f);
        }

    }
}
