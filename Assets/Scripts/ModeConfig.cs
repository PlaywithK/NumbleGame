public enum ModeType { Classic, Endless, Speedrun }

[System.Serializable]
public class ModeConfig
{
    public string name;
    public int digitCount;
    public int maxAttempts;
    public bool allowRepeats;
    public float timeLimit;
    public ModeType type;

    public ModeConfig(string name, int digitCount, int maxAttempts, bool allowRepeats, float timeLimit = 0f, ModeType type = ModeType.Classic)
    {
        this.name = name;
        this.digitCount = digitCount;
        this.maxAttempts = maxAttempts;
        this.allowRepeats = allowRepeats;
        this.timeLimit = timeLimit;
        this.type = type;
    }
}
