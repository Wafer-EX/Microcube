#version 330 core
layout (location = 0) in vec2 position;
layout (location = 1) in vec2 uv;

out VertexData {
    vec2 uv;
} outData;

void main() {
    outData.uv = uv;
    gl_Position = vec4(position.x, position.y, 0.0, 1.0);
}