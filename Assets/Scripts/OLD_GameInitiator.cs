using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private Camera _cameraPrefab;
    [SerializeField] private GameObject _postProcessingVolumePrefab;
    [SerializeField] private GameObject _eventSystemPrefab;
    [SerializeField] private SceneLoader _sceneLoaderPrefab;

    [Header("Manager")]
    [SerializeField] private DeviceManager _deviceManagerPrefab;
    [SerializeField] private GameManager _gameManagerPrefab;
    [SerializeField] private MenuManager _menuManagerPrefab;
    [SerializeField] private AudioManager _audioManagerPrefab;

    private Camera _camera;
    private GameObject _postProcessingVolume;
    private DeviceManager _deviceManager;
    private GameManager _gameManager;
    private MenuManager _menuManager;
    private AudioManager _audioManager;
    private GameObject _eventSystem;
    private SceneLoader _sceneLoader;

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await InitApp();
    }

    private async Task InitApp()
    {
        if (_camera == null)
        {
            _camera = Instantiate(_cameraPrefab);
            DontDestroyOnLoad(_camera);
        }

        if (_postProcessingVolume == null)
        {
            _postProcessingVolume = Instantiate(_postProcessingVolumePrefab);
            DontDestroyOnLoad(_postProcessingVolume);
        }

        if (_eventSystem == null)
        {
            _eventSystem = Instantiate(_eventSystemPrefab);
            DontDestroyOnLoad(_eventSystem);
        }

        if (_gameManager == null)
        {
            _gameManager = Instantiate(_gameManagerPrefab);
            DontDestroyOnLoad(_gameManager);
        }

        if (_sceneLoader == null)
        {
            _sceneLoader = Instantiate(_sceneLoaderPrefab);
            DontDestroyOnLoad(_sceneLoader);
        }

        _deviceManager = Instantiate(_deviceManagerPrefab);
        _menuManager = Instantiate(_menuManagerPrefab);

        await CreateObjects();
    }

    private async Task CreateObjects()
    {
        _deviceManager.InitializeScreen();
        _menuManager.InitializeCanvas(_deviceManager.IsMobile());
        //_menuManager.SetSceneLoader(_sceneLoader);

        _gameManager.InitializeModes();

        await Task.CompletedTask;
    }
}
