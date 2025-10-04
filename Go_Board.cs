using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    public class Go_Board
    {

        static private Size[] dir =
        {
                new Size(1,0),
                new Size(0,1),
                new Size(-1,0),
                new Size(0,-1)
        };

        private int[] score = { 0, 0 };
        private int[][] Go_Grid;
        private List<Go_String>[] Go_Strings;
        private List<Go_String>[] CopyOf_Go_Strings;
        private int totalSize;
        private int CELL_DIMENSION;
        private int middleOffset;
        private int endOfDrawY;
        private int endOfDrawX;

        public const int BORDER_WIDTH = 2;

        private readonly PlayerWindow_Safe parentWindow;

        public Go_Board(PlayerWindow_Safe ownerWindow, BOARD_SIZE sizeOfBoard)
        {
            parentWindow = ownerWindow;
            Go_Strings = new List<Go_String>[2];
            Go_Strings[0] = new List<Go_String>();
            Go_Strings[1] = new List<Go_String>();

            CopyOf_Go_Strings = new List<Go_String>[2];
            CopyOf_Go_Strings[0] = new List<Go_String>();
            CopyOf_Go_Strings[1] = new List<Go_String>();
            
            totalSize = 2 + (int)sizeOfBoard;

            territoryOf = new HashSet<Point>[2];
            territoryOf[0] = new HashSet<Point>((totalSize - 2) * (totalSize - 2),
                new Point_IEqualityComparer());
            territoryOf[1] = new HashSet<Point>((totalSize - 2) * (totalSize - 2),
                new Point_IEqualityComparer());


            ReadjustFields();

            Go_Grid = new int[totalSize][];

            for (int i = 0; i < totalSize; i++)
            {
                Go_Grid[i] = new int[totalSize];

                for (int j = (i > 0 && i < totalSize - 1) ? 1 : totalSize; j < totalSize - 1; j++)
                {
                    Go_Grid[i][j] = (int)CELL_VALUE.EMPTY;
                }
            }

            for (int i = 0; i < totalSize; i++)
            {
                Go_Grid[0][i] =
                Go_Grid[i][0] =
                Go_Grid[totalSize - 1][i] =
                Go_Grid[i][totalSize - 1] = (int)CELL_VALUE.BORDER;
            }

        }

        public bool CanPlaceOn(PLAYER requestingPlayer, CellPos posToPlaceOn)
        {
            if (GetValueAt(posToPlaceOn.GetCellCoord()) == (int)CELL_VALUE.EMPTY)
            {
                Go_String exStr = new Go_String(
                   parentWindow, posToPlaceOn.GetCellCoord(), requestingPlayer, false);
                if (exStr.Go_StringCaptured() || exStr.GetLibertyCount() != 0)
                    return true;
            }

            return false;
        }

        public int GetBlack_WhiteScoreDifference()
        {
            return score[(int)PLAYER.BLACK] - score[(int)PLAYER.WHITE];
        }

        public void PlaceOn(PLAYER requestingPlayer, CellPos posToPlaceOn)
        {
            Go_Strings[(int)requestingPlayer].Add(new Go_String(
                parentWindow, posToPlaceOn.GetCellCoord(), requestingPlayer, true));

            Go_String exStr = new Go_String();

            exStr.UnionizeWith(Go_Strings
                [(int)requestingPlayer][Go_Strings[(int)requestingPlayer].Count - 1]);

            CopyOf_Go_Strings[(int)requestingPlayer].Add(exStr);
        }

        public void Copy_Go_Strings()
        {
            for (int i = 0; i < 2; i++)
            {
                CopyOf_Go_Strings[i] = new List<Go_String>(Go_Strings[i].Capacity);
                for (int j = 0; j < Go_Strings[i].Count; j++)
                    CopyOf_Go_Strings[i].Add(new Go_String(Go_Strings[i][j]));
            }
        }

        public void SaveChanges(Go_String gstr, Point newStone)
        {

            // If I would like to support the creation of multiple Go_String objects whose 
            // changes won't be applied, Go_Grid would have to account for said changes
            // TBD-1 
            for (int i = 0; i < 2; i++)
            {
                Go_Strings[i].Clear();
                for (int j = 0; j < CopyOf_Go_Strings[i].Count; j++)
                {
                    Go_Strings[i].Add(
                        new Go_String(CopyOf_Go_Strings[i][j]));
                }
            }
            UpdateScore(gstr, newStone);
        }

        private HashSet<Point>[] territoryOf;
        private int[] stoneCountOf = { 0, 0 };

        public void UpdateScore(Go_String gstr, Point newStone)
        {
            bool seenTheEnemy;
            int owner = (int)gstr.GetPlayerOwner(), enemy = (int)gstr.GetOpposingPlayer();
            Point p, ex;
            Queue<Point> toBeExplored = new Queue<Point>((totalSize - 2 ) * (totalSize - 2));
            Queue<Point> exploredEmptySpaces = new Queue<Point>((totalSize - 2) * (totalSize - 2));
            HashSet<Point> cellsJustExplored = new HashSet<Point>((totalSize - 2) * (totalSize - 2),
                new Point_IEqualityComparer());

            IEnumerator<Point> enu_TerritoryRoots = gstr.GetLiberties().GetEnumerator();

            while (enu_TerritoryRoots.MoveNext())
            {
                if(!cellsJustExplored.Contains(enu_TerritoryRoots.Current))
                {
                    cellsJustExplored.Add(enu_TerritoryRoots.Current);
                    exploredEmptySpaces.Enqueue(enu_TerritoryRoots.Current);
                    toBeExplored.Enqueue(enu_TerritoryRoots.Current);
                    seenTheEnemy = false;

                    while (toBeExplored.Count > 0)
                    {
                        p = toBeExplored.Dequeue();
                        for (int i = 0; i < 4; i++)
                        {
                            ex = p + dir[i];
                            
                            if (!cellsJustExplored.Contains(ex))
                            {   
                                cellsJustExplored.Add(ex);

                                if (GetValueAt(ex) == (int)PLAYER.NONE)
                                {
                                    exploredEmptySpaces.Enqueue(ex);
                                    toBeExplored.Enqueue(ex);
                                    
                                }else 
                                    if (GetValueAt(ex) == enemy)   seenTheEnemy = true;
                            }
                        }
                    }

                    if (seenTheEnemy)
                        territoryOf[enemy].ExceptWith(exploredEmptySpaces);
                    else
                        territoryOf[owner].UnionWith(exploredEmptySpaces);
                    
                    exploredEmptySpaces.Clear();
                }
            }

            ++stoneCountOf[(int)gstr.GetPlayerOwner()];
            stoneCountOf[(int)gstr.GetOpposingPlayer()] -= gstr.GetCapturedStonesCount();
            territoryOf[(int)gstr.GetOpposingPlayer()].Remove(newStone);
            territoryOf[(int)gstr.GetPlayerOwner()].Remove(newStone);

            score[owner] = territoryOf[owner].Count + stoneCountOf[owner];
            score[enemy] = territoryOf[enemy].Count + stoneCountOf[enemy];

            parentWindow.ShowNewScore(score);
        }

        public List<Go_String> Get_ListOf_Go_String(PLAYER selectedPlayer)
        {
            return CopyOf_Go_Strings[(int)selectedPlayer];
        }

        // Is a bit redundant since I already made a method for getting a specific list,
        // but will still be used to (slightly) improve code readability.
        public Go_String Get_Go_String_Copy(PLAYER selectedPlayer, int indexOfString)
        {
            return CopyOf_Go_Strings[(int)selectedPlayer][indexOfString];
        }

        public void ReadjustFields()
        {
            middleOffset = CELL_DIMENSION / 2 - BORDER_WIDTH / 2;
            endOfDrawY = (totalSize-2) * CELL_DIMENSION;
            endOfDrawX = endOfDrawY + middleOffset;
        }

        public int Get_Go_String_Count(PLAYER chosenPlayer)
        {
            return Go_Strings[(int)chosenPlayer].Count;
        }

        public IEnumerator<Point> GetStonesEnumeratorOf(PLAYER chosenPlayer, int Go_String_Index)
        {
            return CopyOf_Go_Strings[(int)chosenPlayer][Go_String_Index].GetStonesEnumerator();
        }

        public int GetValueAt(Point coord)
        {
            return Go_Grid[coord.Y][coord.X];
        }

        public void SetValueAt(Point coord, int value)
        {
            Go_Grid[coord.Y][coord.X] = value;
        }

        public int Get_CELL_DIMENSION()
        {
            return CELL_DIMENSION;
        }

        public void Set_CELL_DIMENSION(int size)
        {
            CELL_DIMENSION = size;
        }

        public int Get_endOfDrawX()
        {
            return endOfDrawX;
        }

        public int Get_endOfDrawY()
        {
            return endOfDrawY;
        }

        public int Get_middleOffset()
        {
            return middleOffset;
        }
    }
}
