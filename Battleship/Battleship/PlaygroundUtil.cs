using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class PlaygroundUtil
    {
        public static int playground_size;
        int[] ships = { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        int[] shipsWithoutOnes = { 4, 3, 3, 2, 2, 2 };
        int[,] linePositions = { { 7, 9 }, { 9, 7 }, { 0, 2 }, { 2, 0 }, { 9, 0 }, { 0, 9 } };
        int[] ones = { 1, 1, 1, 1 };
        Ship[,] firsLine = {{new Ship(3, 0), new Ship(3, 4), new Ship(2, 8)},
                            {new Ship(3, 0), new Ship(2, 4), new Ship(3, 7)},
                            {new Ship(2, 0), new Ship(3, 3), new Ship(3, 7)}};
        Ship[,] secondLine = {{new Ship(2, 0), new Ship(2, 3), new Ship(4, 6)},
                            {new Ship(2, 0), new Ship(4, 3), new Ship(2, 8)},
                            {new Ship(4, 0), new Ship(2, 5), new Ship(2, 8)}};
        Ship[] firstAngle = {new Ship(4, 0, 0, false), new Ship(3, 0, 5, false), new Ship(2, 0, 9, true),
                            new Ship(3, 2, 0, true), new Ship(2, 6, 0, true), new Ship(2, 9, 0, false)};
        Ship[] secondAngle = {new Ship(4, 0, 0, false), new Ship(3, 0, 5, false), new Ship(2, 0, 9, true),
                            new Ship(2, 2, 0, true), new Ship(3, 5, 0, true), new Ship(2, 9, 0, false)};
        Random random = new Random();

        public PlaygroundUtil(int size)
        {
            playground_size = size;
        }
        public int[,] CreateEmptyPlayground()
        {
            int[,] playground = new int[playground_size, playground_size];

            for (int i = 0; i < playground_size; i++)
            {
                for (int j = 0; j < playground_size; j++)
                {
                    playground[i, j] = 0;
                }
            }

            return playground;
        }

        public int[,] GenerateFullRandomPlayground()
        {
            int[,] playground = GenerateRandomPlaygroundWithoutOnes();
            GenerateOnesShip(playground);
            return playground;
        }

        private void GenerateOnesShip(int[,] playground)
        {
            for (int i = 0; i < ones.Length; i++)
            {
                Ship ship = new Ship(ones[i]);
                GenerateCoordinates(playground, ship);
                AddShipRegion(playground, ship);
                PlaceShipOnPlayground(playground, ship);

            }
        }

        public int[,] GenerateRandomPlaygroundWithoutOnes()
        {
            int[,] playground = CreateEmptyPlayground();
            for (int i = 0; i < shipsWithoutOnes.Length; i++)
            {
                Ship ship = new Ship(ships[i]);
                GenerateCoordinates(playground, ship);
                AddShipRegion(playground, ship);
                PlaceShipOnPlayground(playground, ship);

            }
            return playground;

        }

        private void GenerateCoordinates(int[,] playground, Ship ship)
        {
            GetCoordinates(ship);

            while (CheckPosition(playground, ship))
            {
                GetCoordinates(ship);
            }
        }

        private void GetCoordinates(Ship ship)
        {
            ship.IsVertical = random.Next(2) == 0;
            if (ship.IsVertical)
            {

                ship.X = random.Next(playground_size - ship.Size + 1);
                ship.Y = random.Next(playground_size);
            }
            else
            {
                ship.Y = random.Next(playground_size - ship.Size + 1);
                ship.X = random.Next(playground_size);
            }
        }

        private bool CheckPosition(int[,] playground, Ship ship)
        {
            int[] region = GetShipRegionCoordinates(ship);
            int fromX = region[0];
            int toX = region[1];
            int fromY = region[2];
            int toY = region[3];

            for (int i = fromX; i <= toX; i++)
            {
                for (int j = fromY; j <= toY; j++)
                {
                    if (playground[i, j] == 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void PlaceShipOnPlayground(int[,] playground, Ship ship)
        {
            if (ship.IsVertical)
            {
                for (int i = ship.X; i < ship.Size + ship.X; i++)
                {
                    playground[i, ship.Y] = 1;
                }
            }
            else
            {
                for (int i = ship.Y; i < ship.Size + ship.Y; i++)
                {
                    playground[ship.X, i] = 1;
                }
            }
        }

        public void AddShipRegion(int[,] playground, Ship ship)
        {
            int[] region = GetShipRegionCoordinates(ship);
            int fromX = region[0];
            int toX = region[1];
            int fromY = region[2];
            int toY = region[3];

            if (toY == 10)
            {
                toY -= 1;
            }

            for (int i = fromX; i <= toX; i++)
            {
                for (int j = fromY; j <= toY; j++)
                {
                    if (playground[i, j] == 0)
                    {
                        playground[i, j] = -1;
                    }
                }
            }
        }

        private int[] GetShipRegionCoordinates(Ship ship)
        {
            int[] region = new int[4];

            int kx = ship.IsVertical ? 1 : 0;
            int ky = !ship.IsVertical ? 1 : 0;

            int fromX = ship.X == 0 ? ship.X : ship.X - 1;
            int toX = fromX - kx + ship.Size * kx + 2;

            if (ship.X == playground_size - 1 || ship.X == 0 || ship.X == (playground_size - ship.Size * kx) || toX == playground_size)
            {
                toX -= 1;
            }

            int fromY = ship.Y == 0 ? ship.Y : ship.Y - 1;
            int toY = fromY - ky + ship.Size * ky + 2;

            if (ship.Y == 0 || ship.Y == playground_size - 1 || ship.Y == (playground_size - ship.Size * ky) || toY == playground_size)
            {
                toY -= 1;
            }

            region[0] = fromX;
            region[1] = toX;
            region[2] = fromY;
            region[3] = toY;

            return region;
        }

        public int GetCellNumberForOnesShip(int[,] playground)
        {
            int number = 0;
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    if (playground[i, j] == 0)
                    {
                        number++;
                    }
                }
            }
            return number;
        }

        public int[,] GenerateOptimalPlaygroundWithoutOnes()
        {
            int[,] playground = CreateEmptyPlayground();
            if (IsLineOptimal())
            {
                GenerateLineOptimalPlaygroung(playground);
            }
            else
            {
                GenerateAngleOptimalPlayground(playground);
            }
            return playground;

        }

        private bool IsLineOptimal()
        {
            return random.Next(2) == 0;
        }

        private void GenerateLineOptimalPlaygroung(int[,] playground)
        {
            bool isVertical = random.Next(2) == 0;
            int firstLineNumber = random.Next(firsLine.GetLength(0));
            int secondLineNumber = random.Next(secondLine.GetLength(0));
            int positions = random.Next(linePositions.GetLength(0));
            int firstLinePosition = linePositions[positions, 0];
            int secondLinePosition = linePositions[positions, 1];

            PlaceLine(playground, firsLine, firstLineNumber, firstLinePosition, isVertical);
            PlaceLine(playground, secondLine, secondLineNumber, secondLinePosition, isVertical);
        }

        private void PlaceLine(int[,] playground, Ship[,] line, int lineNumber, int linePosition, bool isVertical)
        {
            for (int i = 0; i < line.GetLength(1); i++)
            {
                Ship ship = line[lineNumber, i];
                ship.IsVertical = isVertical;
                if (isVertical)
                {
                    ship.X = ship.Position;
                    ship.Y = linePosition;
                }
                else
                {
                    ship.Y = ship.Position;
                    ship.X = linePosition;
                }
                AddShipRegion(playground, ship);
                PlaceShipOnPlayground(playground, ship);
            }
        }

        private void GenerateAngleOptimalPlayground(int[,] playground)
        {
            bool isFirstAgngle = random.Next(2) == 0;
            PlaceAngle(playground, isFirstAgngle ? firstAngle : secondAngle);
            FlipOverPlayground(playground, random.Next(4));
        }

        private void PlaceAngle(int[,] playground, Ship[] angle)
        {
            for (int i = 0; i < angle.Length; i++)
            {
                AddShipRegion(playground, angle[i]);
                PlaceShipOnPlayground(playground, angle[i]);
            }
        }

        private void FlipOverPlayground(int[,] playground, int flipNumber)
        {
            for (int i = 0; i < flipNumber; i++)
            {
                FlipOver90(playground);
            }
        }

        private void FlipOver90(int[,] playground)
        {
            int[,] rotated = Copy(playground);

            for (int i = 0; i < rotated.GetLength(0); i++)
            {
                for (int j = 0; j < rotated.GetLength(1); j++)
                {
                    playground[playground.GetLength(0) - 1 - j, i] = rotated[i, j];
                }
            }
        }

        public int[,] Copy(int[,] playground)
        {
            int[,] copy = CreateEmptyPlayground();
            for (int i = 0; i < playground.GetLength(0); i++)
            {
                for (int j = 0; j < playground.GetLength(1); j++)
                {
                    copy[i, j] = playground[i, j];
                }
            }
            return copy;
        }

        public int[,] GenerateFullOptimalPlayground()
        {
            int[,] playground = GenerateOptimalPlaygroundWithoutOnes();
            GenerateOnesShip(playground);
            return playground;
        }

    }
}
