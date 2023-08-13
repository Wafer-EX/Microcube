#version 330 core
out vec4 FragColor;

uniform sampler2D sprite;

in VertexData {
    vec2 uv;
} inData;

void main() {
    FragColor = texture(sprite, inData.uv);
}