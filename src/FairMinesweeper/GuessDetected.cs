using System;
using System.Windows.Forms;

namespace FairMinesweeper
{
    public partial class GuessDetected : Form
    {
        bool continueAnyway = false;
        public GuessDetected()
        {
            this.MaximizeBox = false; // window cannot be maximised
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // window is not resizable
            InitializeComponent();
        }
        private void Yes_Click(object sender, EventArgs e)
        {
            this.Close();
            continueAnyway = true;
        }

        private void No_Click(object sender, EventArgs e)
        {
            this.Close();
            continueAnyway = false;
        }

        public bool Continue
        {
            get { return continueAnyway; }
        }
    }
}
