using CodeKriebels.Analytics;
using CodeKriebels.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    //Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public PlayerSettings playerSettings;

    [Header("References")]

    [SerializeField, Tooltip("Reference to the player input manager, that is in control of player input")]
    private PlayerInputManager playerInputManager;

    [SerializeField, Tooltip("Reference to the Canvas gameObject containing the Main menu UI")]
    private GameObject mainMenuCanvas;

    [SerializeField, Tooltip("Reference to the main-menu camera")]
    private Camera mainMenuCamera;

    [SerializeField, Tooltip("Reference to component which shows player visuals for the winning player")]
    private PlayerToSprite winningPlayerVisuals;

    [Header("Game Sequencing")]

    [SerializeField, Tooltip("Timeline director in control of intro")]
    private PlayableDirector timelineDirector;

    [SerializeField, Tooltip("Timeline sequence in control of the camera panning during character selection")]
    private TimelineAsset selectionScreenSequence;

    [SerializeField, Tooltip("Timeline sequence in control of showing the different introduction comics")]
    private TimelineAsset introductionSequence;

    [SerializeField, Tooltip("Timeline sequence in control of the sequencing of the game countdown intro")]
    private TimelineAsset gameStartSequence;

    [SerializeField, Tooltip("Timeline sequence in control of the sequencing of the game outro after a player wins")]
    private TimelineAsset gameOutroSequence;

    // SURPRISE ANALYTICS!
    private LocationCollection locationCollection = new LocationCollection();


    /// <summary>
    /// Determines the current state of the game-flow
    /// </summary>
    internal GameState currentGameState = GameState.CharacterSelection;

    /// <summary>
    /// Called on the first active frame, starts the timeline for character selection
    /// </summary>
    private void Start()
    {
        //Set singleton instance
        instance = this;

        //Set state to intro
        currentGameState = GameState.CharacterSelection;

        //Start timeline sequence for camera-panning movement over the scene used in the character selection screen
        timelineDirector.Play(selectionScreenSequence, DirectorWrapMode.Loop);

        //Enable the main menu canvas
        mainMenuCanvas.SetActive(true);
        mainMenuCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        locationCollection.Update();
    }

    private void OnGUI()
    {
        locationCollection.OnGUI();
    }

    /// <summary>
    /// Starts the game intro-sequence
    /// </summary>
    [ContextMenu("Start Game")]
    internal void StartGameIntro()
    {
        //Set state to intro
        currentGameState = GameState.Intro;

        //Disable menu UI
        mainMenuCanvas.SetActive(false);

        //Start the introduction sequence
        timelineDirector.Play(introductionSequence, DirectorWrapMode.Hold);
    }

    /// <summary>
    /// Starts gameplay logic
    /// </summary>
    public void StartGameplay()
    {
        //Set state to gameplay
        currentGameState = GameState.Gameplay;

        //Disable the main camera
        mainMenuCamera.gameObject.SetActive(false);

        //Start the game-start sequence with count-down to actual game-start
        timelineDirector.Play(gameStartSequence, DirectorWrapMode.Hold);

        //Loop over all players and enable the cameras of each player
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            //Enable camera for each player
            PlayerManager.Instance.players[i].Input.camera.enabled = true;
        }
    }

    /// <summary>
    /// Callack invoked whenver the gameplay countdown has been finished
    /// </summary>
    public void OnGameplayCountdownFinished()
    {
        //Loop over all players and enable the cameras of each player
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            //Enable camera for each player
            PlayerManager.Instance.players[i].Input.enabled = true;
            PlayerManager.Instance.players[i].Movement.enabled = true;
        }
    }

    /// <summary>
    /// Callback to invoke the end of gameplay
    /// </summary>
    internal void EndGameplay(Player winningPlayer)
    {
        //Set state to intro
        currentGameState = GameState.Outro;

        //Re-enable the camera
        mainMenuCamera.gameObject.SetActive(true);

        //Loop over all players, disable all losing players and enable the winning player
        for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
        {
            PlayerManager.Instance.players[i].PlayerMovement.enabled = false;
            PlayerManager.Instance.players[i].Input.camera.gameObject.SetActive(false);

            // Submit the winner in the analytics as well.
            if (Equals(winningPlayer.Input, PlayerManager.Instance.players[i].Input))
                locationCollection.SubmitWinner(i);
        }

        //Transfer the index of sprite package used from the winning player to the winning player visuals
        winningPlayerVisuals.ChangeUsingSpritePackage(winningPlayer.PlayerToSprite.usingSpritePackageIndex);

        //Start timeline sequence for the game outro
        timelineDirector.Play(gameOutroSequence, DirectorWrapMode.Hold);
    }

    /// <summary>
    /// Restarts the game for another round
    /// </summary>
    public void RestartGame()
    {
        //Reload this scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Enum for determening the current gamestate
    /// </summary>
    public enum GameState
    {
        CharacterSelection = 0,
        Intro = 1,
        Gameplay = 2,
        Outro = 3,
    }

}
