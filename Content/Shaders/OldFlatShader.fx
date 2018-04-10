#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

bool PhongIllumination = true;

float3 SidePosition;

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


struct ShaderData
{
	float4 Position : POSITION0;
	float3 Normal : NORMAL0;
	float3 Color : COLOR0;
};

ShaderData VertexShaderFunction(ShaderData input)
{
	ShaderData output;
	float4 a = mul(input.Position, World);
	float4 b = mul(a, View);
	output.Position = mul(b, Projection);

	float3 worldPosition = mul(SidePosition, World);

	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));

	float3 c = AmbientColor * ka;
	for (int i = 0; i < LightsNum; ++i)
	{
		float sqrDist = 0;
		for (int j = 0; j < 3; ++j)
		{
			sqrDist += (worldPosition[j] - LightPosition[i][j]) * (worldPosition[j] - LightPosition[i][j]);
		}

		float3 lightDirection = normalize(LightPosition[i] - worldPosition);

		float3 r = normalize(2 * dot(lightDirection, output.Normal) * output.Normal - lightDirection);
		float3 v = -normalize(worldPosition - ViewerPosition);

		float3 diffuse = (dot(lightDirection, output.Normal) * input.Color);


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


		c += (diffuse * ks + specular * kd) / sqrDist;
	}

	c = saturate(c);

	output.Color = c;

	return output;
}

float4 PixelShaderFunction(ShaderData input) : COLOR0
{
	return float4(input.Color,1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 VertexShaderFunction();
		PixelShader = compile ps_5_0 PixelShaderFunction();
	}
};