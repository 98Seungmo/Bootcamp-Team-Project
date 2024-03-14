using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Player �±׸� ���� ������Ʈ�� �ƴϸ� �ٷ� ��ȯ
        if (!other.CompareTag("Player"))
            return;

        // ���� Ȱ��ȭ�� ���̸��� ��´�.
        Scene nowScene = SceneManager.GetActiveScene();

        switch (nowScene.name)
        {
            case "Dongeon Enterance":
                SceneManager.LoadScene("Dongeon First");
                break;
            case "Dongeon First":
                SceneManager.LoadScene("Dongeon Enterance");
                break;

        }
    }
}