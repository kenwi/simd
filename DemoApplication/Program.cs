using System;
using Veldrid;

namespace DemoApplication
{ 
    public sealed class Demo : Engine.Application
    {
        static void Main() => Demo.Instance.Run();

        static Demo Instance => _instance;
        static readonly Demo _instance = new Demo();

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            throw new NotImplementedException();
        }
    }
}
