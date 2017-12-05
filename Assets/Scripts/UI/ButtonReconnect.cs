using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReconnect : MonoBehaviour
{
  public void Reconnect()
  {
    PhotonNetwork.Reconnect();
  }

}
