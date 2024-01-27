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

    [SerializeField,Tooltip("Reference to the player input manager, that is in control of player input")]
    private PlayerInputManager playerInputManager;

    [SerializeField, Tooltip("Reference to the Canvas gameObject containing the Main menu UI")]
    private GameObject mainMenuCanvas;

    [SerializeField, Tooltip("Reference to the main-menu camera")]
    private Camera mainMenuCamera;

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


    /// <summary>
    /// Called on the first active frame, starts the timeline for character selection
    /// </summary>
    private void Start()
    {
        //Set singleton instance
        instance = this;

        //Start timeline sequence for camera-panning movement over the scene used in the character selection screen
        timelineDirector.Play(selectionScreenSequence, DirectorWrapMode.Loop);

        //Enable the main menu canvas
        mainMenuCanvas.SetActive(true);
        mainMenuCamera.gameObject.SetActive(true);
    }

    /// <summary>
    /// Starts the game intro-sequence
    /// </summary>
    [ContextMenu("Start Game")]
    private void StartGameIntro()
    {
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
        //Disable the main camera
        //mainMenuCamera.gameObject.SetActive(false);

        //Start the game-start sequence with count-down to actual game-start
        timelineDirector.Play(gameStartSequence, DirectorWrapMode.Hold);
    }

    /// <summary>
    /// Callack invoked whenver the gameplay countdown has been finished
    /// </summary>
    public void OnGameplayCountdownFinished()
    {
      //Enable input for all players @maarten
    }

    /// <summary>
    /// Callback to invoke the end of gameplay
    /// </summary>
    internal void EndGameplay(Player winningPlayer)
    {
        //Loop over all players, disable all losing players and enable the winning player

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

}
