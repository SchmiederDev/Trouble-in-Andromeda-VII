using System;

[Serializable]
public class SaveGame
{
    public int TotalXP;
    public int LevelsPlayed;

    public SaveGame(int totalXP, int levelsPlayed)
    {
        TotalXP = totalXP;
        LevelsPlayed = levelsPlayed;
    }   
}
