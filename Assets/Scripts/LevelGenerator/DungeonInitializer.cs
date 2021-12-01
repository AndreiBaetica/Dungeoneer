using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Random = System.Random;

public class DungeonInitializer : MonoBehaviour
{
    #region Singleton

    public static DungeonInitializer instance;
    
    void Awake()   
    {
        instance = this;
    }
        
    #endregion
    
    public static DungeonInitializer MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DungeonInitializer>();
            }

            return instance;
        }
    }
    
    private const int RoomDimension = 15;
    private int _minRooms;
    private string _theme;
    private List<(double, string)> _npcSpawns = new List<(double, string)>();
    private Vector3 newSpawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        ReadConfig("testLevel");
        //sorting spawnrates in ascending order to give rarer npcs a chance to spawn first
        _npcSpawns.Sort();
        PopulateMap();
    }

    private void PopulateMap(bool firstRun = true)
    {
        DungeonGenerator generator = new DungeonGenerator();
        Room[,] map = generator.GenerateLevel(_minRooms);

        var playerSpawnedRoomFound = false;
        GameObject playerSpawnRoom = null;
        var altarSpawnedRoomFound = false;
        GameObject altarSpawnRoom = null;
        Transform roomsParent = GameObject.Find("Rooms").transform;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    var selectedRoom = Resources.Load("rooms/" + _theme + "/" + map[i, j].GetRoomType());
                    var currentRoom = (GameObject) Instantiate(selectedRoom,
                        new Vector3(i * RoomDimension, 0, j * RoomDimension),
                        Quaternion.identity);
                    currentRoom.transform.parent = roomsParent;

                    if (!playerSpawnedRoomFound)
                    {
                        playerSpawnRoom = currentRoom;
                        playerSpawnedRoomFound = true;
                    }
                    else
                    {
                        //spawn NPCs
                        foreach (Transform child in currentRoom.transform)
                        {
                            if (child.name.Equals("NPCSpawner"))
                            {
                                foreach ((double, string) npc in _npcSpawns)
                                {
                                    Random random = new Random();
                                    if (random.NextDouble() <= npc.Item1)
                                    {
                                        child.GetComponent<Spawner>().Spawn("npc/" + npc.Item2);
                                        break;
                                    }
                                }
                            }

                            if (child.name.Equals("AltarSpawner"))
                            {
                                if (!altarSpawnedRoomFound)
                                {
                                    altarSpawnRoom = currentRoom;
                                    altarSpawnedRoomFound = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        //spawn core and altar
        altarSpawnRoom.transform.Find("AltarSpawner").GetComponent<Spawner>().Spawn("core/Altar");
        if (firstRun) // Don't want to remake a new core if called from DestroyAndRepopulate()
        {
            playerSpawnRoom.transform.Find("PlayerSpawner").GetComponent<Spawner>().Spawn("core/Core");
            GameObject core = GameObject.Find("/Spawnables/Core(Clone)");
            core.transform.parent = null; // Take out core from spawnables to be able to keep it.
        }
        else
        {
            newSpawnPosition = playerSpawnRoom.transform.Find("PlayerSpawner").position;
            Debug.Log("New player core spawn point : " + newSpawnPosition);
        }
    }

    private void ReadConfig(string filename)
    {
        TextAsset config = (TextAsset) Resources.Load("configs/" + filename);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(config.text);

        _minRooms = int.Parse(xmlDoc.DocumentElement.SelectSingleNode("/config/minRooms").InnerText);
        _theme = xmlDoc.DocumentElement.SelectSingleNode("/config/theme").InnerText.Replace("\"", "");

        var spawns = xmlDoc.DocumentElement.SelectSingleNode("/config/npcs");

        foreach (XmlNode spawn in spawns)
        {
            _npcSpawns.Add((double.Parse(spawn.SelectSingleNode("spawnRate").InnerText), spawn.Name));
        }
    }

    public void DestroyAndRepopulate()
    {
        // Destroy old rooms, bandits and altar
        foreach (Transform child in GameObject.Find("Rooms").transform) {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in GameObject.Find("Spawnables").transform) {
            Destroy(child.gameObject);
        }

        // Repopulate
        PopulateMap(false);
        // set core and player position to new spawn point
        GameObject core = GameObject.Find("Core(Clone)");
        core.transform.position = newSpawnPosition;
        GameObject player = GameObject.Find("/Core(Clone)/Player");
        player.transform.position = new Vector3(newSpawnPosition.x, 0.5f, newSpawnPosition.z); // If you touch this, ask Seb before doing it please.

        GameLoopManager.instance.UpdateActiveChars();
    }

}

