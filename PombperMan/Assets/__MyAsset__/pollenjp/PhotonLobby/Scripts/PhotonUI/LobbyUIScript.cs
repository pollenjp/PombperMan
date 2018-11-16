using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonUI
{
  public class LobbyUIScript : MonoBehaviour
  {
    #region Variables

    //部屋作成ウインドウ表示用ボタン
    public Button OpenRoomPanelButton;

    //部屋作成ウインドウ
    public GameObject RoomInfoPanel;
    public GameObject CreateRoomPanel; //部屋作成ウインドウ
    public Text RoomNameText; //作成する部屋名
    public Slider PlayerNumberSlider; //最大入室可能人数用Slider
    public Text PlayerNumberText; //最大入室可能人数表示用Text
    public Button CreateRoomButton; //部屋作成ボタン

    #endregion

    //####################################################################################################

    #region OnClick Function

    //########################################
    //部屋作成ウインドウ表示用ボタンを押したときの処理
    public void OnClick_OpenRoomPanelButton()
    {
      // 部屋作成ウインドウが表示していれば => 部屋作成ウインドウを非表示に
      // そうでなければ                    => 部屋作成ウインドウを表示
      if (CreateRoomPanel.activeSelf)
      {
        CreateRoomPanel.SetActive(false);
      }
      else
      {
        CreateRoomPanel.SetActive(true);
      }
    }

    //########################################
    // ルーム新規作成ボタン処理
    public void OnClick_CreateNewRoomButton()
    {
      RoomInfoPanel.SetActive(value: false);
      CreateRoomPanel.SetActive(value: true);
    }

    //########################################
    //部屋作成ボタンを押したときの処理
    public void OnClick_CreateRoomButton()
    {
      //####################
      //作成する部屋の設定
      RoomOptions roomOptions = new RoomOptions();
      roomOptions.IsVisible = true; //ロビーで見える部屋にする
      roomOptions.IsOpen = true; //他のプレイヤーの入室を許可する
      roomOptions.MaxPlayers = (byte) PlayerNumberSlider.value; //入室可能人数を設定
      //ルームカスタムプロパティで部屋作成者を表示させるため、作成者の名前を格納
      roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
      {
        {"RoomCreator", PhotonNetwork.playerName}
      };
      Debug.Log(message: "=== PhotonNetwork.playerName ===\n" + PhotonNetwork.playerName);
      //####################
      //ロビーにカスタムプロパティの情報を表示させる
      roomOptions.CustomRoomPropertiesForLobby = new string[]
      {
        "RoomCreator",
      };
      //部屋作成
      // - PhotonNetwork.CreateRoom
      //     https://doc-api.photonengine.com/en/pun/current/class_photon_network.html#a08435c2d064fd6a85e51e1520e5a63d8
      //     When successful, this calls the callbacks OnCreatedRoom and OnJoinedRoom
      //     (the latter, cause you join as first player).  <-- Creator have to join the room.
      PhotonNetwork.CreateRoom(roomName: RoomNameText.text, roomOptions: roomOptions, typedLobby: null);
      //PhotonNetwork.JoinOrCreateRoom(roomName: RoomNameText.text, roomOptions: roomOptions, typedLobby: null);

      // パネル切り替え
      CreateRoomPanel.SetActive(value: false);
      RoomInfoPanel.SetActive(value: true);
    }

    #endregion

    //####################################################################################################

    #region MonoBehaviour Callbacks

    //########################################
    // Use this for initialization
    private void Start()
    {
      RoomInfoPanel.SetActive(value: true);
      CreateRoomPanel.SetActive(value: false);
    }

    //########################################
    // Update is called once per frame
    private void Update()
    {
      //部屋人数Sliderの値をTextに代入
      PlayerNumberText.text = PlayerNumberSlider.value.ToString();
    }

    #endregion
  }
}