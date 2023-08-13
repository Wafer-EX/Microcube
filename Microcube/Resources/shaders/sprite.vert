#version 330 core
layout (location = 0) in vec2 position;
layout (location = 1) in vec2 uv;
layout (location = 2) in vec4 color;
layout (location = 3) in float isIgnoreSprite;

uniform mat4 projectionMatrix;

out VertexData {
    vec2 uv;
    vec4 color;
    float isIgnoreSprite;
} outData;

void main() {
    outData.uv = uv;
    outData.color = color;
    outData.isIgnoreSprite = isIgnoreSprite;

    gl_Position = projectionMatrix * vec4(position.x, position.y, 0.0, 1.0);
}