using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace OpenGLGameEngine.Assets;

/// <summary>
/// A simple parser for loading Wavefront .OBJ files into vertex, index, normal, and texture coordinate arrays.
/// </summary>
public static class OBJParser
{
    /// <summary>
    /// Parses an .OBJ file and extracts vertex positions, face indices, normals, and texture coordinates.
    /// </summary>
    /// <param name="path">The file path to the .OBJ file.</param>
    /// <returns>
    /// A tuple containing:
    /// - float[] Vertex positions (x, y, z),
    /// - int[] Indices defining triangle faces,
    /// - float[] Normals (x, y, z),
    /// - float[] Texture coordinates (u, v).
    /// </returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    /// <exception cref="FormatException">Thrown if the file format is invalid.</exception>
    public static (float[], int[], float[], float[]) Parse(string path)
    {
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> texCoords = new List<Vector2>();
        List<int> indices = new List<int>();

        List<float> vertexData = new List<float>();
        List<float> normalData = new List<float>();
        List<float> texCoordData = new List<float>();

        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "v": // Vertex Position
                        positions.Add(new Vector3(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture),
                            float.Parse(parts[3], CultureInfo.InvariantCulture)));
                        break;

                    case "vt": // Texture Coordinate
                        texCoords.Add(new Vector2(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture)));
                        break;

                    case "vn": // Normal
                        normals.Add(new Vector3(
                            float.Parse(parts[1], CultureInfo.InvariantCulture),
                            float.Parse(parts[2], CultureInfo.InvariantCulture),
                            float.Parse(parts[3], CultureInfo.InvariantCulture)));
                        break;

                    case "f": // Face (Supports v/vt/vn format)
                        for (int i = 1; i <= 3; i++)
                        {
                            string[] indicesData = parts[i].Split('/');
                            int vertexIndex = int.Parse(indicesData[0]) - 1;
                            int texCoordIndex = indicesData.Length > 1 && indicesData[1] != "" ? int.Parse(indicesData[1]) - 1 : -1;
                            int normalIndex = indicesData.Length > 2 ? int.Parse(indicesData[2]) - 1 : -1;

                            indices.Add(vertexData.Count / 8); // Track index

                            vertexData.Add(positions[vertexIndex].X);
                            vertexData.Add(positions[vertexIndex].Y);
                            vertexData.Add(positions[vertexIndex].Z);

                            if (texCoordIndex >= 0)
                            {
                                texCoordData.Add(texCoords[texCoordIndex].X);
                                texCoordData.Add(texCoords[texCoordIndex].Y);
                            }
                            else
                            {
                                texCoordData.Add(0);
                                texCoordData.Add(0);
                            }

                            if (normalIndex >= 0)
                            {
                                normalData.Add(normals[normalIndex].X);
                                normalData.Add(normals[normalIndex].Y);
                                normalData.Add(normals[normalIndex].Z);
                            }
                            else
                            {
                                normalData.Add(0);
                                normalData.Add(0);
                                normalData.Add(0);
                            }
                        }
                        break;
                }
            }
        }

        return (vertexData.ToArray(), indices.ToArray(), normalData.ToArray(), texCoordData.ToArray());
    }
}

// Parser Written by ChatGPT
