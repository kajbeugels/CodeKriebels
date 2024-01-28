namespace CodeKriebels.Analytics
{
    using NaughtyAttributes;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class LocationCollection
    {
        private string[] locations = new string[4]
        {
            "Breda",
            "Amsterdam",
            "Zwolle (Saxion)",
            "Leeuwarden"
        };

        private int currentWinnerIndex = -1;
        private bool showGUI;


        /// <summary>
        /// Submits who's the winner and saves it in the PlayerPrefs.
        /// </summary>
        /// <param name="winnerId">The winner ID</param>
        internal void SubmitWinner(int winnerId)
        {
            string playerPrefsKey = $"Player{winnerId}Wins";

            int amountOfTimes = PlayerPrefs.GetInt(playerPrefsKey, 0);

            amountOfTimes++;

            PlayerPrefs.SetInt(playerPrefsKey, amountOfTimes);
        }

        internal void Update()
        {
            if (Keyboard.current != null && Keyboard.current.f1Key.wasPressedThisFrame)
            {
                for (int i = 0; i < locations.Length; i++)
                {
                    int amountOfWins = PlayerPrefs.GetInt($"Player{i}Wins", 0);

                    if (amountOfWins > currentWinnerIndex)
                        currentWinnerIndex = i;
                }

                showGUI = !showGUI;
            }
        }

        /// <summary>
        /// Listens to the input until it has been pressed.
        /// </summary>
        internal void OnGUI()
        {
            // Show the GUI Text.
            if (showGUI)
                GUILayout.Label($"The winner is...\n{locations[currentWinnerIndex]}!", new GUIStyle() { fontSize = 36 });
        }
    }
}