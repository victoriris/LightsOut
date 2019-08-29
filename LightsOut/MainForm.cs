using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;    // Distance from upper-left side of window         
        private const int GridLength = 200;   // Size in pixels of grid  
        private LightsOutGame game;

        ToolStripMenuItem selectedSizeItem;


        public MainForm()
        {
            InitializeComponent();

            game = new LightsOutGame();
            game.NewGame();

        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black 
            StartNewGame();

        }

        private void StartNewGame()
        {
            game.NewGame();
            
            // Redraw grid
            this.Invalidate();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            selectedSizeItem = this.x3ToolStripMenuItem;
            selectedSizeItem.Checked = true;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Control control = (Control)sender;

            int CellLength =  (control.Size.Width - (GridOffset * 3)) / game.GridSize;

            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section 
                    Brush brush;
                    Pen pen;
                    if (game.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;   // On 
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;   // Off 
                    }

                    // Determine (x,y) coord of row and col to draw rectangle  
                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;

                    // Draw outline and inner rectangle 
                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;

            int CellLength = (control.Size.Width - (GridOffset * 3)) / game.GridSize;

            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * game.GridSize + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * game.GridSize + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            game.Move(r, c);

            // Redraw grid 
            this.Invalidate();

            // Check to see if puzzle has been solved 
            if (game.IsGameOver())
            {
                // Display winner dialog box 
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGameButton_Click(sender, e);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitButton_Click(sender, e);
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }

        private void HandleSizeOptionClick(ToolStripMenuItem item)
        {
            // Oncheck prev
            selectedSizeItem.Checked = false;
            selectedSizeItem = item;
            selectedSizeItem.Checked = true; // Check new
            StartNewGame();

        }


        private void X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.GridSize = 3;
            HandleSizeOptionClick((ToolStripMenuItem)sender);
        }

        private void X4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            game.GridSize = 4;
            HandleSizeOptionClick((ToolStripMenuItem)sender);
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.GridSize = 5;
            HandleSizeOptionClick((ToolStripMenuItem)sender);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Redraw grid 
            this.Invalidate();

        }
    }
}
