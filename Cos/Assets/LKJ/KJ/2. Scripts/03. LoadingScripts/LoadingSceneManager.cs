using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    void Start()
    {
        #region �����̴��� �̿��� �ε�â���� ���� ������ �Ѿ��.
        // LoadAsyncSceneCoroutine() �� �ڷ�ƾ���� ����.
        StartCoroutine(LoadAsyncSceneCoroutine());
        #endregion
        #region �ε� ���� ���� ���丮 �ؽ�Ʈ �������� ����
        // ����Ʈ�� gameStory ������Ʈ �߰�
        gameStoryList.Add(gameStory);
        gameStoryList.Add(gameStory2);
        gameStoryList.Add(gameStory3);

        // ���� ������ ����Ʈ ����
        _randomNum = Random.Range(0, gameStoryList.Count);
        randomStory = gameStoryList[_randomNum];

        // �������� ���õ� �ؽ�Ʈ�� Ȱ��ȭ �������� ��Ȱ��ȭ.
        foreach (var story in gameStoryList)
        {
            // ��� ���丮�� ��Ȱ��ȭ.
            story.gameObject.SetActive(false);
        }
        // �������� ���õ� ���丮 Ȱ��ȭ.
        randomStory.gameObject.SetActive(true);
        #endregion
    }
    #region �����̴��� �̿��� �ε�â���� ���� ������ �Ѿ��. 
    [Header("Slider")]
    public Slider slider;
    public string sceneName;

    private float _time;


    IEnumerator LoadAsyncSceneCoroutine()
    {
        // sceneName ���� �񵿱� �������� �Ѿ�� �ϴ� operation ����.
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        // operation �� �Ϸ�Ǿ ���� ������ �Ѿ�°� ����.
        operation.allowSceneActivation = false;

        // operation.isDone �� false �� ���� �ݺ�.
        while (!operation.isDone)
        {
            // ��� �ð��� ��Ȯ�ϰ� ����.
            _time += Time.deltaTime;
            // slider�� ������� 10�ʱ��� ������� ǥ��
            slider.value = _time / 10f;
            
            // 10�ʰ� ������
            if (_time > 10)
            {
                // ���� ������ �Ѿ���� Ȱ��ȭ.
                operation.allowSceneActivation = true;
            }
            
            //���� �����ӱ��� ���.
            yield return null;
        }
    }
    #endregion
    #region �ε� ���� ���� ���丮 �ؽ�Ʈ �������� ����
    [Header("RandomText")]
    public TMP_Text randomStory;
    public TMP_Text gameStory;
    public TMP_Text gameStory2;
    public TMP_Text gameStory3;

    private int _randomNum;

    List<TMP_Text> gameStoryList = new List<TMP_Text>();
    #endregion
}
