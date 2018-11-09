using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonUI
{
  public class RoomElementScript : MonoBehaviour
  {
    //Room情報UI表示用
    public Text RoomName; //部屋名
    public Text PlayerNumber; //人数
    public Text RoomCreator; //部屋作成者名

    //入室ボタン_roomName格納用
    private string _roomName;

    //####################################################################################################
    // GetRoomListからRoom情報をRoomElementにセットしていくための関数
    public void SetRoomInfo(string roomName, int playerNumber, int maxPlayer, string roomCreator)
    {
      Debug.Log(message: "=== SetRoomInfo ===");
      //入室ボタン用_roomName取得
      _roomName = roomName;
      RoomName.text = "部屋名：" + roomName;
      PlayerNumber.text = "人　数：" + playerNumber + "/" + maxPlayer;
      RoomCreator.text = "作成者：" + roomCreator;
    }

    //########################################
    //入室ボタン処理
    public void OnClick_JoinRoomButton()
    {
      //_roomNameの部屋に入室
      Debug.Log(message: "=== GetRoomList() ===\n" + PhotonNetwork.GetRoomList()[0] + "\n");
      PhotonNetwork.JoinRoom(_roomName);
    }

    //####################################################################################################
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
  }
}