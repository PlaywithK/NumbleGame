using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int CurrentModeIndex { get; private set; }

    [HideInInspector] public List<ModeConfig> availableModes;
    [HideInInspector] public ModeConfig currentMode;
    private List<int> _secretCode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeModes();

        int selectedIndex = Mathf.Clamp(GameModeSelection.SelectedIndex, 0, availableModes.Count - 1); //Value, Min, Max
        LoadMode(selectedIndex);
        GameModeSelection.SelectedIndex = selectedIndex;
    }

    public void InitializeModes()
    {
        availableModes = new List<ModeConfig>
        {
            new ModeConfig("Endless", 3, 5, false, 0f, ModeType.Endless),
            new ModeConfig("Speedrun", 5, 5, false, 30f, ModeType.Speedrun),
            new ModeConfig("Classic 3", 3, 5, false, 0f, ModeType.Classic),
            new ModeConfig("Classic 4", 4, 5, false, 0f, ModeType.Classic),
            new ModeConfig("Classic 5", 5, 5, false, 0f, ModeType.Classic),
            new ModeConfig("Classic 6", 6, 5, false, 0f, ModeType.Classic)
        };
    }

    public void LoadMode(int index)
    {
        if (index >= 0 && index < availableModes.Count)
        {
            CurrentModeIndex = index;
            currentMode = availableModes[index];
            GenerateSecretCode();
        }
        else
        {
            Log.Message("Mode index is invalid!");
        }
    }

    public void GenerateSecretCode(int? lengthOverride = null)
    {
        int length = lengthOverride ?? currentMode.digitCount;
        List<int> digits = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        _secretCode = new List<int>();

        for (int i = 0; i < length; i++)
        {
            int nextDigit;
            do
            {
                nextDigit = digits[Random.Range(0, digits.Count)];
            } while (!currentMode.allowRepeats && _secretCode.Contains(nextDigit));

            _secretCode.Add(nextDigit);
        }

        Log.Message("Code: " + string.Join("", _secretCode));
    }

    public List<int> GetCurrentAnswer => _secretCode;
}
