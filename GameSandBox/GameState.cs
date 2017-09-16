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
            return CheckWin();
        }

        private bool CheckWin()
        {
            return false;
        }
    }
}
