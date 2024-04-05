using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 사운드 매니저
     * @detail 소리 볼륨 크기에 따라 이미지 변화, 배경음과 효과음 조절(토글형식, Mute <-> All)
     */
    public class SoundManager : MonoBehaviour
    {
        [Header("Slider")]
        public Slider volumeSlider; ///< 전체 소리 볼륨 슬라이더
        [Header("Volume Image")]        
        public Image volumeImage; ///< 볼륨 이미지                                 
        public Sprite muteSprite; ///< 음소거 이미지 
        public Sprite lowVolumeSprite; ///< 볼륨 높을 때 이미지
        public Sprite highVolumeSprite; ///< 볼륨 낮을 때 이미지
        [Header("AudioSource")] 
        public AudioSource audioSource; ///< 오디오소스
        [Header("Mute Background Music")]
        public Slider backgroundMusicSlider; ///< 배경음악 음소거

        /**
         * @brief 사운드 설정
         */
        void Start()
        {
            #region 사운드 설정
            /* 슬라이더 최대값 최소값 설정 */
            volumeSlider.minValue = 0;
            volumeSlider.maxValue = 10;

            /* 슬라이더의 값을 최대값으로 설정 */
            float initialVolumeValue = audioSource.volume * 10;
            volumeSlider.value = initialVolumeValue;

            // 음량에 따라 이미지 변하는 HandleVolumeChanged 메서드 호출
            volumeSlider.onValueChanged.AddListener(HandleVolumeChanged);
            #endregion
            #region 배경음악 음소거
            // 음소거
            backgroundMusicSlider.minValue = 0;
            // 소리 나옴
            backgroundMusicSlider.maxValue = 1;
            // 토글 변경에 따라 음소거 가능한 BackgrundMusicSwitch메서드 호출
            backgroundMusicSlider.onValueChanged.AddListener(BackgroundMusicSwitch);
            #endregion
        }

        /**
         * @brief 소리에 따라 이미지 변경
         * @param[in] value 슬라이더 값
         */
        public void HandleVolumeChanged(float value)
        {
            /* 오디오 소스 볼륨 슬라이더의 값에 맞춤 */
            audioSource.volume = value * 0.1f;

            /* 음량에 따라 이미지 변경 */
            if (value <= 0)
            {
                volumeImage.sprite = muteSprite;
            }
            else if (value <= 5)
            {
                volumeImage.sprite = lowVolumeSprite;
            }
            else
            {
                volumeImage.sprite = highVolumeSprite;
            }
        }

        /**
         * @brief 배경음악 음소거
         * @param[in] 슬라이더 값
         */ 
        public void BackgroundMusicSwitch(float value)
        {
            audioSource.volume = Mathf.Round(value);
        }
    }
}
