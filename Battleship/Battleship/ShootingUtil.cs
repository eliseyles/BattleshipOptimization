using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class ShootingUtil
    {
        PlaygroundUtil playgroundUtil = new PlaygroundUtil(10);
        int[,] shootingPlayground;
        int[,] playground;
        List<Ship> ships;

        List<int[]> emptyCoord;
        public int result = 0;
        List<int[]> shipList;
        bool flag;
        int isVertical = 0;
        List<int[]> neighbour;
        int[] cell;
        public int shootingNumber = 0;

        public List<int> randomNumber = new List<int>();
        public List<int> optimalNumber = new List<int>();

        int[,] startList = new int[,] { { 3, 0 }, { 7, 0 }, { 9, 2 }, { 9, 6 }, { 1, 0 }, { 5, 0 }, { 9, 0 }, { 9, 4 }, { 9, 8 } };
        int startPosition = 0;
        int[] optimalCell;

        Random random = new Random();

        private void GenerateNewRandomPlayground()
        {
            shootingPlayground = playgroundUtil.GenerateFullRandomPlayground();
            playground = playgroundUtil.CreateEmptyPlayground();
            ships = GenerateShipList();
            GenerateEmptyCellList();
            shipList = new List<int[]>();
            shootingNumber = 0;
            result = 0;
            startPosition = 0;
            startPosition = 0;
            flag = false;
            isVertical = 0;
            optimalCell = null;
            cell = null;
        }

        private void CleanPlayground()
        {
            playground = playgroundUtil.CreateEmptyPlayground();
            ships = GenerateShipList();
            GenerateEmptyCellList();
            shipList = new List<int[]>();
            shootingNumber = 0;
            result = 0;
        }

        #region gettind playgrounds
        public int[,] GetShootingPlayground()
        {
            if (shootingPlayground == null) { GenerateNewRandomPlayground(); }
            return shootingPlayground;
        }

        public int[,] GetNewShootingPlayground()
        {
            GenerateNewRandomPlayground();
            return shootingPlayground;
        }

        public int[,] GetPlayground()
        {
            return playground;
        }
        #endregion 

        #region server part
        private int IsHit(int x, int y)
        {
            int result = 0;
            if (shootingPlayground[x, y] == 1)
            {
                result = 1;
                bool flag = false;
                for (int i = 0; !flag && i < ships.Count; i++)
                {
                    for (int j = 0; !flag && j < ships[i].Coordinates.Count; j++)
                    {
                        if (ships[i].Coordinates[j][0] == x && ships[i].Coordinates[j][1] == y)
                        {

                            ships[i].Coordinates.RemoveAt(j);
                            flag = true;
                            break;
                        }

                    }
                    if (ships[i].Coordinates.Count == 0)
                    {
                        result = 2;
                        ships.RemoveAt(i);
                    }
                }
                if (ships.Count == 0)
                {
                    result = 3;
                }
            }
            Console.WriteLine(result);
            return result;
        }

        private List<Ship> GenerateShipList()
        {
            int[,] copy = playgroundUtil.Copy(shootingPlayground);
            List<Ship> ships = new List<Ship>();
            for (int i = 0; i < copy.GetLength(0); i++)
            {
                for (int j = 0; j < copy.GetLength(1); j++)
                {
                    if (copy[i, j] == 1)
                    {
                        ships.Add(GenerateShip(copy, i, j));
                    }
                }
            }
            return ships;
        }

        private Ship GenerateShip(int[,] playground, int x, int y)
        {
            Ship ship = new Ship(0);
            ship.X = x;
            ship.Y = y;
            if (isHorizontal(x, y))
            {
                while (y < 10 && playground[x, y] == 1)
                {
                    ship.Coordinates.Add(new int[] { x, y });
                    ship.Size++;
                    playground[x, y] = 0;
                    y++;

                }
            }
            else
            {
                while (x < 10 && playground[x, y] == 1)
                {
                    ship.Coordinates.Add(new int[] { x, y });
                    ship.Size++;
                    playground[x, y] = 0;
                    x++;
                }
            }
            return ship;
        }

        private bool isHorizontal(int x, int y)
        {
            if (y != 9)
            {
                return shootingPlayground[x, y + 1] == 1;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region random algo
        private void GenerateEmptyCellList()
        {
            emptyCoord = new List<int[]>();
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    if (playground[i, j] == 0)
                    {
                        emptyCoord.Add(new int[] { i, j });
                    }
                }
            }
        }

        private int[] GenerateShotingCell()
        {
            int index = random.Next(emptyCoord.Count);
            int[] result = emptyCoord[index];
            emptyCoord.RemoveAt(index);
            return result;
        }

        public int[,] Shoot()
        {
            shootingNumber++;
            if (result != 3)
            {
                CheckNeighborhood();
                result = IsHit(cell[0], cell[1]);
                CheckResult();
            }

            return playground;
        }

        private int[] CheckNeighborhood()
        {
            int position;
            bool fl = false;
            if (result == 0 && flag == false && !fl)
            {
                Console.WriteLine("res 0 fl 0");
                cell = emptyCoord[random.Next(emptyCoord.Count)];
                fl = true;
            }
            if (result == 1 && flag == false && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 1 fl 0 iV 0");
                flag = true;
                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                AddNeighbour();
                position = random.Next(neighbour.Count);
                cell = neighbour[position];
                neighbour.RemoveAt(position);
                fl = true;
            }
            if (result == 0 && flag == true && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 0 fl 1 iV 0");
                position = random.Next(neighbour.Count);
                cell = neighbour[position];
                neighbour.RemoveAt(position);
                fl = true;
            }
            if (result == 1 && flag == true && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 1 fl 1 iV 0");
                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                GetDimension();
                GetNextShipCell();
                fl = true;
            }
            if (result == 0 && flag == true && isVertical != 0 && !fl)
            {
                Console.WriteLine("res 0 fl 1 iV 1");

                cell = shipList[0];
                GetNextShipCell();
                fl = true;
            }
            if (result == 1 && flag == true && isVertical != 0 && !fl)
            {
                Console.WriteLine("res 1 fl 1 iV 1");

                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                GetNextShipCell();
                fl = true;
            }

            return cell;
        }

        private void CheckResult()
        {
            if (result == 0)
            {
                playground[cell[0], cell[1]] = -1;
            }
            if (result == 2)
            {
                playground[cell[0], cell[1]] = 1;
                shipList.Add(new int[] { cell[0], cell[1] });
                int[] first = GetLeftAndHighCell();
                playgroundUtil.AddShipRegion(playground, new Ship(shipList.Count, first[0], first[1], isVertical == 1 ? true : false));

                neighbour = null;
                shipList = new List<int[]>();
                isVertical = 0;
                flag = false;
                result = 0;
                GenerateEmptyCellList();
            }
            if (result == 3)
            {
                playground[cell[0], cell[1]] = 1;
                shipList.Add(new int[] { cell[0], cell[1] });
                int[] first = GetLeftAndHighCell();
                playgroundUtil.AddShipRegion(playground, new Ship(shipList.Count, first[0], first[1], isVertical == 1 ? true : false));
            }
            if (result == 1)
            {
                playground[cell[0], cell[1]] = 1;
            }


        }

        private void AddNeighbour()
        {
            neighbour = new List<int[]>();
            if (cell[0] != 0)
            {
                neighbour.Add(new int[] { cell[0] - 1, cell[1] });
            }
            if (cell[0] != 9)
            {
                neighbour.Add(new int[] { cell[0] + 1, cell[1] });
            }
            if (cell[1] != 0)
            {
                neighbour.Add(new int[] { cell[0], cell[1] - 1 });
            }
            if (cell[1] != 9)
            {
                neighbour.Add(new int[] { cell[0], cell[1] + 1 });
            }
        }

        private void GetDimension()
        {
            if (cell[0] == shipList[0][0])
            {
                isVertical = 2;
            }
            else
            {
                isVertical = 1;
            }
        }

        private void GetNextShipCell()
        {
            if (isVertical == 1)
            {
                if (shipList[0][0] > shipList[shipList.Count - 1][0])
                {
                    if (cell[0] - 1 != -1 && playground[cell[0] - 1, cell[1]] == 0)
                    {
                        cell[0] -= 1;
                    }
                    else
                    {
                        cell = shipList[0];
                        cell[0] += 1;
                    }
                }
                else
                {
                    if (cell[0] + 1 != 10 && playground[cell[0] + 1, cell[1]] == 0)
                    {
                        cell[0] += 1;
                    }
                    else
                    {
                        cell = shipList[0];
                        cell[0] -= 1;
                    }
                }

            }
            else
            {
                if (shipList[0][1] > shipList[shipList.Count - 1][1])
                {
                    if (cell[1] - 1 != -1 && playground[cell[0], cell[1] - 1] == 0)
                    {
                        cell[1] -= 1;
                    }
                    else
                    {
                        cell = shipList[0];
                        cell[1] += 1;
                    }
                }
                else
                {
                    if (cell[1] + 1 != 10 && playground[cell[0], cell[1] + 1] == 0)
                    {
                        cell[1] += 1;
                    }
                    else
                    {
                        cell = shipList[0];
                        cell[1] -= 1;
                    }
                }
            }
        }

        private int[] GetLeftAndHighCell()
        {
            int[] leftAndHightCell = { 9, 9 };
            for (int i = 0; i < shipList.Count; i++)
            {
                if (shipList[i][0] < leftAndHightCell[0] || shipList[i][1] < leftAndHightCell[1])
                {
                    leftAndHightCell = shipList[i];
                }
            }
            return leftAndHightCell;
        }
        #endregion

        public void LoopGenerationRandom(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateNewRandomPlayground();
                while (result != 3)
                {
                    Shoot();
                }
                randomNumber.Add(shootingNumber);
            }
        }
        public void LoopGenerationOptimal(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GenerateNewRandomPlayground();
                while (result != 3)
                {
                    ShootOptimal();
                }
                optimalNumber.Add(shootingNumber);
            }
        }

        #region optimal algo
        public int[,] ShootOptimal()
        {
            if (result != 3)
            {
                CheckNeighborhoodOptimal();
                result = IsHit(cell[0], cell[1]);
                CheckOptimalResult();
            }
            return playground;
        }

        private int[] GetNextOptimalCell()
        {
            if (optimalCell == null)
            {
                optimalCell = new int[] { startList[startPosition, 0], startList[startPosition, 1] };
            }
            else
            {
                if (playground[optimalCell[0], optimalCell[1]] != 0)
                {
                    if (optimalCell[0] != 0 && optimalCell[1] != 9)
                    {
                        optimalCell[0] -= 1;
                        optimalCell[1] += 1;
                        GetNextOptimalCell();
                    }
                    else
                    {
                        if (startPosition < startList.GetLength(0) - 1)
                        {
                            startPosition++;
                            optimalCell = new int[] { startList[startPosition, 0], startList[startPosition, 1] };
                        }
                        else
                        {
                            GenerateEmptyCellList();
                            optimalCell = emptyCoord[random.Next(emptyCoord.Count)];
                        }
                        
                    }
                }
            }
            return optimalCell;
        }

        private void CheckOptimalResult()
        {
            shootingNumber++;
            if (result == 0)
            {
                playground[cell[0], cell[1]] = -1;
            }
            if (result == 2)
            {
                playground[cell[0], cell[1]] = 1;
                shipList.Add(new int[] { cell[0], cell[1] });
                int[] first = GetLeftAndHighCell();
                playgroundUtil.AddShipRegion(playground, new Ship(shipList.Count, first[0], first[1], isVertical == 1 ? true : false));

                neighbour = null;
                shipList = new List<int[]>();
                isVertical = 0;
                flag = false;
                result = 0;
                GenerateEmptyCellList();
            }
            if (result == 3)
            {
                playground[cell[0], cell[1]] = 1;
                shipList.Add(new int[] { cell[0], cell[1] });
                int[] first = GetLeftAndHighCell();
                playgroundUtil.AddShipRegion(playground, new Ship(shipList.Count, first[0], first[1], isVertical == 1 ? true : false));
            }
            if (result == 1)
            {
                playground[cell[0], cell[1]] = 1;
            }
        }

        private int[] CheckNeighborhoodOptimal()
        {
            int position;
            bool fl = false;
            if (result == 0 && flag == false && !fl)
            {
                Console.WriteLine("res 0 fl 0");
                cell = GetNextOptimalCell();
                fl = true;
            }
            if (result == 1 && flag == false && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 1 fl 0 iV 0");
                flag = true;
                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                AddNeighbour();
                position = random.Next(neighbour.Count);
                cell = neighbour[position];
                neighbour.RemoveAt(position);
                fl = true;
            }
            if (result == 0 && flag == true && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 0 fl 1 iV 0");
                position = random.Next(neighbour.Count);
                cell = neighbour[position];
                neighbour.RemoveAt(position);
                fl = true;
            }
            if (result == 1 && flag == true && isVertical == 0 && !fl)
            {
                Console.WriteLine("res 1 fl 1 iV 0");
                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                GetDimension();
                GetNextShipCell();
                fl = true;
            }
            if (result == 0 && flag == true && isVertical != 0 && !fl)
            {
                Console.WriteLine("res 0 fl 1 iV 1");

                cell = shipList[0];
                GetNextShipCell();
                fl = true;
            }
            if (result == 1 && flag == true && isVertical != 0 && !fl)
            {
                Console.WriteLine("res 1 fl 1 iV 1");

                shipList.Add(new int[] { cell[0], cell[1] });
                Console.WriteLine(cell[0] + " " + cell[1]);
                GetNextShipCell();
                fl = true;
            }

            return cell;
        }
        #endregion
    }
}
