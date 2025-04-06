#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct VertexShaderOutput
{
	float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
};

sampler2D TexSampler;

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 col = tex2D(TexSampler, input.TexCoord);
	
    float avgCol = (col.r + col.g + col.b) / 3;
	
    float3 mulCol = input.Color.rgb * avgCol;
	
    return float4(mulCol, col.a);

}

technique BasicColorDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};