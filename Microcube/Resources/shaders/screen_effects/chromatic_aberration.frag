#version 330 core
out vec4 FragColor;

uniform sampler2D sprite;
uniform float strength;

in VertexData {
    vec2 uv;
} inData;

void main() {
    float textureWidth = textureSize(sprite, 0).x;
    float offset = strength / textureWidth;

    vec4 offsetLeftColor = texture(sprite, inData.uv - vec2(offset, 0.0));
    vec4 centerColor = texture(sprite, inData.uv);
    vec4 offsetRightColor = texture(sprite, inData.uv + vec2(offset, 0.0));

    FragColor = vec4(offsetLeftColor.r, centerColor.g, offsetRightColor.b, centerColor.a);
}