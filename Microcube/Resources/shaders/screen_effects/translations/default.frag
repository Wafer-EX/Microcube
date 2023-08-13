#version 330 core
out vec4 FragColor;

uniform sampler2D sprite;
uniform float elapsedTime;

in VertexData {
    vec2 uv;
} inData;

void main() {
    float brigthness = abs(0.5 - elapsedTime) * 2.0;
    vec4 color = texture(sprite, inData.uv);
    color.rgb *= brigthness;

    FragColor = color;
}