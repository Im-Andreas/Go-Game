using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Go_Game
{
    public enum CELL_VALUE
    {
        
        BLACK = 0,
        WHITE = 1,
        EMPTY = -1,
        BORDER = 2,
        UNDEFINED = 999
    }
    public enum BOARD_SIZE
    {
        SMALL = 9,
        MEDIUM = 13,
        HISTORICAL = 17,
        NORMAL = 19
    }
    public enum PLAYER
    {
        BLACK = CELL_VALUE.BLACK,
        WHITE = CELL_VALUE.WHITE,
        NONE = CELL_VALUE.EMPTY
    }
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenuWindow());
        }
    }
}
