using KJ;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace KJ
{
    /**
     * @brief UID 를 해시함수(SHA256) 이용해서 ShortUID 로 변환
     */
    /* UID 를 해시함수를 이용해서 ShortUID 로 변환. */
    public static class UIDHelper
    {
        public static string GenerateShortUID(string longUID)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                /* SHA256 해시 값을 계산. */
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(longUID));

                /* 바이트 배열을 String 으로 변환. */
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                /* 해시값의 앞부분만 사용하여 ShortUID 생성. */
                return builder.ToString().Substring(0, 8);  // 앞의 8자리만 사용
            }
        }
    }

    /**
     * @brief 플레이어DB 관리
     */
    public class PlayerDBManager : SingletonLazy<PlayerDBManager>
    {

        private void Start()
        {

        }

        /**
         * @brief 플레이어DB 로드
         */
        public IEnumerator LoadPlayerDB()
        {
            yield return NetData.Instance.LoadPlayerDB();
        }

        public string CurrentShortUID { get; private set; } ///< 현재 ShortUID 값

        /**
         * @brief 해시함수로 변환된 ShortUID 로 플레이어 데이터를 구분
         * @param[in] UID Firebase Auth 인증된 값
         * @param[in] playerData 플레이어 데이터
         */
        public void SaveOrUpdatePlayerData(string UID, Player playerData)
        {
            string shortUID = UIDHelper.GenerateShortUID(UID);
            CurrentShortUID = shortUID;

            playerData.shortUID = shortUID;
            GameData _gamedata = NetData.Instance._gameData;

            if (_gamedata.players.ContainsKey(shortUID))
            {
                _gamedata.players[shortUID] = playerData;
            }
            else
            {
                _gamedata.players.Add(shortUID, playerData);
            }
        }

        /**
         * @brief ShortUID 값 이용해서 플레이어 데이터 체크
         * @param[in] shortUID ShortUID
         */
        public bool CheckPlayerData(string shortUID)
        {
            if (string.IsNullOrEmpty(shortUID)) return false;

            GameData _gamedata = NetData.Instance._gameData;
            return _gamedata.players.ContainsKey(shortUID);
        }

        /**
         * @brief ShortUID 값 이용해서 플레이어 데이터 불러옴
         * @param[in] shortUID ShortUID
         */
        public Player LoadGameData(string shortUID)
        {
            GameData _gamedata = NetData.Instance._gameData;
            if (_gamedata.players.TryGetValue(shortUID, out Player playerData))
            {
                /* 플레이어 위치나 인벤토리 그리고 능력치 및 설정 저장. */
                return playerData;
            }
            else
            {
                /* 새 플레이어 생성 로직 추가. */
                return null;
            }
        }
    }
}


