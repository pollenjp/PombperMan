using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PhotonUI
{
  public class LobbyManagerScript : Photon.MonoBehaviour
  {
    #region Public Variables

    // JoinRoom後のScene
    public string JoinSceneName;

    //部屋一覧表示用オブジェクト
    public GameObject RoomParent; //ScrolViewのcontentオブジェクト
    public GameObject RoomElementPrefab; //部屋情報Prefab

    //ルーム接続情報表示用Text
    public Text InfoText;

    #endregion

    //####################################################################################################

    #region MonoBehaviour CallBacks

    private void Awake()
    {
      //ルーム内のクライアントがMasterClientと同じシーンをロードするように設定
      PhotonNetwork.automaticallySyncScene = true;
    }

    //########################################
    private void Start()
    {
    }

    //########################################
    private void Update()
    {
      //RoomInfo[] roomInfos = PhotonNetwork.GetRoomList();
      //Debug.Log(message: "=== PhotonNetwork.GetRoomList ===\n" + roomInfos);

      //InfoText.text = PhotonNetwork.connected.ToString();
    }

    #endregion

    //####################################################################################################

    #region Public Methods

    public void GetRooms()
    {
      //roomInfoに現在存在するルーム情報を格納・更新
      RoomInfo[] roomInfo = PhotonNetwork.GetRoomList();

      //ルームが無ければreturn
      if (roomInfo == null || roomInfo.Length == 0)
      {
        Debug.Log(message: "No Rooms");
        return;
      }

      //ルームがあればRoomElementでそれぞれのルーム情報を表示
      for (int i = 0; i < roomInfo.Length; i++)
      {
        Debug.Log("====================\n" +
                  "roomInfo[i].Name:" + roomInfo[i].Name + ", " +
                  "roomInfo[i].PlayerCount / roomInfo[i].MaxPlayers : " +
                  roomInfo[i].PlayerCount + " / " + roomInfo[i].MaxPlayers
          //+ roomInfo[i].CustomProperties["roomCreator"].ToString()
        );

        //ルーム情報表示用RoomElementを生成
        Debug.Log("\n=== roomElement = GameObject.Instantiate(RoomElementPrefab); ===\n");
        GameObject roomElement = GameObject.Instantiate(RoomElementPrefab);

        //RoomElementをcontentの子オブジェクトとしてセット    
        Debug.Log("\n=== roomElement.transform.SetParent(RoomParent.transform); ===\n");
        roomElement.transform.SetParent(RoomParent.transform);
        ////////////////////////////////////////
        Debug.Log("\n=== roomInfo[i].Name:" + roomInfo[i].Name + " ===\n");
        Debug.Log("\n=== roomInfo[i].PlayerCount:" + roomInfo[i].PlayerCount + " ===\n");
        Debug.Log("\n=== roomInfo[i].MaxPlayers:" + roomInfo[i].MaxPlayers + " ===\n");
        Debug.Log("\n=== roomInfo[i].CustomProperties[\"RoomCreator\"].ToString():" +
                  roomInfo[i].CustomProperties["RoomCreator"].ToString() + " ===\n");
        Debug.Log("\n=== roomInfo[i].CustomProperties[\"StageName\"].ToString():" +
                  roomInfo[i].CustomProperties["StageName"].ToString() + " ===\n");
        ////////////////////////////////////////
        //RoomElementにルーム情報をセット
        roomElement.GetComponent<RoomElementScript>().SetRoomInfo(
          roomName: roomInfo[i].Name,
          playerNumber: roomInfo[i].PlayerCount,
          maxPlayer: roomInfo[i].MaxPlayers,
          roomCreator: roomInfo[i].CustomProperties["RoomCreator"].ToString(),
          stageName: roomInfo[i].CustomProperties["StageName"].ToString()
        );
      }
    }

    //########################################
    //RoomElementを一括削除
    public static void DestroyChildObject(Transform parentTrans)
    {
      for (int i = 0; i < parentTrans.childCount; ++i)
      {
        GameObject.Destroy(parentTrans.GetChild(i).gameObject);
      }
    }

    #endregion

    //####################################################################################################

    #region Photon.PunBehaviour CallBacks

    //########################################
    //GetRoomListは一定時間ごとに更新され、その更新のタイミングで実行する処理
    private void OnReceivedRoomListUpdate()
    {
      Debug.Log("=== OnReceivedRoomListUpdate ===\n");
      DestroyChildObject(RoomParent.transform); //RoomElementを削除
      GetRooms(); //RoomElementを再生成
    }

    //########################################
    private void OnCreatedRoom()
    {
      Debug.Log(
        message: "====================\n" +
                 "=== OnCreatedRoom ===\n" +
                 "== PhotonNetwork.isMasterClient == : " +
                 PhotonNetwork.isMasterClient + "\n");
    }

    //##########
    private void OnJoinedRoom()
    {
      Debug.Log(
        message: "====================\n" +
                 "=== OnJoinedRoom ===\n" +
                 "=== PhotonNetwork.isMasterClient == : " +
                 PhotonNetwork.isMasterClient + "\n" +
                 "=== PhotonNetwork.room.CustomProperties[\"StageName\"].ToString() == : " +
                 PhotonNetwork.room.CustomProperties["StageName"].ToString() + "\n"
      );

      //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName: JoinSceneName);
      UnityEngine.SceneManagement.SceneManager.LoadScene(
        sceneName: PhotonNetwork.room.CustomProperties["StageName"].ToString());
    }

    //########################################
    // Failed
    //##########
    //ルーム作成失敗した場合
    private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
      //テキストを表示
      var errMessage = "ルームの作成に失敗しました";
      InfoText.text = errMessage;
      Debug.Log(message: "=== OnPhotonCreateRoomFailed ===\n" + errMessage);
    }

    //##########
    //ルームの入室に失敗した場合
    private void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
      //テキストを表示
      var errMessage = "ルームの入室に失敗しました";
      InfoText.text = errMessage;
      Debug.Log(message: "=== OnPhotonJoinRoomFailed ===\n" + errMessage);
    }

    #endregion
  }
}