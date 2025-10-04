using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Go_Game
{
    // Go_String <=> Block_Go
    // (for transition purposes)
    public class Go_String
    {
        // Go String = totality of stones situated on adjacent (N, S, E, W) positions 
        // Liberties = adjacent empty spaces of a Go String
        static private int[,] dir =
        {
                { 1,0 },
                { 0,1 },
                { -1,0 },
                { 0,-1 }
        };

        // Could be set to static since we're only looking for whether
        // a instance has captured another and how many stones it caught
        // specifically at the moment of insertion.
        // Ultimately decided to keep it non-static, as the information tied to it
        // could be used in the future to expand the scope of the project
        private int Go_String_CapturedStones = 0;

        static private PlayerWindow_Safe masterWindow;

        static private int indexOfString;
        static private Point genericStone;

        private IEnumerator<Point> enu_Stone;

        /// <summary>
        /// In order to prepare the predicate for use, 
        /// modify <paramref name="genericStone"/> beforehand
        /// </summary>
        private Predicate<Go_String> GotGenericStone = HasGenericStone;

        static private bool HasGenericStone(Go_String gst)
        {
            return gst.Stones.Contains(genericStone);
        }

        private PLAYER playerOwner;
        private PLAYER opposingPlayer;

        private HashSet<Point> Stones = new HashSet<Point>(new Point_IEqualityComparer());
        private HashSet<Point> Liberties = new HashSet<Point>(new Point_IEqualityComparer());

        /// <summary>
        /// This will only allocate memory for the object. Fields will NOT be initialized.
        /// Meant for operating on Go_String objects separately from the board.
        /// </summary>
        public Go_String() { }

        /// <summary>
        /// Implicitly changes the collections of Go_String items according to 
        /// the game's logic (capturing, modifying older Go_String objects, etc.), 
        /// unless specified otherwise
        /// </summary>
        /// <param name="newStone"></param>
        /// <param name="creator"></param>
        /// <param name="applyChanges"></param>
        public Go_String(PlayerWindow_Safe windowOwner, Point newStone, 
            PLAYER creator, bool applyChanges = true)
        {
            masterWindow = windowOwner;
            playerOwner = masterWindow.GetCurrentPlayer();
            opposingPlayer = masterWindow.GetOpposingPlayer();

            Stones.Add(newStone);

            if (applyChanges)
                masterWindow.Get_Go_Grid().SetValueAt(newStone, (int)playerOwner);

            masterWindow.Get_Go_Grid().Copy_Go_Strings();

            CELL_VALUE adjacentCellType;

            for (int i = 0; i < 4; i++)
            {
                genericStone = new Point(newStone.X + dir[i, 0], newStone.Y + dir[i, 1]);
                adjacentCellType = (CELL_VALUE)masterWindow.Get_Go_Grid()
                    .GetValueAt(genericStone);

                if (adjacentCellType == CELL_VALUE.EMPTY)
                    Liberties.Add(genericStone);
                else
                {
                    if (adjacentCellType == (CELL_VALUE)playerOwner && !HasGenericStone(this))
                        ExtendGo_String(newStone);
                    else
                        if (adjacentCellType == (CELL_VALUE)opposingPlayer)
                            DeleteLiberty(newStone, applyChanges);
                }
            }

            if (applyChanges)
                masterWindow.Get_Go_Grid().SaveChanges(this, newStone);
        }

        public Go_String(Go_String gStr)
        {
            UnionizeWith(gStr);
        }

        private void DeleteLiberty(Point LibertyToDelete, bool applyChanges)
        {
            indexOfString = FetchIndexOfStringAt(opposingPlayer);

            masterWindow.Get_Go_Grid().Get_Go_String_Copy(opposingPlayer, indexOfString)
                .Liberties.Remove(LibertyToDelete);

            // checking whether the current player captured a Go_String
            if (masterWindow.Get_Go_Grid().Get_Go_String_Copy(opposingPlayer, indexOfString)
                .Liberties.Count == 0)
            {
                Go_String_CapturedStones += masterWindow.Get_Go_Grid()
                    .Get_Go_String_Copy(opposingPlayer, indexOfString).Stones.Count;

                //affected by TBD-1
                if (applyChanges)
                {
                    RemoveGo_String(indexOfString);
                }
            }
        }

        public HashSet<Point>.Enumerator GetStonesEnumerator()
        {
            return this.Stones.GetEnumerator();
        }

        private void RemoveGo_String(int indexOfDeletedString)
        {
            enu_Stone = masterWindow.Get_Go_Grid().
                GetStonesEnumeratorOf(opposingPlayer, indexOfDeletedString);
            CELL_VALUE adjacentCellType;

            while (enu_Stone.MoveNext())
            {
                // We have to cede the newly added liberties to the adjacent stones of the
                // current player, which are contained within the Stones we're about to delete
                for (int j = 0; j < 4; j++)
                {
                    genericStone = new Point(enu_Stone.Current.X + dir[j, 0],
                        enu_Stone.Current.Y + dir[j, 1]);
                    adjacentCellType = (CELL_VALUE)masterWindow.Get_Go_Grid().GetValueAt(genericStone);

                    if (Stones.Contains(genericStone))       
                        Liberties.Add(enu_Stone.Current);
                    else
                        if (adjacentCellType == (CELL_VALUE)playerOwner)
                    {
                        indexOfString = masterWindow.Get_Go_Grid().Get_ListOf_Go_String(playerOwner)
                            .FindIndex(GotGenericStone);
                        masterWindow.Get_Go_Grid().Get_Go_String_Copy(playerOwner, indexOfString)
                            .Liberties.Add(enu_Stone.Current);
                    }
                }
                masterWindow.Get_Go_Grid().SetValueAt(enu_Stone.Current, (int)CELL_VALUE.EMPTY);
                masterWindow.DrawEmptyCell(enu_Stone.Current);
            }

            // At last, we remove the opponent's captured string
            masterWindow.Get_Go_Grid().Get_ListOf_Go_String(opposingPlayer)
                .RemoveAt(indexOfDeletedString);
        }

        /// <summary>
        /// Adds the specified Stone to the adjacent Go String, regardless of owner. 
        /// Implicitly applies changes to the board unless specified.
        /// </summary>
        /// <param name="StoneToAdd"></param>
        /// <param name="applyChanges"></param>
        private void ExtendGo_String(Point StoneToAdd)
        {
            indexOfString = FetchIndexOfStringAt(playerOwner);

            Stones.UnionWith(masterWindow.Get_Go_Grid()
                .Get_Go_String_Copy(playerOwner,indexOfString).Stones);
            Liberties.UnionWith(masterWindow.Get_Go_Grid()
                .Get_Go_String_Copy(playerOwner,indexOfString).Liberties);
            Liberties.Remove(StoneToAdd);

            // Since we transferred the old Go String content to the new one
            // defined by the newStone, we will remove it
            // if(applyChange)

            masterWindow.Get_Go_Grid().Get_ListOf_Go_String(playerOwner)
                .RemoveAt(indexOfString);
        }

        private int FetchIndexOfStringAt(PLAYER selectedPlayer)
        {
            return masterWindow.Get_Go_Grid().Get_ListOf_Go_String(selectedPlayer)
                .FindIndex(GotGenericStone);
        }

        /// <summary>
        /// Returns TRUE if the caller has captured at least a Go_String instance
        /// in its most recent extension
        /// </summary>
        /// <returns></returns>
        public bool Go_StringCaptured()
        {
            return Go_String_CapturedStones > 0;
        }

        /// <summary>
        /// Returns the amount of stones that have been captured 
        /// by the instance in its most recent extension
        /// </summary>
        /// <returns></returns>
        public int GetCapturedStonesCount()
        {
            return Go_String_CapturedStones;
        }

        /// <summary>
        /// this = this U <paramref name="gStr"/>
        /// </summary>
        /// <param name="gStr"></param>
        public void UnionizeWith(Go_String gStr)
        {
            Stones.UnionWith(gStr.Stones);
            Liberties.UnionWith(gStr.Liberties);
            playerOwner = gStr.playerOwner;
            opposingPlayer = gStr.opposingPlayer;
        }

        /// <summary>
        /// Clears all non-static fields of an instance
        /// </summary>
        public void ClearContent()
        {
            Stones.Clear();
            Liberties.Clear();
            playerOwner = PLAYER.NONE;
            opposingPlayer = PLAYER.NONE;
        }

        public HashSet<Point> GetLiberties()
        {
            return Liberties;
        }

        public PLAYER GetPlayerOwner()
        {
            return playerOwner;
        }

        public PLAYER GetOpposingPlayer()
        {
            return opposingPlayer;
        }

        public int GetLibertyCount()
        {
            return Liberties.Count;
        }
    }
}
