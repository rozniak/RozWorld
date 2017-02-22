#version 330 core

layout(location = 0) in vec3 VertexPosition;
layout(location = 1) in vec2 VertexUV;

out vec2 UV;

void main()
{
    gl_Position.xyz = VertexPosition;
    gl_Position.w = 1.0;
    
    UV = VertexUV;
}