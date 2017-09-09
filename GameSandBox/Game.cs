﻿using System;
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
            _window.KeyDown += Window_KeyDown;
            _window.KeyUp += Window_KeyUp;
            _window.KeyPress += Window_KeyPress;
            _window.MouseMove += Window_MouseMove;
            _window.MouseWheel += Window_MouseWheel;
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Console.WriteLine("Delta: {0} / {1}\nValue: {2} / {3}\n", e.Delta, e.DeltaPrecise, e.Value, e.ValuePrecise);
        }

        Vector2 mousePos;
        private void Window_MouseMove(object sender, MouseMoveEventArgs e)
        {
            //Console.WriteLine("MouseMove: {0}, {1}", e.X, e.Y);
            mousePos = new Vector2(e.X, e.Y);
        }

        private void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            //Console.WriteLine("KeyDown: {0} - {1}", e.Key, sender);
        }

        private void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
        {
            //Console.WriteLine("KeyUp: {0} - {1}", e.Key, sender);
        }

        private void Window_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Console.WriteLine("KeyPress: {0} - {1}", e.KeyChar, sender);
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

        KeyboardState prevState;
        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Enter) && prevState.IsKeyUp(Key.Enter))
            {
                Console.WriteLine("Enter!");
            }
            prevState = keyState;

            //Console.WriteLine("MousePos: {0}, {1}", _window.Mouse.X, _window.Mouse.Y);
            MouseState mState = Mouse.GetCursorState();
            Point mPos = _window.PointToClient(new Point(mState.X, mState.Y));
            //Console.WriteLine("MousePos: {0}, {1}", mPos.X, mPos.Y);
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
