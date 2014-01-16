using Nancy.Hosting.Self;
using System;

namespace WebApp
{
    /// <summary>
    /// Verify that WebApp.csproj is configured to launch this console application when F5ing in Visual Studio (right click the project file, Properties->Application->Startup Object)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start();
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Within a Browser navigate to ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("http://localhost:1234");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" + (routes configured within our NancyModules)");
                Console.ReadLine();
            }
        }
    }
}
