using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
  public string PhotonLobbySceneName;
  public GameObject PlayerPrefab;
  public bool CanDestroyPlayer;

  private PhotonPlayer _localPlayer;
  private int _i;
  private int _playerNumber = 0;
  private Vector3 _playerPosition;
  private Quaternion _playerRotation;

  // Use this for initialization
  private void Start()
  {
    if (!PhotonNetwork.connected) // Photonに接続されていなければ
    {
      if (!string.IsNullOrEmpty(value: PhotonLobbySceneName))
      {
        SceneManager.LoadScene(PhotonLobbySceneName);
      }

      return;
    }

    switch (PhotonNetwork.room.CustomProperties["IsReady"].ToString())
    {
      case "0":
        CanDestroyPlayer = false;
        break;
      case "1":
        CanDestroyPlayer = true;
        break;
      default:
        Debug.Log("=== Error ===");
        break;
    }

    _localPlayer = PhotonNetwork.player;

    //########################################
    // Debug
    // Check Player List Order
    _i = 0;
    var photonPlayerList = PhotonNetwork.playerList;
    System.Array.Sort(
      photonPlayerList,
      (instance1, instance2) => instance1.NickName.CompareTo(instance2.NickName));
    foreach (var photonPlayer in photonPlayerList)
    {
      if (photonPlayer.NickName == _localPlayer.NickName)
      {
        _playerNumber = _i;

        Debug.Log(
          message: "====================\n" +
                   "=== PhotonNetwork.playerList === : " +
                   photonPlayer.NickName + ", " + _playerNumber + "\n\n");
        break;
      }

      _i++;
    }

    //########################################
    // プレイヤーの初期位置の指定
    //  - Quaternion.AngleAxis - Unity スクリプトリファレンス
    //    - https://docs.unity3d.com/ja/2017.4/ScriptReference/Quaternion.AngleAxis.html
    //  - Vector3.up - Unity スクリプトリファレンス
    //    - https://docs.unity3d.com/ja/2017.4/ScriptReference/Vector3-up.html
    switch (_playerNumber)
    {
      case 0:
        _playerPosition = new Vector3(-5f, 0f, -5f);
        _playerRotation = Quaternion.AngleAxis(45, Vector3.up);
        break;
      case 1:
        _playerPosition = new Vector3(5f, 0f, 5f);
        _playerRotation = Quaternion.AngleAxis(-135, Vector3.up);
        break;
      case 2:
        _playerPosition = new Vector3(5f, 0f, -5f);
        _playerRotation = Quaternion.AngleAxis(-45, Vector3.up);
        break;
      case 3:
        _playerPosition = new Vector3(-5f, 0f, 5f);
        _playerRotation = Quaternion.AngleAxis(135, Vector3.up);
        break;
      default:
        _playerPosition = new Vector3(1f, 0f, 1f);
        _playerRotation = Quaternion.AngleAxis(0, Vector3.up);
        break;
    }

    GameObject player = PhotonNetwork.Instantiate(
      prefabName: this.PlayerPrefab.name,
      position: _playerPosition,
      //rotation: Quaternion.identity,
      rotation: _playerRotation,
      group: 0);
    player.name = PhotonNetwork.player.NickName;
    Debug.Log(message: "=== player.name ===\n" + player.name + "\n");
  }

  // Update is called once per frame
  private void Update()
  {
  }
}