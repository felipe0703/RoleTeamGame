using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class LlamaradaGame
    {
        //public const float ASTEROIDS_MIN_SPAWN_TIME = 5.0f;
        //public const float ASTEROIDS_MAX_SPAWN_TIME = 10.0f;

        //public const float PLAYER_RESPAWN_TIME = 4.0f;

        public const int PLAYER_MAX_ENERGIES = 3;

        public const string PLAYER_ENERGIES = "PlayerEnergies";
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        public const string PLAYER_TURN = "PlayerTurn";

        public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }

        public static Vector3 GetPosition(int positionChoice)
        {
            Vector3 positionPlayer = new Vector3();

            switch (positionChoice)
            {
                case 0: return positionPlayer = new Vector3(20, 30, 0);
                case 1: return positionPlayer = new Vector3(140, 30, 0);
                case 2: return positionPlayer = new Vector3(20, 130, 0);
                case 3: return positionPlayer = new Vector3(140, 130, 0);
                case 4: return positionPlayer = new Vector3(20, 50, 0);
                case 5: return positionPlayer = new Vector3(140, 110, 0);
                case 6: return positionPlayer = new Vector3(20, 70, 0);
                case 7: return positionPlayer = new Vector3(140, 90, 0);
                case 8: return positionPlayer = new Vector3(20, 30, 0);
            }

            return positionPlayer =  Vector3.zero;
        }
    }
}