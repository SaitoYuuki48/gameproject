using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    public float damageInterval = 1.0f; // ダメージを与える間隔（秒）
    public float damageAmount = 1.0f;  // ダメージ量

    private HashSet<PlayerScript> playersInTrigger = new HashSet<PlayerScript>(); // 床に触れているプレイヤーを管理

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null && !playersInTrigger.Contains(player))
            {
                // プレイヤーがトリガーに入った場合、ダメージ処理を開始
                playersInTrigger.Add(player);
                StartCoroutine(DealDamage(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null && playersInTrigger.Contains(player))
            {
                // プレイヤーがトリガーから出た場合、ダメージ処理を停止
                playersInTrigger.Remove(player);
            }
        }
    }

    private IEnumerator DealDamage(PlayerScript player)
    {
        while (playersInTrigger.Contains(player))
        {
            // プレイヤーが無敵状態でなければダメージを与える
            if (!player.IsInvincible) // PlayerScriptに無敵状態を確認するプロパティを追加します
            {
                player.TakeDamage(damageAmount);
                // プレイヤーがダメージを受けた後、一定間隔待機
                yield return new WaitForSeconds(player.InvincibleDuration); // 無敵時間分待機
            }
            else
            {
                // 無敵時間中はダメージを与えずに待機
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }
}
