using System.Collections;
using System.Collections.Generic;
using ExitGames.UtilityScripts;
using UnityEngine;
using UnityEngineInternal.Input;
using WaitStart;

public class WaitingRoomCanvasScript : MonoBehaviour
{
  //public GameObject StartManagerGameObject;

  private string LoadSceneName;
  private PhotonView m_photonView = null;

  private void Awake()
  {
    m_photonView = this.GetComponent<PhotonView>();
  }

  // Use this for initialization
  void Start()
  {
//    if (PhotonNetwork.room.CustomProperties["IsReady"].ToString() == "0")
//    {
//      PhotonNetwork.player.SetCustomProperties(
//        new ExitGames.Client.Photon.Hashtable()
//        {
//          {"IsReady", "0"}
//        }
//      );
//    }

    //LoadSceneName = UnityEngine.GameObject.Find(name: "StartManager").GetComponent<StartManagerScript>().LoadSceneName;
    if (PhotonNetwork.room.CustomProperties["IsReady"].ToString() == "1")
    {
      GameObject.Find(name: "StartButton").SetActive(value: false);
    }
  }

  // Update is called once per frame
  void Update()
  {
  }

  #region OnClick Callbacks

  public void OnClick_StartButton()
  {
    Debug.Log(message: "=== OnClick_StartButton ===\n");
    ////////////////////////////////////////
    ////////////////////////////////////////
    var player = PhotonNetwork.player;
    var cp = player.CustomProperties;
    cp["IsReady"] = "1";
    player.SetCustomProperties(cp);
    ////////////////////////////////////////
    ////////////////////////////////////////
    if (CheckAllPlayerIsReady())
    {
      m_photonView.RPC(methodName: "LoadScene", target: PhotonTargets.All);
    }
    else
    {
      Debug.Log("=== ほかの人がスタートを押すのを待っています ===\n");
      m_photonView.RPC(methodName: "PunUpdatePlayerNameList", target: PhotonTargets.All);
    }

  }

  #endregion

  public void UpdatePlayerOrder()
  {
    m_photonView.RPC(methodName: "PunUpdatePlayerOrder", target: PhotonTargets.All);
  }

  [PunRPC]
  private void PunUpdatePlayerOrder()
  {
    UnityEngine.GameObject.Find(name: "InfoCanvas").GetComponent<StartManagerScript>().UpdatePlayerOrderList();
  }

  [PunRPC]
  public void PunUpdatePlayerNameList()
  {
    ////////////////////////////////////////
    ////////////////////////////////////////
    // Update Player Info Text
    //StartManagerGameObject.GetComponent<StartManagerScript>().UpdatePlayerList();
    UnityEngine.GameObject.Find(name: "InfoCanvas").GetComponent<StartManagerScript>().UpdatePlayerList();
  }

  [PunRPC]
  private void LoadScene()
  {
    ////////////////////////////////////////
    // - 現在のSceneの再読み込みをスマートなコードで書く - ぱふの自由帳
    //   - http://pafu-of-duck.hatenablog.com/entry/2017/10/08/215131
    // 現在のScene名を取得する
    var loadScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
    // Sceneの読み直し
    UnityEngine.SceneManagement.SceneManager.LoadScene(loadScene.name);
  }

  private bool CheckAllPlayerIsReady()
  {
    foreach (var photonPlayer in PhotonNetwork.playerList)
    {
      if (photonPlayer.CustomProperties["IsReady"].ToString() == "0")
      {
        return false;
      }
    }

    ////////////////////////////////////////
    //ルームのカスタムプロパティを利用する – Photon
    // https://support.photonengine.jp/hc/ja/articles/215913128-%E3%83%AB%E3%83%BC%E3%83%A0%E3%81%AE%E3%82%AB%E3%82%B9%E3%82%BF%E3%83%A0%E3%83%97%E3%83%AD%E3%83%91%E3%83%86%E3%82%A3%E3%82%92%E5%88%A9%E7%94%A8%E3%81%99%E3%82%8B
    var room = PhotonNetwork.room;
    var cp = room.CustomProperties;
    cp["IsReady"] = "1";
    room.SetCustomProperties(cp);
    Debug.Log("=== CheckPlayerIsReady() => true");
    return true;
  }
}