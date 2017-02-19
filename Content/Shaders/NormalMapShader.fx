#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

bool PhongIllumination = true;

float3 ViewerPosition;
float4x4 WorldInverseTranspose;

#define LIGHTS_MAX 200

int LightsNum;

float3 LightPosition[LIGHTS_MAX];
float3 AmbientColor = float3(1, 1, 1);
float3 ka = float3(0.1, 0.1, 0.1);

float3 kd;

float3 LightColor[LIGHTS_MAX];
float3 ks;
float Shininess;

texture normalMap;

sampler BasicTextureSampler = sampler_state {
	texture = (normalMap);
};


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TextureCoordinate : TEXTCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	output.Position = input.Position;
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 normal = tex2D(BasicTextureSampler, input.TextureCoordinate).rgb - float3(1, 1, 0);

	float3 position = input.WorldPosition;

	float3 res = AmbientColor * ka[0];
	for (int i = 0; i < LightsNum; ++i)
	{
		float sqrDist = 0;
		for (int j = 0; j < 3; ++j)
		{
			sqrDist += (position[j] - LightPosition[i][j]) * (position[j] - LightPosition[i][j]);
		}

		float3 lightDirection = normalize(LightPosition[i] - position);

		float3 r = normalize(2 * dot(lightDirection, normal) * normal - lightDirection);
		float3 v = -normalize(position - ViewerPosition);

		float3 diffuse = (dot(lightDirection, normal) * input.Color);

		float3 specularDotProduct;
		if (PhongIllumination)
		{
			specularDotProduct = dot(r, v);
		}
		else
		{
			specularDotProduct = dot(r, normalize(lightDirection + v));
		}

		float3 specular = (max(pow(specularDotProduct, Shininess), 0)) * LightColor[i];


		res += (diffuse + specular) / sqrDist * 5;
	}

	return float4(saturate(res), 1);
}


technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
};