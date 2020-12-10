using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Form1 : Form
    {
        int[,] playgroundRandom;
        int[,] playgroundOptimal;
        PlaygroundUtil PlaygroundUtil = new PlaygroundUtil(10);

        public Form1()
        {
            InitializeComponent();

            playgroundRandom = PlaygroundUtil.CreateEmptyPlayground();
            playgroundOptimal = PlaygroundUtil.CreateEmptyPlayground();

            DataGridViewInit(dataGridView1, playgroundRandom);
            DataGridViewInit(dataGridView2, playgroundOptimal);

            DrawPlayground(dataGridView1, playgroundRandom);
            DrawPlayground(dataGridView2, playgroundOptimal);

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
        }

        private void DrawPlayground(DataGridView dataGridView, int[,] playground)
        {
            int i = 0;
            int j = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                j = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {

                    if (playground[i, j] == 1)
                    {
                        cell.Style.BackColor = Color.Chocolate;
                    }
                    else if (playground[i, j] == -1)
                    {
                        cell.Style.BackColor = Color.Yellow;
                    }
                    else
                    {
                        cell.Style.BackColor = Color.Azure;
                    }
                    j++;
                }
                i++;

            }
        }

        private void Random_playground_Click(object sender, EventArgs e)
        {
            playgroundRandom = PlaygroundUtil.GenerateRandomPlaygroundWithoutOnes();
            DrawPlayground(dataGridView1, playgroundRandom);
            label1.Text = "Available cells for single-deck ships = " + PlaygroundUtil.GetCellNumberForOnesShip(playgroundRandom);
        }

        private void OptimalPlayground_Click(object sender, EventArgs e)
        {
            playgroundOptimal = PlaygroundUtil.GenerateOptimalPlaygroundWithoutOnes();
            DrawPlayground(dataGridView2, playgroundOptimal);
            label2.Text = "Available cells for single-deck ships = " + PlaygroundUtil.GetCellNumberForOnesShip(playgroundOptimal);

        }

        private void GenerateBoth_Click(object sender, EventArgs e)
        {
            Random_playground_Click(sender, e);
            OptimalPlayground_Click(sender, e);

        }

        private void DataGridViewInit(DataGridView dataGridView, int[,] playground)
        {

            for (int i = 0; i < playground.GetLength(0); i++)
            {
                string[] row = new string[playground.GetLength(1)];

                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    row[j] = "";
                }

                dataGridView.Rows.Add(row);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
