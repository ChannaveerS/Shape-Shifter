using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 🔥 Only keep one instance alive
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ✅ Don't destroy this object when loading a new scene
    }
}
