using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressionUtil
{
    public const int statHpMultiplier = 5;
    public const int statStaminaMultiplier = 5;
    public const int statAttackMultiplier = 1;

    public static int GetNextLevelExp(int level)
    {
        return level * 2 + 1;
    }

    public static int GetPreviousLevelExp(int level)
    {
        return level * 2 - 1;
    }
}