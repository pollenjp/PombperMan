using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwoPlayer
{
  public class CameraScript : MonoBehaviour
  {
    private const float YAngle_MIN = -89.0f; //カメラのY方向の最小角度
    private const float YAngle_MAX = 89.0f; //カメラのY方向の最大角度

    public Transform ObjTarget; //追跡するオブジェクトのtransform
    public Vector3 ObjOffset = new Vector3(0, 1, 0); //追跡対象の中心位置調整用オフセット
    private Vector3 _lookAt; //targetとoffsetによる注視する座標

    [SerializeField] public float CameraPosY = 2.0f;

    [SerializeField] private float _charaCameraDistance = 3.0f; //キャラクターとカメラ間の距離
    private float _distance_min = 1.0f; //キャラクターとの最小距離
    private float _distance_max = 20.0f; //キャラクターとの最大距離
    private float _currentX = 0.0f; //カメラをX方向に回転させる角度
    private float _currentY = 0.0f; //カメラをY方向に回転させる角度

    //カメラ回転用係数(値が大きいほど回転速度が上がる)
    [SerializeField] private float _moveX = 4.0f; //マウスドラッグによるカメラX方向回転係数
    [SerializeField] private float _moveY = 2.0f; //マウスドラッグによるカメラY方向回転係数
    [SerializeField] private float _moveX_QE = 2.0f; //QEキーによるカメラX方向回転係数

    //####################################################################################################

    #region MonoBehaviour Callbacks

    private void Start()
    {
    }

    //########################################
    private void Update()
    {
      //########################################
      // QとEキーでカメラ回転
      if (Input.GetKey(KeyCode.Q))
      {
        _currentX += -_moveX_QE;
      }

      if (Input.GetKey(KeyCode.E))
      {
        _currentX += _moveX_QE;
      }

      //########################################
      // マウス右クリックを押しているときだけマウスの移動量に応じてカメラが回転
      if (Input.GetMouseButton(1))
      {
        _currentX += Input.GetAxis("Mouse X") * _moveX;
        _currentY += Input.GetAxis("Mouse Y") * _moveY;
        _currentY = Mathf.Clamp(_currentY, YAngle_MIN, YAngle_MAX);
      }

      _charaCameraDistance = Mathf.Clamp(value: _charaCameraDistance - Input.GetAxis("Mouse ScrollWheel"),
        min: _distance_min, max: _distance_max);
    }

    //########################################
    private void LateUpdate()
    {
      if (ObjTarget != null) //targetが指定されるまでのエラー回避
      {
        // 注視座標はtarget位置+offsetの座標
        // (x, 0, z) + (0, y, 0)
        _lookAt = ObjTarget.position + ObjOffset;

        //カメラ旋回処理
        Vector3 dir = new Vector3(0, CameraPosY - _lookAt.y, -_charaCameraDistance);
        Quaternion rotation = Quaternion.Euler(-_currentY, _currentX, 0);

        //transform.position = _lookAt + rotation * dir; //カメラの位置を変更
        transform.position = _lookAt - ObjTarget.forward.normalized * _charaCameraDistance +
                             new Vector3(0, CameraPosY, 0); //カメラの位置を変更
        transform.LookAt(_lookAt); //カメラをLookAtの方向に向けさせる
      }
    }

    #endregion
  }
}