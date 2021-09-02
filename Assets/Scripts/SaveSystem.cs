using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dung";
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerSaveData data = new PlayerSaveData(player);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerSaveData loadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dung";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData data = formatter.Deserialize(stream) as PlayerSaveData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("No save file found in " + path);
            return null;
        }

    }
}
