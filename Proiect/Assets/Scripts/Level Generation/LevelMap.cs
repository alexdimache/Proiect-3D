using UnityEngine;
using System.Collections.Generic;
using LevelGeneration;

public class LevelMap
{
    //array of rooms
    private List<Room> rooms;
    //number of lines of the level array
    private int nrOfLines = 60;
    //the level map array
    private char[,] level;
    //the maximum number of rooms in the level
    private int maxNrOfRooms;
    //room dimensions
    private int minDimension = 5;
    private int maxDimension = 9;
    //blocking tiles
    private string traversableTiles = "xT";

    #region Populating Level Map
    // Use this for initialization
    public LevelMap()
    {
        Random.InitState(System.Environment.TickCount);
        maxNrOfRooms = Random.Range(10,16);
        rooms = new List<Room>(maxNrOfRooms);
        level = new char[nrOfLines, nrOfLines];
        string debugOutput = "Max nr of rooms: "+ maxNrOfRooms.ToString();
        Debug.Log(debugOutput);
        InitLevel();
        PopulateLevel();
        DrawTunnels();
        //ClearUnusedDoors();
        debugOutput += "\nNumber of rooms added: " + rooms.Count.ToString();
        Debug.Log(debugOutput);
        PrintLevel();
    }

    public int GetNrOfLines()
    {
        return nrOfLines;
    }

    public char[,] GetLevel()
    {
        return level;
    }

    //initializing the level
    private void InitLevel()
    {
        for (int i = 0; i < nrOfLines; i++)
        {
            level[i, 0] = 'X';
            level[i, nrOfLines - 1] = 'X';
            level[0, i] = 'X';
            level[nrOfLines - 1, i] = 'X';
        }
        for (int i = 1; i < nrOfLines - 1; i++)
            for (int j = 1; j < nrOfLines - 1; j++)
                level[i, j] = 'x';

        Debug.Log("LEVEL INITIALIZED. \nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
    }
    
    //returns an array with all 4 doors
    private Point[] GetRoomDoors(int x, int y, int rWidth, int rLen)
    {
        Point[] res = new Point[4];
        int c = 0;
        for (int i = x; i < x + rWidth; i++)
            for (int j = y; j < y + rLen; j++)
                if (level[i, j] == 'D')
                {
                    res[c] = new Point(i, j);
                    c++;
                }
        return res;
    }

    //adds the room to the array and draws it on the level map
    private void AddRoom(Room givenRoom)
    {
        //drawing the room on the level
        for (int i = givenRoom.GetCornerX(), tempI = 0; i < givenRoom.GetCornerX() + givenRoom.GetRoomWidth(); i++, tempI++)
            for (int j = givenRoom.GetCornerY(), tempJ = 0; j < givenRoom.GetCornerY() + givenRoom.GetRoomLength(); j++, tempJ++)
            {
                level[i, j] = givenRoom.GetRoomTiles()[tempI, tempJ];
            }
        givenRoom.SetDoors(GetRoomDoors(givenRoom.GetCornerX(), givenRoom.GetCornerY(), givenRoom.GetRoomWidth(), givenRoom.GetRoomLength()));

        Debug.Log("ROOM ADDED.");
    }

    //checks the area on the level to see if the room colides with another
    private bool CheckArea(int cornerX, int cornerY, int rWidth, int rLen)
    {
        for (int i = cornerX - 2; i < cornerX + rWidth + 3; i++)
            for (int j = cornerY - 2; j < cornerY + rLen + 3; j++)
                if (level[i, j] != 'x')
                    return false;
        return true;
    }

    //populates the level with rooms
    private void PopulateLevel()
    {
        Random.InitState(System.Environment.TickCount);

        //Starting room (9 tiles, 5x5 with walls)
        int rLength;
        int rWidth;
        int givenX = Random.Range(3, nrOfLines - 9);
        int givenY = Random.Range(3, nrOfLines - 9);
        
        rooms.Add(new StartRoom(givenX, givenY));
        AddRoom(rooms[0]);
        
        //Boss room (81 tiles, 11x11 with walls)
        bool bossRoomPlaced = false;
        while (!bossRoomPlaced)
        {
            givenX = Random.Range(3, nrOfLines - 4);
            givenY = Random.Range(3, nrOfLines - 4);
            if (CheckArea(givenX, givenY, 11, 11))
            {
                rooms.Add(new BossRoom(givenX, givenY));
                AddRoom(rooms[1]);
                bossRoomPlaced = true;
            }
        }
        /*
        //the rest of the rooms
        while (rooms.Count < maxNrOfRooms)
        {
            rLength = Random.Range(minDimension, maxDimension + 1);
            rWidth = Random.Range(minDimension, maxDimension + 1);
            givenX = Random.Range(3, nrOfLines - 4);
            givenY = Random.Range(3, nrOfLines - 4);

            if (CheckArea(givenX, givenY, rWidth, rLength))
            {
                rooms.Add(new NormalRoom(givenX, givenY, rLength, rWidth));
                AddRoom(rooms[rooms.Count - 1]);
            }
        }*/
        
        Debug.Log("LEVEL POPULATED. \nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
    }
    #endregion

    #region Pathfinding - A*

    //Manhattan distance
    private int Distance(Point p1, Point p2)
    {
        int xDist = Mathf.Abs(p1.GetX() - p2.GetX());
        int yDist = Mathf.Abs(p1.GetY() - p2.GetY());
        return xDist + yDist;
    }

    /*
     * returns the valid neighbors
     * given by the traversable tiles array
     */
    private List<Point> GetValidNeighbors(List<Point> neighbors)
    {
        List<Point> res = new List<Point>();
        foreach (Point neighbor in neighbors)
            if (traversableTiles.IndexOf(level[neighbor.GetX(), neighbor.GetY()]) != -1)
                res.Add(neighbor);
        return res;
    }

    //returns the lowest fScore from the openSet
    private Point GetLowestScore(List<Point> openSet, Dictionary<Point, int> fScore, Point destination)
    {
        Point closestPoint = openSet[0];

        for (int i = 1; i < openSet.Count; i++)
            if (fScore[closestPoint] > fScore[openSet[i]] || fScore[closestPoint] == fScore[openSet[i]] && 
                                                             Distance(closestPoint, destination) > Distance(openSet[i], destination))
                closestPoint = openSet[i];

        return closestPoint;
    }

    //used instead of overriding equals
    private bool AreEqual(Point node1, Point node2)
    {
        if (node1.GetX() == node2.GetX() &&
            node1.GetY() == node2.GetY())
            return true;
        return false;
    }

    //A* BEGIN
    private int FindPath(Point start, Point destination)
    {
        destination = GetValidNeighbors(destination.GetNeighbors(level))[0];
        start = GetValidNeighbors(start.GetNeighbors(level))[0];

        HashSet<Point> closedSet = new HashSet<Point>();
        List<Point> openSet = new List<Point> { start };
        Dictionary<Point, int> gScore = new Dictionary<Point, int> { { start, 0 } };
        Dictionary<Point, int> fScore = new Dictionary<Point, int> { { start, Distance(start, destination) } };
        Dictionary<Point, Point> cameFrom = new Dictionary<Point, Point>();

        Point current = start;

        int runs = 0;

        while (openSet.Count != 0 && runs<3000)
        {
            current = GetLowestScore(openSet, fScore, destination);

            openSet.Remove(current);
            closedSet.Add(current);

            if (AreEqual(current, destination))
            {
                Debug.Log("PATH FOUND.\nRUNS: " + runs.ToString() + "\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
                return GenerateTunnel(start, current, cameFrom);
            }

            foreach (Point neighbor in GetValidNeighbors(current.GetNeighbors(level)))
            {
                Debug.Log("LOOP FOREACH");
                if (closedSet.Contains(neighbor))
                    continue;

                int possibleScore = gScore[current] + 1;

                if (openSet.IndexOf(neighbor) == -1)
                    openSet.Add(neighbor);
                else
                    if (possibleScore < gScore[neighbor])
                        continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = possibleScore;
                fScore[neighbor] = possibleScore + Distance(neighbor, destination);
            }
            runs++;
        }

        Debug.Log("PATH NOT FOUND.\nRUNS: " + runs.ToString() +
            "\nSTART: " + start.GetX() + " " + start.GetY() +
            "\nDEST:  " + destination.GetX() + " " + destination.GetY()
            + "\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
        return GenerateTunnel(start, current, cameFrom);
        //return -1;
    }
    //A* END

    //draws the tunnel on the level map
    private int GenerateTunnel(Point start, Point current, Dictionary<Point, Point> cameFrom)
    {
        //variable used to see if anything got generated
        Point tmpDest = current;

        while (!AreEqual(current, start))
        {
            level[current.GetX(), current.GetY()] = 'T';
            current = cameFrom[current];
        }
        level[current.GetX(), current.GetY()] = 'T';

        Debug.Log("TUNNEL GENERATED.\nSTART: "+start.GetX()+" "+ start.GetY()+
                                    "\nDEST:  "+ tmpDest.GetX() + " " + tmpDest.GetY()
                                    +"\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
        return 0;
    }

    #endregion

    #region Drawing Tunnels
    //draws the tunnels between each two rooms
    private void DrawTunnels()
    {
        FindPath(rooms[0].GetDoors()[0], rooms[1].GetDoors()[3]);
    }
    #endregion

    //deletes any door that doesn't have an adjacent tunnel and makes it a wall
    private void ClearUnusedDoors()
    {
        for (int i = 0; i < nrOfLines - 1; i++)
            for (int j = 0; j < nrOfLines - 1; j++)
                if (level[i, j] == 'D' &&
                    (level[i, j + 1] != 'T' && level[i, j - 1] != 'T' && level[i + 1, j] != 'T' && level[i - 1, j] != 'T'))
                    level[i, j] = 'W';
    }

    //prints the level in the console used for debugging
    private void PrintLevel()
    {
        string output = null;
        for (int i = 0; i < nrOfLines; i++)
        {
            for (int j = 0; j < nrOfLines; j++)
                output += level[i, j] + " ";
            output += '\n';
        }
        Debug.Log(output);
    }
}
