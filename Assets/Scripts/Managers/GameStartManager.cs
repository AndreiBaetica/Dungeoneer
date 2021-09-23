public static class GameStartManager
{
    public static bool playingSavedGame;
    
    public static bool PlayingSavedGame
    {
        get => playingSavedGame;
        set => playingSavedGame = value;
    }
}