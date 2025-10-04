using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    public class CellPos_ExtraPrecision : CellPos
    {
        // Screen coordinates relative to the panel in which the Go board is drawn
        private Point posCoord;

        public CellPos_ExtraPrecision(int size)
            : base(size)
        {
            posCoord.X = posCoord.Y = cellCoord.X * cellDim;
        }

        public CellPos_ExtraPrecision(CellPos_ExtraPrecision cellToCopy, int sizeOfCell)
            : base(cellToCopy, sizeOfCell)
        {
            posCoord.X = cellToCopy.posCoord.X * sizeOfCell;
            posCoord.Y = cellToCopy.posCoord.Y * sizeOfCell;
        }

        public Point GetPosCoord()
        {
            return posCoord;
        }

        // Refers to a rectangle area in screen coordinates
        override public bool IsInsideArea(Point UpperLeftPoint, Point LowerRightPoint)
        {   
            return (posCoord.X > UpperLeftPoint.X 
                && posCoord.X + cellDim < LowerRightPoint.X
                && posCoord.Y > UpperLeftPoint.Y 
                && posCoord.Y + cellDim < LowerRightPoint.Y);
        }

        override public void SetCoord(Point newCoord)
        {
            posCoord.X = newCoord.X;
            posCoord.Y = newCoord.Y;

            cellCoord.X = posCoord.X / cellDim;
            cellCoord.Y = posCoord.Y / cellDim;
        }
    }
}
