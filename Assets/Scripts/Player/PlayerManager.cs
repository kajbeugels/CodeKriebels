using CodeKriebels.Player;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField, Tooltip("Prefab for player visuals")]
    public PlayerToSprite playerVisualsPrefab;

    [SerializeField, Tooltip("Reference to the input actions of the player")]
    internal InputActionAsset inputActions;

    [SerializeField,Tooltip("COLORS")]
    internal Color[] PlayerColors;

    /// <summary>
    /// List of all players in the game
    /// </summary>
    internal List<Player> players = new List<Player>();

    /// <summary>
    /// Spawn locations array, indexed by player
    /// </summary>
    [SerializeField, Tooltip("Spawn locations array, indexed by player")]
    internal Transform[] spawnLocations;

    /// <summary>
    /// Reference to text component that shows users they can start the game
    /// </summary>
    [SerializeField, Tooltip("Reference to text component that shows users they can start the game")]
    internal GameObject pressStartToPlayText;


    private void Start()
    {
        //Set singleton instance of the PlayerManager
        instance = this;

        //Register callbacks for player joining/leaving
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;

        //Register callback for when te game should start
        inputActions.FindAction("StartGame").performed += StartGamePressed;
    }

    public int GetPlayerIndex(Player p )
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == p)
                return i;
        }

        return 0;
    }
    /// <summary>
    /// Callback that gets invoked whenever a player has pressed the start game button
    /// </summary>
    /// <param name="obj"></param>
    private void StartGamePressed(InputAction.CallbackContext obj)
    {
        //Can't start with 1 splayer
        if (players.Count < 2)
            return;

        //Start the gameplay!
        GameManager.Instance.StartGameIntro();

        //Disable start text
        pressStartToPlayText.gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    [Button]
    private void EDITOR_JoinDummyPlayer()
    {
        PlayerInputManager.instance.JoinPlayer();
    }
#endif

    private void OnPlayerJoined(PlayerInput obj)
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.CharacterSelection)
        {
            Destroy(obj);
            return;

        }


        Player p = obj.GetComponent<Player>();

        //Set player to appropiate spawn position
        p.transform.position = spawnLocations[players.Count].position;
        p.transform.rotation = spawnLocations[players.Count].rotation;

        //Add player object to player list
        players.Add(p);

        PlayerToSprite pts = Instantiate(playerVisualsPrefab);
        pts.forwardReference = p.transform;
        pts.ChangeUsingSpritePackage(p.Input.playerIndex);
        pts.forwardReference = p.PlayerMovement.FartIndicatorPivot; 
        
        p.Fart = pts.GetComponentInChildren<PlayerFart>();
        p.Fart.parent = p;
        p.Movement.playerFarts = p.Fart;

        p.Ass = pts.GetComponentInChildren<PlayerAss>();

        //Inform user when they can start the game
        pressStartToPlayText.gameObject.SetActive(players.Count >= 2);

        GameManager.Instance.CameraSetings.ToggleCameraSettings(false);
    }

    private void OnPlayerLeft(PlayerInput obj)
    {
        if (GameManager.Instance.currentGameState != GameManager.GameState.CharacterSelection)
            return;

        Player p = obj.GetComponent<Player>();

        //Add player object to player list
        players.Remove(p);

        //Inform user when they can start the game
        pressStartToPlayText.gameObject.SetActive(players.Count >= 2);
    }
}
