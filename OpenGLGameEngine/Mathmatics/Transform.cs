using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace OpenGLGameEngine.Mathmatics;
/// <summary>
/// 
/// </summary>
/// <remarks> Copy/Pasted from last project, intend on replacing with new one</remarks>
public class Transform : BaseObject
{ 
    private Vector3 _position = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 _scale = new Vector3(1.0f, 1.0f, 1.0f);

    private Quaternion _rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

    // direction pointing frontwards from the Transform
    private Vector3 _front = -Vector3.UnitZ;

    // direction pointing upwards from the Transform
    private Vector3 _up = Vector3.UnitY;

    // direction pointing out of side of the Transform
    private Vector3 _right = Vector3.UnitX;


    private Vector2 _front2d = Vector2.UnitY; //-Vector3.UnitZ * (1.0f, 0.0f, 1.0f);

    private Vector2 _right2d = Vector2.UnitX; //Vector3.UnitX * (1.0f, 0.0f, 1.0f);


    // Rotation around the X axis (radians)
    private float _pitch;

    // Rotation around the Y axis (radians)
    private float _yaw;// = -MathHelper.PiOver2; // Without this, you would be started rotated 90 degrees right.

    // Rotation around the Z axis (radians)
    private float _roll;

    //public Matrix4 transformMatrix { get; set; } = Matrix4.Identity;

    // Empty Constructor
    public Transform() { }
    // 3D Constructors
    public Transform(Vector3 position) => Position = position;
    public Transform(Vector3 position, Quaternion rotation) => (Position, Rotation) = (position, rotation);
    public Transform(Vector3 position, Vector3 rotation) => (Position, Rotation) = (position, Quaternion.FromEulerAngles(rotation));
    public Transform(Vector3 position, Quaternion rotation, Vector3 scale) => (Position, Rotation, Scale) = (position, rotation, scale);
    public Transform(Vector3 position, Vector3 rotation, Vector3 scale) => (Position, Rotation, Scale) = (position, Quaternion.FromEulerAngles(rotation), scale);
    // 2D Constructors
    private void setValues2d(Vector2 position, float rotation, Vector2 scale)
    {
        Position = new Vector3(position.X, position.Y, 0.0f);
        Rotation = Quaternion.FromEulerAngles(0.0f, rotation, 0.0f);
        Scale = new Vector3(scale.X, 0.0f, scale.Y);
    }
    public Transform(Vector2 position) => setValues2d(position, 0, new Vector2(1.0f, 1.0f));
    public Transform(Vector2 position, float rotation) => setValues2d(position, rotation, new Vector2(1.0f, 1.0f));
    public Transform(Vector2 position, float rotation, Vector2 scale) => setValues2d(position, rotation, scale);

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            UpdateVectors();
            UpdateEulerAngles();
        }
    }
    public Vector3 Scale
    {
        get => _scale;
        set => _scale = value;
    }

    public Vector3 Front => _front;
    public Vector3 Up => _up;
    public Vector3 Right => _right;

    public Vector3 Front2D => (_front2d.X, 0.0f, _front2d.Y);

    public float pitch
    // We convert from degrees to radians as soon as the property is set to improve performance.
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
            // of weird "bugs" when you are using euler angles for rotation.
            // If you want to read more about this you can try researching a topic called gimbal lock
            //var angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            //UpdateQuaternion();
        }
    }

    public float Pitch
    // We convert from degrees to radians as soon as the property is set to improve performance.
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
            // of weird "bugs" when you are using euler angles for rotation.
            // If you want to read more about this you can try researching a topic called gimbal lock
            //var angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }
    public float Yaw
    // We convert from degrees to radians as soon as the property is set to improve performance.
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }
    public float Roll
    {
        get => MathHelper.RadiansToDegrees(_roll);
        set
        {
            _roll = MathHelper.DegreesToRadians(value);
            UpdateVectors();
            UpdateQuaternion();
        }
    }

    //called when any Euler Angles change, this makes sure the Quaternion stays synced
    private void UpdateQuaternion() => _rotation = Quaternion.FromEulerAngles(_pitch, _yaw, _roll);
    //called when the Quaternion is changed, this makes sure the Euler Angles stay synced
    private void UpdateEulerAngles() => (_pitch, _yaw, _roll) = _rotation.ToEulerAngles();

    private void UpdateVectors()
    // This function is going to update the direction vertices using some of the math learned in the web tutorials.
    {
        // First, the front matrix is calculated using some basic trigonometry.
        _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        _front.Y = MathF.Sin(_pitch);
        _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
        _front = Vector3.Normalize(_front);

        // Calculate both the right and the up vector using cross product.
        // Note that we are calculating the right from the global up; this behaviour might
        // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));


        _front2d.X = MathF.Cos(_yaw);
        _front2d.Y = MathF.Sin(_yaw);
    }

    public Matrix4 GetLookAtMatrix() => Matrix4.LookAt(_position, _position + _front, _up);
}
