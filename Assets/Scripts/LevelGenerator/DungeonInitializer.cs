using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DungeonInitializer : MonoBehaviour
{
    private const int RoomDimension = 15;
    private int _minRooms;
    private string _theme;
    
    
    // Start is called before the first frame update
    void Start()
    {
        ReadConfig("testLevel");
        PopulateMap();
    }

    private void PopulateMap()
    {
        DungeonGenerator generator = new DungeonGenerator();
        Room[,] map = generator.GenerateLevel(_minRooms);
        
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] != null)
                {
                    var selectedRoom = Resources.Load("rooms/" + _theme + "/" + map[i,j].GetRoomType());
                    Instantiate(selectedRoom, 
                        new Vector3(i * RoomDimension, 0, j * RoomDimension), 
                        Quaternion.identity);
                }
            }
        }
    }

    private void ReadConfig(string filename)
    {
        TextAsset config = (TextAsset) Resources.Load("configs/" + filename);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(config.text);

        _minRooms = int.Parse(xmlDoc.DocumentElement.SelectSingleNode("/config/minRooms").InnerText);
        _theme = xmlDoc.DocumentElement.SelectSingleNode("/config/theme").InnerText.Replace("\"", "");
    }
}
