#version 450

layout(location = 0) in vec2 fsin_Position;
layout(location = 1) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;
const float epsilon = 0.5;

float sdf(vec2 p)
{
    return length(p) - epsilon;
}

vec2 getNormal(vec2 p)
{
    float x = sdf(vec2(p.x + epsilon, p.y)) - sdf(vec2(p.x - epsilon, p.y));
    float y = sdf(vec2(p.x, p.y + epsilon)) - sdf(vec2(p.x, p.y - epsilon));
    return normalize(vec2(x, y));
}

void main()
{
    fsout_Color = vec4(0);
    vec2 camera = vec2(0.5, 0.5);
    if(sdf(fsin_Position) < 0)
    {    
        fsout_Color = fsin_Color * dot(getNormal(camera), fsin_Position);
    }
}