#version 330 core

in vec2 UV;

out vec3 Colour;

uniform float fTime;
uniform sampler2D TextureSampler;


void main()
{
    //Colour = vec3(abs(sin(fTime)), abs(cos(fTime)), abs(sin(fTime / 5f)));
    Colour = texture(TextureSampler, UV).rgb;
}