using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mobileCanvas;
    [SerializeField] private GameObject _pcCanvas;

    private DeviceManager _deviceManager;
    private GameObject _currentCanvas;
    [HideInInspector] public MainMenuPanels currentPanels;

    private void Start()
    {
        if (_deviceManager != null)
        {
            InitializeCanvas(_deviceManager.IsMobile());
        }
        else
        {
            Log.Warning("DeviceManager not found! Canvas could not be initialized. Defaulting to PC Canvas.");
            InitializeCanvas(false);
        }
    }

    private void Update()
    {
        if (currentPanels == null) return;
        if (HandleBackInput()) return;

        //Nur wenn im Startbildschirm
        if (currentPanels.mainMenuPanel != null && currentPanels.mainMenuPanel.activeSelf)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.anyKey.wasPressedThisFrame)
            {
                ShowPanel(currentPanels.modeSelectPanel);
            }
        }
    }

    private bool HandleBackInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            GoBack();
            return true;
        }

        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            ButtonControl bButton = gamepad.buttonEast;
            if (bButton != null && bButton.wasPressedThisFrame)
            {
                GoBack();
                return true;
            }
        }
        return false;
    }

    private void GoBack()
    {
        if (currentPanels == null) return;

        if (currentPanels.settingsPanel.activeSelf || currentPanels.modeSelectPanel.activeSelf)
        {
            ShowPanel(currentPanels.mainMenuPanel);
        }
        else if (currentPanels.classicPanel.activeSelf)
        {
            ShowPanel(currentPanels.modeSelectPanel);
        }
    }

    public void InitializeCanvas(bool isMobile)
    {
        _currentCanvas = Instantiate(isMobile ? _mobileCanvas : _pcCanvas);
        InitializePanels();
    }

    private void InitializePanels()
    {
        currentPanels = _currentCanvas.GetComponent<MainMenuPanels>();
        currentPanels.Initialize(this);
        ShowPanel(currentPanels.mainMenuPanel);
    }

    private void HideAllPanels()
    {
        currentPanels.mainMenuPanel.SetActive(false);
        currentPanels.settingsPanel.SetActive(false);
        currentPanels.modeSelectPanel.SetActive(false);
        currentPanels.classicPanel.SetActive(false);
    }

    public void ShowPanel(GameObject panel)
    {
        if (currentPanels == null || panel == null) return;

        HideAllPanels();
        panel.SetActive(true);
        currentPanels.SelectPanelDefault(panel);
    }

    public void OnModeSelectPressed(int modeIndex)
    {
        GameModeSelection.SelectedIndex = modeIndex;
        GameManager.Instance.LoadMode(modeIndex);
        SceneLoader.Instance.LoadScene("GameScene");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
