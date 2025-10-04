using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Go_Game
{
    public partial class PlayerWindow : Form
    {
        private Go_Board Go_Grid;

        private int selectedBoardSize;
        private int maxPossibleScore; 

        private PLAYER currentPlayer = PLAYER.BLACK;
        private PLAYER opposingPlayer = PLAYER.WHITE;

        private CellPos currentMouse;
        private CellPos previousMouse;

        private Graphics drawOnGrid;

        /// <summary>
        /// Index - Color: 0 - Black,       1 - White       2 - Board_Color
        /// </summary>
        private SolidBrush[] brushColor;


        private readonly Color primaryColor = Color.FromArgb(45, 45, 48);
        private readonly Color secondaryColor = Color.FromArgb(62, 62, 66);
        private readonly Color accentColor = Color.FromArgb(0, 122, 204);
        private readonly Color textColor = Color.FromArgb(241, 241, 241);
        private readonly Color goldAccent = Color.FromArgb(255, 185, 0);
        private readonly Color successColor = Color.FromArgb(40, 167, 69);
        private readonly Color dangerColor = Color.FromArgb(220, 53, 69);

        public PlayerWindow(BOARD_SIZE sizeOfBoard)
        {
            InitializeComponent();
            InitializeGridDrawingComponents();
            
            selectedBoardSize = (int)sizeOfBoard;
            maxPossibleScore = selectedBoardSize * selectedBoardSize;
            
            SetupUI();

            this.AutoSize = true;

            Go_Grid = new Go_Board(new PlayerWindow_Safe(this), (BOARD_SIZE)selectedBoardSize);

            currentMouse = new CellPos_ExtraPrecision(Go_Grid.Get_CELL_DIMENSION());
            previousMouse = new CellPos(Go_Grid.Get_CELL_DIMENSION());

            this.MinimumSize = new Size(400, 400);
            PlayerWindow_SizeChanged(this, new EventArgs());

            passButton.Visible = false;

            Add_UI_EventHandlers();
            this.AutoSize = false;
            this.SizeChanged += ScaleUI;
        }

        public PlayerWindow(BOARD_SIZE sizeOfBoard, Rectangle parentWindowBounds) : this(sizeOfBoard)
        {
            if (parentWindowBounds.Width > 0 && parentWindowBounds.Height > 0)
            {
                this.Size = parentWindowBounds.Size;
                
                if (IsLocationValid(parentWindowBounds.Location))
                {
                    this.Location = parentWindowBounds.Location;
                }
                else
                {
                    this.StartPosition = FormStartPosition.CenterScreen;
                }
            }
            this.WindowState = FormWindowState.Normal;
        }

        private bool IsLocationValid(Point location)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Bounds.Contains(location))
                {
                    return true;
                }
            }
            return false;
        }

        private void SetupUI()
        {
            this.BackColor = primaryColor;
            this.ForeColor = textColor;

            if (tableLayoutPanel_InGame != null)
            {
                tableLayoutPanel_InGame.BackColor = secondaryColor;
                tableLayoutPanel_InGame.Padding = new Padding(10);
            }

            StyleGameButton(buttonStart, successColor, "▶ Start Game");
            StyleGameButton(buttonExit, dangerColor, "✕ Exit");
            StyleGameButton(passButton, Color.FromArgb(108, 117, 125), "⏭ Pass");

            StyleScoreLabel(blackLabel, Color.FromArgb(33, 37, 41));
            StyleScoreLabel(whiteLabel, Color.FromArgb(248, 249, 250));

            blackLabel.Text = "●  Black: 0";
            whiteLabel.Text = "○  White: 0";
            
            SetMinimumScoreLabelSizes();

            if (panelGrid != null)
            {
                panelGrid.BackColor = Color.FromArgb(240, 176, 96);
            }
        }

        private void SetMinimumScoreLabelSizes()
        {
            string maxScoreText = $"●  Black: {maxPossibleScore}";
            
            using (Graphics g = CreateGraphics())
            {
                Font currentFont = blackLabel.Font ?? new Font("Segoe UI", 11f, FontStyle.Bold);
                SizeF textSize = g.MeasureString(maxScoreText, currentFont);
                
                int minWidth = (int)(textSize.Width * 1.3f) ; 
                int minHeight = (int)(textSize.Height * 1.2f) + 16; 
                
                blackLabel.MinimumSize = new Size(minWidth, minHeight);
                whiteLabel.MinimumSize = new Size(minWidth, minHeight);
                
                if (tableLayoutPanel_InGame.ColumnStyles.Count > 0)
                {
                    tableLayoutPanel_InGame.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, minWidth + 20);
                }
                else
                {
                    tableLayoutPanel_InGame.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, minWidth + 20));
                }
            }
        }

        private void StyleGameButton(Button button, Color baseColor, string text)
        {
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = baseColor;
            button.ForeColor = Color.White;
            button.Cursor = Cursors.Hand;
            button.Font = new Font("Segoe UI", button.Font.Size, FontStyle.Bold);

            button.MouseEnter += (s, e) => {
                button.BackColor = LightenColor(baseColor, 15);
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = LightenColor(baseColor, 30);
            };

            button.MouseLeave += (s, e) => {
                button.BackColor = baseColor;
                button.FlatAppearance.BorderSize = 0;
            };

            button.Paint += (s, e) => {
                Button btn = s as Button;
                Rectangle rect = new Rectangle(1, 1, btn.Width - 2, btn.Height - 2);

                using (GraphicsPath path = GetRoundedRectangle(rect, 6))
                using (LinearGradientBrush brush = new LinearGradientBrush(rect,
                    btn.BackColor, DarkenColor(btn.BackColor, 10), LinearGradientMode.Vertical))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);

                    using (Pen glowPen = new Pen(LightenColor(btn.BackColor, 20), 1))
                    {
                        Rectangle innerRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
                        using (GraphicsPath innerPath = GetRoundedRectangle(innerRect, 5))
                        {
                            e.Graphics.DrawPath(glowPen, innerPath);
                        }
                    }

                    TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect,
                        btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            };
        }

        private void StyleScoreLabel(Label label, Color bgColor)
        {
            label.BackColor = bgColor;
            label.ForeColor = bgColor.GetBrightness() > 0.5 ? Color.Black : Color.White;
            label.Font = new Font("Segoe UI", label.Font.Size, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Padding = new Padding(8);
            label.Dock = DockStyle.Fill;

            label.Paint += (s, e) => {
                Label lbl = s as Label;
                Rectangle rect = new Rectangle(0, 0, lbl.Width, lbl.Height);

                using (GraphicsPath path = GetRoundedRectangle(rect, 8))
                using (SolidBrush brush = new SolidBrush(lbl.BackColor))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(brush, path);

                    using (Pen borderPen = new Pen(DarkenColor(lbl.BackColor, 20), 1))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }

                    Rectangle shadowRect = new Rectangle(rect.X + 2, rect.Y + 2, rect.Width, rect.Height);
                    using (GraphicsPath shadowPath = GetRoundedRectangle(shadowRect, 8))
                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(20, Color.Black)))
                    {
                        e.Graphics.FillPath(shadowBrush, shadowPath);
                    }

                    e.Graphics.FillPath(brush, path);

                    using (Pen borderPen = new Pen(DarkenColor(lbl.BackColor, 20), 1))
                    {
                        e.Graphics.DrawPath(borderPen, path);
                    }

                    TextRenderer.DrawText(e.Graphics, lbl.Text, lbl.Font, rect,
                        lbl.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
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

        private List<int> cachedGrainOffsets = null;

        private void CacheGrainPattern()
        {
            if (cachedGrainOffsets == null)
            {
                cachedGrainOffsets = new List<int>();
                Random rand = new Random(42);
                for (int i = 0; i < panelGrid.Width; i += 8)
                {
                    cachedGrainOffsets.Add(rand.Next(-2, 3));
                }
            }
        }

        private void Draw_Go_Grid()
        {
            if (panelGrid.Width == 0 || panelGrid.Height == 0) 
                return;

            Go_Grid.ReadjustFields();

            using (LinearGradientBrush boardBrush = new LinearGradientBrush(
                new Rectangle(0, 0, panelGrid.Width, panelGrid.Height),
                Color.FromArgb(250, 185, 110),
                Color.FromArgb(230, 166, 86),
                LinearGradientMode.ForwardDiagonal))
            {
                drawOnGrid.FillRectangle(boardBrush, 0, 0, panelGrid.Width, panelGrid.Height);
            }

            CacheGrainPattern();

            using (Pen grainPen = new Pen(Color.FromArgb(15, 139, 69, 19), 1))
            {
                int offsetIndex = 0;
                for (int i = 0; i < panelGrid.Width; i += 8)
                {
                    int offset = cachedGrainOffsets[offsetIndex++];
                    drawOnGrid.DrawLine(grainPen, i + offset, 0, i + offset, panelGrid.Height);
                }
            }

            drawOnGrid.SmoothingMode = SmoothingMode.AntiAlias;

            for (int i = Go_Grid.Get_CELL_DIMENSION(); i <= Go_Grid.Get_endOfDrawY();
                i += Go_Grid.Get_CELL_DIMENSION())
            {
                using (Pen shadowPen = new Pen(Color.FromArgb(40, 0, 0, 0), Go_Board.BORDER_WIDTH))
                {
                    drawOnGrid.DrawLine(shadowPen,
                        i + Go_Grid.Get_middleOffset() + 1,
                        Go_Grid.Get_CELL_DIMENSION() + Go_Grid.Get_middleOffset() + 1,
                        i + Go_Grid.Get_middleOffset() + 1,
                        Go_Grid.Get_endOfDrawY() + Go_Grid.Get_middleOffset() + 1);
                }

                using (Pen linePen = new Pen(Color.FromArgb(180, 0, 0, 0), Go_Board.BORDER_WIDTH))
                {
                    drawOnGrid.DrawLine(linePen,
                        i + Go_Grid.Get_middleOffset(),
                        Go_Grid.Get_CELL_DIMENSION() + Go_Grid.Get_middleOffset(),
                        i + Go_Grid.Get_middleOffset(),
                        Go_Grid.Get_endOfDrawY() + Go_Grid.Get_middleOffset());
                }

                using (Pen shadowPen = new Pen(Color.FromArgb(40, 0, 0, 0), Go_Board.BORDER_WIDTH))
                {
                    drawOnGrid.DrawLine(shadowPen,
                        Go_Grid.Get_CELL_DIMENSION() + Go_Grid.Get_middleOffset() + 1,
                        i + Go_Grid.Get_middleOffset() + 1,
                        Go_Grid.Get_endOfDrawY() + Go_Grid.Get_middleOffset() + 1,
                        i + Go_Grid.Get_middleOffset() + 1);
                }

                using (Pen linePen = new Pen(Color.FromArgb(180, 0, 0, 0), Go_Board.BORDER_WIDTH))
                {
                    drawOnGrid.DrawLine(linePen,
                        Go_Grid.Get_CELL_DIMENSION() + Go_Grid.Get_middleOffset(),
                        i + Go_Grid.Get_middleOffset(),
                        Go_Grid.Get_endOfDrawY() + Go_Grid.Get_middleOffset(),
                        i + Go_Grid.Get_middleOffset());
                }
            }

            if (selectedBoardSize >= 13)
            {
                int cellDim = Go_Grid.Get_CELL_DIMENSION();
                int offset = Go_Grid.Get_middleOffset();
                int starRadius = Math.Max(3, cellDim / 12);

                List<Point> starPoints = new List<Point>();

                if (selectedBoardSize == 19)
                {
                    starPoints.AddRange(new[] {
                        new Point(4, 4), new Point(10, 4), new Point(16, 4),
                        new Point(4, 10), new Point(10, 10), new Point(16, 10),
                        new Point(4, 16), new Point(10, 16), new Point(16, 16)
                    });
                }
                else if (selectedBoardSize == 13)
                {
                    starPoints.AddRange(new[] {
                        new Point(4, 4), new Point(7, 7), new Point(10, 4),
                        new Point(4, 10), new Point(10, 10)
                    });
                }

                using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    foreach (Point star in starPoints)
                    {
                        int x = star.X * cellDim + offset;
                        int y = star.Y * cellDim + offset;
                        drawOnGrid.FillEllipse(starBrush, x - starRadius, y - starRadius, starRadius * 2, starRadius * 2);
                    }
                }
            }
        }

        public void DrawEmptyCell(Point cellToEmptyPos)
        {
            xCellCoord = cellToEmptyPos.X;
            yCellCoord = cellToEmptyPos.Y;
            lengthEmptyVert = Go_Grid.Get_CELL_DIMENSION();

            xEmptyVert = xCellCoord * lengthEmptyVert;
            yEmptyVert = yCellCoord * lengthEmptyVert;


            using (LinearGradientBrush boardBrush = new LinearGradientBrush(
                new Rectangle(0, 0, panelGrid.Width, panelGrid.Height),
                Color.FromArgb(250, 185, 110),
                Color.FromArgb(230, 166, 86),
                LinearGradientMode.ForwardDiagonal))
            {
                drawOnGrid.FillEllipse(boardBrush, xEmptyVert, yEmptyVert, lengthEmptyVert, lengthEmptyVert);
            }


            using (Region originalClip = drawOnGrid.Clip.Clone())
            {
                using (GraphicsPath cellPath = new GraphicsPath())
                {
                    cellPath.AddEllipse(xEmptyVert, yEmptyVert, lengthEmptyVert, lengthEmptyVert);
                    drawOnGrid.SetClip(cellPath);


                    if (cachedGrainOffsets != null)
                    {
                        using (Pen grainPen = new Pen(Color.FromArgb(15, 139, 69, 19), 1))
                        {
                            int offsetIndex = 0;
                            for (int i = 0; i < panelGrid.Width; i += 8)
                            {
                                int offset = cachedGrainOffsets[offsetIndex++];
                                drawOnGrid.DrawLine(grainPen, i + offset, 0, i + offset, panelGrid.Height);
                            }
                        }
                    }


                    drawOnGrid.Clip = originalClip;
                }
            }

            if (xCellCoord == 1)
            {
                xEmptyVert = xEmptyHoriz = lengthEmptyVert + Go_Grid.Get_middleOffset();

                if (yCellCoord == 1)
                {
                    yEmptyHoriz = yEmptyVert = xEmptyVert;
                    lengthEmptyVert = lengthEmptyHoriz = lengthEmptyVert / 2 + 2;
                }
                else
                {
                    if (yCellCoord == selectedBoardSize)
                    {
                        yEmptyVert = Go_Grid.Get_endOfDrawY();
                        yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                        lengthEmptyVert = lengthEmptyVert / 2 + 1;
                        lengthEmptyHoriz = lengthEmptyVert + 1;
                    }
                    else
                    {
                        yEmptyVert = yCellCoord * lengthEmptyVert;
                        yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                        lengthEmptyHoriz = lengthEmptyVert / 2 + 2;
                    }
                }
            }
            else
            {
                if (xCellCoord == selectedBoardSize)
                {
                    xEmptyVert = Go_Grid.Get_endOfDrawX();
                    xEmptyHoriz = xEmptyVert - Go_Grid.Get_middleOffset();

                    if (yCellCoord == 1)
                    {
                        yEmptyHoriz = yEmptyVert = lengthEmptyVert + Go_Grid.Get_middleOffset();
                        lengthEmptyHoriz = lengthEmptyVert / 2 + 1;
                        lengthEmptyVert = lengthEmptyHoriz + 1;
                    }
                    else
                    {
                        if (yCellCoord == selectedBoardSize)
                        {
                            yEmptyVert = Go_Grid.Get_endOfDrawY();
                            yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                            lengthEmptyVert = lengthEmptyHoriz = lengthEmptyVert / 2 + 1;
                        }
                        else
                        {
                            yEmptyVert = yCellCoord * lengthEmptyVert;
                            yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                            lengthEmptyHoriz = lengthEmptyVert / 2 + 1;
                        }
                    }
                }
                else
                {
                    xEmptyHoriz = xCellCoord * lengthEmptyVert;
                    xEmptyVert = xEmptyHoriz + Go_Grid.Get_middleOffset();
                    lengthEmptyHoriz = lengthEmptyVert;

                    if (yCellCoord == 1)
                    {
                        yEmptyVert = yEmptyHoriz = lengthEmptyVert + Go_Grid.Get_middleOffset();
                        lengthEmptyVert = lengthEmptyVert / 2 + 2;
                    }
                    else
                    {
                        if (yCellCoord == selectedBoardSize)
                        {
                            yEmptyVert = Go_Grid.Get_endOfDrawY();
                            yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                            lengthEmptyVert = lengthEmptyVert / 2 + 1;
                        }
                        else
                        {
                            yEmptyVert = yCellCoord * lengthEmptyVert;
                            yEmptyHoriz = yEmptyVert + Go_Grid.Get_middleOffset();
                        }
                    }
                }
            }

            using (Pen shadowPen = new Pen(Color.FromArgb(40, 0, 0, 0), Go_Board.BORDER_WIDTH))
            {
                drawOnGrid.DrawLine(shadowPen, xEmptyVert + 1, yEmptyVert + 1, xEmptyVert + 1, yEmptyVert + lengthEmptyVert + 1);
            }
            using (Pen linePen = new Pen(Color.FromArgb(180, 0, 0, 0), Go_Board.BORDER_WIDTH))
            {
                drawOnGrid.DrawLine(linePen, xEmptyVert, yEmptyVert, xEmptyVert, yEmptyVert + lengthEmptyVert);
            }

            using (Pen shadowPen = new Pen(Color.FromArgb(40, 0, 0, 0), Go_Board.BORDER_WIDTH))
            {
                drawOnGrid.DrawLine(shadowPen, xEmptyHoriz + 1, yEmptyHoriz + 1, xEmptyHoriz + lengthEmptyHoriz + 1, yEmptyHoriz + 1);
            }
            using (Pen linePen = new Pen(Color.FromArgb(180, 0, 0, 0), Go_Board.BORDER_WIDTH))
            {
                drawOnGrid.DrawLine(linePen, xEmptyHoriz, yEmptyHoriz, xEmptyHoriz + lengthEmptyHoriz, yEmptyHoriz);
            }

            if (selectedBoardSize >= 13)
            {
                int cellDim = Go_Grid.Get_CELL_DIMENSION();
                int offset = Go_Grid.Get_middleOffset();
                int starRadius = Math.Max(3, cellDim / 12);

                List<Point> starPoints = new List<Point>();

                if (selectedBoardSize == 19)
                {
                    starPoints.AddRange(new[] {
                        new Point(4, 4), new Point(10, 4), new Point(16, 4),
                        new Point(4, 10), new Point(10, 10), new Point(16, 10),
                        new Point(4, 16), new Point(10, 16), new Point(16, 16)
                    });
                }
                else if (selectedBoardSize == 13)
                {
                    starPoints.AddRange(new[] {
                        new Point(4, 4), new Point(7, 7), new Point(10, 4),
                        new Point(4, 10), new Point(10, 10)
                    });
                }

                foreach (Point star in starPoints)
                {
                    if (star.X == xCellCoord && star.Y == yCellCoord)
                    {
                        using (SolidBrush starBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                        {
                            int starX = star.X * cellDim + offset;
                            int starY = star.Y * cellDim + offset;
                            drawOnGrid.FillEllipse(starBrush, starX - starRadius, starY - starRadius, starRadius * 2, starRadius * 2);
                        }
                        break;
                    }
                }
            }
        }

        private void InitializeGridDrawingComponents()
        {
            if (drawOnGrid == null)
            {
                brushColor = new SolidBrush[3];

                brushColor[0] = new SolidBrush(Color.Black);
                brushColor[1] = new SolidBrush(Color.White);
                // Go board main color
                brushColor[2] = new SolidBrush(Color.FromArgb(240, 176, 96));
            }
            drawOnGrid = panelGrid.CreateGraphics();
        }

        public void ShowNewScore(int[] score)
        {
            blackLabel.Text = "●  Black: " + score[(int)PLAYER.BLACK];
            whiteLabel.Text = "○  White: " + score[(int)PLAYER.WHITE];
        }

        private void Add_UI_EventHandlers()
        {
            buttonExit.Click += buttonExit_Click;
        }

        public Go_Board Get_Go_Grid()
        {
            return Go_Grid;
        }

        public PLAYER GetCurrentPlayer()
        {
            return currentPlayer;
        }

        public PLAYER GetOpposingPlayer()
        {
            return opposingPlayer;
        }

        private int xEmptyVert, xEmptyHoriz, yEmptyVert, yEmptyHoriz;
        private int lengthEmptyVert, lengthEmptyHoriz, xCellCoord, yCellCoord;

        private Point startPoint = new Point(0, 0);

        private int passCount = 0;

        private void passButton_Click(object sender, EventArgs e)
        {
            (currentPlayer, opposingPlayer) = (opposingPlayer, currentPlayer);
            string whoPassed;
            if (opposingPlayer == PLAYER.BLACK)
                whoPassed = "Black";
            else
                whoPassed = "White";

            ShowStyledMessageBox(whoPassed + " just passed.", "Player Passed", MessageBoxIcon.Information);

            if (++passCount == 2)
            {
                string conclusionMessage = null;
                int black_whiteScoreDifference = Go_Grid.GetBlack_WhiteScoreDifference();
                if (black_whiteScoreDifference < 0)
                    conclusionMessage = "🏆 White defeated Black by "
                        + (-black_whiteScoreDifference).ToString() + " points!";
                if (black_whiteScoreDifference > 0)
                    conclusionMessage = "🏆 Black defeated White by "
                        + black_whiteScoreDifference.ToString() + " points!";
                if (black_whiteScoreDifference == 0)
                    conclusionMessage = "🤝 The game ended in a tie!";

                ShowStyledMessageBox(conclusionMessage, "Game Over", MessageBoxIcon.Information);
                Close();
            }
        }

        private void ShowStyledMessageBox(string message, string title, MessageBoxIcon icon)
        {
            Form messageForm = new Form();
            messageForm.Text = title;
            messageForm.Size = new Size(450 * 2, 220 * 2);
            messageForm.StartPosition = FormStartPosition.CenterParent;
            messageForm.BackColor = primaryColor;
            messageForm.ForeColor = textColor;
            messageForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            messageForm.MaximizeBox = false;
            messageForm.MinimizeBox = false;

            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.AutoSize = false;
            messageLabel.Size = new Size(410 * 2, 50 * 2);
            messageLabel.Location = new Point(20 * 2, 30 * 2);
            messageLabel.ForeColor = textColor;
            messageLabel.Font = new Font("Segoe UI", 14 * 2, FontStyle.Regular);
            messageLabel.TextAlign = ContentAlignment.MiddleCenter;

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Size = new Size(80 * 2, 35 * 2);
            okButton.Location = new Point(185 * 2, 100 * 2);
            okButton.Font = new Font("Segoe UI", 16, FontStyle.Regular);
            StyleGameButton(okButton, accentColor, "OK");
            okButton.Click += (s, e) => messageForm.Close();

            messageForm.Controls.Add(messageLabel);
            messageForm.Controls.Add(okButton);
            messageForm.ShowDialog(this);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.AutoSize = false;
            Draw_Go_Grid();
            panelGrid.MouseMove += panelGrid_MouseMove;
            panelGrid.MouseClick += panelGrid_PutPiece;
            this.SizeChanged -= ScaleUI;
            this.SizeChanged += PlayerWindow_SizeChanged;
            buttonStart.Hide();
            buttonStart.Enabled = false;
            passButton.Visible = true;

            UpdatePlayerIndicator();
        }

        private void UpdatePlayerIndicator()
        {
            if (currentPlayer == PLAYER.BLACK)
            {
                blackLabel.BackColor = goldAccent;
                blackLabel.ForeColor = Color.Black;
                whiteLabel.BackColor = Color.FromArgb(248, 249, 250);
                whiteLabel.ForeColor = Color.Black;
            }
            else
            {
                whiteLabel.BackColor = goldAccent;
                whiteLabel.ForeColor = Color.Black;
                blackLabel.BackColor = Color.FromArgb(33, 37, 41);
                blackLabel.ForeColor = Color.White;
            }

            blackLabel.Invalidate();
            whiteLabel.Invalidate();
        }

        private void panelGrid_MouseMove(object sender, EventArgs e)
        {
            currentMouse.SetCoord(PointToClient(MousePosition) - (Size)panelGrid.Location);

            // Seemingly useless, as these coordinates would indicate that the mouse
            // is situated inside panelGrid, the event would still trigger in 
            // special circumstances, such as holding down the left mouse button!
            if (currentMouse.IsInsideArea(startPoint, (Point)panelGrid.Size))
            {

                // Verifies if, after moving the mouse, it's on the same cell
                if (!currentMouse.GetCellCoord().Equals(previousMouse.GetCellCoord()))
                {
                    if (IsValidGameCell(previousMouse.GetCellCoord()) && 
                        Go_Grid.GetValueAt(previousMouse.GetCellCoord()) == (int)CELL_VALUE.EMPTY)
                    {
                        DrawEmptyCell(previousMouse.GetCellCoord());
                    }

                    previousMouse.SetCoord(currentMouse.GetCellCoord());

                    if (IsValidGameCell(currentMouse.GetCellCoord()) && 
                        Go_Grid.CanPlaceOn(currentPlayer, currentMouse))
                    {
                        int cellDim = Go_Grid.Get_CELL_DIMENSION();
                        int stoneSize = (int)(cellDim * 0.85f);
                        int x = currentMouse.GetCellCoord().X * cellDim + cellDim / 2;
                        int y = currentMouse.GetCellCoord().Y * cellDim + cellDim / 2;
                        int stoneX = x - stoneSize / 2;
                        int stoneY = y - stoneSize / 2;

                        drawOnGrid.SmoothingMode = SmoothingMode.AntiAlias;

                        if (currentPlayer == PLAYER.BLACK)
                        {
                            using (GraphicsPath stonePath = new GraphicsPath())
                            {
                                stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(36, 0, 0, 0)))
                                {
                                    drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                                }

                                using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                                {
                                    stoneBrush.CenterColor = Color.FromArgb(153, 45, 45, 45);
                                    stoneBrush.SurroundColors = new[] { Color.FromArgb(153, 0, 0, 0) };
                                    stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                                    drawOnGrid.FillPath(stoneBrush, stonePath);
                                }

                                using (Pen rimPen = new Pen(Color.FromArgb(48, 255, 255, 255), 1))
                                {
                                    drawOnGrid.DrawEllipse(rimPen, stoneX + 1, stoneY + 1, stoneSize - 2, stoneSize - 2);
                                }
                            }
                        }
                        else
                        {
                            using (GraphicsPath stonePath = new GraphicsPath())
                            {
                                stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(48, 0, 0, 0)))
                                {
                                    drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                                }

                                using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                                {
                                    stoneBrush.CenterColor = Color.FromArgb(153, 255, 255, 255);
                                    stoneBrush.SurroundColors = new[] { Color.FromArgb(153, 220, 220, 220) };
                                    stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                                    drawOnGrid.FillPath(stoneBrush, stonePath);
                                }

                                using (Pen borderPen = new Pen(Color.FromArgb(72, 160, 160, 160), 1))
                                {
                                    drawOnGrid.DrawEllipse(borderPen, stoneX, stoneY, stoneSize, stoneSize);
                                }

                                int highlightSize = (int)(stoneSize * 0.3f);
                                using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(60, 255, 255, 255)))
                                {
                                    drawOnGrid.FillEllipse(highlightBrush,
                                        stoneX + stoneSize / 4,
                                        stoneY + stoneSize / 4,
                                        highlightSize, highlightSize);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IsValidGameCell(previousMouse.GetCellCoord()) && 
                    Go_Grid.GetValueAt(previousMouse.GetCellCoord()) == (int)CELL_VALUE.EMPTY)
                {
                    DrawEmptyCell(previousMouse.GetCellCoord());
                }
                previousMouse.SetCoord(new Point(-1, -1));
            }
        }

        private bool IsValidGameCell(Point cellCoord)
        {
            return cellCoord.X >= 1 && cellCoord.X <= selectedBoardSize && 
                   cellCoord.Y >= 1 && cellCoord.Y <= selectedBoardSize;
        }

        private void panelGrid_PutPiece(object sender, EventArgs e)
        {

            panelGrid.MouseMove -= panelGrid_MouseMove;

            if (Go_Grid.CanPlaceOn(currentPlayer, currentMouse))
            {
                passCount = 0;
                Go_Grid.PlaceOn(currentPlayer, currentMouse);
                
                DrawPlacedStone(currentMouse.GetCellCoord(), currentPlayer);
                
                (currentPlayer, opposingPlayer) = (opposingPlayer, currentPlayer);
                UpdatePlayerIndicator();
            }

            panelGrid.MouseMove += panelGrid_MouseMove;
        }

        private void DrawPlacedStone(Point cellCoord, PLAYER player)
        {
            int cellDim = Go_Grid.Get_CELL_DIMENSION();
            int stoneSize = (int)(cellDim * 0.85f);
            int x = cellCoord.X * cellDim + cellDim / 2;
            int y = cellCoord.Y * cellDim + cellDim / 2;
            int stoneX = x - stoneSize / 2;
            int stoneY = y - stoneSize / 2;

            drawOnGrid.SmoothingMode = SmoothingMode.AntiAlias;

            if (player == PLAYER.BLACK)
            {
                using (GraphicsPath stonePath = new GraphicsPath())
                {
                    stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                    {
                        drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                    }

                    using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                    {
                        stoneBrush.CenterColor = Color.FromArgb(255, 45, 45, 45); // Fully opaque
                        stoneBrush.SurroundColors = new[] { Color.FromArgb(255, 0, 0, 0) }; // Fully opaque
                        stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                        drawOnGrid.FillPath(stoneBrush, stonePath);
                    }

                    using (Pen rimPen = new Pen(Color.FromArgb(80, 255, 255, 255), 1))
                    {
                        drawOnGrid.DrawEllipse(rimPen, stoneX + 1, stoneY + 1, stoneSize - 2, stoneSize - 2);
                    }
                }
            }
            else 
            {
                using (GraphicsPath stonePath = new GraphicsPath())
                {
                    stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                    {
                        drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                    }

                    using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                    {
                        stoneBrush.CenterColor = Color.FromArgb(255, 255, 255, 255); 
                        stoneBrush.SurroundColors = new[] { Color.FromArgb(255, 220, 220, 220) }; 
                        stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                        drawOnGrid.FillPath(stoneBrush, stonePath);
                    }

                    using (Pen borderPen = new Pen(Color.FromArgb(120, 160, 160, 160), 1))
                    {
                        drawOnGrid.DrawEllipse(borderPen, stoneX, stoneY, stoneSize, stoneSize);
                    }

                    int highlightSize = (int)(stoneSize * 0.3f);
                    using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                    {
                        drawOnGrid.FillEllipse(highlightBrush,
                            stoneX + stoneSize / 4,
                            stoneY + stoneSize / 4,
                            highlightSize, highlightSize);
                    }
                }
            }
        }

        private void PlayerWindow_SizeChanged(object sender, EventArgs e)
        {
            ScaleUI(sender, e);
            
            Draw_Go_Grid();
            Redraw_All_Pieces();

            Invalidate();
        }

        private void ScaleUI(object sender, EventArgs e)
        {
            int availableWidth = Width - tableLayoutPanel_InGame.Width;
            int availableHeight = ClientSize.Height;

            int maxCellDim = Math.Min(availableWidth, availableHeight) / (selectedBoardSize + 3);
            Go_Grid.Set_CELL_DIMENSION(maxCellDim);

            currentMouse.SetCellDim(Go_Grid.Get_CELL_DIMENSION());
            previousMouse.SetCellDim(Go_Grid.Get_CELL_DIMENSION());

            int dimGrid = Go_Grid.Get_CELL_DIMENSION() * (selectedBoardSize + 2);
            panelGrid.Size = panelGrid.MaximumSize = new Size(dimGrid, dimGrid);

            int leftMargin = (availableWidth - dimGrid) / 2;
            int topMargin = (availableHeight - dimGrid) / 2;

            panelGrid.Location = new Point(leftMargin, topMargin);

            ScaleFonts();
            
            InitializeGridDrawingComponents();
            cachedGrainOffsets = null;
        }

        private void ScaleFonts()
        {
            float baseSize = Math.Min(Width, Height) / 40f;

            float buttonFontSize = Math.Max(baseSize * 1.0f, 9f);
            float labelFontSize = Math.Max(baseSize * 1.0f, 11f);
            buttonStart.Font = new Font("Segoe UI", buttonFontSize, FontStyle.Bold);
            buttonExit.Font = new Font("Segoe UI", buttonFontSize, FontStyle.Bold);
            passButton.Font = new Font("Segoe UI", buttonFontSize, FontStyle.Bold);
            blackLabel.Font = new Font("Segoe UI", labelFontSize, FontStyle.Bold);
            whiteLabel.Font = new Font("Segoe UI", labelFontSize, FontStyle.Bold);
            
            SetMinimumScoreLabelSizes();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Redraw_All_Pieces()
        {
            int CountOf_Go_String;
            for (int i = 0; i < 2; i++)
            {
                CountOf_Go_String = Go_Grid.Get_Go_String_Count((PLAYER)i);
                for (int j = 0; j < CountOf_Go_String; j++)
                {
                    IEnumerator<Point> enu_Stone = Go_Grid.GetStonesEnumeratorOf((PLAYER)i, j);
                    while (enu_Stone.MoveNext())
                    {

                        int cellDim = Go_Grid.Get_CELL_DIMENSION();
                        int stoneSize = (int)(cellDim * 0.85f);
                        int x = enu_Stone.Current.X * cellDim + cellDim / 2;
                        int y = enu_Stone.Current.Y * cellDim + cellDim / 2;
                        int stoneX = x - stoneSize / 2;
                        int stoneY = y - stoneSize / 2;

                        drawOnGrid.SmoothingMode = SmoothingMode.AntiAlias;

                        if ((PLAYER)i == PLAYER.BLACK)
                        {
                            using (GraphicsPath stonePath = new GraphicsPath())
                            {
                                stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                                {
                                    drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                                }

                                using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                                {
                                    stoneBrush.CenterColor = Color.FromArgb(255, 45, 45, 45);
                                    stoneBrush.SurroundColors = new[] { Color.FromArgb(255, 0, 0, 0) };
                                    stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                                    drawOnGrid.FillPath(stoneBrush, stonePath);
                                }

                                using (Pen rimPen = new Pen(Color.FromArgb(80, 255, 255, 255), 1))
                                {
                                    drawOnGrid.DrawEllipse(rimPen, stoneX + 1, stoneY + 1, stoneSize - 2, stoneSize - 2);
                                }
                            }
                        }
                        else
                        {
                            using (GraphicsPath stonePath = new GraphicsPath())
                            {
                                stonePath.AddEllipse(stoneX, stoneY, stoneSize, stoneSize);

                                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0)))
                                {
                                    drawOnGrid.FillEllipse(shadowBrush, stoneX + 2, stoneY + 2, stoneSize, stoneSize);
                                }

                                using (PathGradientBrush stoneBrush = new PathGradientBrush(stonePath))
                                {
                                    stoneBrush.CenterColor = Color.FromArgb(255, 255, 255, 255); 
                                    stoneBrush.SurroundColors = new[] { Color.FromArgb(255, 220, 220, 220) }; 
                                    stoneBrush.CenterPoint = new PointF(stoneX + stoneSize * 0.3f, stoneY + stoneSize * 0.3f);
                                    drawOnGrid.FillPath(stoneBrush, stonePath);
                                }

                                using (Pen borderPen = new Pen(Color.FromArgb(120, 160, 160, 160), 1))
                                {
                                    drawOnGrid.DrawEllipse(borderPen, stoneX, stoneY, stoneSize, stoneSize);
                                }

                                int highlightSize = (int)(stoneSize * 0.3f);
                                using (SolidBrush highlightBrush = new SolidBrush(Color.FromArgb(100, 255, 255, 255)))
                                {
                                    drawOnGrid.FillEllipse(highlightBrush,
                                        stoneX + stoneSize / 4,
                                        stoneY + stoneSize / 4,
                                        highlightSize, highlightSize);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
