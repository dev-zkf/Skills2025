using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [BoxGroup("Panels"), SerializeField] private GameObject panelMainMenu;
    [BoxGroup("Panels"), SerializeField] private GameObject panelSettings;
    [BoxGroup("Panels"), SerializeField] private GameObject panelPauseMenu;

    public bool MainMenuEnabled => panelMainMenu != null && panelMainMenu.activeSelf;
    public bool SettingsEnabled => panelSettings != null && panelSettings.activeSelf;
    public bool PauseMenuEnabled => panelPauseMenu != null && panelPauseMenu.activeSelf;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleMainMenu() => panelMainMenu.SetActive(!panelMainMenu.activeSelf);
    public void ToggleMainMenu(bool value) => panelMainMenu.SetActive(value);
    

    public void ToggleSettings() => panelSettings.SetActive(!panelSettings.activeSelf);
    public void ToggleSettings(bool value) => panelSettings.SetActive(value);
    
    
    public void TogglePauseMenu() => panelPauseMenu.SetActive(!panelPauseMenu.activeSelf);
    public void TogglePauseMenu(bool value) => panelPauseMenu.SetActive(value);
    

    public UIManager ClearUI() // used in code
    {
        ClearUI_Internal();
        return this;
    }
    
    public void ClearUI_Event() => ClearUI_Internal();

    private void ClearUI_Internal()
    {
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelPauseMenu != null) panelPauseMenu.SetActive(false);
        
        Debug.Log("Cleared Ui");
    }

    [Button]
    private void DebugCheckBools()
    {
        Debug.Log($"MainMenuEnabled: {MainMenuEnabled}, SettingsEnabled: {SettingsEnabled}, PauseMenuEnabled: {PauseMenuEnabled}");
    }
    
    public void QuitGame() => Application.Quit();
    
}
