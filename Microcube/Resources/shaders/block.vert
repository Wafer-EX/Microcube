#version 330 core

const float NECESSARY_TOP_COLOR_NORMAL = 0.99;

// Mesh attributes
layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec2 uv;

// Instance attributes
layout (location = 3) in vec3 color;
layout (location = 4) in vec3 topColor;
layout (location = 5) in vec3 edgesColor;
layout (location = 6) in vec4 row1;
layout (location = 7) in vec4 row2;
layout (location = 8) in vec4 row3;
layout (location = 9) in vec4 row4;
layout (location = 10) in float displayEdges;

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;

out VertexData {
	vec3 normal;
	vec2 uv;
	vec3 color;
	vec3 edgesColor;
	float displayEdges;
} outData;

void main() {
	mat4 modelMatrix = mat4(row1, row2, row3, row4);
	mat3 normalMatrix = mat3(transpose(inverse(modelMatrix)));

	outData.normal = normalize(normalMatrix * normal);
	outData.uv = uv;
	outData.color = outData.normal.y >= NECESSARY_TOP_COLOR_NORMAL ? topColor : color;
	outData.edgesColor = edgesColor;
	outData.displayEdges = displayEdges;

	gl_Position = projectionMatrix * viewMatrix * modelMatrix * vec4(position, 1.0);
}