using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PhotonUI
{
  public class ConnectServerScript : MonoBehaviour
  {
    public GameObject Menu1, Menu2;
    
    [SerializeField]
    private Text PhotonMasterServerAddress;
    [SerializeField]
    private  Text PhotonMasterServerPort;
    [SerializeField]
    private  Text PhotonAppId;
    [SerializeField]
    private  Text GameVersion;
    
    //####################################################################################################

    #region MonoBehaviour callbacks

    //########################################
    // Use this for initialization
    private void Start()
    {
      // 初期で表示するCanvasの設定
      Menu1.SetActive(value: true);
      Menu2.SetActive(value: true);
      Menu1.GetComponent<Canvas>().enabled = true;
      Menu2.GetComponent<Canvas>().enabled = false;
      
      //
      PhotonMasterServerAddress = UnityEngine.GameObject.Find(name: "PhotonMasterServerAddressInputField").transform.Find("Text").GetComponent<Text>();
      PhotonMasterServerPort    = UnityEngine.GameObject.Find(name: "PhotonMasterServerPortInputField").transform.Find("Text").GetComponent<Text>();
      PhotonAppId               = UnityEngine.GameObject.Find(name: "PhotonAppIdInputField").transform.Find("Text").GetComponent<Text>();
      GameVersion               = UnityEngine.GameObject.Find(name: "GameVersionInputField").transform.Find("Text").GetComponent<Text>();
    }

    //########################################
    // Update is called once per frame
    private void Update()
    {
      
    }

    #endregion

    //####################################################################################################
    public void OnClick_ConnectServerButton()
    {
      //PhotonNetwork.ConnectUsingSettings(gameVersion: "v1.0");
      PhotonNetwork.ConnectToMaster(
        masterServerAddress: PhotonMasterServerAddress.text,
        port: int.Parse(PhotonMasterServerPort.text),
        appID: PhotonAppId.text,
        gameVersion: GameVersion.text);
      //ChangeCanvas(canvasGameObject1: Menu1, canvasGameObject2: Menu2);
    }

    //####################################################################################################

    #region Photon Callbacks

    private void OnConnectedToMaster()
    {
      PhotonNetwork.JoinLobby();
    }
    
    private void OnJoinedLobby()
    {
      ChangeCanvas(canvasGameObject1: Menu1, canvasGameObject2: Menu2);
      
      //########################################
      //  入力情報の保存（次回入力省略）
      //  - player name
      Debug.Log("=== OnJoinedLobby ===\n");
      GameObject userIdInputField = UnityEngine.GameObject.Find("UserIdInputField");
      GameObject photonPlayerName = UnityEngine.GameObject.FindWithTag(tag: "PhotonPlayerName");
      Debug.Log(message: "== photonPlayerNameArr ==\n" + photonPlayerName);
      Text playerIdText = photonPlayerName.GetComponent<Text>();
      Debug.Log(message: "== playerIdText.text ==\n" + playerIdText.text);
      //SetPlayerName(value: playerIdText.text);
      Debug.Log(message: "=== SetPlayerName ===\n" + playerIdText.text + "\n");
      PhotonNetwork.playerName = playerIdText.text + " "; //今回ゲームで利用するプレイヤーの名前を設定
      Debug.Log(PhotonNetwork.player.NickName); //playerの名前の確認。（動作が確認できればこの行は消してもいい）
      
      //########################################
      //  入力情報の保存（次回入力省略）
      //  - server ip
      //  - server port
      //  - appid
      //  - game version
      GameObject[] connectServerInputFieldTag = UnityEngine.GameObject.FindGameObjectsWithTag(tag: "ConnectServerInputField");
      foreach (var inputFieldGameObject in connectServerInputFieldTag)
      {
        var inputKeyword = inputFieldGameObject.GetComponent<InputFieldScript>().InputKeyword;
        var inputValue   = inputFieldGameObject.transform.Find(n: "Text").gameObject.GetComponent<Text>().text;
        Debug.Log(message: "== inputKeyword ==\n" + inputKeyword + "\n");
        Debug.Log(message: "== inputValue.text ==\n" + playerIdText.text + "\n");
        UnityEngine.PlayerPrefs.SetString(key: inputKeyword, value: inputValue);
      }
      //PlayerPrefs.SetString(InputKeyword, value);
      Debug.Log(message: "== PhotonNetwork.isMasterClient ==\n" + PhotonNetwork.isMasterClient);
    }

    #endregion

    //####################################################################################################

    #region Public Functions

    // Connect server Menu --> Join Room Menu
    public void ChangeCanvas(GameObject canvasGameObject1, GameObject canvasGameObject2)
    {
      canvasGameObject1.GetComponent<Canvas>().enabled = false;
      canvasGameObject2.GetComponent<Canvas>().enabled = true;
    }

    public void SetPlayerName(string value)
    {
      Debug.Log(message: "=== SetPlayerName ===\n" + value + "\n");
      PhotonNetwork.playerName = value + " "; //今回ゲームで利用するプレイヤーの名前を設定
      Debug.Log(PhotonNetwork.player.NickName); //playerの名前の確認。（動作が確認できればこの行は消してもいい）
    }

    #endregion
  }
}