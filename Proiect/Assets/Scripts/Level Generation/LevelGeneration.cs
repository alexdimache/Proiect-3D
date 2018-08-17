using System.Collections.Generic;

namespace LevelGeneration
{
    public class Point
    {
        private int x;
        private int y;
        private List<Point> neighbors;

        public Point(int givenX, int givenY)
        {
            x = givenX;
            y = givenY;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public List<Point> GetNeighbors(char[,] level)
        {
            neighbors = new List<Point>
            {
                new Point(x - 1, y),
                new Point(x + 1, y),
                new Point(x, y - 1),
                new Point(x, y + 1)
            };
            return neighbors;
        }
    }
    
}

