using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;

namespace GameSandBox
{
    class Game
    {
        private GameWindow _window;

        Texture2D texture;

        public Game(GameWindow w)
        {
            _window = w;

            _window.Load += Window_Load;
            _window.UpdateFrame += Window_UpdateFrame;
            _window.RenderFrame += Window_RenderFrame;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.6f);

            texture = ContentPipe.LoadTexture("Content/penguin.png");
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            //Console.WriteLine("Update");
            //throw new NotImplementedException();
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(0, _window.Width, _window.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);

            Matrix4 modelViewMatrix = Matrix4.CreateScale(.5f, .5f, 1f) * Matrix4.CreateRotationZ(0f) * Matrix4.CreateTranslation(0f, 0f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);
            DrawPenguin();

            modelViewMatrix = Matrix4.CreateScale(.6f, .4f, 1f) * Matrix4.CreateRotationZ(0f) * Matrix4.CreateTranslation(300f, 0f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);
            DrawPenguin();

            modelViewMatrix = Matrix4.CreateScale(.5f, .5f, 1f) * Matrix4.CreateRotationZ(0.4f) * Matrix4.CreateTranslation(200f, 250f, 0f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);
            DrawPenguin();

            modelViewMatrix = Matrix4.CreateScale(1f, 1f, 1f) * Matrix4.CreateRotationZ(0.4f) * Matrix4.CreateTranslation(0f, 0f, -0.1f);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelViewMatrix);
            DrawPenguin();

            _window.SwapBuffers();
        }

        private void DrawPenguin()
        {
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(1f, 1f, 1f, 1f);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(500, 525);
            GL.TexCoord2(0, 1);
            GL.Vertex2(0, 525);

            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(500, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(500, 525);

            GL.End();
        }

        private void DrawSquare()
        {
            //GL.Begin(PrimitiveType.Quads);
            //GL.Color4(1f, 1f, 1f, 1f);
            //GL.TexCoord2(0, 0);
            //GL.Vertex2(1, 1);
            //GL.TexCoord2(1, 0);
            //GL.Vertex2(0, 1);
            //GL.TexCoord2(1, 1);
            //GL.Vertex2(0, 0);
            //GL.TexCoord2(0, 1);
            //GL.Vertex2(1, 0);
            //GL.End();
        }
    }
}
