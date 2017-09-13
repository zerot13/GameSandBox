using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace GameSandBox
{
    struct Vertex
    {
        private Vector2 _position;
        private Vector2 _texCoord;
        public Vector4 _color;

        public Vertex(Vector2 position, Vector2 texCoord, Vector4 color)
        {
            _position = position;
            _texCoord = texCoord;
            _color = color;
        }

        public Vertex(Vector2 position, Vector2 texCoord, Color color)
        {
            _position = position;
            _texCoord = texCoord;
            _color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f); ;
        }

        public Vertex(Vector2 position, Color color)
        {
            _position = position;
            _texCoord = new Vector2(0, 0);
            _color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f); ;
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 TexCoord
        {
            get { return _texCoord; }
            set { _texCoord = value; }
        }

        public Vector4 V4Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Color Color
        {
            get { return System.Drawing.Color.FromArgb((int)(_color.W * 255), (int)(_color.X * 255), (int)(_color.Y * 255), (int)(_color.Z * 255)); }
            set { this._color = new Vector4(value.R / 255f, value.G / 255f, value.B / 255f, value.A / 255f); }
        }

        public static int SizeInBytes { get { return Vector4.SizeInBytes + (Vector2.SizeInBytes * 2); } }
    }
}
