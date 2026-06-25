using UnityEngine;

public class AchievementUnlocker : MonoBehaviour
{
    public void OnCodeSolved(int codeLength, int attemptsUsed, bool lastAttempt, bool firstAttempt)
    {
        if (codeLength == 3)
            AchievementManager.Instance.Unlock("win_3_felder");

        if (codeLength == 4)
            AchievementManager.Instance.Unlock("win_4_felder");

        if (codeLength == 5)
            AchievementManager.Instance.Unlock("win_5_felder");

        if (codeLength == 6)
            AchievementManager.Instance.Unlock("win_6_felder");

        if (lastAttempt)
            AchievementManager.Instance.Unlock("last_try_win");

        if (firstAttempt)
            AchievementManager.Instance.Unlock("first_try_win");
    }

    public void OnEndlessRoundsCompleted(int rounds)
    {
        if (rounds >= 10)
            AchievementManager.Instance.Unlock("endless_10_runden");
    }
}

