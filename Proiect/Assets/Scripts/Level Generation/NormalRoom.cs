using UnityEngine;

public class NormalRoom : Room {

    //constructor
    public NormalRoom(int givenCornerX, int givenCornerY)
    {
        cornerX = givenCornerX;
        cornerY = givenCornerY;
        roomLength = 7;
        roomWidth = 7;
        Debug.Log("-ROOM COORDINATES SET.");
        InitRoom();
        PopulateRoom();
        PrintRoom();
    }

    //pick an item based on percentage
    private char PickTile(int tileChance)
    {
        if (tileChance <= 5)
            return 'P';
        if (tileChance > 5 && tileChance <= 80)
            return '#';
        return 'O';
    }

    override protected void PopulateRoom()
    {
        Random.InitState(System.Environment.TickCount);
        int tileChance;

        for (int i = 1; i < roomWidth - 1; i++)
            for (int j = 1; j < roomLength - 1; j++)
            {
                if (roomTiles[i, j + 1] == 'D' || roomTiles[i, j - 1] == 'D' || roomTiles[i + 1, j] == 'D' || roomTiles[i - 1, j] == 'D')
                    roomTiles[i, j] = '#';
                else
                {
                    tileChance = Random.Range(1, 101);
                    roomTiles[i, j] = PickTile(tileChance);
                }
            }

        Debug.Log("-NORMAL ROOM POPULATED.");
    }
}
