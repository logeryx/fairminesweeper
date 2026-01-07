using System;
using System.Windows.Forms;

namespace FairMinesweeper
{
    internal static class Program
    {
        private struct Options
        {
            public Options(bool clickedStart, int width, int height, int minesCount, 
                           bool guessNotification, bool punishMode, bool debugMode) 
            {
                ClickedStart = clickedStart; Width = width; Height = height; MinesCount = minesCount;
                GuessNotification = guessNotification; PunishMode = punishMode; DebugMode = debugMode;
            }
            public bool ClickedStart;
            public int Width, Height, MinesCount;
            public bool GuessNotification, PunishMode, DebugMode;
        }
        /// <summary>
        /// Opens the main menu.
        /// Returned options are used as a parameter to Launch the Game.
        /// </summary>
        /// <returns>Structure holding all the chosen options.</returns>
        static Options LaunchMenu()
        {
            MainMenu mainMenu = new MainMenu();
            Application.Run(mainMenu);
            Options options = new Options(mainMenu.ClickedStart, mainMenu.GridWidth, mainMenu.GridHeight, mainMenu.MinesCount,
                                          mainMenu.NotifyOfGuesses, mainMenu.PunishGuesses, mainMenu.LeaveMinesVisible);
            mainMenu.Dispose();
            return options;
        }
        /// <summary>
        /// Create and open a new game window.
        /// Reenables EventHandlers
        /// </summary>
        static void LaunchGame(Options options)
        {
            if (!options.ClickedStart) { return; }

            EventHandlers.Enable();
            Game game = new Game(options.Width, options.Height, options.MinesCount, 
                                 options.GuessNotification, options.PunishMode, options.DebugMode);
            Application.Run(game);
            game.Dispose();
        }

        static bool game = true, menu = false; // are read by the Main method to launch the game in a loop as long as the player wants to

        public static void Restart()
        {
            game = true;
        }

        public static void GoToMenu()
        {
            menu = true;
        }

        [STAThread]
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             
            Options options = LaunchMenu();

            while ((game || menu) /*&& !(game && menu) "code does not create this scenario"*/) 
            // repeatedly launches game while player wishes to restart or go to menu
            {
                if (menu)
                {   
                    menu = false;
                    options = LaunchMenu();
                    LaunchGame(options);
                }
                else if (game) 
                {
                    game = false;
                    LaunchGame(options); // use the same options
                }
            }
        }
    }
}