using OpenGLGameEngine.Mathmatics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Rendering;
public class Camera : GameObject
{
    // The field of view of the camera (radians)
    private float _fov = MathHelper.PiOver2;

    public Camera(Vector3 position, float aspectRatio)
    {
        Transform.Position = position;
        Transform.Scale *= 0.1f;
        AspectRatio = aspectRatio;
    }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1f, 90f);//TODO: expand if this is limiting the FOV to a maximum of 90
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    // This is simply the aspect ratio of the viewport, used for the projection matrix.
    public float AspectRatio { private get; set; }

    // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
    public Matrix4 GetViewMatrix() => Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Front, Transform.Up);

    // Get the projection matrix using the same method we have used up until this point
    public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
}
