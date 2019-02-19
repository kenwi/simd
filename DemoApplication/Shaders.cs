namespace DemoApplication
{
       struct DemoShaders
    {
        public const string VertexCode = @"
#version 450

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec2 fsin_Position;
layout(location = 1) out vec4 fsin_Color;

void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
    fsin_Position = Position;
}";

        public const string FragmentCode = @"
#version 450

layout(location = 0) in vec2 fsin_Position;
layout(location = 1) in vec4 fsin_Color;

layout(location = 0) out vec4 fsout_Color;

void main()
{
    fsout_Color = fsin_Color;

    if(distance(fsin_Position, vec2(0)) - 0.75 < 0)
    {
        fsout_Color = fsin_Color * vec4(0.5);
    }

}";
    }
}

