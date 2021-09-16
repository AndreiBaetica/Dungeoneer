
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int level;

    public int health;

    public int mana;

    public float[] position;
    
    public PlayerSaveData(PlayerController player)
    {
        level = player.currentLevel;
        health = player.CurrentHealth;
        mana = player.CurrentMana;
        position = new float[3];
        var playerPos = player.transform.position;
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;
    }

    public static Vector3 ApplyPlayerSavedData(PlayerController player, PlayerSaveData data)
    {
        player.currentLevel = data.level;
        player.currentHealth = data.health;
        player.currentMana = data.mana;
        var position = new Vector3(data.position[0],data.position[1],data.position[2]);
        return position;
    }

}
