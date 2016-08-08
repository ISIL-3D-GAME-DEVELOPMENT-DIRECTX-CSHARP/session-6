﻿cbuffer Fx_MiscInputBuffer : register(b0) {
	uniform float ambientIntensity;
	uniform float3 lightDirection;
	uniform float4 ambientColor;
	uniform float4 diffuseLighting;
	uniform float4x4 worldViewProj;
};

struct VS_IN {
	float3 pos : POSITION;
	float3 normal : NORMAL;
};

struct PS_IN {
	float4 pos : SV_POSITION;
	float3 normal : NORMAL0;
	float4 ambientColor : POSITION0;
	float4 diffuseLighting : POSITION1;
	float3 lightDirection : NORMAL1;
	float ambientIntensity : PSIZE0;
};

PS_IN VS( VS_IN input ) {
	PS_IN output = (PS_IN)0;

	float4 position = float4(input.pos.xyz, 1.0);

	output.pos = mul(position, worldViewProj);
	output.normal = normalize(mul(input.normal, worldViewProj));
	output.ambientColor = ambientColor;
	output.diffuseLighting = diffuseLighting;
	output.lightDirection = lightDirection;
	output.ambientIntensity = ambientIntensity;

	return output;
}

float4 PS(PS_IN input) : SV_Target {
	return (input.ambientColor * input.ambientIntensity) + 
		input.diffuseLighting * saturate(dot(input.lightDirection, input.normal));
}
