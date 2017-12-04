using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpponentNameText : MonoBehaviour
{
  TurnController turnController;
  Text text;

  protected void Awake()
  {
    text = GetComponent<Text>();
    turnController = GameObject.FindObjectOfType<TurnController>();
  }

  protected void OnEnable()
  {
    PhotonPlayer player = null;
    for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
    {
      if(PhotonNetwork.playerList[i].IsLocal)
      {
        continue;
      }
      player = PhotonNetwork.playerList[i];
      break;
    }

    string opponentName = player.NickName;
    if (string.IsNullOrWhiteSpace(opponentName))
    {
      opponentName = "Your Opponent";
    }
    text.text = $"Waiting on {opponentName}";
  }
}
