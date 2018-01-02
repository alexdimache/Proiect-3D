using UnityEngine;
using System.Collections.Generic;
using LevelGeneration;

public class Level
{
    //array of rooms
    private static List<Room> rooms;
    //number of lines of the level array
    private static int nrOfLines = 50;
    //the level array
    private static char[,] level = new char[nrOfLines, nrOfLines];
    //the maximum number of rooms in the level
    private static int maxNrOfRooms;
    //room dimensions
    private int minDimension = 5;
    private int maxDimension = 9;
    //blocking tiles
    private static string traversableTiles = "xT";

    // Use this for initialization
    public Level()
    {
        Random.InitState(System.Environment.TickCount);
        maxNrOfRooms = Random.Range(10,16);
        rooms = new List<Room>(maxNrOfRooms);
        string debugOutput = "Max nr of rooms: "+ maxNrOfRooms.ToString();
        Debug.Log(debugOutput);
        InitLevel();
        PopulateLevel();
        DrawTunnels();
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

    private static Point[] GetRoomDoors(int x, int y, int rWidth, int rLen)
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
    private static void AddRoom(Room givenRoom)
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
    private static bool CheckArea(int cornerX, int cornerY, int rWidth, int rLen)
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
        int givenX = Random.Range(3, nrOfLines - 4);
        int givenY = Random.Range(3, nrOfLines - 4);

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
        }

        Debug.Log("LEVEL POPULATED. \nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
    }

    //A* BEGIN

    //Manhattan distance
    private static float Distance(Point p1, Point p2)
    {
        int xDist = Mathf.Abs(p1.GetX() - p2.GetX());
        int yDist = Mathf.Abs(p1.GetY() - p2.GetY());
        return xDist + yDist;
    }
    /*
     * returns the valid neighbors
     * given by the traversable tiles array
     */
    private static List<AStarNode> GetValidNeighbors(List<Point> neighbors)
    {
        List<AStarNode> res = new List<AStarNode>();
        foreach (Point neighbor in neighbors)
            if (traversableTiles.IndexOf(level[neighbor.GetX(), neighbor.GetY()]) != -1)
                res.Add(new AStarNode(neighbor));
        return res;
    }
    //returns the lowest score from the openSet
    private static AStarNode GetLowestScore(List<AStarNode> openSet, AStarNode current, AStarNode destination)
    {
        AStarNode minPoint = openSet[0];

        foreach (AStarNode openPoint in openSet)
            if (openPoint.GetFScore() < minPoint.GetFScore() || 
                openPoint.GetFScore() == minPoint.GetFScore() 
                && Distance(openPoint.GetValue(), destination.GetValue()) < Distance(current.GetValue(), destination.GetValue()))
                minPoint = openPoint;

        return minPoint;
    }
    //used instead of overriding equals
    private static bool AreEqual(AStarNode node1, AStarNode node2)
    {
        if (node1.GetValue().GetX() == node2.GetValue().GetX() &&
            node1.GetValue().GetY() == node2.GetValue().GetY())
            return true;
        return false;
    }
    //the A* itself
    private static int FindPath(Point pStart, Point pDestination)
    {
        AStarNode destination = GetValidNeighbors(pDestination.GetNeighbors(level))[0];
        AStarNode start = GetValidNeighbors(pStart.GetNeighbors(level))[0];
        start.SetOrigin(start);

        HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
        List<AStarNode> openSet = new List<AStarNode> { start };
        AStarNode current = start;
        int runs = 0;

        while (openSet.Count != 0 && runs<5000)
        {
            current = GetLowestScore(openSet, current, destination);

            if (AreEqual(current, destination))
            {
                Debug.Log("PATH FOUND.\nRUNS: " + runs.ToString() + "\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
                return GenerateTunnel(start, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (AStarNode neighbor in GetValidNeighbors(current.GetValue().GetNeighbors(level)))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float possibleScore = current.GetGScore() + 1;

                if (possibleScore < neighbor.GetGScore() || openSet.IndexOf(neighbor) == -1)
                {
                    neighbor.SetOrigin(current);
                    neighbor.SetGScore(possibleScore);
                    neighbor.SetFScore(possibleScore + Distance(neighbor.GetValue(), destination.GetValue()));
                    openSet.Add(neighbor);
                }
            }
            runs++;
        }

        Debug.Log("PATH NOT FOUND.\nRUNS: " + runs.ToString() +
            "\nSTART: " + start.GetValue().GetX() + " " + start.GetValue().GetY() +
            "\nDEST:  " + destination.GetValue().GetX() + " " + destination.GetValue().GetY()
            + "\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");

        return -1;
    }

    //draws the tunnel on the level map
    private static int GenerateTunnel(AStarNode start, AStarNode current)
    {
        //variable used to see if anything got generated
        AStarNode tmpDest = current;
        while (!AreEqual(current, start))
        {
            level[current.GetValue().GetX(), current.GetValue().GetY()] = 'T';
            current = current.GetOrigin();
        }
        level[current.GetValue().GetX(), current.GetValue().GetY()] = 'T';

        Debug.Log("TUNNEL GENERATED.\nSTART: "+start.GetValue().GetX()+" "+ start.GetValue().GetY()+
                                    "\nDEST:  "+ tmpDest.GetValue().GetX() + " " + tmpDest.GetValue().GetY()
                                    +"\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
        return 0;
    }

    //A* END

    //I run A* for every two closest doors

    //returns the closest door to the given door
    private static int[] GetClosestDoor(int givenRoomIndex, Point givenDoor, List<int> visited)
    {
        //initial door in the first available room
        int closestDoorIndex = 0;
        int closestRoomIndex = 0;
        if (givenRoomIndex == 0)
        {
            closestDoorIndex = 1;
            closestRoomIndex = 1;
        }
        
        int[] res = new int[2];

        for (int i = 0; i < rooms.Count; i++)
        {
            if (visited.IndexOf(i) != -1 || rooms[givenRoomIndex] == rooms[i])
                continue;

            for (int j = 0; j < 4; j++)
                if (Distance(givenDoor, rooms[i].GetDoors()[j]) < Distance(givenDoor, rooms[closestRoomIndex].GetDoors()[closestDoorIndex])
                    && !rooms[i].GetDoorStatus()[j])
                {
                    closestDoorIndex = j;
                    closestRoomIndex = i;
                }
        }
            
        res[0] = closestRoomIndex;
        res[1] = closestDoorIndex;
        
        return res;
    }

    //returns the doors without a connected tunnel from the given room (given by index)
    public static List<int> GetFreeDoors(int roomIndex)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < 4; i++)
            if (rooms[roomIndex].GetDoorStatus()[i] == false)
                res.Add(i);

        return res;
    }

    //draws the tunnels between each two rooms
    private static void DrawTunnels()
    {
        List<int> visited = new List<int>();
        int nrTunnels = 0;
        Point givenDoor;
        int doorIndex;
        int[] tempRes;
        List<int> freeDoors;
        for (int i = 0; i < rooms.Count; i++)
        {
            visited.Add(i);
            freeDoors = GetFreeDoors(i);
            if (freeDoors.Count == 0)
                continue;
            doorIndex = Random.Range(0, freeDoors.Count);
            givenDoor = rooms[i].GetDoors()[freeDoors[doorIndex]];
            tempRes = GetClosestDoor(i, givenDoor, visited);

            if (FindPath(givenDoor, rooms[tempRes[0]].GetDoors()[tempRes[1]]) != -1)
                nrTunnels++;

            rooms[i].ConnectDoor(doorIndex);
            rooms[tempRes[0]].ConnectDoor(tempRes[1]);
            Debug.Log("TUNNEL FROM ROOM: " + i.ToString() + " TO " + tempRes[0].ToString() +
                        "\nDOOR: " + freeDoors[doorIndex].ToString() + " TO " + tempRes[1].ToString());
        }
        Debug.Log("TUNNELS DRAWN: " + nrTunnels.ToString() + " / " + rooms.Count.ToString()+
                                    "\nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
    }

    //prints the level in the console used for debugging
    private static void PrintLevel()
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
