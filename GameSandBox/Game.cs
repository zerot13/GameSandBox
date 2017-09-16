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
        Texture2D textureX, textureO;
        int XVBO, XIBO, OVBO, OIBO, AnimVBO, AnimIBO, BackVBO, BackIBO;
        List<Vertex> xVList;
        List<uint> xUList;
        List<Vertex> oVList;
        List<uint> oUList;
        int sCount = 0;
        GameState gState;

        int hQuads, vQuads;

        int animFrames = 21;

        int animX, animY, curFrame;
        int curPlayer = 0;
        int numPlayers;

        public Game(GameWindow w)
        {
            _window = w;
            hQuads = 3;
            vQuads = 3;
            numPlayers = 2;

            _window.Load += Window_Load;
            _window.UpdateFrame += Window_UpdateFrame;
            _window.RenderFrame += Window_RenderFrame;
        }

        public Game(GameWindow w, int horizontalQuadrants, int verticalQuadrants, int numberOfPlayers)
        {
            _window = w;
            hQuads = horizontalQuadrants;
            vQuads = verticalQuadrants;
            numPlayers = numberOfPlayers;

            _window.Load += Window_Load;
            _window.UpdateFrame += Window_UpdateFrame;
            _window.RenderFrame += Window_RenderFrame;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            gState = new GameState(vQuads, hQuads);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.6f);
            textureX = ContentPipe.LoadTexture("Content/xTexture.png");
            textureO = ContentPipe.LoadTexture("Content/oTexture.png");

            XVBO = GL.GenBuffer();
            XIBO = GL.GenBuffer();

            OVBO = GL.GenBuffer();
            OIBO = GL.GenBuffer();

            AnimVBO = GL.GenBuffer();
            AnimIBO = GL.GenBuffer();

            BackVBO = GL.GenBuffer();
            BackIBO = GL.GenBuffer();

            xVList = new List<Vertex>();
            xUList = new List<uint>();
            oVList = new List<Vertex>();
            oUList = new List<uint>();

            curFrame = -1;
        }

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
                
                if (!ClickedOnGrid(x, y))
                {
                    // segW and segH are the width and height of each square on the board
                    int segW = _window.Width / hQuads;
                    int segH = _window.Height / vQuads;
                    int quadX = x / segW;
                    int quadY = y / segH;
                    if(!gState.IsSquareTaken(quadX, quadY))
                    {
                        gState.TakeSquare(quadX, quadY, curPlayer);
                        animX = (segW * quadX) + (segW / 2);
                        animY = (segH * quadY) + (segH / 2);
                        curFrame = 0;
                    }
                }
            }
            prevMState = mState;

            if (curFrame == animFrames)
            {
                if(curPlayer == 0)
                {
                    AddSticker(animX, animY, ref xVList, ref xUList);
                }
                else
                {
                    AddSticker(animX, animY, ref oVList, ref oUList);
                }
                curPlayer = (curPlayer + 1) % numPlayers;
                curFrame = -1;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, XVBO);
            Vertex[] xVArray = xVList.ToArray();
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * xVArray.Length), xVArray, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, XIBO);
            uint[] xUArray2 = xUList.ToArray();
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * xUArray2.Length), xUArray2, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, OVBO);
            Vertex[] oVArray = oVList.ToArray();
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * oVArray.Length), oVArray, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, OIBO);
            uint[] oUArray = oUList.ToArray();
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * oUArray.Length), oUArray, BufferUsageHint.DynamicDraw);

            if(curFrame > -1)
            {
                int animOffset;
                animOffset = 50 - (5 * (curFrame - ((animFrames - 1) / 2)));

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

                GL.BindBuffer(BufferTarget.ArrayBuffer, AnimVBO);
                GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * animArray.Length), animArray, BufferUsageHint.DynamicDraw);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, AnimIBO);
                GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray.Length), uArray, BufferUsageHint.DynamicDraw);

                curFrame++;
            }
        }

        private bool ClickedOnGrid(int x, int y)
        {
            int width = _window.Width;
            int height = _window.Height;
            if (x > width / hQuads - 10 && x < width / hQuads + 10)
            {
                return true;
            }
            if (x > 2 * width / hQuads - 10 && x < 2 * width / hQuads + 10)
            {
                return true;
            }
            if (y > height / vQuads - 10 && y < height / vQuads + 10)
            {
                return true;
            }
            if (y > 2 * height / vQuads - 10 && y < 2 * height / vQuads + 10)
            {
                return true;
            }
            return false;
        }

        private void DrawGrid()
        {
            int width = _window.Width;
            int height = _window.Height;

            int gridCount = (vQuads - 1) + (hQuads - 1);

            Vertex[] gridArray = new Vertex[gridCount * 4];
            uint[] uArray = new uint[gridCount * 6];

            int index = 0;
            // Store the vertical lines first
            for (; index < (vQuads - 1); index++)
            {
                int vIndex = index * 4;
                gridArray[vIndex] = new Vertex(new Vector2((index + 1) * width / hQuads - 5, 0), new Vector2(0, 0), Color.White);
                gridArray[vIndex + 1] = new Vertex(new Vector2((index + 1) * width / hQuads + 5, 0), new Vector2(1, 0), Color.White);
                gridArray[vIndex + 2] = new Vertex(new Vector2((index + 1) * width / hQuads + 5, height), new Vector2(1, 1), Color.White);
                gridArray[vIndex + 3] = new Vertex(new Vector2((index + 1) * width / hQuads - 5, height), new Vector2(0, 1), Color.White);

                uint uIndex = (uint)index * 6;
                uArray[uIndex] = (uint)vIndex;
                uArray[uIndex + 1] = (uint)vIndex + 1;
                uArray[uIndex + 2] = (uint)vIndex + 2;
                uArray[uIndex + 3] = (uint)vIndex;
                uArray[uIndex + 4] = (uint)vIndex + 2;
                uArray[uIndex + 5] = (uint)vIndex + 3;
            }
            // Then store the horizontal lines
            for (; index < gridCount; index++)
            {
                int vIndex = index * 4;
                int multiplierIndex = index - (vQuads - 1);
                gridArray[vIndex] = new Vertex(new Vector2(0, (multiplierIndex + 1) * height / vQuads - 5), new Vector2(0, 0), Color.White);
                gridArray[vIndex + 1] = new Vertex(new Vector2(0, (multiplierIndex + 1) * height / vQuads + 5), new Vector2(1, 0), Color.White);
                gridArray[vIndex + 2] = new Vertex(new Vector2(width, (multiplierIndex + 1) * height / vQuads + 5), new Vector2(1, 1), Color.White);
                gridArray[vIndex + 3] = new Vertex(new Vector2(width, (multiplierIndex + 1) * height / vQuads - 5), new Vector2(0, 1), Color.White);

                int uIndex = index * 6;
                uArray[uIndex] = (uint)vIndex;
                uArray[uIndex + 1] = (uint)vIndex + 1;
                uArray[uIndex + 2] = (uint)vIndex + 2;
                uArray[uIndex + 3] = (uint)vIndex;
                uArray[uIndex + 4] = (uint)vIndex + 2;
                uArray[uIndex + 5] = (uint)vIndex + 3;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, BackVBO);
            GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, (IntPtr)(Vertex.SizeInBytes * gridArray.Length), gridArray, BufferUsageHint.DynamicDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, BackIBO);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, (IntPtr)(sizeof(uint) * uArray.Length), uArray, BufferUsageHint.DynamicDraw);
        }

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.Gray);
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
            int iCount = ((vQuads - 1) + (hQuads - 1)) * 6;
            GL.DrawElements(PrimitiveType.Triangles, iCount, DrawElementsType.UnsignedInt, 0);
            // END TTT grid

            if (xVList.Count > 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, textureX.ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, XVBO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, XIBO);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
                GL.DrawElements(PrimitiveType.Triangles, xUList.Count, DrawElementsType.UnsignedInt, 0);
            }
            if (oVList.Count > 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, textureO.ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, OVBO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, OIBO);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
                GL.DrawElements(PrimitiveType.Triangles, oUList.Count, DrawElementsType.UnsignedInt, 0);
            }
            if(curFrame > -1)
            {
                if(curPlayer == 0)
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureX.ID);
                }
                else
                {
                    GL.BindTexture(TextureTarget.Texture2D, textureO.ID);
                }
                GL.BindBuffer(BufferTarget.ArrayBuffer, AnimVBO);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, AnimIBO);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, 0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, Vector2.SizeInBytes * 2);
                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
            }

            _window.SwapBuffers();
        }

        private void AddSticker(int x, int y, ref List<Vertex> curVList, ref List<uint> curUList)
        {
            curVList.Add(new Vertex(new Vector2(x - 50, y - 50), new Vector2(0, 0), Color.Red));
            curVList.Add(new Vertex(new Vector2(x + 50, y - 50), new Vector2(1, 0), Color.Yellow));
            curVList.Add(new Vertex(new Vector2(x + 50, y + 50), new Vector2(1, 1), Color.Blue));
            curVList.Add(new Vertex(new Vector2(x - 50, y + 50), new Vector2(0, 1), Color.Green));
            int offset = curVList.Count - 4;
            curUList.Add((uint)(0 + offset));
            curUList.Add((uint)(1 + offset));
            curUList.Add((uint)(2 + offset));
            curUList.Add((uint)(0 + offset));
            curUList.Add((uint)(2 + offset));
            curUList.Add((uint)(3 + offset));
        }
    }
}
