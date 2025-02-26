using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGameEngine;
public static class PathH
{
    public static string GetRelative(string relativePath)
    {
        string projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        return Path.Combine(projectRoot, relativePath);
    }
}
