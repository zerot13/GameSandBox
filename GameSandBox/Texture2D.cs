using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSandBox
{
    class Texture2D
    {
        private int id, width, height;

        public int ID { get { return id; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Texture2D(int id, int Width, int Height)
        {
            this.id = id;
            width = Width;
            height = Height;
        }
    }
}
