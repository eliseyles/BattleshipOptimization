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
        int[,] shootedPlayground;
        int[,] playground;
        PlaygroundUtil PlaygroundUtil = new PlaygroundUtil(10);
        ShootingUtil shootingUtil = new ShootingUtil();

        public Form1()
        {
            InitializeComponent();

            playgroundRandom = PlaygroundUtil.CreateEmptyPlayground();
            playgroundOptimal = PlaygroundUtil.CreateEmptyPlayground();
            shootedPlayground = PlaygroundUtil.CreateEmptyPlayground();
            playground = PlaygroundUtil.CreateEmptyPlayground();

            DataGridViewInit(dataGridView1, playgroundRandom);
            DataGridViewInit(dataGridView2, playgroundOptimal);
            DataGridViewInit(dataGridView3, shootedPlayground);
            DataGridViewInit(dataGridView4, playground);

            DrawPlayground(dataGridView1, playgroundRandom);
            DrawPlayground(dataGridView2, playgroundOptimal);
            DrawPlayground(dataGridView3, shootedPlayground);
            DrawPlayground(dataGridView4, playground);
            Shoot.Enabled = false;
            ShootOptimal.Enabled = false;

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView2.ClearSelection();
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView3.ClearSelection();
        }

        private void dataGridView4_SelectionChanged(object sender, EventArgs e)
        {
            dataGridView4.ClearSelection();
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

        private void GenerateBothFull_Click(object sender, EventArgs e)
        {
            playgroundRandom = PlaygroundUtil.GenerateFullRandomPlayground();
            DrawPlayground(dataGridView1, playgroundRandom);
            label1.Text = "";
            playgroundOptimal = PlaygroundUtil.GenerateFullOptimalPlayground();
            DrawPlayground(dataGridView2, playgroundOptimal);
            label2.Text = "";
        }

        private void GetNewPlayground_Click(object sender, EventArgs e)
        {
            shootedPlayground = shootingUtil.GetNewShootingPlayground();
            DrawPlayground(dataGridView3, shootedPlayground);
            Shoot.Enabled = true;
            ShootOptimal.Enabled = true;
            playground = shootingUtil.GetPlayground();
            DrawPlayground(dataGridView4, playground);
        }

        private void Shoot_Click(object sender, EventArgs e)
        {
            playground =  shootingUtil.Shoot();
            DrawPlayground(dataGridView4, playground);
            if (shootingUtil.result == 3)
            {
                label3.Text = "You win! Shooting number = " + shootingUtil.shootingNumber;
            }
            ShootOptimal.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Int32.Parse(textBox1.Text);
                shootingUtil.LoopGenerationRandom(count);
                label4.Text = "Random algo results: " + GetStringFromList(shootingUtil.randomNumber);
            } catch(FormatException)
            {
                MessageBox.Show("Wrong data", "Can't parse data", MessageBoxButtons.OK);
            }
        }

        private String GetStringFromList(List<int> list)
        {
            String result = "";
            foreach(var i in list)
            {
                result += i;
                result += " ";
            }
            return result;
        }

        private void ShootOptimal_Click(object sender, EventArgs e)
        {
            playground = shootingUtil.ShootOptimal();
            DrawPlayground(dataGridView4, playground);
            if (shootingUtil.result == 3)
            {
                label3.Text = "You win! Shooting number = " + shootingUtil.shootingNumber;
            }
            Shoot.Enabled = false;
        }

        private void OptimalLoop_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Int32.Parse(textBox1.Text);
                shootingUtil.LoopGenerationOptimal(count);
                label6.Text = "Optimal algo results: " + GetStringFromList(shootingUtil.optimalNumber);
            }
            catch (FormatException)
            {
                MessageBox.Show("Wrong data", "Can't parse data", MessageBoxButtons.OK);
            }
        }
    }
    
}
