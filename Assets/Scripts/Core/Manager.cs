using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hung
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        public List<int> levelLoseList = new List<int>();
        public List<int> levelWinList = new List<int>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        private void Start()
        {
            LoadWinLoseList();
            SceneManager.LoadScene(CurrentLevel);
        }

        #region PlayerData
        public int Level
        {
            get { return PlayerPrefs.GetInt("Level", 1); }
            set { PlayerPrefs.SetInt("Level", value); }
        }

        public int CurrentLevel
        {
            get { return PlayerPrefs.GetInt("CurrentLevel", 1); }
            set { PlayerPrefs.SetInt("CurrentLevel", value); }
        }

        public int Season
        {
            get { return PlayerPrefs.GetInt("Season", 1); }
            set { PlayerPrefs.SetInt("Season", value); }
        }

        public int Theme
        {
            get { return PlayerPrefs.GetInt("Theme", 1); }
            set { PlayerPrefs.SetInt("Theme", 1); }
        }

        public string LevelLoses
        {
            get { return PlayerPrefs.GetString("LevelLoses",""); }
            set { PlayerPrefs.SetString("LevelLoses", value); }
        }

        public string LevelWins
        {
            get { return PlayerPrefs.GetString("LevelWins",""); }
            set { PlayerPrefs.SetString("LevelWins", value); }
        }

        #endregion

        #region PlayerSetting
        public bool IsMuteSound
        {
            get { return PlayerPrefs.GetInt("IsMuteSound", 0) == 0 ? false : true; }
            set { PlayerPrefs.GetInt("IsMuteSound", value ? 1 : 0); }
        }

        public bool IsMuteMusic
        {
            get { return PlayerPrefs.GetInt("IsMuteMusic", 0) == 0 ? false : true; }
            set { PlayerPrefs.GetInt("IsMuteMusic", value ? 1 : 0); }
        }
        #endregion

        public void LoadNextLevel(bool isWin)
        {
            if(isWin) levelWinList.Add(CurrentLevel);
            else levelLoseList.Add(CurrentLevel);

            CurrentLevel++;
            Level++;
            SceneManager.LoadScene(CurrentLevel);
        }

        public void ChangeSeason()
        {
            LevelLoses = null;
            LevelWins = null;
            levelWinList.Clear();
            levelLoseList.Clear();
            CurrentLevel = 1;
            Season++;
        }

        void LoadWinLoseList()
        {
            if (!string.IsNullOrEmpty(LevelWins))
            {
                levelWinList = LevelWins.Split(';').Select(int.Parse).ToList();
            }

            if (!string.IsNullOrEmpty(LevelLoses))
            {
                levelLoseList = LevelLoses.Split(';').Select(int.Parse).ToList();
            }
        }

        private void OnDestroy()
        {
            LevelLoses = string.Join(";", levelLoseList.ToArray());
            LevelWins = string.Join(";", levelWinList.ToArray());
        }
    }
}
