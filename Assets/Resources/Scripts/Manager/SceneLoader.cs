using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    void Awake()
    {
        // Singleton Pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 지정된 이름의 씬을 로드합니다.
    /// </summary>
    /// <param name="sceneName">로드할 씬의 이름</param>
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name cannot be empty!");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 게임을 종료합니다.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        // 에디터에서 실행 중일 경우, 플레이 모드를 중지합니다.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 어플리케이션을 종료합니다.
        Application.Quit();
#endif
    }
}