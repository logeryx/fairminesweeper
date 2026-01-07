using System;
using System.Collections.Generic;
using System.Linq;

namespace FairMinesweeper
{
    class Grid
    {
        Game game;
        Tile[,] grid;

        bool guessNotification, punishMode, debugMode;

        public Queue<Tile> BFSqueue; // this queue is used in certainty logic by all tiles.

        int Xsize, Ysize;
        int minesCount; // number of mines in total
        static int[,] proximity = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

        // used for mine counter and win-state detection
        int flaggedTiles = 0; // number of flagged tiles
        int flaggedMines = 0; // number of flagged Mines

        /// <summary>
        /// Creates a barebone instance of a Tile, initiate using Init()
        /// </summary>
        /// <param name="minesCount">Total desired number of mines. This number can change during the game, but will be kept approximately</param>
        /// <param name="game">Pointer to the game class instance to which this grid belongs.</param>
        public Grid(int Xsize, int Ysize, int minesCount, Game game, bool guessNotification, bool punishMode, bool debugMode)
        {
            this.Xsize = Xsize;
            this.Ysize = Ysize;
            this.minesCount = minesCount;
            grid = new Tile[Xsize, Ysize];
            this.game = game;
            BFSqueue = new Queue<Tile>();
            this.guessNotification = guessNotification;
            this.punishMode = punishMode;
            this.debugMode = debugMode;
        }
        /// <summary>
        /// Construct all tiles in grid, 
        /// randomly plant mines.
        /// Call Init() on all tiles.
        /// </summary>
        public void Init()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    grid[x, y] = new Tile(false, x, y, this);
                }
            }
            PlantMines();

            // has to be done AFTER PlantMines()
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    grid[x, y].Init(GetNeighbours(x, y));
                }
            }
            // debugMode
            if (debugMode) ShowAllMines();
        }
        /// <summary>
        /// Randomly plant `minesCount` number of mines
        /// </summary>
        void PlantMines()
        {
            Random rng = new Random();
            int randX, randY, x = 0;
            // theoretically endless loop
            while (x < minesCount) // however even in extreme cases (mineCount = number of tiles - i.e. all tiles are mines)
                                   // the loop ends almost instantly
            {
                randX = rng.Next(Xsize);
                randY = rng.Next(Ysize);
                if (!grid[randX, randY].Mine) // not already a mine
                {
                    grid[randX, randY].Mine = true;
                    x++;
                }
            }
        }
        /// <summary>
        /// Returns a list of up to 8 neighbours.
        /// around tile with coordinates [x,y]
        /// Uses static variable proximity to get neighbour offsets
        /// </summary>
        public List<Tile> GetNeighbours(int x, int y)
        {
            List<Tile> list = new List<Tile>();

            for (int i = 0; i < 8; i++)
            {
                int dx = proximity[i, 0];
                int dy = proximity[i, 1];
                if (InBounds(x + dx, y + dy))
                {
                    list.Add(grid[x + dx, y + dy]);
                }
            }
            return list;
        }
        /// <summary>
        /// Return true only if the indexes x,y are in the range of the grid array.
        /// </summary>
        public bool InBounds(int x, int y)
        {
            return (x > -1 &&
                     y > -1 &&
                     x < Xsize &&
                     y < Ysize);
        }
        /// <summary>
        /// Show locations of leftover undiscovered bombs.
        /// </summary>
        void ShowAllMines()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    Tile tile = grid[x, y];
                    if (tile.Mine && !tile.Flagged && !tile.Uncovered) // "hidden" bombs
                    {
                        grid[x, y].ShowMine();
                    }
                }
            }
        }
        /// <summary>
        /// Hide locations of leftover undiscovered bombs.
        /// </summary>
        void HideAllMines()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    Tile tile = grid[x, y];
                    if (!tile.Uncovered) 
                    {
                        grid[x, y].Cover();
                    }
                }
            }
        }
        /// <summary>
        /// Reveals all numbers on non-mine tiles. 
        /// Is called at the end of the game as a quality of life feature.
        /// </summary>
        void ShowAllNumbers()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    Tile tile = grid[x, y];
                    if (!tile.Uncovered && !tile.Mine && !tile.Flagged) // "hidden" numbers
                    {
                        grid[x, y].ShowNumber();
                    }
                }
            }
        }
        /// <summary>
        /// Disable gameplay, show leftover mines, call GameOver function on Game to show dialog window.
        /// </summary>
        public void GameOver(bool win)
        {
            // DISABLES CLICKING ALLTOGETHER
            EventHandlers.Disable();
            ShowAllMines();
            game.GameOver(win);
        }
        /// <summary>
        /// If the player uncovered or flagged every tile correctly, then
        /// reveals remaining non-bomb tiles, ends the game.
        /// </summary>
        public void CheckForWin()
        {
            if ((flaggedMines == minesCount) && (flaggedTiles == minesCount))
            // all mines are flagged and there are no flags without a mine beneath
            {
                ShowAllNumbers();
                GameOver(true);
            }
        }
        /// <summary>
        /// An edge tile is defined as a covered neighbour to an uncovered tile.
        /// </summary>
        /// <returns>A List of all edge tiles.</returns>
        public List<Tile> GetEdgeTiles()
        {
            HashSet<Tile> edges = new HashSet<Tile>(); // Set ensures no repetition

            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    if (!grid[x, y].Uncovered)
                    {
                        continue; // skip covered tiles, we want neighbours of Uncovered
                    }
                    // else (Uncovered tiles)
                    for (int i = 0; i < 8; i++)
                    {
                        int dx = proximity[i, 0];
                        int dy = proximity[i, 1];
                        if (InBounds(x + dx, y + dy) && !grid[x + dx, y + dy].Uncovered)
                        {
                            edges.Add(grid[x + dx, y + dy]);
                        }
                    }
                }
            }
            return edges.ToList<Tile>();
        }
        /// <summary>
        /// Rearranges mines in the edge tiles so that the targetTile ends up how we want it. (mine or safe)
        /// </summary>
        /// <param name="edges">List of all Edge Tiles</param>
        /// <param name="targetTile">This tile's mine atribute will end up true/false based on targetEndsUpMine</param>
        /// <param name="targetEndsUpMine">Should target tile end up mine or safe?</param>
        public void RearrangeEdgeBombs(List<Tile> edges, Tile targetTile, bool targetEndsUpMine = false)
        {
            if (targetTile.Mine == targetEndsUpMine) // already done
            {
                Console.WriteLine("INFO: No changes needed, already wasn't a mine.");
                return;
            }
            bool TryAllStatesFromIndex(int index)
            {
                if (index == edges.Count) // we've gone through all edge tile -> done
                {
                    return true;
                }

                Tile tile = edges[index];

                if (! tile.TrueBlank) // state of this tile has been previously enforces, do not change it.
                {                     // this always includes targetTile btw 
                    if ( TryAllStatesFromIndex(index + 1) ) return true;
                    else return false;
                }

                tile.MarkHypoFlagged();
               
                if ( ! tile.CheckNeighboursForIllegalities() // check if this state is legal so far, if not go straight to "try different state"
                  || ! TryAllStatesFromIndex(index + 1) )             // OR '||' continued here => legal -> try to fill in the rest, if it fails...
                {
                    // try different state:
                    tile.MarkHypoUncovered();
                    if ( ! tile.CheckNeighboursForIllegalities() // if even second option is illegal, backtrack and return false
                      || ! TryAllStatesFromIndex(index + 1) )             // bubble up backtrack and returning of false
                    {
                        tile.ResetHypothetical(); // backtrack
                        return false;
                    }
                    else
                    {
                        return true; // second option was legal and TryAllStatesFromIndex found a suitable layout -> bubble up true
                    }
                }
                else return true; // this tile's surrounding is legal and the rest was filled in successfully.
            }
            Console.WriteLine("INFO: Player would click on a mine. Finding new suitable layout...");
            // use existing code to optimise (fill in parts that become certain with the change to target tile)
            targetTile.Certain(!targetEndsUpMine, false);

            TryAllStatesFromIndex(0); // Find suitable mine layout with targetTile and its surrounding enforced by line {this - 2}

            ApplyHypothetical();
            ResetHypothetical();
            CountSurroundingMines();
            
            if (debugMode)
            {
                HideAllMines();
                ShowAllMines();
            }
        }
        /// <summary>
        /// Resets all hypothetical information to its real state.
        /// </summary>
        public void ResetHypothetical()
        {
            BFSqueue.Clear();
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    grid[x, y].ResetHypothetical();
                }
            }
        }
        /// <summary>
        /// Uses hypothetical information to change positions of mines to be aligned to hypothetically flagged tiles.
        /// </summary>
        void ApplyHypothetical()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    grid[x, y].ApplyHypothetical();
                }
            }
        }
        /// <summary>
        /// Forces all tiles to recount the number of their surrounding mines.
        /// </summary>
        void CountSurroundingMines()
        {
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    if (grid[x, y].Uncovered) continue; // no need to recount it for uncovered tiles.

                    grid[x, y].CountSurroundingMines();
                }
            }
        }
        /// <summary>
        /// If guess notification is enabled, bubble up guess detected call to the game class.
        /// </summary>
        public bool GuessDetected()
        {
            if (guessNotification) // option in Main Menu
                return game.OpenDialog_GuessDetected();

            else return true;
        }
        /// <summary>
        /// Searches the grid for Certain tiles.
        /// </summary>
        /// <returns>True if at least one certain tile is found, false when none are found.</returns>
        public bool SearchForCertainOption()
        {
            // Does player have a different (certain) option?
            for (int x = 0; x < Xsize; x++)
            {
                for (int y = 0; y < Ysize; y++)
                {
                    if (!grid[x, y].Uncovered && (grid[x, y].Certain() || grid[x, y].Certain(true)) )
                    {
                        return true;
                    }
                }
            }
            return false; 
        }
        /// <summary>
        /// Increments the counter of flagged TILES, counter of flagged MINES if it was correctlyMine.
        /// Also calls win-state checking method.
        /// </summary>
        /// <param name="correctlyMine">Currently flagged tiles is actually a mine.</param>
        public void IncrementFlagged(bool correctlyMine)
        {
            flaggedTiles++;
            if (correctlyMine)
            {
                flaggedMines++;
            }
            CheckForWin();
        }
        /// <summary>
        /// Decrements the counter of flagged TILES, counter of flagged MINES if it wasOnMine.
        /// Also calls win-state checking method.
        /// </summary>
        /// <param name="wasOnMine">Flag was removed from an actual mine.</param>
        public void DecrementFlagged(bool wasOnMine)
        {
            flaggedTiles--;
            if (wasOnMine)
            {
                flaggedMines--;
            }
            CheckForWin();
        }
        /// <summary>
        /// Change the store count of total mines.
        /// This number is used in win-state detection.
        /// </summary>
        /// <param name="byHowMuch"></param>
        public void NumberOfMinesChanged(int byHowMuch)
        {
            minesCount += byHowMuch;
        }
        public Tile[,] TileGrid
        {
            get { return grid; } // the game needs this pointer the add all the buttons to the WinForms Controls
        }
        public bool PunishMode
        {
            get { return punishMode; }
        }
    }
}