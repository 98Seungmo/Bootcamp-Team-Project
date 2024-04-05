using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonLazy<T> : MonoBehaviour where T : class
{
    // Lazy 인스턴스를 사용해 지연 초기화된 단일 인스턴스를 제공
    private static readonly Lazy<T> _instance =
        new Lazy<T>(() =>
        {
            // 해당 타입의 인스턴스를 찾고
            T instance = FindObjectOfType(typeof(T)) as T;

            // 인스턴스가 없는 경우
            if (instance == null)
            {
                // 새로운 게임 오브젝트를 만들고 해당 타입의 컴포넌트를 추가하여 인스턴스를 만듭니다.
                GameObject obj = new GameObject("SingletonLazy");
                instance = obj.AddComponent(typeof(T)) as T;

                // 씬 변경 시에도 유지되도록 설정합니다.
                DontDestroyOnLoad(obj);
            }
            else
            {
                // 이미 존재하는 인스턴스가 있으면 해당 인스턴스를 제거합니다.
                Destroy(instance as GameObject);
            }

            return instance;
        });

    // 외부에서 인스턴스에 접근할 수 있는 속성을 제공합니다.
    public static T Instance
    {
        get
        {
            // Lazy로 초기화된 인스턴스를 반환합니다.
            return _instance.Value;
        }
    }
}
// 싱글톤 상속
public class SFX_Manager : SingletonLazy<SFX_Manager>
{
    //카메라 변수 선언
    public Camera Camera;
    //오디오 소스 변수 선언
    public AudioSource BGM;



    //유니티에서 접근 가능한 프라이빗 오디오클립 리스트 컴포넌넌트 SFX변수 선언
    [SerializeField]
    private List<AudioClip> _audioClips_sfx;
    //유니티에서 접근 가능한 프라이빗 오디오클립 리스트 컴포넌넌트 BGM 변수 선언
    [SerializeField]
    private List<AudioClip> _audioBGM;

    //브금 오디오소스 변수가 널일때 카메라에 있는 오디오소스를 열고 브금플레이( 브금 확정 시 변경 )함수를 실행 / 실험용
    public void Start()
    {
        if (BGM == null)
        {
            Camera.GetComponent<AudioSource>();
        }
        BGMPLAY(1);
    }

    //브금플레이 함수는 브금 오디오소스가 플레이중일때 멈추면
    public void BGMPLAY(int bgm)
    {
        if (BGM.isPlaying) BGM.Stop();

        if (bgm < 0 || bgm >= _audioBGM.Count)
        {
            Debug.LogError("U can't play BGM");
            return;

        }

        AudioClip bgmclip = _audioBGM[bgm];


        //브금클립을 할당하고
        BGM.clip = bgmclip;
        //브금을 플레이한다.
        BGM.Play();
    }
    public void VFX(int sfx)
    {
        if (sfx < 0 || sfx >= _audioClips_sfx.Count)
        {
            Debug.LogError("Can't play SFX");
            return;
        }
        // audiosource 선언, destroysfx 선언
        AudioClip clip = _audioClips_sfx[sfx];

        //go 가 sfx 쓰는 게임오브젝트임
        GameObject go = new GameObject(sfx.ToString());

     
        // 위에서 선언한 audiosource에 go 게임오브젝트(오디오소스 인스펙터창을 추가한)넣음
        AudioSource audiosource = go.AddComponent<AudioSource>();
        //위에서 선언한 destrosfx에 go 게임오브젝트(DestroySFX 다른 스크립트 인스펙터창)를 넣음
        DestroySFX destroysfx = go.AddComponent<DestroySFX>();

        //destroysfx에 있는 _audioSource는 audiosource이다
        destroysfx._audioSource = audiosource;
        //audiosource에 클립은 사용할 clip를 받아온다
        audiosource.clip = clip;

        //go의 생성위치(카메라 애착)
        go.transform.parent = Camera.transform;
        go.transform.position = Vector3.zero;

        //audiosource.volume = 0.5f;  
        //audiosource 실행
        audiosource.Play();
    }


}
