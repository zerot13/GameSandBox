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
        int VBO, IBO, SVBO2, SIBO2, BackVBO, BackIBO;
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
            IBO = GL.GenBuffer();

            SVBO2 = GL.GenBuffer();
            SIBO2 = GL.GenBuffer();

            BackVBO = GL.GenBuffer();
            BackIBO = GL.GenBuffer();

            vList = new List<Vertex>();
            uList = new List<uint>();

            curFrame = -1;
        }

        KeyboardState prevState;
        MouseState prevMState;
        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            DrawGrid();

            MouseState mState = Mouse.GetCursorState();
            if (mState.IsButtonDown(MouseButton.Left) && prevMState.IsButtonUp(MouseButton.Left) && curFrame == -1)
            {
                sCount++;
                int x = _window.Mouse.X;
                int y = _window.Mouse.Y;
                Console.WriteLine("Down! {0}, {1}", x, y);

                animX = x;
                animY = y;
                
                if (!ClickedOnGrid(x, y))
                {
                    curFrame = 0;
                }
            }
            prevMState = mState;

            if (curFrame == 21)
            {
                AddSticker(animX, animY);
                curFrame = -1;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            Vertex[] vArray2 = vList.ToArray();
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * vArray2.Length), vArray2, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
            uint[] uArray2 = uList.ToArray();
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray2.Length), uArray2, BufferUsageHint.DynamicDraw);

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
                Vertex[] animArray = new Vertex[4] {
                    new Vertex(new Vector2(animX - 50 - animOffset, animY - 50 - animOffset), new Vector2(0, 0), Color.Red),
                    new Vertex(new Vector2(animX + 50 + animOffset, animY - 50 - animOffset), new Vector2(1, 0), Color.Yellow),
                    new Vertex(new Vector2(animX + 50 + animOffset, animY + 50 + animOffset), new Vector2(1, 1), Color.Blue),
                    new Vertex(new Vector2(animX - 50 - animOffset, animY + 50 + animOffset), new Vector2(0, 1), Color.Green),
                };
                uint[] uArray = new uint[6]{
                    0, 1, 2,
                    0, 2, 3
                };

                GL.BindBuffer(BufferTarget.ArrayBuffer, SVBO2);
                GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * animArray.Length), animArray, BufferUsageHint.DynamicDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, SIBO2);
                GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray.Length), uArray, BufferUsageHint.DynamicDraw);

                curFrame++;
            }
        }

        private bool ClickedOnGrid(int x, int y)
        {
            int width = _window.Width;
            int height = _window.Height;
            if(x > width / 3 - 5 && x < width / 3 + 5)
            {
                return true;
            }
            if (x > 2 * width / 3 - 5 && x < 2 * width / 3 + 5)
            {
                return true;
            }
            if (y > height / 3 - 5 && y < height / 3 + 5)
            {
                return true;
            }
            if (y > 2 * height / 3 - 5 && y < 2 * height / 3 + 5)
            {
                return true;
            }
            return false;
        }

        private void DrawGrid()
        {
            int width = _window.Width;
            int height = _window.Height;

            Vertex[] gridArray = new Vertex[16]
            {
                new Vertex(new Vector2(width / 3 - 5, 0), new Vector2(0, 0), Color.White),
                new Vertex(new Vector2(width / 3 + 5, 0), new Vector2(1, 0), Color.White),
                new Vertex(new Vector2(width / 3 + 5, height), new Vector2(1, 1), Color.White),
                new Vertex(new Vector2(width / 3 - 5, height), new Vector2(0, 1), Color.White),
                
                new Vertex(new Vector2(2 * width / 3 - 5, 0), new Vector2(0, 0), Color.White),
                new Vertex(new Vector2(2 * width / 3 + 5, 0), new Vector2(1, 0), Color.White),
                new Vertex(new Vector2(2 * width / 3 + 5, height), new Vector2(1, 1), Color.White),
                new Vertex(new Vector2(2 * width / 3 - 5, height), new Vector2(0, 1), Color.White),
                
                new Vertex(new Vector2(0, height / 3 - 5), new Vector2(0, 0), Color.White),
                new Vertex(new Vector2(0, height / 3 + 5), new Vector2(1, 0), Color.White),
                new Vertex(new Vector2(width, height / 3 + 5), new Vector2(1, 1), Color.White),
                new Vertex(new Vector2(width, height / 3 - 5), new Vector2(0, 1), Color.White),
                
                new Vertex(new Vector2(0, 2 * height / 3 - 5), new Vector2(0, 0), Color.White),
                new Vertex(new Vector2(0, 2 * height / 3 + 5), new Vector2(1, 0), Color.White),
                new Vertex(new Vector2(width, 2 * height / 3 + 5), new Vector2(1, 1), Color.White),
                new Vertex(new Vector2(width, 2 * height / 3 - 5), new Vector2(0, 1), Color.White),
            };
            uint[] uArray = new uint[24]{
                0, 1, 2,
                0, 2, 3,
                4, 5, 6,
                4, 6, 7,
                8, 9, 10,
                8, 10, 11,
                12, 13, 14,
                12, 14, 15
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, BackVBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * gridArray.Length), gridArray, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BackIBO);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray.Length), uArray, BufferUsageHint.DynamicDraw);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(0, _window.ClientSize.Width, _window.ClientSize.Height, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.TextureCoordArray);
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.IndexArray);

            // Draw TTT grid
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, BackVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BackIBO);
            GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
            GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
            GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
            //int iCount = uList.Count;
            GL.DrawElements(PrimitiveType.Triangles, 24, DrawElementsType.UnsignedInt, 0);
            // END TTT grid

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            if (vList.Count > 0)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, IBO);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
                int iCount = uList.Count;
                GL.DrawElements(PrimitiveType.Triangles, iCount, DrawElementsType.UnsignedInt, 0);
            }
            if(curFrame > -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, SVBO2);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, SIBO2);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
                int iCount = 6;
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
    }
}
