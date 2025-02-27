using OpenGLGameEngine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection.Metadata;
using System.IO;

namespace OpenGLGameEngine.Rendering;
/// <summary>
/// Handles the management of shaders.
/// </summary>
/// <remarks> This class relied upon <see href="https://github.com/opentk/LearnOpenTK"/> as a reference during creation </remarks>
public class Shader : Asset
{
    public readonly int Handle;

    public Shader(string vertPath, string fragPath, string? geomPath = null)
    {
        Console.WriteLine($"Loading vert Shader {vertPath}");
        Console.WriteLine($"Loading frag Shader {fragPath}");
        if (geomPath != null) Console.WriteLine($"Loading geom Shader {geomPath}");
        int vertexShader = CompileShader(vertPath, ShaderType.VertexShader);
        int fragmentShader = CompileShader(fragPath, ShaderType.FragmentShader);
        int geometryShader = geomPath != null ? CompileShader(geomPath, ShaderType.GeometryShader) : 0;

        // Initialize the program
        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        //Link the program
        LinkProgram(Handle);

        // Cleanup shaders
        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        if (geometryShader != 0)
        {
            GL.DetachShader(Handle, geometryShader);
            GL.DeleteShader(geometryShader);
        }
    }
    // A wrapper function that enables the shader program.
    public void Use()
    {
        GL.UseProgram(Handle);
    }

    private static int CompileShader(string shaderPath, ShaderType shaderType)
    {
        //if (!File.Exists(shaderPath))
        //    throw new FileNotFoundException($"Error: Shader not found {shaderPath}");

        // Load and compile the shader
        string shaderSource = File.ReadAllText(shaderPath);
        int shader = GL.CreateShader(shaderType);
        GL.ShaderSource(shader, shaderSource);

        // Try to compile the shader
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error: {shaderType} Shader Compilation Error ({shaderPath}):\n{infoLog}");
        }

        return shader;
    }
    private static void LinkProgram(int program)
    {
        // We link the program
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error: Error occurred whilst linking Program({program})");
        }
    }
    public void SetInt(string name, int value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(location, value);
    }

    public void SetFloat(string name, float value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(location, value);
    }

    public void SetMatrix4(string name, Matrix4 matrix)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.UniformMatrix4(location, false, ref matrix);
    }

    public void SetVector3(string name, Vector3 vector)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform3(location, vector);
    }

    public void SetSampler2D(string name, Texture texture, int textureUnit = 0)
    {
        GL.ActiveTexture(TextureUnit.Texture0 + textureUnit); // Activate correct texture unit
        GL.BindTexture(TextureTarget.Texture2D, texture.Handle); // Bind the texture

        int location = GL.GetUniformLocation(Handle, name);
        if (location != -1) // Ensure uniform exists
        {
            GL.Uniform1(location, textureUnit); // Set the sampler2D uniform to use this texture unit
        }
    }

}
