using System;
using System.Collections;
using System.Collections.Generic;
using TwoPlayer;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
  //################################################################################
  // Photon
  public PhotonView MyPhotonView;
  public PhotonTransformView MyPhotonTransformView;

  private Camera _mainCamera;

  //########################################
  // Character
  // 移動
  public Animator CharAnimator; // モーション

  public CharacterController CharController; // キャラの移動管理

  // 移動パラメータ
  public float CharSpeed = 3f;
  public float CharJumpSeed = 5f;
  public float CharRotateSpeed = 10f;
  public float CharGravity = 20f;

  Vector3 _charTargetDirection;
  Vector3 _charMoveVector = Vector3.zero;

  //####################################################################################################
  // Use this for initialization
  private void Start()
  {
    // 自分のキャラクターならば
    if (MyPhotonView.isMine)
    {
      _mainCamera = Camera.main;
      _mainCamera.GetComponent<CameraScript>().ObjTarget = this.gameObject.transform;
    }
  }

  // Update is called once per frame
  private void Update()
  {
    if (!MyPhotonView.isMine) return;

    MoveControl();
    RotationControl();
    CharController.Move(motion: _charMoveVector * Time.deltaTime);

    // スムーズな同期のためにPhotonTransformViewに速度値を渡す
    Vector3 velocity = CharController.velocity;
    MyPhotonTransformView.SetSynchronizedValues(speed: velocity, turnSpeed: 0);
  }

  //####################################################################################################
  private void MoveControl()
  {
    //########################################
    // 進行方向を計算
    // Get Key Input
    float v = Input.GetAxisRaw(axisName: "Vertical");
    float h = Input.GetAxisRaw(axisName: "Horizontal");

    // カメラの向いている方向(y軸は無視)を正規化して取得
    Vector3 forward = Vector3.Scale(a: Camera.main.transform.forward, b: new Vector3(1, 0, 1));
    Vector3 right = Camera.main.transform.right; // カメラから見て右方向のベクトル取得
    _charTargetDirection = (h * right + v * forward).normalized;

    //####################
    // 地面にいるとき
    if (CharController.isGrounded)
    {
      _charMoveVector = _charTargetDirection * CharSpeed;
      if (Input.GetButton("Jump"))
      {
        _charMoveVector.y = CharJumpSeed;
      }
    }
    //##########
    // 地面にいないとき
    else
    {
      // 重力に従って落ちる処理
      _charMoveVector.y -= CharGravity * Time.deltaTime;
    }

    //########################################
    // アニメーション管理
    if (v > .1 || v < -.1 || h > .1 || h < -.1)
    {
      CharAnimator.SetFloat(name: "Speed", value: 1f);
    }
    else
    {
      CharAnimator.SetFloat(name: "Speed", value: 0f);
    }
  }

  //####################################################################################################
  private void RotationControl()
  {
    Vector3 rotationDirection = _charMoveVector;
    rotationDirection.y = 0;

    // それなりに移動方向が変化する場合のみ移動方向を変える
    if (rotationDirection.sqrMagnitude > 0.01)
    {
      float step = CharRotateSpeed * Time.deltaTime;
      Vector3 newDir = Vector3.Slerp(a: transform.forward, b: rotationDirection, t: step);
      transform.rotation = Quaternion.LookRotation(forward: newDir);
    }
  }
}