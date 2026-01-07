using System;
using System.Windows.Forms;
using FairMinesweeper.Properties;

namespace FairMinesweeper
{
    public partial class GameOver : Form
    {
        bool restart = false, menu = false;
        /// <summary>
        /// Opens the gameover dialog.
        /// </summary>
        /// <param name="win">True if player won the game.</param>
        public GameOver(bool win)
        {
            InitializeComponent();
            this.Icon = Resources.minesweeper_icon;
            this.MaximizeBox = false; // window cannot be maximised
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // window is not resizable
            if (win) this.Title.Text = "Game over, you won!  :D";
            else     this.Title.Text = "Game over, you exploded  :(";
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            this.Close();
            restart = true;
        }

        private void Menu_Click(object sender, EventArgs e)
        {
            this.Close();
            menu = true;
        }

        public bool ClickedRestart
        {
            get { return restart; }
        }
        public bool ClickedMenu
        {
            get { return menu; }
        }
    }
}
