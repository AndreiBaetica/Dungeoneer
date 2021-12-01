using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Random = System.Random;

public class DungeonInitializer : MonoBehaviour
{
    private const int RoomDimension = 15;
    private int _minRooms;
    private string _theme;
    private List<(double, string)> _npcSpawns = new List<(double, string)>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        ReadConfig("testLevel");
        //sorting spawnrates in ascending order to give rarer npcs a chance to spawn first
        _npcSpawns.Sort();
        PopulateMap();
    }

    private void PopulateMap()
    {
        DungeonGenerator generator = new DungeonGenerator();
        Room[,] map = generator.GenerateLevel(_minRooms);

        var playerSpawnedRoomFound = false;
        GameObject playerSpawnRoom = null;
        var altarSpawnedRoomFound = false;
        GameObject altarSpawnRoom = null;
        
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    var selectedRoom = Resources.Load("rooms/" + _theme + "/" + map[i,j].GetRoomType());
                    var currentRoom = (GameObject) Instantiate(selectedRoom, 
                        new Vector3(i * RoomDimension, 0, j * RoomDimension), 
                        Quaternion.identity);

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
        //spawn core
        playerSpawnRoom.transform.Find("PlayerSpawner").GetComponent<Spawner>().Spawn("core/Core");
        altarSpawnRoom.transform.Find("AltarSpawner").GetComponent<Spawner>().Spawn("core/Altar");
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
}

