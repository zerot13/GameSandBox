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

        public GameState()
        {
            gameBoard = new int[9];
            for(int index = 0; index < 9; index++)
            {
                gameBoard[index] = 0;
            }
        }

        public bool IsSquareTaken(int x, int y)
        {
            int index = 3 * y + x;
            return gameBoard[index] > 0;
        }

        internal bool TakeSquare(int x, int y, bool curPlayer)
        {
            int index = 3 * y + x;
            gameBoard[index] = curPlayer ? 2 : 1;
            return CheckWin();
        }

        private bool CheckWin()
        {
            return false;
        }
    }
}
