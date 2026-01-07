using System;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using FairMinesweeper.Properties;

namespace FairMinesweeper
{
    public partial class MainMenu : Form
    {
        const int defaultSize = 15, defaultPercentage = 16;

        int gridHeight = defaultSize, gridWidth = defaultSize,
            chosenPercentage = defaultPercentage;

        bool guessNotification = true, punishMode = false, debugMode = false; 

        bool clickedStart = false;

        public MainMenu()
        {
            InitializeComponent();
            this.Icon = Resources.minesweeper_icon;
            this.Text = "Fair Minesweeper - Menu";
            this.StartPosition = FormStartPosition.CenterScreen;
            height.Text = width.Text = Convert.ToString(defaultSize);
            minePercentage.Text = Convert.ToString(defaultPercentage);
            this.MaximizeBox = false; // window cannot be maximised
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // window is not resizable

            // Press Enter to start:
            this.AcceptButton = Start;
        }

        void Enable_CustomSize(bool b)
        {
            height.Enabled = b; width.Enabled = b;
        }
        void Update_CustomSizeText()
        {
            height.Text = gridHeight.ToString();
            width.Text = gridWidth.ToString();
        }
        void Update_CustomDifficultyText()
        {
            minePercentage.Text = chosenPercentage.ToString();
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            box.SelectAll();
        }
        private void TextBox_Focus(TextBox box)
        {
            box.Focus();
            box.SelectAll();
        }

        // SIZE

        private void Small_CheckedChanged(object sender, EventArgs e)
        {
            gridHeight = gridWidth = 9;
            Update_CustomSizeText();
            Enable_CustomSize(false);
        }

        private void Medium_CheckedChanged(object sender, EventArgs e)
        {
            gridHeight = gridWidth = 15;
            Update_CustomSizeText();
            Enable_CustomSize(false);
        }

        private void Large_CheckedChanged(object sender, EventArgs e)
        {
            gridHeight = gridWidth = 23;
            Update_CustomSizeText();
            Enable_CustomSize(false);
        }

        private void CustomSize_CheckedChanged(object sender, EventArgs e)
        {
            Enable_CustomSize(true);
            //Quality of life feature:
            TextBox_Focus(height);

        }

        private void Height_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            int number;
            int.TryParse(box.Text, out number);

            // invalid input correction
            if ((number <= 0))
            {
                gridHeight = defaultSize;
                Update_CustomSizeText();

                box.SelectAll();
            }
            else
            {
                gridHeight = number;
            }
        }

        private void Width_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            int number;
            int.TryParse(box.Text, out number);

            // invalid input correction
            if ((number <= 0))
            {
                gridWidth = defaultSize;
                Update_CustomSizeText();

                box.SelectAll();
            }
            else
            {
                gridWidth = number;
            }
        }

        // DIFFICULTY

        private void Easy_CheckedChanged(object sender, EventArgs e)
        {
            chosenPercentage = 12;
            Update_CustomDifficultyText();
            minePercentage.Enabled = false;
        }

        private void Moderate_CheckedChanged(object sender, EventArgs e)
        {
            chosenPercentage = 16;
            Update_CustomDifficultyText();
            minePercentage.Enabled = false;
        }

        private void Hard_CheckedChanged(object sender, EventArgs e)
        {
            chosenPercentage = 20;
            Update_CustomDifficultyText();
            minePercentage.Enabled = false;
        }

        private void CustomMines_CheckedChanged(object sender, EventArgs e)
        {
            minePercentage.Enabled = true;
            //Quality of life feature:
            TextBox_Focus(minePercentage);
        }
        private void MinePercentage_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            int number;
            int.TryParse(box.Text, out number);

            // invalid input correction
            if ((number <= 0) || (number > 100))
            {
                chosenPercentage = defaultPercentage;
                Update_CustomDifficultyText();

                box.SelectAll();
            }
            else
            {
                chosenPercentage = number;
            }

        }

        private void Start_Click(object sender, EventArgs e)
        {
            clickedStart = true;
            this.Close();
        }

        // GAME ALTERING CHECKER BOXES
        
        private void GuessNotification_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;

            guessNotification = box.Checked;
        }

        private void PunishMode_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;

            punishMode = box.Checked;
        }

        private void DebugMode_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = sender as CheckBox;

            debugMode = box.Checked;
        }

        // PARAMETER GETTERS
        public int GridHeight
        {
            get { return gridHeight; }
        }

        public int GridWidth
        {
            get { return gridWidth; }
        }
        public int MinesCount
        {
            get
            {   // calculate number of mines
                int n = (gridHeight * gridWidth * chosenPercentage) / 100;
                return ( n <= 0) ? 1 : n ; // dont allow zero bombs
            } 
        }
        public bool NotifyOfGuesses
        {
            get { return guessNotification; }
        }
        public bool PunishGuesses
        {
            get { return punishMode; }
        }
        public bool LeaveMinesVisible
        {
            get { return debugMode; }
        }
        public bool ClickedStart 
        { 
            get { return clickedStart; } 
        }
    }
}
