
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
        health = player.currentHealth;
        mana = player.currentMana;
        position = new float[3];
        var playerPos = player.transform.position;
        position[0] = playerPos.x;
        position[1] = playerPos.y;
        position[2] = playerPos.z;
    }

}
