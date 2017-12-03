using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPlayersText : MonoBehaviour
{
  Text text;

  protected void Awake()
  {
    text = GetComponent<Text>();
    text.text = "???";
  }

  protected void OnLobbyStatisticsUpdate()
  {
    List<TypedLobbyInfo> lobbyList = PhotonNetwork.LobbyStatistics;
    int totalPlayers = 1; // Add me
    for (int i = 0; i < lobbyList.Count; i++)
    {
      TypedLobbyInfo lobbyInfo = lobbyList[i];
      totalPlayers += lobbyInfo.PlayerCount;
    }

    text.text = totalPlayers.ToString("N0");
  }
}
