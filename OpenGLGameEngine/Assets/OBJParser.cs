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
    /// - float[] Tangent (x, y, z),
    /// - float[] Texture coordinates (u, v).
    /// </returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    /// <exception cref="FormatException">Thrown if the file format is invalid.</exception>
    public static (float[], int[], float[], float[], float[]) Parse(string path)
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

        float[] tangentData = CalculateTangents(vertexData.ToArray(), indices.ToArray(), texCoordData.ToArray());

        return (vertexData.ToArray(), indices.ToArray(), normalData.ToArray(), tangentData, texCoordData.ToArray());
    }

    public static float[] CalculateTangents(float[] vertices, int[] indices, float[] texCoords)
    {
        int vertexCount = vertices.Length / 3;
        float[] tangents = new float[vertexCount * 3]; // One tangent per vertex

        for (int i = 0; i < indices.Length; i += 3)
        {
            int i0 = indices[i] * 3;
            int i1 = indices[i + 1] * 3;
            int i2 = indices[i + 2] * 3;

            // Get vertex positions
            Vector3 v0 = new Vector3(vertices[i0], vertices[i0 + 1], vertices[i0 + 2]);
            Vector3 v1 = new Vector3(vertices[i1], vertices[i1 + 1], vertices[i1 + 2]);
            Vector3 v2 = new Vector3(vertices[i2], vertices[i2 + 1], vertices[i2 + 2]);

            // Get texture coordinates
            int t0 = indices[i] * 2;
            int t1 = indices[i + 1] * 2;
            int t2 = indices[i + 2] * 2;

            Vector2 uv0 = new Vector2(texCoords[t0], texCoords[t0 + 1]);
            Vector2 uv1 = new Vector2(texCoords[t1], texCoords[t1 + 1]);
            Vector2 uv2 = new Vector2(texCoords[t2], texCoords[t2 + 1]);

            // Compute tangent vector
            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;
            Vector2 deltaUV1 = uv1 - uv0;
            Vector2 deltaUV2 = uv2 - uv0;

            float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

            Vector3 tangent = new Vector3(
                f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X),
                f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y),
                f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z)
            );

            // Normalize tangent
            tangent.Normalize();

            // Assign the tangent to each vertex of the triangle
            for (int j = 0; j < 3; j++)
            {
                int vIndex = indices[i + j] * 3;
                tangents[vIndex] += tangent.X;
                tangents[vIndex + 1] += tangent.Y;
                tangents[vIndex + 2] += tangent.Z;
            }
        }

        // Normalize all tangents
        for (int i = 0; i < tangents.Length; i += 3)
        {
            Vector3 t = new Vector3(tangents[i], tangents[i + 1], tangents[i + 2]).Normalized();
            tangents[i] = t.X;
            tangents[i + 1] = t.Y;
            tangents[i + 2] = t.Z;
        }

        return tangents;
    }
}

// Parser Written by ChatGPT
