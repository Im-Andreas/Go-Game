using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Go_Game
{
    public partial class MainMenuWindow : Form
    {
        private readonly Color primaryColor = Color.FromArgb(45, 45, 48);
        private readonly Color secondaryColor = Color.FromArgb(62, 62, 66);
        private readonly Color accentColor = Color.FromArgb(0, 122, 204);
        private readonly Color textColor = Color.FromArgb(241, 241, 241);
        private readonly Color goldAccent = Color.FromArgb(255, 185, 0);
        
        public MainMenuWindow()
        {
            InitializeComponent();
            MinimumSize = this.Size;
            SetupUI();
            MainMenuWindow_Resize(this, new EventArgs());
        }

        private void SetupUI()
        {
            this.BackColor = primaryColor;
            this.ForeColor = textColor;
            
            gameTitle.ForeColor = goldAccent;
            gameTitle.BackColor = Color.Transparent;
            
            indicationLabel.ForeColor = textColor;
            indicationLabel.BackColor = Color.Transparent;
            
            StyleButton(startButton, accentColor);
            StyleButton(exitButton, Color.FromArgb(220, 53, 69));
            
            StyleComboBox(tableSizeComboBox);
            
            tableLayoutPanel1.BackColor = Color.Transparent;
        }

        private void StyleButton(Button button, Color baseColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = baseColor;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;
            
            button.MouseEnter += (s, e) => {
                button.BackColor = LightenColor(baseColor, 20);
                button.Invalidate();
            };
            
            button.MouseLeave += (s, e) => {
                button.BackColor = baseColor;
                button.Invalidate();
            };
            
            button.Paint += (s, e) => {
                Button btn = s as Button;
                Rectangle rect = new Rectangle(0, 0, btn.Width, btn.Height);
                
                using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, 
                    btn.BackColor, DarkenColor(btn.BackColor, 15), LinearGradientMode.Vertical))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);
                    
                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect, 
                        btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }

        private void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.BackColor = secondaryColor;
            comboBox.ForeColor = textColor;
            comboBox.Font = new Font("Segoe UI", comboBox.Font.Size, FontStyle.Regular);
            
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox.DrawItem += (s, e) => {
                if (e.Index >= 0)
                {
                    e.DrawBackground();
                    
                    Color itemColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected 
                        ? accentColor : secondaryColor;
                    
                    using (SolidBrush brush = new SolidBrush(itemColor))
                    {
                        e.Graphics.FillRectangle(brush, e.Bounds);
                    }
                    
                    TextRenderer.DrawText(e.Graphics, comboBox.Items[e.Index].ToString(), 
                        e.Font, e.Bounds, textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                    
                    e.DrawFocusRectangle();
                }
            };
            
            comboBox.DropDownClosed += (s, e) => {
                comboBox.Invalidate();
            };
            
            comboBox.SelectionChangeCommitted += (s, e) => {
                comboBox.Invalidate();
            };
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            return path;
        }

        private Color LightenColor(Color color, int amount)
        {
            return Color.FromArgb(
                Math.Min(255, color.R + amount),
                Math.Min(255, color.G + amount),
                Math.Min(255, color.B + amount));
        }

        private Color DarkenColor(Color color, int amount)
        {
            return Color.FromArgb(
                Math.Max(0, color.R - amount),
                Math.Max(0, color.G - amount),
                Math.Max(0, color.B - amount));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.ClientRectangle.Width == 0 || this.ClientRectangle.Height == 0)
                return;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle, primaryColor, DarkenColor(primaryColor, 10), 
                LinearGradientMode.Vertical))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
            
            base.OnPaint(e);
        }

        private void ShowStartMenu(object sender, EventArgs e)
        {
            if (exitButton.Text == "Exit")
            {
                exitButton.Text = "Back";
                indicationLabel.Show();
                tableSizeComboBox.Show();
                
                AnimateControlFadeIn(indicationLabel);
                AnimateControlFadeIn(tableSizeComboBox);
            }
            else if (exitButton.Text == "Back")
            {
                if (tableSizeComboBox.SelectedItem == null)
                {
                    ShowStyledMessageBox("Please select a board size before starting the game.", 
                        "Board Size Required", MessageBoxIcon.Warning);
                    return;
                }
                
                Rectangle currentBounds = GetEffectiveWindowBounds();
                PlayerWindow matchWindow = new PlayerWindow(selectedSizeOfBoard, currentBounds);
                Hide();
                matchWindow.ShowDialog();
                ExitButton_Click(sender, e);
                Show();
            }
        }

        private Rectangle GetEffectiveWindowBounds()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                return Screen.FromControl(this).WorkingArea;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                return this.RestoreBounds;
            }
            else
            {
                return new Rectangle(this.Location, this.Size);
            }
        }

        private void AnimateControlFadeIn(Control control)
        {
            control.Visible = true;
            Timer fadeTimer = new Timer();
            fadeTimer.Interval = 50;
            int steps = 0;
            
            fadeTimer.Tick += (s, e) => {
                steps++;
                if (steps >= 10)
                {
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                }
                control.Invalidate();
            };
            
            fadeTimer.Start();
        }

        private void ShowStyledMessageBox(string message, string title, MessageBoxIcon icon)
        {
            Form messageForm = new Form();
            messageForm.Text = title;
            messageForm.Size = new Size(350*2, 150*2);
            messageForm.StartPosition = FormStartPosition.CenterParent;
            messageForm.BackColor = primaryColor;
            messageForm.ForeColor = textColor;
            messageForm.Font = new Font("Segoe UI", 20, FontStyle.Regular);
            messageForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            messageForm.MaximizeBox = false;
            messageForm.MinimizeBox = false;
            
            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.AutoSize = false;
            messageLabel.Size = new Size(320*2, 60*2);
            messageLabel.Location = new Point(15*2, 20*2);
            messageLabel.ForeColor = textColor;
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;
            
            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Size = new Size(75*2, 30*2);
            okButton.Location = new Point(137*2, 85*2);
            okButton.Font = new Font("Segoe UI", 16, FontStyle.Regular);
            StyleButton(okButton, accentColor);
            okButton.Click += (s, e) => messageForm.Close();
            
            messageForm.Controls.Add(messageLabel);
            messageForm.Controls.Add(okButton);
            messageForm.ShowDialog(this);
        }

        private BOARD_SIZE selectedSizeOfBoard = BOARD_SIZE.NORMAL;
        
        private void ExitButton_Click(object sender, EventArgs e)
        {
            switch (exitButton.Text)
            {
                case "Exit":
                    Close();
                    break;

                case "Back":
                    exitButton.Text = "Exit";
                    selectedSizeOfBoard = BOARD_SIZE.NORMAL;
                    tableSizeComboBox.SelectedItem = null;
                    indicationLabel.Hide();
                    tableSizeComboBox.Hide();
                    break;
            }
        }

        private void tableSizeComboBox_TextChanged(object sender, EventArgs e)
        {
            switch (tableSizeComboBox.Text)
            {
                case "Small (9x9)":
                    selectedSizeOfBoard = BOARD_SIZE.SMALL;
                    break;
                case "Medium (13x13)":
                    selectedSizeOfBoard = BOARD_SIZE.MEDIUM;
                    break;
                case "Historical (17x17)":
                    selectedSizeOfBoard = BOARD_SIZE.HISTORICAL;
                    break;
                case "Normal (19x19)":
                    selectedSizeOfBoard = BOARD_SIZE.NORMAL;
                    break;
            }
        }

        float titleSize, buttonSize, comboSize;
        bool HeightLessThanWidth;
        
        private void MainMenuWindow_Resize(object sender, EventArgs e)
        {
            HeightLessThanWidth = Height < Width;
            
            float baseSize;
            if (HeightLessThanWidth)
            {
                baseSize = (float)Math.Floor(Height / 16.5);
            }
            else
            {
                baseSize = (float)Math.Floor(Width / 16.5);
            }

            titleSize = baseSize * 1.5f;
            buttonSize = baseSize / 1.75f;
            comboSize = baseSize / 2.5f;

            gameTitle.Font = new Font("Segoe UI", titleSize, FontStyle.Bold);
            startButton.Font = new Font("Segoe UI", buttonSize, FontStyle.Regular);
            exitButton.Font = new Font("Segoe UI", buttonSize, FontStyle.Regular);
            indicationLabel.Font = new Font("Segoe UI", buttonSize, FontStyle.Regular);
            tableSizeComboBox.Font = new Font("Segoe UI", comboSize, FontStyle.Regular);

            UpdateComboBoxSize();
            Invalidate(); 
        }

        private void UpdateComboBoxSize()
        {
            if (tableSizeComboBox != null)
            {
                int comboWidth = Math.Max(160, (int)(Width * 0.25f));
                int comboHeight = Math.Max(21, (int)(comboSize * 1.5f));
                
                tableSizeComboBox.Size = new Size(comboWidth, comboHeight);
            }
        }
    }
}
