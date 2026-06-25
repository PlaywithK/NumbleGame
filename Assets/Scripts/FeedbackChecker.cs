using UnityEngine;
using System.Collections.Generic;

public class FeedbackChecker
{
    public enum FeedbackResult { Wrong, WrongPosition, Correct }

    public static FeedbackResult[] GetFeedback(List<int> guess, List<int> answer)
    {
        FeedbackResult[] result = new FeedbackResult[guess.Count];
        bool[] answerUsed = new bool[answer.Count];

        for (int i = 0; i < guess.Count; i++)
        {
            if (guess[i] == answer[i])
            {
                result[i] = FeedbackResult.Correct;
                answerUsed[i] = true;
            }
        }

        for (int i = 0; i < guess.Count; i++)
        {
            if (result[i] == FeedbackResult.Correct) continue;

            for (int j = 0; j < answer.Count; j++)
            {
                if (!answerUsed[j] && guess[i] == answer[j])
                {
                    result[i] = FeedbackResult.WrongPosition;
                    answerUsed[j] = true;
                    break;
                }
            }

            if (result[i] == 0) result[i] = FeedbackResult.Wrong;
        }

        return result;
    }
}
