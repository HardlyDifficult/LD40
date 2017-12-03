using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PhotonGravityView : MonoBehaviour
{
  Rigidbody m_Body;

  void Awake()
  {
    this.m_Body = GetComponent<Rigidbody>();
  }

  public void OnPhotonSerializeView(
    PhotonStream stream,
    PhotonMessageInfo info)
  {
    if (stream.isWriting == true)
    {
      stream.SendNext(this.m_Body.useGravity);
    }
    else
    {
      this.m_Body.useGravity = (bool)stream.ReceiveNext();
    }
  }
}
