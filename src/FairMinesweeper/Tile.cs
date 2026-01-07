using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FairMinesweeper.Properties;

namespace FairMinesweeper
{   
    /// <summary>
    /// PictureBox from WinForms expanded with a pointer to its tile
    /// </summary>
    class Button1 : PictureBox
    {
        Tile tile;
        public Button1(Tile tile) : base()
        {
            this.tile = tile;
        }
        public Tile Tile
        {
            get { return this.tile; }
        }
    }
    class Tile 
    {
        Grid grid;
        Button1 button;
        List<Tile> neighbours;
        
        public const int buttonSize = 32;

        bool mine; // is this tile a mine?
        bool uncovered = false;
        bool flagged = false;
        int X, Y; // position in grid [X,Y]
        int surroundingMines = 0; // how many mines surround this tile, irrelevant if it's a mine

        // used in CERTAINTY LOGIC:
        bool hypotheticallyFlagged = false;
        bool hypotheticallyUncovered = false;
        // actually uncovered/flagged tiles are automaticly also hypothetically that.

        // NEIGHBOUR atributes
        int remainingCoveredNeighbours = 8;
        int remainingMinesToBeFlagged; 
        // always need to be updated by UpdateNeighbourAtributes() before reading

        // CERTAINTY LOGIC atributes (aka Solvable atributes):
        bool Illegal { get { return (remainingCoveredNeighbours < remainingMinesToBeFlagged) // not enough space for needed mines
                                 || (remainingMinesToBeFlagged < 0); } }                     // or already too much flagged mines
        bool Completed { get { return remainingCoveredNeighbours == 0; } } // there are no remaining covered tiles
        bool SatisfiedUnfinished { get { return (remainingMinesToBeFlagged <= 0) && !Completed; } } // all surrounding empty tiles can be uncovered safely
        bool OnlyMinesLeft { get { return remainingMinesToBeFlagged == remainingCoveredNeighbours; } } // all remaining covered tiles are mines
        // always need to be updated by UpdateNeighbourAtributes() before reading

        /// <summary>
        /// Creates a barebone instance of a Tile, initiate using Init()
        /// </summary>
        /// <param name="mine">Is this tile a mine?</param>
        /// <param name="x">X coordinate (column) in the grid</param>
        /// <param name="y">Y coordinate (row) in the grid</param>
        /// <param name="grid">Pointer to its grid.</param>
        public Tile(bool mine, int x, int y, Grid grid)
        {
            this.mine = mine;
            X = x;
            Y = y;
            this.grid = grid;
            button = new Button1(this);
        }
        
        /// <summary>
        /// Count surrounding mines, attach a list of pointers to neighbouring tiles.
        /// Fill desired WinForms properties, attach event handlers and so on...
        /// </summary>
        /// <param name="neighbours">Complete list of all neighbour tiles. (all (up to) 8 orthogonal and diagonal)</param>
        public void Init(List<Tile> neighbours)
        {
            // tile atributes
            this.neighbours = neighbours;
            CountSurroundingMines();
            //remainingMinesToBeFlagged = surroundingMines;

            // general
            button.Name = $"[{X},{Y}] button";
            button.Location = new Point(X * buttonSize, Y * buttonSize);
            button.Size = new Size(buttonSize, buttonSize);
            button.Image = Resources.blank_tile;

            // click event handlers
            button.MouseDown += new MouseEventHandler(EventHandlers.OnPress);
            button.MouseUp += new MouseEventHandler(EventHandlers.OnRelease);
            button.MouseClick += new MouseEventHandler(EventHandlers.OnClick);
        }
        /// <summary>
        /// Count how many mines surround this tile and store that number in
        /// the surroundingMines atribute.
        /// </summary>
        public void CountSurroundingMines()
        {
            int count = 0;
            foreach (Tile neighbour in neighbours)
            {
                if (neighbour.Mine)
                {
                    count++;
                }
            }
            surroundingMines = count;
        }
        
        #region CERTAINTY LOGIC:
        /// <summary>
        /// Uses information about surrounding tiles (including hypothetical) to determine 
        /// how many empty spaces are left and how many remaining bombs there are to be flagged
        /// among this tile's neighbours.
        /// Modifies (updates) remainingCoveredNeighbours and remainingMinesToBeFlagged.
        /// </summary>
        void UpdateNeighbourAtributes()
        {
            // counters:
            int flaggedMines = 0, emptySpaces = 0;

            foreach (Tile neighbour in neighbours) 
            {
                if (neighbour.hypotheticallyFlagged)
                {
                    flaggedMines++;
                }
                else if (!neighbour.hypotheticallyUncovered)
                {
                    emptySpaces++;
                }
            }
            // update NEIGHBOUR atributes
            remainingCoveredNeighbours = emptySpaces;
            remainingMinesToBeFlagged = surroundingMines - flaggedMines;
        }
        /// <summary>
        /// If the surrounding of this tile can be determined, do so (hypothetically)
        /// and for each flagged or uncovered tile enqueue them for a check of their surrounding.
        /// </summary>
        /// <returns>
        /// False if it runs into a contradiction. (Illegal position)
        /// </returns>
        bool HypotheticallySolve()
        {
            UpdateNeighbourAtributes();
            if (!uncovered) return true; // cannot solve for covered tiles
            // HERE /\ it's only UNCOVERED, because we cant look for contradictions based on information hidden to the player. !!!
            if (Illegal) return false; // contradiction found

            foreach (Tile neighbour in neighbours)
            {
                if (!neighbour.hypotheticallyUncovered
                 && !neighbour.hypotheticallyFlagged  )
                {
                    // based of Neighbour Atributes determine how to fill in all blank tiles.
                    if (SatisfiedUnfinished)
                    {
                        neighbour.hypotheticallyUncovered = true;
                    }
                    else if (OnlyMinesLeft)
                    {
                        neighbour.hypotheticallyFlagged = true;
                    }
                    else return true; // not solvable yet
                    
                    grid.BFSqueue.Enqueue(neighbour);
                }
            }
            return true;
        }
        /// <summary>
        /// Checks surrounding area for solvable tiles and contradictions.
        /// (That is executed by a call of their HypotheticallySolve method)
        /// Hypothetically solves all solvable tiles.
        /// </summary>
        /// <returns>
        /// False if it runs into a contradiction (meaning this tile cannot be a mine/uncovered).
        /// True otherwise.
        /// </returns>
        bool TryToSolveSurrounding()
        {
            bool result = true;

            foreach (Tile neighbour in neighbours)
            {
                result = neighbour.HypotheticallySolve(); // once false, stays false, for loop ends
                if (!result) break;
            }
            return result;
        }
        /// <summary>
        /// Determine if the player can be certain, that this tile is NOT a bomb. 
        /// (or that it certainly IS a bomb when invertFunctionToCertainMine is true)
        /// If yes, return true, false otherwise.
        /// </summary>
        public bool Certain(bool invertFunctionToCertainMine = false, bool resetHypotheticalInfo = true)
        {
            // first hypothetical change:
            if (invertFunctionToCertainMine) 
                hypotheticallyUncovered = true; 
            else
                hypotheticallyFlagged = true; 

            grid.BFSqueue.Enqueue(this);

            bool result = true;

            while ( result && grid.BFSqueue.Count > 0) 
            {
                result = grid.BFSqueue.Dequeue().TryToSolveSurrounding(); // while loop ends on false
            }

            if (resetHypotheticalInfo) grid.ResetHypothetical();
            return !result;
        }
        #endregion

        #region MINE SWITCHING:
        /// <summary>
        /// Check if this tile could hypothetically be both mine and not-mine. That means player must be guessing.
        /// Search the grid for a certain option.
        /// If there is none call the RearrangeEdgeBombs function to make sure player does not hit a mine.
        /// If there are certain options notify player of that fact.
        /// </summary>
        /// <returns>
        /// True if game should continue to uncover this tile, 
        /// false if game should withdraw.
        /// </returns>
        bool CheckForGuessing()
        {
            if (!Certain(false) && !Certain(true)) // player clicked a tile that could hypothetically both be and not be a mine
            {
                Console.WriteLine("Guess detected!");

                if (grid.SearchForCertainOption())
                {
                    if (!grid.GuessDetected()) return false; // withdraw before any mines are changed
                    if (grid.PunishMode)
                    {
                        grid.RearrangeEdgeBombs(grid.GetEdgeTiles(), this, true); 
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("INFO: Rearranging mines in player's benefit.");
                    grid.RearrangeEdgeBombs(grid.GetEdgeTiles(), this);
                }
            }
            return true;
        }
        /// <summary>
        /// Checks if all neighbours are in a legal position.
        /// Also considers hypothetical information.
        /// </summary>
        /// <returns>True if everything is alright,
        /// false, if it finds an Illegal tile.</returns>
        public bool CheckNeighboursForIllegalities()
        {
            foreach (Tile neighbour in neighbours)
            {
                if (!neighbour.Uncovered) continue; // covered tiles do not concern us with their needs

                neighbour.UpdateNeighbourAtributes();
                if (neighbour.Illegal) return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Reveal contents of this tile in the form. 
        /// Checks if player was guessing (see CheckForGuessing() )
        /// If this has 0 surrounding mines,
        /// also uncover all of those.
        /// if this tile is a mine, end the game.
        /// </summary>
        /// <param name="playerClick"> True if player did the clicking, false when called automaticly. (Auto-uncover of zero tiles)</param>
        public void Uncover(bool playerClick)
        {   
            if ( uncovered ) { return; }
            
            // else:
            if ( playerClick ) // is skipped when Uncover() is called automatically around zero-tiles
            {
                if (! CheckForGuessing()) // player clicked a tile that could hypothetically both be and not be a mine
                {
                    button.Image = Resources.blank_tile;
                    return;
                }
            }
            
            uncovered = hypotheticallyUncovered = true;

            if (mine) 
            { 
                button.Image = Resources.oops;
                grid.GameOver(false);
                return;
            }
            // else:
            ShowNumber();
        }
        /// <summary>
        /// Reveals number image corresponding to the nubmer of surrounding mines.
        /// If it's zero, automaticaly uncovers surrounding tiles.
        /// </summary>
        public void ShowNumber()
        {
            switch (surroundingMines)
            {
                case 0:
                    button.Image = Resources._0;
                    foreach (Tile neighbour in neighbours)
                    {
                        neighbour.Uncover(false); // when 0, automaticly uncover all neighbours
                    }
                    break;
                case 1:
                    button.Image = Resources._1; break;
                case 2:
                    button.Image = Resources._2; break;
                case 3:
                    button.Image = Resources._3; break;
                case 4:
                    button.Image = Resources._4; break;
                case 5:
                    button.Image = Resources._5; break;
                case 6:
                    button.Image = Resources._6; break;
                case 7:
                    button.Image = Resources._7; break;
                case 8:
                    button.Image = Resources._8; break;
            }
        }
        /// <summary>
        /// Changes button image to a non-red bomb image.
        /// </summary>
        public void ShowMine()
        {
            button.Image = Resources.mine;
        }
        /// <summary>
        /// Changes button image to blank covered tile.
        /// </summary>
        public void Cover()
        {
            button.Image = Resources.blank_tile;
        }
        /// <summary>
        /// Places a flag on (or removes existing from) the tile (different image), 
        /// notes that in the bool flagged and calls mine-counting methods in grid.
        /// </summary>
        public void ToggleFlagged()
        {
            if (flagged)
            {
                flagged = false;
                button.Image = Resources.blank_tile;
                grid.DecrementFlagged(mine);
            }
            else
            {
                flagged = true;
                button.Image = Resources.flagged;
                grid.IncrementFlagged(mine);
            }
            // in every case:
            hypotheticallyFlagged = flagged;
        }
        /// <summary>
        /// Notes this tile as a hypotheticallyFlagged.
        /// </summary>
        public void MarkHypoFlagged()
        {
            hypotheticallyFlagged = true;
            hypotheticallyUncovered = false;
        }
        /// <summary>
        /// Notes this tile as a hypotheticallyUncovered.
        /// </summary>
        public void MarkHypoUncovered()
        {
            hypotheticallyUncovered = true;
            hypotheticallyFlagged = false;
        }
        /// <summary>
        /// Align this tiles hypothetical atributes to match the real.
        /// This means blank tiles loose their hypothetical info.
        /// </summary>
        public void ResetHypothetical()
        {
            hypotheticallyFlagged = flagged;
            hypotheticallyUncovered = uncovered;
        }
        /// <summary>
        /// Change this tile's mine atribute to match its hypothetical state.
        /// HypoFlagged -> mine
        /// HypoUncovered -> not-mine
        /// Also notes changes in the total number of mines in the grid.
        /// </summary>
        public void ApplyHypothetical()
        {
            if (uncovered || flagged) return;

            // switch (align) the mine atribute state based of hypothetical data:
            if (hypotheticallyFlagged)
            {
                if (!mine) grid.NumberOfMinesChanged(+1);
                mine = true;
            }
            else if (hypotheticallyUncovered)
            {
                if (mine) grid.NumberOfMinesChanged(-1);
                mine = false;
            }
        }
       
        // setters/getters:
        public Button1 Button
        {
            get { return button; } // needed to add button to form controls
        }
        public bool TrueBlank
        {
            get { return (!hypotheticallyFlagged && !hypotheticallyUncovered); }
        }
        public bool Uncovered
        {
            get { return uncovered; }
            //set { uncovered = hypotheticallyUncovered = value; }
        }
        public bool Flagged
        {
            get { return flagged; }
            //set { flagged = hypotheticallyFlagged = value; }
        }
        public bool Mine
        {
            get { return mine; }
            set { mine = value; }
        }
    }
}
