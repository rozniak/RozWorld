/**
 * RozWorld.Graphics.Shaders -- RozWorld OpenGL Shader Definitions
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

namespace RozWorld.Graphics
{
    public static class Shaders
    {
        public const string VertexShader = @"
#version 130

in vec3 vertexPosition;
in vec2 vertexUV;

out vec2 uv;

void main(void)
{
    uv = vertexUV;
    gl_Position = vec4(vertexPosition, 1.0);
}
";

        public const string FragmentShader = @"
#version 130

uniform sampler2D texture;

in vec2 uv;
uniform vec4 tint;

out vec4 fragment;

void main(void)
{
    fragment = texture2D(texture, uv);

    float red = (tint.w * tint.x) + ((1 - tint.w) * fragment.x);
    float green = (tint.w * tint.y) + ((1 - tint.w) * fragment.y);
    float blue = (tint.w * tint.z) + ((1 - tint.w) * fragment.z);
    float alpha = fragment.w;

    fragment = vec4(red, green, blue, alpha);
}
";
    }
}
