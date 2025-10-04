namespace Go_Game
{
    partial class PlayerWindow
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
            this.panelGrid = new System.Windows.Forms.Panel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.blackLabel = new System.Windows.Forms.Label();
            this.whiteLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel_InGame = new System.Windows.Forms.TableLayoutPanel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.passButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel_InGame.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGrid
            // 
            this.panelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelGrid.BackColor = System.Drawing.Color.Silver;
            this.panelGrid.Location = new System.Drawing.Point(0, 0);
            this.panelGrid.Margin = new System.Windows.Forms.Padding(0);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(1064, 681);
            this.panelGrid.TabIndex = 0;
            // 
            // buttonExit
            // 
            this.buttonExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(3, 498);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(120, 95);
            this.buttonExit.TabIndex = 0;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            // 
            // blackLabel
            // 
            this.blackLabel.AutoSize = true;
            this.blackLabel.BackColor = System.Drawing.Color.Transparent;
            this.blackLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blackLabel.Location = new System.Drawing.Point(3, 301);
            this.blackLabel.Name = "blackLabel";
            this.blackLabel.Size = new System.Drawing.Size(120, 50);
            this.blackLabel.TabIndex = 2;
            this.blackLabel.Text = "Black: 0";
            // 
            // whiteLabel
            // 
            this.whiteLabel.AutoSize = true;
            this.whiteLabel.BackColor = System.Drawing.Color.Transparent;
            this.whiteLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whiteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.whiteLabel.Location = new System.Drawing.Point(3, 378);
            this.whiteLabel.Name = "whiteLabel";
            this.whiteLabel.Size = new System.Drawing.Size(120, 50);
            this.whiteLabel.TabIndex = 3;
            this.whiteLabel.Text = "White: 0";
            // 
            // tableLayoutPanel_InGame
            // 
            this.tableLayoutPanel_InGame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel_InGame.AutoSize = true;
            this.tableLayoutPanel_InGame.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel_InGame.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel_InGame.ColumnCount = 1;
            this.tableLayoutPanel_InGame.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_InGame.Controls.Add(this.buttonStart, 0, 1);
            this.tableLayoutPanel_InGame.Controls.Add(this.passButton, 0, 3);
            this.tableLayoutPanel_InGame.Controls.Add(this.blackLabel, 0, 5);
            this.tableLayoutPanel_InGame.Controls.Add(this.whiteLabel, 0, 7);
            this.tableLayoutPanel_InGame.Controls.Add(this.buttonExit, 0, 9);
            this.tableLayoutPanel_InGame.Location = new System.Drawing.Point(938, 0);
            this.tableLayoutPanel_InGame.Margin = new System.Windows.Forms.Padding(0, 0, 40, 0);
            this.tableLayoutPanel_InGame.Name = "tableLayoutPanel_InGame";
            this.tableLayoutPanel_InGame.RowCount = 11;
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel_InGame.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.tableLayoutPanel_InGame.Size = new System.Drawing.Size(126, 679);
            this.tableLayoutPanel_InGame.TabIndex = 4;
            // 
            // buttonStart
            // 
            this.buttonStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(3, 36);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(120, 95);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // passButton
            // 
            this.passButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.passButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.passButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passButton.Location = new System.Drawing.Point(3, 170);
            this.passButton.Name = "passButton";
            this.passButton.Size = new System.Drawing.Size(120, 95);
            this.passButton.TabIndex = 4;
            this.passButton.Text = "Pass";
            this.passButton.UseVisualStyleBackColor = true;
            this.passButton.Click += new System.EventHandler(this.passButton_Click);
            // 
            // PlayerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(1064, 681);
            this.Controls.Add(this.tableLayoutPanel_InGame);
            this.Controls.Add(this.panelGrid);
            this.Name = "PlayerWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Go Game";
            this.tableLayoutPanel_InGame.ResumeLayout(false);
            this.tableLayoutPanel_InGame.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Label blackLabel;
        private System.Windows.Forms.Label whiteLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_InGame;
        private System.Windows.Forms.Button passButton;
        private System.Windows.Forms.Button buttonStart;
    }
}

