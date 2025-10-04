using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    /// <summary>
    /// Proxy class made solely to prevent any (unwanted) modifications to
    /// the main window's components from outside its own scope.
    /// </summary>
    public class PlayerWindow_Safe
    {
        private readonly PlayerWindow wrappedWindow;

        public PlayerWindow_Safe(PlayerWindow windowToWrap)
        {
            wrappedWindow = windowToWrap;
        }

        public void DrawEmptyCell(Point drawCoord)
        {
            wrappedWindow.DrawEmptyCell(drawCoord);
        }

        public Go_Board Get_Go_Grid()
        {
            return wrappedWindow.Get_Go_Grid();
        }

        public PLAYER GetCurrentPlayer()
        {
            return wrappedWindow.GetCurrentPlayer();
        }

        public PLAYER GetOpposingPlayer()
        {
            return wrappedWindow.GetOpposingPlayer();
        }

        public void ShowNewScore(int[] score)
        {
            wrappedWindow.ShowNewScore(score);
        }
    }
}
