using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
public class PhotonShowHideChild : MonoBehaviour
{
  GameObject[] childGameObjectList;

  protected void Awake()
  {
    childGameObjectList = new GameObject[transform.childCount];
    for (int i = 0; i < childGameObjectList.Length; i++)
    {
      childGameObjectList[i] = transform.GetChild(i).gameObject;
    }
  }

  public void OnPhotonSerializeView(
    PhotonStream stream,
    PhotonMessageInfo info)
  {
    if (stream.isWriting == true)
    {
      for (int i = 0; i < childGameObjectList.Length; i++)
      {
        stream.SendNext(childGameObjectList[i].activeSelf);
      }
    }
    else
    {
      for (int i = 0; i < childGameObjectList.Length; i++)
      {
        childGameObjectList[i].SetActive((bool)stream.ReceiveNext());
      }
    }
  }
}
