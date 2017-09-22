using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSandBox
{
    class GameState
    {
        public int[] gameBoard;
        private int vQuads;
        private int hQuads;

        public GameState()
        {
            vQuads = 3;
            hQuads = 3;
            int count = vQuads * hQuads;
            gameBoard = new int[count];
            for (int index = 0; index < count; index++)
            {
                gameBoard[index] = 0;
            }
        }

        public GameState(int v, int h)
        {
            vQuads = v;
            hQuads = h;
            int count = vQuads * hQuads;
            gameBoard = new int[count];
            for (int index = 0; index < count; index++)
            {
                gameBoard[index] = 0;
            }
        }

        public bool IsSquareTaken(int x, int y)
        {
            int index = hQuads * y + x;
            return gameBoard[index] > 0;
        }

        internal bool TakeSquare(int x, int y, int curPlayer)
        {
            int index = hQuads * y + x;
            gameBoard[index] = curPlayer + 1;
            return CheckWin(x, y, curPlayer + 1);
        }

        private bool CheckWin(int x, int y, int curPlayer)
        {
            bool isWin = true;
            // Check vertical win condition
            for (int yIndex = 0; yIndex < vQuads; yIndex++)
            {
                int test = (yIndex * hQuads) + x;
                int test2 = gameBoard[(yIndex * hQuads) + x];
                if(gameBoard[(yIndex * hQuads) + x] != curPlayer)
                {
                    isWin = false;
                    break;
                }
            }
            if (isWin)
            {
                return true;
            }
            isWin = true;
            // Then, check horizontal win condition
            for (int xIndex = 0; xIndex < hQuads; xIndex++)
            {
                int test = (y * hQuads) + xIndex;
                int test2 = gameBoard[(y * hQuads) + xIndex];
                if (gameBoard[(y * hQuads) + xIndex] != curPlayer)
                {
                    isWin = false;
                    break;
                }
            }
            if (isWin)
            {
                return true;
            }
            isWin = true;
            // Finally, check diagonal win condition (if possible)
            if(hQuads == vQuads)
            {
                // If x == y, the top left to bottom right diag is possible
                if(x == y)
                {
                    for(int index = 0; index < hQuads; index++)
                    {
                        if(gameBoard[index * (hQuads + 1)] == curPlayer)
                        {
                            isWin = false;
                            break;
                        }
                    }
                    if (isWin)
                    {
                        return true;
                    }
                    isWin = true;
                }
                
                // if x + y + 1 == hQuads (or vQuads), the bottom left to top right diag is possible
                if(x + y + 1 == hQuads)
                {
                    for (int index = 1; index <= hQuads; index++)
                    {
                        if (gameBoard[index * (hQuads - 1)] == curPlayer)
                        {
                            isWin = false;
                            break;
                        }
                    }
                    if (isWin)
                    {
                        return true;
                    }
                }
            }
            return isWin;
        }
    }
}
