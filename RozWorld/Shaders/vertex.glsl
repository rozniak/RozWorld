#version 330 core

layout(location = 0) in vec3 VertexPosition;

void main()
{
    gl_Position.xyz = VertexPosition;
	gl_Position.w = 1.0;
}