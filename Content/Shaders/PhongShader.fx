#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

bool PhongIllumination = true;

float4x4 World;
float4x4 View;
float4x4 Projection;
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


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float3 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float3 Color : COLOR0;
	float3 WorldPosition : POSITION1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 a = mul(input.Position, World);
	float4 b = mul(a, View);
	
	output.Position = mul(b, Projection);
	output.WorldPosition = a;
	output.Normal = input.Normal;
	output.Color = input.Color;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 normal = normalize(mul(input.Normal, WorldInverseTranspose));

	float3 position = input.WorldPosition;

	float3 res = AmbientColor * ka[0];
	for (int i = 0; i < LightsNum; ++i)
	{
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


		res += (diffuse * kd[0] + specular * ks[0]);
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