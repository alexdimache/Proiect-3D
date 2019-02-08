using UnityEngine;
using System.Collections.Generic;
using LevelGeneration;

public class LevelMap
{
    //array of rooms
    private List<Room> rooms;
    //number of lines of the level array
    private int nrOfLines = 56;
    //the level map array
    private char[,] level;
    private bool[,] miniMap;
    //the maximum number of rooms in the level
    private int maxNrOfRooms;
    //room dimensions
    private int minDimension = 5;
    private int maxDimension = 10;
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
        miniMap = new bool[8, 8];
        string debugOutput = "Max nr of rooms: "+ maxNrOfRooms.ToString();
        Debug.Log(debugOutput);
        InitLevel();
        PopulateLevel();
        ClearUnusedDoors();
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
        
        //Boss room (81 tiles, 10x10 with walls)
        bool bossRoomPlaced = false;
        while (!bossRoomPlaced)
        {
            givenX = Random.Range(3, nrOfLines - 4);
            givenY = Random.Range(3, nrOfLines - 4);
            if (CheckArea(givenX, givenY, 10, 10))
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
                rooms.Add(new NormalRoom(givenX, givenY));
                AddRoom(rooms[rooms.Count - 1]);
            }
        }
        
        Debug.Log("LEVEL POPULATED. \nTIME: " + Time.realtimeSinceStartup.ToString() + " seconds");
    }
    #endregion

    private List<Point> GetValidNeighbors(List<Point> neighbors)
    {
        List<Point> res = new List<Point>();
        foreach (Point neighbor in neighbors)
            if (traversableTiles.IndexOf(level[neighbor.GetX(), neighbor.GetY()]) != -1)
                res.Add(neighbor);
        return res;
    }

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
