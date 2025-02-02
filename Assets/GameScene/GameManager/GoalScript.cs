using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private GameObject clearText; // ClearTextオブジェクトを格納

    public static bool isGameClear = false;

    void Start()
    {
        GameObject canvas = GameObject.Find("Canvas"); // Canvas を見つける
        if (canvas != null)
        {
            clearText = canvas.transform.Find("ClearText")?.gameObject; // Canvas の子から ClearText を探す
        }

        if (clearText != null)
        {
            Debug.Log("ClearText が見つかりました。");
            clearText.SetActive(false);
        }
        else
        {
            Debug.LogError("ClearText が見つかりません。Canvas の子に ClearText が存在するか確認してください。");
        }

        isGameClear = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (clearText != null)
        {
            clearText.SetActive(true); // ゴール時に表示
        }
        isGameClear = true;
        Debug.Log("GOAL");
    }

}
