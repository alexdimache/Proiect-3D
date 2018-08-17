using UnityEngine;

public class StartRoom : Room {

    //constructor
    public StartRoom(int givenCornerX, int givenCornerY)
    {
        cornerX = givenCornerX;
        cornerY = givenCornerY;
        roomLength = 5;
        roomWidth = 5;
        Debug.Log("-ROOM COORDINATES SET.");
        InitRoom();
        PopulateRoom();
        PrintRoom();
    }

    override protected void PopulateRoom()
    {
        for (int i = 1; i < roomWidth - 1; i++)
            for (int j = 1; j < roomLength - 1; j++)
            {
                roomTiles[i, j] = '#';
            }
        //roomTiles[roomWidth / 2, roomLength / 2] = '@';

        Debug.Log("-START ROOM POPULATED.");
    }
}
