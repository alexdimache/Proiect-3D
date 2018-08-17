using UnityEngine;

public class BossRoom: Room {

    //constructor
    public BossRoom(int givenCornerX, int givenCornerY)
    {
        cornerX = givenCornerX;
        cornerY = givenCornerY;
        roomLength = 11;
        roomWidth = 11;
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
        roomTiles[2, 2] = 'P';
        roomTiles[roomWidth - 3, 2] = 'P';
        roomTiles[2, roomLength - 3] = 'P';
        roomTiles[roomWidth - 3, roomLength - 3] = 'P';
        roomTiles[4, 4] = 'P';
        roomTiles[roomWidth - 5, 4] = 'P';
        roomTiles[4, roomLength - 5] = 'P';
        roomTiles[roomWidth - 5, roomLength - 5] = 'P';
        //DELET DIS
        //roomTiles[roomWidth / 2, roomLength / 2] = '@';
        //^^^

        Debug.Log("-BOSS ROOM POPULATED.");
    }
}
