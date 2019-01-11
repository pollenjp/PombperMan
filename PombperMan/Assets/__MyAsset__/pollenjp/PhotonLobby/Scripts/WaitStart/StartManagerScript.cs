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
    public GameObject PlayInfoPanelGameObject;
    public Text UserNameListText;

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

      if (PhotonNetwork.room.CustomProperties["IsReady"].ToString() == "0")
      {
        // sceneLoaderを生成
        GameObject sceneLoader = PhotonNetwork.Instantiate(
          prefabName: this.SceneLoaderObject.name,
          position: new Vector3(0f, 0f, 0f),
          rotation: Quaternion.identity,
          group: 0);
      }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    #endregion

    public void UpdatePlayerList()
    {
      UserNameListText.text = "";
      foreach (var photonPlayer in PhotonNetwork.playerList)
      {
        UserNameListText.text = photonPlayer.NickName + "\n";
      }
    }

    #region Photon Callbacks

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
    }

    #endregion
  }
}