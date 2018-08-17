using UnityEngine;

public class LevelDisplay : MonoBehaviour {
    //GameObject with all the prefabs that will be instantiated
    public GameObject[] tiles;
    public GameObject playerObject;

    private GameObject worldTerrain;
    private GameObject worldProps;
    private GameObject characters;

    //the level object
    private LevelMap levelMap;
    //the maximum number of lines of the level
    private int nrOfLines;
    //the level map itself
    private char[,] level;
    //available tiles for a room
    private string availableTiles = "#OpPB";

    // Use this for initialization
    void Awake ()
    {
        worldTerrain = transform.Find("/World/Terrain").gameObject;
        worldProps = transform.Find("/World/Props").gameObject;
        characters = transform.Find("/World/Characters").gameObject;
        levelMap = new LevelMap();
        nrOfLines = levelMap.GetNrOfLines();
        level = levelMap.GetLevel();
        DisplayLevel(tiles, playerObject);
	}
    //display the level
    private void DisplayLevel(GameObject[] tiles, GameObject playerObject)
    {
        for (int i = 1; i < nrOfLines - 1; i++)
            for(int j = 1; j < nrOfLines - 1; j++)
            {
                switch (level[i, j])
                {
                    //PLAYER SPAWN
                    case '@':
                        Instantiate(playerObject, new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 10, tiles[0].GetComponent<Renderer>().bounds.size.x * j), playerObject.transform.rotation, characters.transform);
                        Instantiate(tiles[0], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[0].transform.rotation, worldTerrain.transform);
                        break;
                    //NORMAL FLOOR
                    case '#':
                        Instantiate(tiles[0], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[0].transform.rotation, worldTerrain.transform);
                        break;
                    //HOLE FLOOR
                    case 'O':
                        Instantiate(tiles[7], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[7].transform.rotation, worldTerrain.transform);
                        break;

                    //PILLAR
                    case 'P':
                        Instantiate(tiles[0], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[0].transform.rotation, worldTerrain.transform);
                        Instantiate(tiles[8], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[8].transform.rotation, worldProps.transform);
                        Instantiate(tiles[8], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, tiles[8].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[8].transform.rotation, worldProps.transform);
                        break;

                    //BARREL
                    case 'B':
                        Instantiate(tiles[0], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[0].transform.rotation, worldTerrain.transform);
                        Instantiate(tiles[1], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[1].transform.rotation, worldProps.transform);
                        break;

                    //DOOR
                    case 'D':
                        Instantiate(tiles[4], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[4].transform.rotation, worldTerrain.transform);
                        Quaternion firstAngle = tiles[2].transform.rotation;
                        Quaternion secondAngle = tiles[2].transform.rotation;
                        float IModifier = 0;
                        float JModifier = 0;
                        //NORTH
                        if (availableTiles.IndexOf(level[i - 1, j]) != -1)
                        {
                            IModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2 * (-1);
                            JModifier = 0;
                            firstAngle = tiles[2].transform.rotation;
                            secondAngle = Quaternion.Euler(0, 90, 0);
                        }
                        //SOUTH
                        if(availableTiles.IndexOf(level[i + 1, j]) != -1)
                        {
                            IModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2;
                            JModifier = 0;
                            firstAngle = tiles[2].transform.rotation;
                            secondAngle = Quaternion.Euler(0, 90, 0);
                        }
                        //WEST
                        if (availableTiles.IndexOf(level[i, j - 1]) != -1)
                        {
                            IModifier = 0;
                            JModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2 * (-1);
                            firstAngle = Quaternion.Euler(0, 90, 0);
                            secondAngle = tiles[2].transform.rotation;
                        }
                        //EAST
                        if(availableTiles.IndexOf(level[i, j + 1]) != -1)
                        {
                            IModifier = 0;
                            JModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2;
                            firstAngle = Quaternion.Euler(0, 90, 0);
                            secondAngle = tiles[2].transform.rotation;
                        }
                        //placing the door itself with a wall above it
                        Instantiate(tiles[2], new Vector3( tiles[0].GetComponent<Renderer>().bounds.size.x * i + IModifier,  0,  tiles[0].GetComponent<Renderer>().bounds.size.x * j + JModifier), firstAngle, worldTerrain.transform);
                        Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + IModifier, tiles[2].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + JModifier), firstAngle, worldTerrain.transform);

                        //I'm placing 2 walls on each side of the door
                        Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i - JModifier, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j - IModifier), secondAngle, worldTerrain.transform);
                        Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + JModifier, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j + IModifier), secondAngle, worldTerrain.transform);
                        Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i - JModifier, tiles[2].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j - IModifier), secondAngle, worldTerrain.transform);
                        Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + JModifier, tiles[2].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + IModifier), secondAngle, worldTerrain.transform);

                        break;

                    //WALL
                    case 'W':
                        firstAngle = tiles[2].transform.rotation;
                        secondAngle = tiles[2].transform.rotation;
                        IModifier = 0;
                        JModifier = 0;
                        float sImod = 0;
                        float sJmod = 0;
                        //I'm checking the orientation of the wall horizontal or vertical
                        //I'm also placing torches on the walls here
                        //SOUTH
                        if (availableTiles.IndexOf(level[i - 1, j]) != -1)
                        {
                            IModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2 * (-1);
                            JModifier = 0;
                            sImod = tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8 * (-1);
                            sJmod = 0;
                            firstAngle = tiles[2].transform.rotation;
                            secondAngle = Quaternion.Euler(0, 0, 0);
                        }
                        //NORTH
                        if (availableTiles.IndexOf(level[i + 1, j]) != -1)
                        {
                            IModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2;
                            JModifier = 0;
                            sImod = tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8;
                            sJmod = 0;
                            firstAngle = tiles[2].transform.rotation;
                            secondAngle = Quaternion.Euler(0, 180, 0);
                        }
                        //EAST
                        if (availableTiles.IndexOf(level[i, j - 1]) != -1)
                        {
                            IModifier = 0;
                            JModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2 * (-1);
                            sImod = 0;
                            sJmod = tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8 * (-1);
                            firstAngle = Quaternion.Euler(0, 90, 0);
                            secondAngle = Quaternion.Euler(0, 270, 0);
                        }
                        //WEST
                        if (availableTiles.IndexOf(level[i, j + 1]) != -1)
                        {
                            IModifier = 0;
                            JModifier = tiles[0].GetComponent<Renderer>().bounds.size.x / 2;
                            sImod = 0;
                            sJmod = tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8;
                            firstAngle = Quaternion.Euler(0, 90, 0);
                            secondAngle = Quaternion.Euler(0, 90, 0);
                        }
                        if (IModifier != JModifier)
                        {
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + IModifier, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j + JModifier), firstAngle, worldTerrain.transform);
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + IModifier, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + JModifier), firstAngle, worldTerrain.transform);
                            Instantiate(tiles[5], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + IModifier + sImod, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + JModifier + sJmod), secondAngle, worldProps.transform);
                        } 
                        break;

                    //TUNNEL
                    case 'T':
                        Instantiate(tiles[4], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[4].transform.rotation, worldTerrain.transform);
                        //if there isn't a tunnel or a door on the right then place a wall there
                        if (level[i, j + 1] != 'T' && level[i, j + 1] != 'D')
                        {
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j + tiles[0].GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 90, 0), worldTerrain.transform);
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + tiles[0].GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 90, 0), worldTerrain.transform);
                            if (i % 2 == 1)
                                Instantiate(tiles[5], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j + tiles[0].GetComponent<Renderer>().bounds.size.x / 2 - tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8), Quaternion.Euler(0, 270, 0), worldProps.transform);
                        }
                        //if there isn't a tunnel or a door on the left then place a wall there
                        if (level[i, j - 1] != 'T' && level[i, j - 1] != 'D')
                        {
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j - tiles[0].GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 90, 0), worldTerrain.transform);
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j - tiles[0].GetComponent<Renderer>().bounds.size.x / 2), Quaternion.Euler(0, 90, 0), worldTerrain.transform);
                            if (i % 2 == 0)
                                Instantiate(tiles[5], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j - tiles[0].GetComponent<Renderer>().bounds.size.x / 2 + tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8), Quaternion.Euler(0, 90, 0), worldProps.transform);
                        }
                        //if there isn't a tunnel or a door beneath then place a wall there
                        if (level[i + 1, j] != 'T' && level[i + 1, j] != 'D')
                        {
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + tiles[0].GetComponent<Renderer>().bounds.size.x / 2, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[3].transform.rotation, worldTerrain.transform);
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + tiles[0].GetComponent<Renderer>().bounds.size.x / 2, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[3].transform.rotation, worldTerrain.transform);
                            if (j % 2 == 1)
                                Instantiate(tiles[5], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i + tiles[0].GetComponent<Renderer>().bounds.size.x / 2 - tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j), Quaternion.Euler(0, 360, 0), worldProps.transform);
                        }
                        //if there isn't a tunnel or a door above then place a wall there
                        if (level[i - 1, j] != 'T' && level[i - 1, j] != 'D')
                        {
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i - tiles[0].GetComponent<Renderer>().bounds.size.x / 2, 0, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[3].transform.rotation, worldTerrain.transform);
                            Instantiate(tiles[3], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i - tiles[0].GetComponent<Renderer>().bounds.size.x / 2, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[3].transform.rotation, worldTerrain.transform);
                            if (j % 2 == 0)
                                Instantiate(tiles[5], new Vector3(tiles[0].GetComponent<Renderer>().bounds.size.x * i - tiles[0].GetComponent<Renderer>().bounds.size.x / 2 + tiles[5].GetComponent<Renderer>().bounds.size.x * 5 / 8, tiles[3].GetComponent<Renderer>().bounds.size.y, tiles[0].GetComponent<Renderer>().bounds.size.x * j), tiles[5].transform.rotation, worldProps.transform);
                        }
                        break;
                }
            }
    }
}
