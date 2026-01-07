namespace FairMinesweeper
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.small = new System.Windows.Forms.RadioButton();
            this.medium = new System.Windows.Forms.RadioButton();
            this.large = new System.Windows.Forms.RadioButton();
            this.customSize = new System.Windows.Forms.RadioButton();
            this.height = new System.Windows.Forms.TextBox();
            this.width = new System.Windows.Forms.TextBox();
            this.minePercentage = new System.Windows.Forms.TextBox();
            this.Size = new System.Windows.Forms.Label();
            this.difficulty = new System.Windows.Forms.Label();
            this.hard = new System.Windows.Forms.RadioButton();
            this.moderate = new System.Windows.Forms.RadioButton();
            this.easy = new System.Windows.Forms.RadioButton();
            this.customMines = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.percentage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Start = new System.Windows.Forms.Button();
            this.GuessNotification = new System.Windows.Forms.CheckBox();
            this.PunishMode = new System.Windows.Forms.CheckBox();
            this.DebugMode = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // small
            // 
            this.small.AutoSize = true;
            this.small.Location = new System.Drawing.Point(0, 0);
            this.small.Name = "small";
            this.small.Size = new System.Drawing.Size(48, 17);
            this.small.TabIndex = 0;
            this.small.Text = "small";
            this.small.UseVisualStyleBackColor = true;
            this.small.CheckedChanged += new System.EventHandler(this.Small_CheckedChanged);
            // 
            // medium
            // 
            this.medium.AutoSize = true;
            this.medium.Checked = true;
            this.medium.Location = new System.Drawing.Point(0, 23);
            this.medium.Name = "medium";
            this.medium.Size = new System.Drawing.Size(61, 17);
            this.medium.TabIndex = 1;
            this.medium.TabStop = true;
            this.medium.Text = "medium";
            this.medium.UseVisualStyleBackColor = true;
            this.medium.CheckedChanged += new System.EventHandler(this.Medium_CheckedChanged);
            // 
            // large
            // 
            this.large.AutoSize = true;
            this.large.Location = new System.Drawing.Point(0, 46);
            this.large.Name = "large";
            this.large.Size = new System.Drawing.Size(48, 17);
            this.large.TabIndex = 2;
            this.large.Text = "large";
            this.large.UseVisualStyleBackColor = true;
            this.large.CheckedChanged += new System.EventHandler(this.Large_CheckedChanged);
            // 
            // customSize
            // 
            this.customSize.AutoSize = true;
            this.customSize.Location = new System.Drawing.Point(0, 80);
            this.customSize.Name = "customSize";
            this.customSize.Size = new System.Drawing.Size(59, 17);
            this.customSize.TabIndex = 3;
            this.customSize.Text = "custom";
            this.customSize.UseVisualStyleBackColor = true;
            this.customSize.CheckedChanged += new System.EventHandler(this.CustomSize_CheckedChanged);
            // 
            // height
            // 
            this.height.Enabled = false;
            this.height.Location = new System.Drawing.Point(59, 79);
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(36, 20);
            this.height.TabIndex = 4;
            this.height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.height.Click += new System.EventHandler(this.TextBox_Click);
            this.height.TextChanged += new System.EventHandler(this.Height_TextChanged);
            // 
            // width
            // 
            this.width.Enabled = false;
            this.width.Location = new System.Drawing.Point(98, 79);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(36, 20);
            this.width.TabIndex = 5;
            this.width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.width.Click += new System.EventHandler(this.TextBox_Click);
            this.width.TextChanged += new System.EventHandler(this.Width_TextChanged);
            // 
            // minePercentage
            // 
            this.minePercentage.Enabled = false;
            this.minePercentage.Location = new System.Drawing.Point(62, 80);
            this.minePercentage.Name = "minePercentage";
            this.minePercentage.Size = new System.Drawing.Size(28, 20);
            this.minePercentage.TabIndex = 6;
            this.minePercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.minePercentage.Click += new System.EventHandler(this.TextBox_Click);
            this.minePercentage.TextChanged += new System.EventHandler(this.MinePercentage_TextChanged);
            // 
            // Size
            // 
            this.Size.AutoSize = true;
            this.Size.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Size.Location = new System.Drawing.Point(12, 11);
            this.Size.Name = "Size";
            this.Size.Size = new System.Drawing.Size(98, 13);
            this.Size.TabIndex = 7;
            this.Size.Text = "Select grid size:";
            // 
            // difficulty
            // 
            this.difficulty.AutoSize = true;
            this.difficulty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.difficulty.Location = new System.Drawing.Point(189, 11);
            this.difficulty.Name = "difficulty";
            this.difficulty.Size = new System.Drawing.Size(167, 13);
            this.difficulty.TabIndex = 8;
            this.difficulty.Text = "Difficulty (mine percentage):\r\n";
            // 
            // hard
            // 
            this.hard.AutoSize = true;
            this.hard.Location = new System.Drawing.Point(3, 48);
            this.hard.Name = "hard";
            this.hard.Size = new System.Drawing.Size(46, 17);
            this.hard.TabIndex = 11;
            this.hard.Text = "hard";
            this.hard.UseVisualStyleBackColor = true;
            this.hard.CheckedChanged += new System.EventHandler(this.Hard_CheckedChanged);
            // 
            // moderate
            // 
            this.moderate.AutoSize = true;
            this.moderate.Checked = true;
            this.moderate.Location = new System.Drawing.Point(3, 25);
            this.moderate.Name = "moderate";
            this.moderate.Size = new System.Drawing.Size(69, 17);
            this.moderate.TabIndex = 10;
            this.moderate.TabStop = true;
            this.moderate.Text = "moderate";
            this.moderate.UseVisualStyleBackColor = true;
            this.moderate.CheckedChanged += new System.EventHandler(this.Moderate_CheckedChanged);
            // 
            // easy
            // 
            this.easy.AutoSize = true;
            this.easy.Location = new System.Drawing.Point(3, 2);
            this.easy.Name = "easy";
            this.easy.Size = new System.Drawing.Size(47, 17);
            this.easy.TabIndex = 9;
            this.easy.Text = "easy";
            this.easy.UseVisualStyleBackColor = true;
            this.easy.CheckedChanged += new System.EventHandler(this.Easy_CheckedChanged);
            // 
            // customMines
            // 
            this.customMines.AutoSize = true;
            this.customMines.Location = new System.Drawing.Point(3, 82);
            this.customMines.Name = "customMines";
            this.customMines.Size = new System.Drawing.Size(59, 17);
            this.customMines.TabIndex = 12;
            this.customMines.Text = "custom";
            this.customMines.UseVisualStyleBackColor = true;
            this.customMines.CheckedChanged += new System.EventHandler(this.CustomMines_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(59, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "height × width ";
            // 
            // percentage
            // 
            this.percentage.AutoSize = true;
            this.percentage.Location = new System.Drawing.Point(92, 84);
            this.percentage.Name = "percentage";
            this.percentage.Size = new System.Drawing.Size(15, 13);
            this.percentage.TabIndex = 14;
            this.percentage.Text = "%";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.width);
            this.panel1.Controls.Add(this.height);
            this.panel1.Controls.Add(this.customSize);
            this.panel1.Controls.Add(this.large);
            this.panel1.Controls.Add(this.medium);
            this.panel1.Controls.Add(this.small);
            this.panel1.Location = new System.Drawing.Point(15, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(142, 120);
            this.panel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.percentage);
            this.panel2.Controls.Add(this.customMines);
            this.panel2.Controls.Add(this.hard);
            this.panel2.Controls.Add(this.moderate);
            this.panel2.Controls.Add(this.easy);
            this.panel2.Controls.Add(this.minePercentage);
            this.panel2.Location = new System.Drawing.Point(189, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(140, 121);
            this.panel2.TabIndex = 16;
            // 
            // Start
            // 
            this.Start.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Start.Font = new System.Drawing.Font("Modern No. 20", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Start.Location = new System.Drawing.Point(266, 216);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(80, 22);
            this.Start.TabIndex = 17;
            this.Start.Text = "START";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // GuessNotification
            // 
            this.GuessNotification.AutoSize = true;
            this.GuessNotification.Checked = true;
            this.GuessNotification.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GuessNotification.Location = new System.Drawing.Point(15, 175);
            this.GuessNotification.Name = "GuessNotification";
            this.GuessNotification.Size = new System.Drawing.Size(185, 17);
            this.GuessNotification.TabIndex = 18;
            this.GuessNotification.Text = "notify me of unnecessary guesses";
            this.GuessNotification.UseVisualStyleBackColor = true;
            this.GuessNotification.CheckedChanged += new System.EventHandler(this.GuessNotification_CheckedChanged);
            // 
            // PunishMode
            // 
            this.PunishMode.AutoSize = true;
            this.PunishMode.Location = new System.Drawing.Point(15, 198);
            this.PunishMode.Name = "PunishMode";
            this.PunishMode.Size = new System.Drawing.Size(252, 17);
            this.PunishMode.TabIndex = 19;
            this.PunishMode.Text = "punish unnecessary guesses (mine is always hit)";
            this.PunishMode.UseVisualStyleBackColor = true;
            this.PunishMode.CheckedChanged += new System.EventHandler(this.PunishMode_CheckedChanged);
            // 
            // DebugMode
            // 
            this.DebugMode.AutoSize = true;
            this.DebugMode.Location = new System.Drawing.Point(15, 221);
            this.DebugMode.Name = "DebugMode";
            this.DebugMode.Size = new System.Drawing.Size(223, 17);
            this.DebugMode.TabIndex = 20;
            this.DebugMode.Text = "debug mode (mines are visible at all times)";
            this.DebugMode.UseVisualStyleBackColor = true;
            this.DebugMode.CheckedChanged += new System.EventHandler(this.DebugMode_CheckedChanged);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 252);
            this.Controls.Add(this.DebugMode);
            this.Controls.Add(this.PunishMode);
            this.Controls.Add(this.GuessNotification);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.difficulty);
            this.Controls.Add(this.Size);
            this.Name = "MainMenu";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton small;
        private System.Windows.Forms.RadioButton medium;
        private System.Windows.Forms.RadioButton large;
        private System.Windows.Forms.RadioButton customSize;
        private System.Windows.Forms.TextBox height;
        private System.Windows.Forms.TextBox width;
        private System.Windows.Forms.TextBox minePercentage;
        private System.Windows.Forms.Label Size;
        private System.Windows.Forms.Label difficulty;
        private System.Windows.Forms.RadioButton hard;
        private System.Windows.Forms.RadioButton moderate;
        private System.Windows.Forms.RadioButton easy;
        private System.Windows.Forms.RadioButton customMines;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label percentage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.CheckBox GuessNotification;
        private System.Windows.Forms.CheckBox PunishMode;
        private System.Windows.Forms.CheckBox DebugMode;
    }
}