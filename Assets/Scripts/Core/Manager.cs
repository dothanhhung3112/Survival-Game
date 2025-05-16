using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hung
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance;
        public SceneSO sceneSO;
        [HideInInspector] public HashSet<int> levelLoseList = new HashSet<int>();
        [HideInInspector] public HashSet<int> levelWinList = new HashSet<int>();
        public bool isRevived = false;
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
            Application.targetFrameRate = 60;
            LoadWinLoseList();
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

        public int Money
        {
            get { return PlayerPrefs.GetInt("Money", 0); }
            set { PlayerPrefs.SetInt("Money", value); }
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
            set { PlayerPrefs.SetInt("IsMuteSound", value ? 1 : 0); }
        }

        public bool IsMuteMusic
        {
            get { return PlayerPrefs.GetInt("IsMuteMusic", 0) == 0 ? false : true; }
            set { PlayerPrefs.SetInt("IsMuteMusic", value ? 1 : 0); }
        }

        public bool IsOffVibration
        {
            get
            {
                return PlayerPrefs.GetInt("IsOffVibration", 0) == 0 ? false : true;
            }
            set
            {
                PlayerPrefs.SetInt("IsOffVibration", value ? 1 : 0);
            }
        }

        public bool SetSound()
        {
            bool isMuteSound = !IsMuteSound;
            IsMuteSound = isMuteSound;
            SoundManager.Instance.SetMuteSounds();
            return isMuteSound;
        }

        public bool SetMusic()
        {
            bool isMuteMusic = !IsMuteMusic;
            IsMuteMusic = isMuteMusic;
            SoundManager.Instance.SetMuteMusic(true);
            return isMuteMusic;
        }

        public bool SetVibration()
        {
            bool isOffVibration = !IsOffVibration;
            IsOffVibration = isOffVibration;
            return isOffVibration;
        }
        #endregion

        public void LoadNextLevel(bool isWin)
        {
            if (isWin)
            {
                if (!levelWinList.Contains(CurrentLevel))
                {
                    LevelWins = levelWinList.Count > 0 ? string.Join(";", levelWinList) : "";
                    levelWinList.Add(CurrentLevel);
                }
            }
            else
            {
                if (!levelLoseList.Contains(CurrentLevel))
                {
                    levelLoseList.Add(CurrentLevel);
                    LevelLoses = levelLoseList.Count > 0 ? string.Join(";", levelLoseList) : "";
                }
            }

            CurrentLevel++;
            Level++;
            if(CurrentLevel > SceneManager.sceneCountInBuildSettings - 1)
            {
                ChangeSeason();
            }
            SceneLoader.Instance.LoadSceneByIndex(CurrentLevel);
            Manager.Instance.isRevived = false;
        }

        public void ChangeSeason()
        {
            LevelLoses = "";
            LevelWins = "";
            levelWinList.Clear();                                                                                                                           
            levelLoseList.Clear();
            CurrentLevel = 1;
            Season++;
        }

        void LoadWinLoseList()
        {
            if (!string.IsNullOrEmpty(LevelWins))
            {
                levelWinList = LevelWins.Split(';').Select(int.Parse).ToHashSet();
            }

            if (!string.IsNullOrEmpty(LevelLoses))
            {
                levelLoseList = LevelLoses.Split(';').Select(int.Parse).ToHashSet();
            }
        }
    }
}
