using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameState = GameSandBox.GameState;
using System.Diagnostics;

namespace GameTest
{
    [TestClass]
    public class GameState3x3Tests
    {
        [TestMethod]
        public void Test3x3SquareTaken()
        {
            GameState testState = new GameState(3, 3);
            testState.TakeSquare(1, 1, 1);
            Debug.Assert(testState.gameBoard[4] == 2);
            Debug.Assert(testState.IsSquareTaken(1, 1));
        }

        [TestMethod]
        public void Test3x3DiagWin()
        {
            GameState testState = new GameState(3, 3);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(1, 1, 0));
            //Debug.Assert(testState.TakeSquare(2, 2, 0));
        }

        [TestMethod]
        public void Test3x3HzWin()
        {
            GameState testState = new GameState(3, 3);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(1, 0, 0));
            //Debug.Assert(testState.TakeSquare(2, 0, 0));
        }

        [TestMethod]
        public void Test3x3VtWin()
        {
            GameState testState = new GameState(3, 3);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(0, 1, 0));
            //Debug.Assert(testState.TakeSquare(0, 2, 0));
        }
    }
    [TestClass]
    public class GameState5x5Tests
    {
        [TestMethod]
        public void Test5x5SquareTaken()
        {
            GameState testState = new GameState(5, 5);
            testState.TakeSquare(4, 4, 1);
            Debug.Assert(testState.gameBoard[(5 * 5) - 1] == 2);
            Debug.Assert(testState.IsSquareTaken(4, 4));
        }

        [TestMethod]
        public void Test5x5DiagWin()
        {
            GameState testState = new GameState(5, 5);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(1, 1, 0));
            //Debug.Assert(!testState.TakeSquare(2, 2, 0));
            //Debug.Assert(!testState.TakeSquare(3, 3, 0));
            //Debug.Assert(testState.TakeSquare(4, 4, 0));
        }

        [TestMethod]
        public void Test5x5HzWin()
        {
            GameState testState = new GameState(5, 5);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(1, 0, 0));
            //Debug.Assert(!testState.TakeSquare(2, 0, 0));
            //Debug.Assert(!testState.TakeSquare(3, 0, 0));
            //Debug.Assert(testState.TakeSquare(4, 0, 0));
        }

        [TestMethod]
        public void Test5x5VtWin()
        {
            GameState testState = new GameState(5, 5);
            //Debug.Assert(!testState.TakeSquare(0, 0, 0));
            //Debug.Assert(!testState.TakeSquare(0, 1, 0));
            //Debug.Assert(!testState.TakeSquare(0, 2, 0));
            //Debug.Assert(!testState.TakeSquare(0, 3, 0));
            //Debug.Assert(testState.TakeSquare(0, 4, 0));
        }
    }
}
