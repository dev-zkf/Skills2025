using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
	public static DontDestroyOnLoad Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);
		
		DontDestroyOnLoad(this.gameObject);
	}
}
