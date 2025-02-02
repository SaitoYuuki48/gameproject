using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private GameObject clearText; // ClearText�I�u�W�F�N�g���i�[

    public static bool isGameClear = false;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas"); // Canvas ��������
        if (canvas != null)
        {
            clearText = canvas.transform.Find("ClearText")?.gameObject; // Canvas �̎q���� ClearText ��T��
        }

        if (clearText != null)
        {
            Debug.Log("ClearText ��������܂����B");
            clearText.SetActive(false);
        }
        else
        {
            Debug.LogError("ClearText ��������܂���BCanvas �̎q�� ClearText �����݂��邩�m�F���Ă��������B");
        }

        isGameClear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (clearText != null)
        {
            clearText.SetActive(true); // �S�[�����ɕ\��
        }
        isGameClear = true;
        Debug.Log("GOAL");
    }

}
