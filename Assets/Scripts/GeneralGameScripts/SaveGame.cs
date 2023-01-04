using System;

[Serializable]
public class SaveGame
{
    public int TotalXP;
    public int LevelsPlayed;
    public bool[] unlockedWeapons;

    public SaveGame(int totalXP, int levelsPlayed)
    {
        TotalXP = totalXP;
        LevelsPlayed = levelsPlayed;
    }   

    public void TransferUnlockedWeapons()
    {
        unlockedWeapons = new bool[TheGame.theGameInst.PlayerUnionFighter.FighterWeapons.Count];

        for(int i = 0; i < unlockedWeapons.Length; i++)
        {
            unlockedWeapons[i] = TheGame.theGameInst.PlayerUnionFighter.FighterWeapons[i].IsActivated;
        }
    }
}
