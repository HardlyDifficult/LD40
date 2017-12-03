using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PhotonShowHideChild : MonoBehaviour
{
  [SerializeField]
  GameObject childGameObject;
  
  public void OnPhotonSerializeView(
    PhotonStream stream,
    PhotonMessageInfo info)
  {
    if (stream.isWriting == true)
    {
      stream.SendNext(childGameObject.activeSelf);
    }
    else
    {
      childGameObject.SetActive((bool)stream.ReceiveNext());
    }
  }
}
