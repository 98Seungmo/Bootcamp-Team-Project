using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using KJ;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace KJ
{
    public class NetData : SingletonLazy<NetData>
    {
        public GameData gameData { get; private set; } ///< 클래스 데이터 참조
        public GameData _gameData { get; private set; } ///< 플레이어 데이터 참조

        /**
         * @brief 
         */
        private IEnumerator Start()
        {
            yield return null;
        }

        public IEnumerator LoadPlayerDB()
        {
            yield return null;
        }

        /**
         * @brief 플레이어 및 클래스 데이터 불러옴
         * @param[in] user 유저
         * @param[in] jsondata json으로 변환한 데이터
         */
        public IEnumerator LoadPlayerDB(FirebaseUser user, string jsondata)
        {
            if (gameData == null && _gameData == null)
            {
                TextAsset classData = Resources.Load<TextAsset>("ClassDB");
                gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(classData.text);

                TextAsset playerData = Resources.Load<TextAsset>("PlayerDB");
                _gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(playerData.text);
            }

            // Debug.Log("LoadPlayerDB(FirebaseUser user, string jsondata)");
            Player p = new Player();
            p.uid = user.UserId;
            p.shortUID = UIDHelper.GenerateShortUID(user.UserId);

            yield return null;
        }

        /**
         * @brief 저장했던 플레이어 데이터 블러오기 
         * @param userkey 유저 키 값
         */
        public void ReadDataPlayer(string userkey, Action callback = null)
        {
            DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("players");

            db.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                    Debug.LogError("ReadData  IsFaulted");
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("childerenCount" + snapshot.ChildrenCount);

                    /*유저 데이터를 복원 한다.*/
                    string strplayerdata = snapshot.Value.ToString();
                    if (!string.IsNullOrEmpty(strplayerdata))
                    {
                        Debug.Log("복원완료!" + _gameData);
                        _gameData = Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(strplayerdata);
                    }

                    if (callback != null)
                    {
                        callback();
                    }
                }
            });

        }

        /**
         * @brief 저장된 아이템 불러오기
         * @param[in] userkey 유저 키 값
         */
        public void ReadDataItem(string userkey, Action callback = null)
        {
            DatabaseReference db = FirebaseDatabase.DefaultInstance.GetReference("Items");

            db.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                    Debug.LogError("ReadData  IsFaulted");
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("childerenCount" + snapshot.ChildrenCount);

                    /* 아이템 데이터를 복원. */
                    string strItemdata = snapshot.Value.ToString();
                    Debug.Log($"{!string.IsNullOrEmpty(strItemdata)}");
                    if (!string.IsNullOrEmpty(strItemdata))
                    {
                        Debug.Log("복원완료2!" + ItemDBManager.Instance._itemData);
                        ItemDBManager.Instance._itemData = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemData>(strItemdata);
                    }

                    if (callback != null)
                    {
                        callback();
                    }
                }
            });

        }

        /**
         * @brief 현재 진행 중인 플레이어 데이터(클래스 포함)를 jsondata 로 저장한다.
         * @param[in] user 파이어베이스 유저값
         */
        public void SavePlayerDB(FirebaseUser user)
        {
            string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(_gameData.players);

            WritePlayerData(_gameData.players.Values.ToList()[0].uid, "players", jsondata);

            string jsondata2 = Newtonsoft.Json.JsonConvert.SerializeObject(ItemDBManager.Instance._itemData.items);

            Debug.Log($"{ItemDBManager.Instance._itemData.items[0].id}");
            WriteItemData(ItemDBManager.Instance._itemData.items[0].id, "Items", jsondata2);



        }
        /**
         * @brief Firebase Realtime Database 에 플레이어 데이터 저장 (직렬화)
         * @param[in] usermail 경로
         * @param[in] datakey 키 값
         * @param[in] jsondata value 값
         */
        public void WritePlayerData(string usermail, string datakey, string jsondata)
        {
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(ItemDBManager.Instance._itemData);

            DatabaseReference db = null;
            db = FirebaseDatabase.DefaultInstance.GetReference(usermail);

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(datakey, jsondata);

            db.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("dataupdate complete");
            });

        }

        /**
         * @brief Firebase Realtime Database 에 아이템 데이터 저장 (직렬화)
         * @param[in] usermail 경로
         * @param[in] idatakey 키 값
         * @param[in] jsondata2 value 값
         */
        public void WriteItemData(string itemdb, string idatakey, string jsondata2)
        {
            DatabaseReference db = null;
            db = FirebaseDatabase.DefaultInstance.GetReference(itemdb);

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(idatakey, jsondata2);

            db.UpdateChildrenAsync(data).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("dataupdate complete");
            });

        }
    }
}
