using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string nextSceneName;
    private FadeManager fadeManager;

    void Start()
    {
        fadeManager = FindObjectOfType<FadeManager>();
        if (fadeManager == null)
        {
            Debug.LogError("GameManager: FadeManager not found!");
            return;
        }

        // フェードイン完了後の処理（必要なら設定）
        fadeManager.onFadeInComplete = () =>
        {
            Debug.Log("Fade-in completed");
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 0"))
        {
            StartSceneTransition();
        }
    }

    void StartSceneTransition()
    {
        if (fadeManager != null)
        {
            fadeManager.onFadeOutComplete = () =>
            {
                // フェードアウト完了後にシーン遷移を実行
                SceneManager.LoadScene(nextSceneName);
            };

            fadeManager.StartFadeOut();
        }
    }
}
