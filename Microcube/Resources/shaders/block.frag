#version 330 core
out vec4 FragColor;

// TODO: add block textures (should I use texture arrays?)

const float AMBIENT_ALBEDO = 0.3;
const float DIFFUSE_ALBEDO = 0.7;
const float GAMMA = 2.2;
const float LINE_THICKNESS = 0.035;

uniform vec3 lightDirection;

in VertexData {
	vec3 normal;
	vec2 uv;
	vec3 color;
	vec3 edgesColor;
	float displayEdges;
} inData;

void main() {
	vec3 ambient = inData.color * AMBIENT_ALBEDO;
	vec3 diffuse = inData.color * max(0, dot(lightDirection, inData.normal)) * DIFFUSE_ALBEDO;

	if (inData.displayEdges != 0 && min(min(abs(inData.uv.x), abs(inData.uv.x - 1.0)), min(abs(inData.uv.y), abs(inData.uv.y - 1.0))) < LINE_THICKNESS) {
		// TODO: should I add specular here?
		FragColor = vec4(pow(inData.edgesColor, vec3(1.0 / GAMMA)), 1.0);
        return;
    }

	FragColor = vec4(pow(ambient + diffuse, vec3(1.0 / GAMMA)), 1.0);
}