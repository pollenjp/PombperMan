using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaitStart;

public class WaitingRoomCanvasScript : MonoBehaviour
{

	private string LoadSceneName;
		
	private PhotonView m_photonView = null;
	
	private void Awake()
	{
		m_photonView = this.GetComponent<PhotonView>();
	}

	// Use this for initialization
	void Start ()
	{
		LoadSceneName = UnityEngine.GameObject.Find(name: "StartManager").GetComponent<StartManagerScript>().LoadSceneName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	#region OnClick Callbacks

	public void OnClick_StartButton()
	{
		Debug.Log(message: "=== OnClick_StartButton ===\n");
		m_photonView.RPC(methodName: "LoadScene", target: PhotonTargets.All);
	}

	#endregion
    
	[PunRPC]
	private void LoadScene()
	{
		// エフェクトを生成.
		// 適当な時間が経過したら消すように設定.
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName: LoadSceneName);
	}

	
}
