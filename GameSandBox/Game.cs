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
        int VBO, IBO;
        Vertex[] vertices;
        uint[] indices;
        List<Vertex> vList;
        List<uint> uList;
        int sCount = 0;

        int animX, animY, curFrame, prevFrame;

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
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            IBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            vList = new List<Vertex>();
            uList = new List<uint>();

            curFrame = -1;
        }

        KeyboardState prevState;
        MouseState prevMState;
        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            //KeyboardState keyState = Keyboard.GetState();

            //if (keyState.IsKeyDown(Key.Enter) && prevState.IsKeyUp(Key.Enter))
            //{
            //    Console.WriteLine("Enter!");
            //}
            //prevState = keyState;

            //Console.WriteLine("MousePos: {0}, {1}", _window.Mouse.X, _window.Mouse.Y);
            //MouseState mState = Mouse.GetCursorState();
            //Point mPos = _window.PointToClient(new Point(mState.X, mState.Y));
            //Console.WriteLine("MousePos: {0}, {1}", mPos.X, mPos.Y);
            MouseState mState = Mouse.GetCursorState();
            if (mState.IsButtonDown(MouseButton.Left) && prevMState.IsButtonUp(MouseButton.Left) && curFrame == -1)
            {
                sCount++;
                int x = _window.Mouse.X;
                int y = _window.Mouse.Y;
                Console.WriteLine("Down! {0}, {1}", x, y);

                animX = x;
                animY = y;
                curFrame = 0;

                //AddSticker(x, y);

                //Vertex[] vArray = vList.ToArray();
                //GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vArray.Length), vArray, BufferUsageHint.DynamicDraw);

                //uint[] uArray = uList.ToArray();
                //GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray.Length), uArray, BufferUsageHint.DynamicDraw);
            }
            prevMState = mState;

            if(curFrame > -1)
            {
                int animOffset;
                //if (curFrame < 10)
                //{
                //    animOffset = (10 * curFrame);
                //}
                //else
                //{
                    animOffset = 50 - (5 * (curFrame - 10));
                //}
                Vertex[] vArray = new Vertex[4] {
                    new Vertex(new Vector2(animX - 50 - animOffset, animY - 50 - animOffset), new Vector2(0, 0), Color.Red),
                    new Vertex(new Vector2(animX + 50 + animOffset, animY - 50 - animOffset), new Vector2(1, 0), Color.Yellow),
                    new Vertex(new Vector2(animX + 50 + animOffset, animY + 50 + animOffset), new Vector2(1, 1), Color.Blue),
                    new Vertex(new Vector2(animX - 50 - animOffset, animY + 50 + animOffset), new Vector2(0, 1), Color.Green),
                };
                uint uAoffset = (uint)vList.Count;
                uint[] uArray = new uint[6]{
                    0 + uAoffset, 1 + uAoffset, 2 + uAoffset,
                    0 + uAoffset, 2 + uAoffset, 3 + uAoffset
                };

                Vertex[] vArray2 = vList.ToArray().Concat(vArray).ToArray();
                GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vArray2.Length), vArray2, BufferUsageHint.DynamicDraw);

                uint[] uArray2 = uList.ToArray().Concat(uArray).ToArray();
                GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray2.Length), uArray2, BufferUsageHint.DynamicDraw);

                curFrame++;
                if (curFrame == 21)
                {
                    AddSticker(animX, animY);
                    curFrame = -1;
                }
            }
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.CornflowerBlue);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(0, _window.ClientSize.Width, _window.ClientSize.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.IndexArray);

            GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);

            if (vList.Count > 0 || curFrame > -1)
            {
                int iCount = uList.Count + (curFrame > 0 ? 6 : 0);
                GL.DrawElements(PrimitiveType.Triangles, iCount, DrawElementsType.UnsignedInt, 0);
            }

            _window.SwapBuffers();
        }

        private void AddSticker(int x, int y)
        {
            vList.Add(new Vertex(new Vector2(x - 50, y - 50), new Vector2(0, 0), Color.Red));
            vList.Add(new Vertex(new Vector2(x + 50, y - 50), new Vector2(1, 0), Color.Yellow));
            vList.Add(new Vertex(new Vector2(x + 50, y + 50), new Vector2(1, 1), Color.Blue));
            vList.Add(new Vertex(new Vector2(x - 50, y + 50), new Vector2(0, 1), Color.Green));
            int offset = vList.Count - 4;
            uList.Add((uint)(0 + offset));
            uList.Add((uint)(1 + offset));
            uList.Add((uint)(2 + offset));
            uList.Add((uint)(0 + offset));
            uList.Add((uint)(2 + offset));
            uList.Add((uint)(3 + offset));
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

        private void MultiPenguin()
        {
            //Matrix4 modelViewMatrix = Matrix4.CreateScale(.5f, .5f, 1f) * Matrix4.CreateRotationZ(0f) * Matrix4.CreateTranslation(0f, 0f, 0f);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref modelViewMatrix);
            //DrawPenguin();

            //modelViewMatrix = Matrix4.CreateScale(.6f, .4f, 1f) * Matrix4.CreateRotationZ(0f) * Matrix4.CreateTranslation(300f, 0f, 0f);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref modelViewMatrix);
            //DrawPenguin();
            //GL.LoadIdentity();

            //modelViewMatrix = Matrix4.CreateScale(.5f, .5f, 1f) * Matrix4.CreateRotationZ(0.4f) * Matrix4.CreateTranslation(200f, 250f, 0f);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref modelViewMatrix);
            //DrawPenguin();

            //modelViewMatrix = Matrix4.CreateScale(1f, 1f, 1f) * Matrix4.CreateRotationZ(0.4f) * Matrix4.CreateTranslation(0f, 0f, -0.1f);
            //GL.MatrixMode(MatrixMode.Modelview);
            //GL.LoadMatrix(ref modelViewMatrix);
            //DrawPenguin();
            //GL.BindTexture(TextureTarget.Texture2D, texture.ID);
        }
    }
}
