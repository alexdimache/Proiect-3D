using System.Collections.Generic;
namespace LevelGeneration
{
    public class Point
    {
        private int x;
        private int y;

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
            List<Point> res = new List<Point>
            {
                new Point(x - 1, y),
                new Point(x + 1, y),
                new Point(x, y - 1),
                new Point(x, y + 1)
            };

            return res;
        }
    }
    //used for A* in the Level script to draw tunnels between rooms
    public class AStarNode
    {
        private Point value;
        private AStarNode origin;

        private float fScore;
        private float gScore;

        public AStarNode(Point givenValue)
        {
            value = givenValue;
            origin = null;
        }

        public void SetOrigin(AStarNode givenOrigin)
        {
            origin = givenOrigin;
        }

        public void SetFScore(float givenFScore)
        {
            fScore = givenFScore;
        }

        public void SetGScore(float givenGScore)
        {
            gScore = givenGScore;
        }

        public float GetFScore()
        {
            return fScore;
        }

        public float GetGScore()
        {
            return gScore;
        }

        public Point GetValue()
        {
            return value;
        }

        public AStarNode GetOrigin()
        {
            return origin;
        }
    }
}

