#version 330 core
out vec4 FragColor;

uniform sampler2D sprite;

in VertexData {
    vec2 uv;
    vec4 color;
    float isIgnoreSprite;
} inData;

void main() {
    if (inData.isIgnoreSprite != 0.0) {
        // If this shader should ignore sprite it draws just a color.
        FragColor = inData.color;
    }
    else {
        FragColor = texture(sprite, inData.uv) * inData.color;
    }
}