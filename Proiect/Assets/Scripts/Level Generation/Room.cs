using UnityEngine;
using LevelGeneration;

public abstract class Room
{
    //x coordinate of the center of the room
    protected int cornerX;
    //y coordinate of the center of the room
    protected int cornerY;

    //the length and width of the room
    protected int roomLength;
    protected int roomWidth;

    //contents of the room by tile
    protected char[,] roomTiles;

    //the list of doors
    protected Point[] doors;
    //array showing which door is connected
    protected bool[] connectedDoors;

    public int GetCornerX()
    {
        return cornerX;
    }

    public int GetCornerY()
    {
        return cornerY;
    }

    public char[,] GetRoomTiles()
    {
        return roomTiles;
    }

    public Point[] GetDoors()
    {
        return doors;
    }

    public bool[] GetDoorStatus()
    {
        return connectedDoors;
    }

    public int GetRoomLength()
    {
        return roomLength;
    }

    public int GetRoomWidth()
    {
        return roomWidth;
    }

    public void ConnectDoor(int doorIndex)
    {
        connectedDoors[doorIndex] = true;
    }

    //Creating walls and doors
    protected void InitRoom()
    {
        roomTiles = new char[roomWidth, roomLength];
        for (int i = 0; i < roomLength; i++)
        {
            roomTiles[0, i] = 'W';
            roomTiles[roomWidth - 1, i] = 'W';
        }
        for (int i = 0; i < roomWidth; i++)
        {
            roomTiles[i, 0] = 'W';
            roomTiles[i, roomLength - 1] = 'W';
        }

        //North
        roomTiles[0, Random.Range(2, roomLength - 2)] = 'D';

        //South
        roomTiles[roomWidth - 1, Random.Range(2, roomLength - 2)] = 'D';

        //West
        roomTiles[Random.Range(2, roomWidth - 2), 0] = 'D';

        //East
        roomTiles[Random.Range(2, roomWidth - 2), roomLength - 1] = 'D';

        Debug.Log("-ROOM INITIALIZED.");
    }

    //Creating each tile
    abstract protected void PopulateRoom();

    /*
     * Setting each door coordinate according 
     * to the center of the room and room dimensions
     */
    public void SetDoors(Point[] coords)
    {
        doors = new Point[4];
        connectedDoors = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            doors[i] = new Point(coords[i].GetX(), coords[i].GetY());
            connectedDoors[i] = false;
        }

        Debug.Log("-ROOM DOORS SET.");
    }

    public void PrintRoom()
    {
        string tempOutput = null;
        for (int i = 0; i < roomWidth; i++)
        {
            for (int j = 0; j < roomLength; j++)
            {
                tempOutput += roomTiles[i, j]+" ";
            }
            tempOutput += "\n";
        }
        tempOutput += "\n Room dimension: " + roomLength.ToString() + "x" + roomWidth.ToString()+"\n";
        tempOutput += "Center: " + cornerX.ToString() + " " + cornerY.ToString() + "\n";
        Debug.Log(tempOutput);
    }

}
