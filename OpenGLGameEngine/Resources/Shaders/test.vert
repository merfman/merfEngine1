#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aNormal;
layout(location = 2) in vec3 aTangent;
layout(location = 3) in vec2 aTexCoord;

out vec3 FragPos;
out vec3 Normal;
out vec3 Tangent;
out vec2 TexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = mat3(transpose(inverse(model))) * aNormal; // Transform normals
    Tangent = normalize(mat3(model) * aTangent); // Transform tangents
    TexCoord = aTexCoord;

    gl_Position = projection * view * vec4(FragPos, 1.0);
}


//this Shader was made by ChatGPT and is intended to be replaced soon