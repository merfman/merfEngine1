using OpenGLGameEngine.Mathmatics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine.Rendering;
/// <summary>
/// <see cref="Component"/> that holds Camera Info and can be sent to the renderer as the Main Scene Camera.
/// </summary>
public class CameraComponent : Component
{
    // The field of view of the camera (radians)
    private float _fov = MathHelper.PiOver2;
       
    // /// <summary>
    // /// The Camera <see cref="Mathmatics.Transform"/>. (note: this just gets the parent <see cref="GameObject"/>'s <see cref="Mathmatics.Transform"/>, this holds no data so it is always synced)
    // /// </summary>
    // public Transform Transform
    // {
    //     get => GameObject.Transform;
    //     set => GameObject.Transform = value;
    // } 

    /// <summary>
    /// Creates a <see cref="CameraComponent"/> with a reference to a <see cref="GameObject"/> and an Aspect Ratio.
    /// </summary>
    /// <param name="gameObject"> The GameObject this is a component to.</param>
    /// <param name="aspectRatio"> The ratio of the Camera's width and Height. </param>
    public CameraComponent(GameObject gameObject, float aspectRatio) : base(gameObject)
    {
        AspectRatio = aspectRatio;
    }

    /// <summary>
    /// Field Of View of the Camera. <see href="https://en.wikipedia.org/wiki/Field_of_view_in_video_games#:~:text=The%20FOV%20in%20a%20video,ratio%20of%20the%20rendering%20resolution."/>
    /// </summary>
    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1f, 90f); //TODO: expand if this is limiting the FOV to a maximum of 90
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    // This is simply the aspect ratio of the viewport, used for the projection matrix.
    public float AspectRatio { private get; set; }

    // Get the view matrix using the amazing LookAt function described more in depth on the web tutorials
    public Matrix4 GetViewMatrix() => Matrix4.LookAt(GameObject.Transform.Position, GameObject.Transform.Position + GameObject.Transform.Front, GameObject.Transform.Up);

    // Get the projection matrix using the same method we have used up until this point
    public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
}
