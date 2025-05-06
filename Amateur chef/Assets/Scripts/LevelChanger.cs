using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    public Levels level;
    
    public void InitializeLevelChange() => LevelManager.Instance.ChangeLevel(level);
}
