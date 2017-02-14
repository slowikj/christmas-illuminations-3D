﻿#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#else
#define VS_SHADERMODEL vs_4_0_level_9_3
#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

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

	float3 worldPosition = a;

	output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
	//output.Normal = normalize(input.Normal);
	
	float3 c = AmbientColor * 0.0;
	for (int i = 0; i < LightsNum; ++i)
	{
		float3 lightDirection = normalize(mul(normalize(LightPosition[i] - worldPosition), World));
		//float3 lightDirection = normalize(LightPosition[i] - worldPosition);
		float3 r = normalize(2 * dot(lightDirection, output.Normal) * output.Normal - lightDirection);//reflect(lightDirection, output.Normal));
		//2 * dot(light, normal) * normal - light
		float3 v = normalize(worldPosition - ViewerPosition);

		float3 diffuse = (dot(lightDirection, output.Normal) * input.Color);
		float3 specular = (max(pow(dot(r, v), Shininess), 0)) * LightColor[i];
		

		c += (diffuse + specular) ;
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
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
};