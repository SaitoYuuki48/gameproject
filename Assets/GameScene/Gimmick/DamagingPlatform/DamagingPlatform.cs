using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    public float damageInterval = 1.0f; // �_���[�W��^����Ԋu�i�b�j
    public float damageAmount = 1.0f;  // �_���[�W��

    private HashSet<PlayerScript> playersInTrigger = new HashSet<PlayerScript>(); // ���ɐG��Ă���v���C���[���Ǘ�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null && !playersInTrigger.Contains(player))
            {
                // �v���C���[���g���K�[�ɓ������ꍇ�A�_���[�W�������J�n
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
                // �v���C���[���g���K�[����o���ꍇ�A�_���[�W�������~
                playersInTrigger.Remove(player);
            }
        }
    }

    private IEnumerator DealDamage(PlayerScript player)
    {
        while (playersInTrigger.Contains(player))
        {
            // �v���C���[�����G��ԂłȂ���΃_���[�W��^����
            if (!player.IsInvincible) // PlayerScript�ɖ��G��Ԃ��m�F����v���p�e�B��ǉ����܂�
            {
                player.TakeDamage(damageAmount);
                // �v���C���[���_���[�W���󂯂���A���Ԋu�ҋ@
                yield return new WaitForSeconds(player.InvincibleDuration); // ���G���ԕ��ҋ@
            }
            else
            {
                // ���G���Ԓ��̓_���[�W��^�����ɑҋ@
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }
}
