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
        Debug.Log("SAVE SYSTEM DATA SAVED = data pos x:"+data.position[0]+" y:" 
                  +data.position[1]+" z:"+data.position[2] +" currenthealth:"+data.health+" mana:"+data.mana + "shield:"+ data.shield + "gold:"+data.gold+"location:"+data.location);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerSaveData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dung";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaveData data = formatter.Deserialize(stream) as PlayerSaveData;
            stream.Close();
            Debug.Log("Player controller save system data variable = data pos x,y,z:"+data.position
                      +" currenthealth:"+data.health+" mana:"+data.mana);
            return data;
        }
        else
        {
            Debug.LogError("No save file found in " + path);
            return null;
        }

    }
}
