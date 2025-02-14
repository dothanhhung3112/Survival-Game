using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hung
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
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
            SceneManager.LoadScene(1);
        }

        #region PlayerData
        public int Level
        {
            get { return PlayerPrefs.GetInt("Level", 1); }
            set { PlayerPrefs.SetInt("Level", value); }
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

        public void LoadNextLevel()
        {
            Level++;
            SceneManager.LoadScene(Level);
        }
    }
}
