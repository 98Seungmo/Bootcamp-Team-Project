using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingLight : MonoBehaviour
{
    public int healAmount = 100; // �÷��̾�� ȸ����ų ü�·�

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(1);
        // �浹�� ��ü�� �÷��̾����� Ȯ��
        if (other.CompareTag("Player"))
        {
            Debug.Log(2);
            // �÷��̾��� HealthController ��ũ��Ʈ�� ������
            // HealthController playerHealth = other.GetComponent<HealthController>();

            // HealthController ��ũ��Ʈ�� �����ϸ�
            //if (playerHealth != null)
            //{
            // �÷��̾��� ü���� ��� ȸ��
            //playerHealth.Heal(playerHealth.maxHealth);

            // �ش� ��ü�� �ı�
            Destroy(gameObject);
            //}
        }
    }
}
