
[System.Serializable]
public class PlayerSaveData
{
    public int level;

    public int health;

    public int mana;

    public float[] position;
    
    public PlayerSaveData(PlayerManager player)
    {
        level = player.currentLevel;
        health = player.currentHealth;
        mana = player.currentMana;
        position = new float[3];
        var playerPos = player.transform.position;
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;
    }

    public void ApplyPlayerSavedData(PlayerManager player)
    {
        player.currentLevel = this.level;
        player.currentHealth = health;
        player.currentMana = mana;
        position = new float[3];
        var playerPos = player.transform.position;
        playerPos.x = position[0];
        playerPos.y = position[1];
        playerPos.z = position[2];
    }

}
