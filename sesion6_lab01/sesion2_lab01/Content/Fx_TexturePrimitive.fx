cbuffer ShaderInputParameters : register(b0) {
	uniform float4x4 worldViewProj;
};

struct VS_IN {
	float4 pos : POSITION0;
	float4 col : COLOR;
	float2 textCoord : TEXCOORD;
};

struct PS_IN {
	float4 pos : SV_POSITION;
	float4 col : COLOR;
	float2 textCoord : TEXCOORD;
};

Texture2D picture : register(t0);
SamplerState pictureSampler : register(s0);

PS_IN VS( VS_IN input ) {
	PS_IN output = (PS_IN)0;
	
	output.pos = mul(input.pos, worldViewProj);
	output.col = input.col;
	output.textCoord = input.textCoord;
	
	return output;
}

float4 PS( PS_IN input ) : SV_Target {
	return picture.Sample(pictureSampler, input.textCoord) * input.col;
}

technique10 Render {
	pass P0 {
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}