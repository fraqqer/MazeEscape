#version 330
 
in vec2 v_TexCoord;
in vec3 v_Normal;
in vec3 v_FragPos;
uniform sampler2D s_texture;

out vec4 Color;

void FirstCalculation(vec3 lightPos, vec3 lightColor);
void CalculateLight(vec3 lightPos, vec3 lightColor);

void main()
{
	vec3 lightColor = vec3(1,1,1);
	vec3 lightPos = vec3(0,10,0);

	FirstCalculation(lightPos, lightColor);

	vec3 secondLightColor = vec3(0.3, 0.3, 0.3);
	vec3 secondLightPos = vec3(-10, 10, 0);

	CalculateLight(secondLightPos, secondLightColor);

	vec3 thirdLightColor = vec3(0.4, 0.4, 0.4);
	vec3 thirdLightPos = vec3(0, 10, 10);

	CalculateLight(thirdLightPos, thirdLightColor);

	vec3 fourthLightColour = vec3(0.3, 0.3, 0.3);
	vec3 fourthLightPos = vec3(0, 10, -10);

	CalculateLight(fourthLightPos, fourthLightColour);
}

void CalculateLight(vec3 lightPos, vec3 lightColor)
{
	vec3 norm = normalize(v_Normal);
	vec3 lightDir = normalize(lightPos - v_FragPos); 
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	Color = Color + texture2D(s_texture, v_TexCoord) * vec4(diffuse, 0);
}

void FirstCalculation(vec3 lightPos, vec3 lightColor)
{
	vec4 lightAmbient = vec4(0.1, 0.1, 0.1, 1.0);
	vec3 norm = normalize(v_Normal);
	vec3 lightDir = normalize(lightPos - v_FragPos); 
	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;

	Color = lightAmbient + texture2D(s_texture, v_TexCoord) * vec4(diffuse, 0);
}