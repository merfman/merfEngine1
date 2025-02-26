#version 330 core

in vec3 FragPos;
in vec3 Normal;
in vec3 Tangent;
in vec2 TexCoord;

out vec4 FragColor;

uniform sampler2D textureSampler;

void main()
{
    // Normalize normal
    vec3 norm = normalize(Normal);

    // Sample texture color
    vec4 texColor = texture(textureSampler, TexCoord);

    // Basic lighting (fake light direction)
    vec3 lightDir = normalize(vec3(0.5, 1.0, 0.3));
    float diff = max(dot(norm, lightDir), 0.0);

    // Apply lighting effect
    vec3 lighting = diff * texColor.rgb;

    FragColor = vec4(lighting, texColor.a);
}

//this Shader was made by ChatGPT and is intended to be replaced soon