using System.Drawing;
using System.Windows.Forms;
using FairMinesweeper.Properties;

namespace FairMinesweeper
{
    public partial class Game : Form
    {
        const int buttonSize = 32, // matches image size
                  unknownPadding = 21, // unexplainable constant padding needed for exact alignment
                  panelOffset = 23; // window size (only height) includes the top panel

        Grid grid; // stores pointers to all tiles, which store relevant information
        
        public Game(int gridWidth, int gridHeight, int minesCount, bool guessNotification, bool punishMode, bool debugMode)
        {
            grid = new Grid(gridWidth, gridHeight, minesCount, this, guessNotification, punishMode, debugMode);
            grid.Init();
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Controls.Add(grid.TileGrid[x, y].Button); // add buttons to the form
                }
            }
            this.Size = new Size(gridWidth * buttonSize + unknownPadding, gridHeight * buttonSize + panelOffset + unknownPadding); // resize
            this.Icon = Resources.minesweeper_icon;
            this.Text = "Fair Minesweeper - Game";
            this.BackColor = Color.Gray; // used for small outline at the right and bottom
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // window is not resizable
            this.MaximizeBox = false; // window cannot be maximised
        }
        /// <summary>
        /// Calls funcion to open a dialog window with options to Restart or Go to menu.
        /// If this window is closed the game window remains open (for screenshots etc...)
        /// Otherwise it closes.
        /// </summary>
        /// <param name="win">Did the player win or loose. True = won; False = lost</param>
        public void GameOver(bool win)
        {
            bool closeGame = OpenDialog_GameOver(win);
            if (closeGame) { this.Close(); }
        }
        /// <summary>
        /// Opens dialog window with options to Restart or Go to menu.
        /// Calls funcions in class Program to enfoce players decision. (Program knows whether to launch another game, the menu, or neither...)
        /// </summary>
        /// <param name="win">Did the player win?</param>
        /// <returns>True if the game window should close with this dialog.</returns>
        bool OpenDialog_GameOver(bool win)
        {
            bool close = false;
            GameOver gameOver = new GameOver(win);
            this.Update(); // ugly black outline on one of the buttons fix
            gameOver.ShowDialog(); 

            if (gameOver.ClickedRestart)
            {
                close = true;
                Program.Restart();
            }
            else if (gameOver.ClickedMenu)
            {   
                close = true;
                Program.GoToMenu();
            }
            return close;
        }
        /// <summary>
        /// Opens the Guess detected dialog and returns players choice to continue with this play.
        /// </summary>
        public bool OpenDialog_GuessDetected()
        {
            GuessDetected guessDetected = new GuessDetected();
            this.Update(); // ugly black outline on one of the buttons fix
            guessDetected.ShowDialog();

            return guessDetected.Continue;
        }
    }
}
