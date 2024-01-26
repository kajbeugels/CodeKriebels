using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private readonly List<Player> players = new();

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }


    public void AddPlayer ()
    {
        if (players.Count < gameManager.playerSettings.maxPlayers)
        {
            return;
        }

        var playerInstance = Instantiate(playerPrefab);

        var player = playerInstance.GetComponent<Player>();

        players.Add(player);
    }
}
