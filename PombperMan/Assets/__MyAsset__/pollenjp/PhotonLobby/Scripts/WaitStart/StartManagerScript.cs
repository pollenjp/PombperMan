using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WaitStart
{
  public class StartManagerScript : MonoBehaviour
  {
    public string PhotonLobbySceneName;
    public string LoadSceneName;
    public GameObject SceneLoaderObject;
    public GameObject WaitingCanvas;
    public Text UserNameListText;
    public GameObject EndBattleCanvas;
    public Text PlayerOrderText;
    public GameObject WaitingMusic;
    public GameObject BattleMusic;

    private bool _endStartFunc = false;
    private bool _isBattleScene = false;

    #region MonoBehaviour Callbacks

    // Use this for initialization
    private void Start()
    {
      //########################################
      if (!PhotonNetwork.connected) // Photonに接続されていなければ
      {
        if (!string.IsNullOrEmpty(value: PhotonLobbySceneName))
        {
          UnityEngine.SceneManagement.SceneManager.LoadScene(PhotonLobbySceneName);
        }

        return;
      }

      //########################################
      Debug.Log(message: "=== PhotonNetwork.isMasterClient ===\n" + PhotonNetwork.isMasterClient + "\n");
      //if (PhotonNetwork.isMasterClient)
      //{
      // buttonを生成
      //########################################
      // sceneLoaderを生成
      GameObject sceneLoader = PhotonNetwork.Instantiate(
        prefabName: this.SceneLoaderObject.name,
        position: new Vector3(0f, 0f, 0f),
        rotation: Quaternion.identity,
        group: 0);
      if (PhotonNetwork.room.CustomProperties["IsReady"].ToString() == "0") // 待機画面
      {
        WaitingCanvas.SetActive(value: true);

        WaitingMusic.SetActive(value: true); // 曲選択
        BattleMusic.SetActive(value: false); // 曲選択

        _isBattleScene = false;
        UpdatePlayerList();
      }
      else // バトル画面
      {
        WaitingMusic.SetActive(value: false); // 曲選択
        BattleMusic.SetActive(value: true); // 曲選択
        _isBattleScene = true;
      }


      _endStartFunc = true;
    }

    // Update is called once per frame
    private void Update()
    {
//      if (!_isBattleScene && _endStartFunc)
//      {
//        ////////////////////////////////////////
//        // PlayerNameListの初期化
//        UpdatePlayerList();
//        ////////////////////////////////////////
//      }
    }

    #endregion

    public void ActivateEndBattleCanvas()
    {
      EndBattleCanvas.SetActive(value: true);
    }

    public void UpdatePlayerOrderList()
    {
      if (PhotonNetwork.player.CustomProperties["IsDead"].ToString() == "1")
      {
        Debug.Log(message: "====================\n" +
                           "UpdatePlayerOrderList()\n\n");
        PlayerOrderText.text = "Rank | User Name\n";
        var photonPlayerList = PhotonNetwork.playerList;

        System.Array.Sort(
          photonPlayerList,
          (instance1, instance2) =>
            instance1.CustomProperties["DeadUtcTime"].ToString().CompareTo(
              instance2.CustomProperties["DeadUtcTime"].ToString()));

        var length = photonPlayerList.Length;
        for (int i = length - 1; i >= 0; i--)
        {
          Debug.Log("====================\n" +
                    "photonPlayerList[i].NickName : " + photonPlayerList[i].NickName +
                    "photonPlayerList[i].CustomProperties[\"DeadUtcTime\"].ToString() : " +
                    photonPlayerList[i].CustomProperties["DeadUtcTime"].ToString());
          if (photonPlayerList[i].CustomProperties["IsDead"].ToString() == "0")
          {
            Debug.Log("====================\n" +
                      "PlayerOrderPanel : IsDead = 0");
            //PlayerOrderText.text = "Order | User Name\n";
            PlayerOrderText.text += "         | " + photonPlayerList[i].NickName + "\n";
          }
          else
          {
            Debug.Log("====================\n" +
                      "PlayerOrderPanel : IsDead = 1");
            //PlayerOrderText.text = "Order | User Name\n";
            PlayerOrderText.text += "      " + (length - i).ToString() + " | " + photonPlayerList[i].NickName + "\n";
          }
        }
      }
    }

    public void UpdatePlayerList()
    {
      ////////////////////////////////////////////////////////////////////////////////
      Debug.Log("\n====================" +
                "public void UpdatePlayerList()" + "\n");
      UserNameListText.text = "Ready? | User Name\n";
      ////////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////////
      // Sort Player Name List
      //  - Sorting a List by Variable? - Unity Answers
      //    - https://answers.unity.com/questions/677070/sorting-a-list-linq.html
      var photonPlayerList = PhotonNetwork.playerList;
      System.Array.Sort(photonPlayerList, (instance1, instance2) => instance1.NickName.CompareTo(instance2.NickName));
//      string[] photonPlayerNameList = new string[photonPlayerList.Length];
//      for (int i = 0; i < photonPlayerList.Length; i++)
//      {
//        photonPlayerNameList[i] = photonPlayerList[i].NickName;
//      }
//      Array.Sort(array: photonPlayerNameList, comparer: StringComparer.InvariantCulture);
      ////////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////////
      foreach (var photonPlayer in photonPlayerList)
      {
        Debug.Log("\n" +
                  "=== photonPlayer.NickName ===" + photonPlayer.NickName + "\n");
        if (photonPlayer.CustomProperties["IsReady"].ToString() == "0")
        {
          //                       "Ready? | User Name\n";
          UserNameListText.text += " NO       ";
        }
        else
        {
          //                       "Ready? | User Name\n";
          UserNameListText.text += " OK       ";
        }

        UserNameListText.text += "| " + photonPlayer.NickName + "\n";
      }

      Debug.Log("\n==================== UserNameListText.text\n" +
                UserNameListText.text + "\n");
      ////////////////////////////////////////////////////////////////////////////////
      ////////////////////////////////////////////////////////////////////////////////
    }

    #region Photon Callbacks

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
      UpdatePlayerList();
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
      UpdatePlayerList();
    }

    #endregion
  }
}