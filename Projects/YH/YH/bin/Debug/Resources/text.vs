#version 330 core
layout (location = 0) in vec4 vertex; // <vec2 pos, vec2 tex>
out vec2 TexCoords;

uniform mat4 projection;

void main()
{
    gl_Position = projection * vec4(vertex.xy, 0.0, 1.0);
    float x = (vertex.x/800.0) * 2.0 - 1.0f;
    float y = (vertex.y/600.0) * 2.0 - 1.0f;
    gl_Position = vec4(x, y, 0.0, 1.0);
    TexCoords = vertex.zw;
}