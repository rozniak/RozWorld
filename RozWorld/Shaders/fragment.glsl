#version 330 core


out vec3 Colour;

uniform float fTime;


void main()
{
    Colour = vec3(abs(sin(fTime)), abs(cos(fTime)), abs(sin(fTime / 5f)));
}