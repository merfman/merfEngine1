using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGLGameEngine.Asset;
public class Texture : Asset
{
    public int Handle; // OpenGL 2d texture handle
    public int Width;
    public int Height;
    public PixelInternalFormat Format;
    public TextureWrapMode WrapMode;
    public override void LoadFromFile(string path)
    {
        throw new NotImplementedException();
    }
}
