using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    public class CellPos
    {
        // Coordinates in regards to the Go_Grid matrix
        protected Point cellCoord;

        protected int cellDim;

        public CellPos(int size)
        {
            cellCoord.X = cellCoord.Y = 2;
            cellDim = size;
        }

        public CellPos(CellPos cellToCopy, int sizeOfCell)
        {
            cellCoord.X = cellToCopy.cellCoord.X;
            cellCoord.Y = cellToCopy.cellCoord.Y;
            cellDim = sizeOfCell;
        }

        public void SetCellDim(int newDim)
        {
            cellDim = newDim;
        }

        public int GetCellDim()
        {
            return cellDim;
        }

        public Point GetCellCoord()
        {
            return cellCoord;
        }

        // Refers to a rectangle area on the grid matrix
        virtual public bool IsInsideArea(Point UpperLeftPoint, Point LowerRightPoint)
        {
            return (cellCoord.X > UpperLeftPoint.X 
                && cellCoord.X < LowerRightPoint.X
                && cellCoord.Y > UpperLeftPoint.Y 
                && cellCoord.Y < LowerRightPoint.Y);
        }

        virtual public void SetCoord(Point newCoord)
        {
            cellCoord.X = newCoord.X;
            cellCoord.Y = newCoord.Y;
        }

    }
}
