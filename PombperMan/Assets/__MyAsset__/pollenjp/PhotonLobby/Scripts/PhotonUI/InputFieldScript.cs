using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonUI
{
  public class InputFieldScript : MonoBehaviour
  {
    #region Variable Definition

    public InputField InputField;
    public string InputKeyword;

    #endregion

    //####################################################################################################

    #region MonoBehaviour callbacks

    // Use this for initialization
    void Start()
    {
      //前回プレイ開始時に入力した名前をロードして表示
      if (InputField != null)
      {
        if (PlayerPrefs.HasKey(InputKeyword))
        {
          InputField.text = PlayerPrefs.GetString(InputKeyword);
        }
      }
    }

    // Update is called once per frame
    void Update()
    {
    }

    #endregion

    //####################################################################################################

    #region Public Method
    public void SetPlayerName(string value)
    {
      Debug.Log(message: "=== SetPlayerName ===\n" + value + "\n");
      PhotonNetwork.playerName = value + " "; //今回ゲームで利用するプレイヤーの名前を設定
      PlayerPrefs.SetString(InputKeyword, value); //今回の名前をセーブ
      Debug.Log(PhotonNetwork.player.NickName); //playerの名前の確認。（動作が確認できればこの行は消してもいい）
    }
    #endregion
  }
}