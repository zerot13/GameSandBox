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
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            //Console.WriteLine("Update");
            //throw new NotImplementedException();
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            //Console.WriteLine("Render");
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(Color.Green);
            GL.Vertex3(-1, 0, .5f);
            GL.Color3(Color.OrangeRed);
            GL.Vertex3(1, 0, .5f);
            GL.Color3(Color.Yellow);
            GL.Vertex3(0, 1, .5f);

            GL.Color4(1f, 1f, 1f, .5f);
            GL.Vertex3(-0.25f, 1f, .8f);
            GL.Vertex3(1f, -0.25f, .1f);
            GL.Vertex3(-0.25f, -0.25f, .1f);

            GL.End();

            _window.SwapBuffers();
        }
    }
}
