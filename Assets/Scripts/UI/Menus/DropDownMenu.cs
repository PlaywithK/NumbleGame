using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    [Header("Main Panel")]
    public GameObject dropdownMenu;
    public GameObject pcUIRoot;
    public GameObject mobileUIRoot;
    private GameObject _activeUIRoot;

    [Header("Sub Menus")]
    private GameObject _optionsPanel;
    private GameObject _achievementPanel;
    private GameObject _quitConfirmPanel;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        pcUIRoot.SetActive(false);
        mobileUIRoot.SetActive(true);
        _activeUIRoot = mobileUIRoot;
#else
        pcUIRoot.SetActive(true);
        mobileUIRoot.SetActive(false);
        _activeUIRoot = pcUIRoot;
#endif
        _optionsPanel = _activeUIRoot.transform.Find("OptionsPanel")?.gameObject;
        _achievementPanel = _activeUIRoot.transform.Find("AchievementPanel")?.gameObject;
        _quitConfirmPanel = _activeUIRoot.transform.Find("QuitConfirmPanel")?.gameObject;

        if (!_optionsPanel || !_achievementPanel || !_quitConfirmPanel)
        {
            Log.Warning("One or more dropdown panels not found!");
        }
    }

    public void ToggleMenu()
    {
        bool currentlyActive = dropdownMenu.activeSelf;
        dropdownMenu.SetActive(!currentlyActive);
        Log.Message("Dropdown is now: " + (!currentlyActive));
    }

    public void OpenOptions()
    {
        if (_optionsPanel) _optionsPanel.SetActive(true);
        dropdownMenu.SetActive(false);
    }

    public void OpenAchievements()
    {
        if (_achievementPanel) _achievementPanel.SetActive(true);
        dropdownMenu.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        if (_quitConfirmPanel) _quitConfirmPanel.SetActive(true);
        dropdownMenu.SetActive(false);
    }

    public void ConfirmQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void CancelQuit()
    {
        if (_quitConfirmPanel) _quitConfirmPanel.SetActive(false);
    }
}
