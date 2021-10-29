using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            // OpenTK.GameWindow baseWindow = new OpenTK.GameWindow(800, 600, new OpenTK.Graphics.GraphicsMode(32, 8, 0, 0));
            OpenTK.GameWindow baseWindow = new OpenTK.GameWindow(800, 600);

            Game game = new Game(baseWindow, 3, 3, 2);
            baseWindow.Run();
        }
    }
}
