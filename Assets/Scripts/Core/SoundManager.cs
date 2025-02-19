using UnityEngine;

namespace Hung
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [Header("AudioSource")]
        [SerializeField] AudioSource soundSource;
        [SerializeField] AudioSource musicSource;

        [Header("AudioClip")]
        [SerializeField] AudioClip musicClip;
        [SerializeField] AudioClip winClip;
        [SerializeField] AudioClip loseClip;
        [SerializeField] AudioClip timeCountClip;

        [Header("GreenRedLight")]
        [SerializeField] AudioSource enemySound;
        [SerializeField] AudioClip walkSound;
        [SerializeField] AudioClip enemySearching;
        [SerializeField] AudioClip gunShooting;
        [SerializeField] AudioClip maleHited;
        [SerializeField] AudioClip femaleHited;

        [Header("Dalgona")]
        [SerializeField] AudioClip needleMove;
        [SerializeField] AudioClip candyBreak;

        [Header("TugOfWar")]
        [SerializeField] AudioClip tugSound;
        [SerializeField] AudioClip tugClick;

        [Header("GlassStepping")]
        [SerializeField] AudioClip glassBreak;
        [SerializeField] AudioClip jumpSound;

        [Header("GameFight")]
        [SerializeField] AudioClip slash;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        public void SetMuteSounds()
        {

        }

        public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (Manager.Instance.IsMuteSound) return;

            if (clip != null)
                soundSource.pitch = pitch;
            soundSource.PlayOneShot(clip, volume);
        }

        public void StopSound()
        {
            if (soundSource.isPlaying)
            {
                if (soundSource.loop == true)
                {
                    soundSource.loop = false;
                }
                soundSource.Stop();
            }
        }
        #region UI
        public void PlaySoundWin()
        {
            PlaySound(winClip);
        }

        public void PlaySoundLose()
        {
            PlaySound(loseClip);
        }

        public void PlaySoundTimeCount()
        {
            PlaySound(timeCountClip);
        }


        #endregion

        #region GreenRedLight
        public void PlaySoundWalk()
        {
            PlaySound(walkSound);
        }

        public void PlaySoundEnemy(float pitch)
        {
            enemySound.pitch = pitch;
            enemySound.Play();
        }

        public void PlaySoundEnemySearching()
        {
            PlaySound(enemySearching);
        }

        public void PlaySoundGunShooting()
        {
            PlaySound(gunShooting, 0.5f);
        }

        public void PlaySoundMaleHited()
        {
            PlaySound(maleHited, 0.5f);
        }
        public void PlaySoundFemaleHited()
        {
            PlaySound(femaleHited, 0.5f);
        }
        #endregion

        #region Dalgona
        public void PlaySoundCandyBreak()
        {
            PlaySound(candyBreak);
        }

        public void PlaySoundNeedleMove()
        {
            if (soundSource.isPlaying) return;
            PlaySound(needleMove);
        }
        #endregion

        #region TugOfWar
        public void PlaySoundTug()
        {
            PlaySound(tugSound);
        }

        public void PLaySoundTugClick()
        {
            PlaySound(tugClick);
        }
        #endregion

        #region GlassStepping
        public void PLaySoundGlassBreak()
        {
            PlaySound(glassBreak);
        }

        public void PLaySoundJump()
        {
            PlaySound(jumpSound);
        }
        #endregion

        #region GameFight
        public void PlaySoundSlash()
        {
            PlaySound(slash);
        }
        #endregion
    }
}
