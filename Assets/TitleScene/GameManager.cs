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

        // �t�F�[�h�C��������̏����i�K�v�Ȃ�ݒ�j
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
                // �t�F�[�h�A�E�g������ɃV�[���J�ڂ����s
                SceneManager.LoadScene(nextSceneName);
            };

            fadeManager.StartFadeOut();
        }
    }
}
