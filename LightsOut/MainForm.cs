using System;
using System.Drawing;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private const int GridOffset = 25;    // Distance from upper-left side of window         
        private const int GridLength = 200;   // Size in pixels of grid         
        private const int NumCells = 3;       // Number of cells in grid         
        private int CellLength = GridLength / NumCells;
        private int NumCellsOption = NumCells;  // The selected size number
        ToolStripMenuItem selectedSizeItem;     // Selected size item from the menu

        private bool[,] grid;                   // Stores on/off state of cells in grid 
        private Random rand;                    // Used to generate random numbers 


        public MainForm()
        {
            InitializeComponent();
            rand = new Random();    // Initializes random number generator 
            resizeBoard(NumCells);

        }

        private void GameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // Fill grid with either white or black 
            RandomizeGrid();

        }

        private void RandomizeGrid()
        {
            // Fill grid with either white or black 
            for (int r = 0; r < NumCellsOption; r++)
                for (int c = 0; c < NumCellsOption; c++)
                    grid[r, c] = rand.Next(2) == 1;

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

            Int32 width = control.Size.Width;
            CellLength = (width - (GridOffset * 3)) / NumCellsOption;

            for (int r = 0; r < NumCellsOption; r++)
            {
                for (int c = 0; c < NumCellsOption; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section 
                    Brush brush;
                    Pen pen;
                    if (grid[r, c])
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
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * NumCellsOption + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * NumCellsOption + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            // Invert selected box and all surrounding boxes
            for (int i = r - 1; i <= r + 1; i++)
                for (int j = c - 1; j <= c + 1; j++)
                   if (i >= 0 && i < NumCellsOption && j >= 0 && j < NumCellsOption)
                        grid[i, j] = !grid[i, j];

            // Redraw grid 
            this.Invalidate();

            // Check to see if puzzle has been solved 
            if (PlayerWon())
            {
                // Display winner dialog box 
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private bool PlayerWon()
        {
            bool result = false;
            for (int r = 0; r < NumCellsOption; r++)
                for (int c = 0; c < NumCellsOption; c++)
                    result = grid[r, c] || result;
            return !result;
        }

        private void resizeBoard(int size)
        {
            NumCellsOption = size;
            grid = new bool[size, size];

            // Turn entire grid on 
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    grid[r, c] = true;

            // Redraw grid
            RandomizeGrid();

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

            // Check new
            selectedSizeItem.Checked = true;

        }


        private void X3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleSizeOptionClick((ToolStripMenuItem)sender);
            resizeBoard(3);
        }

        private void X4ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HandleSizeOptionClick((ToolStripMenuItem)sender);
            resizeBoard(4);
        }

        private void X5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HandleSizeOptionClick((ToolStripMenuItem)sender);
            resizeBoard(5);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {

            // Redraw grid 
            this.Invalidate();

        }
    }
}
