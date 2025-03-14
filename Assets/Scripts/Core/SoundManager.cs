using DG.Tweening;
using Hung.Gameplay.GlassStepping;
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
        [SerializeField] AudioClip[] musicClip;
        [SerializeField] AudioClip winClip;
        [SerializeField] AudioClip loseClip;
        [SerializeField] AudioClip timeCountClip;
        [SerializeField] AudioClip moneyDrop;

        [Header("GreenRedLight")]
        [SerializeField] AudioSource enemySound;
        [SerializeField] AudioClip walkSound;
        [SerializeField] AudioClip enemySearching;
        [SerializeField] AudioClip gunShooting;
        [SerializeField] AudioClip maleHited;
        [SerializeField] AudioClip femaleHited;

        [Header("Dalgona")]
        [SerializeField] AudioClip openBox;
        [SerializeField] AudioClip needleMove;
        [SerializeField] AudioClip candyBreak;

        [Header("TugOfWar")]
        [SerializeField] AudioClip tugSound;
        [SerializeField] AudioClip tugClick;

        [Header("GlassStepping")]
        [SerializeField] AudioClip glassBreak;
        [SerializeField] AudioClip jumpSound;
        [SerializeField] AudioClip manHitGround;

        [Header("GameFight")]
        [SerializeField] AudioClip slash;

        [Header("SixLegged")]
        [SerializeField] AudioClip flipCard;
        [SerializeField] AudioClip throwStone;
        [SerializeField] AudioClip paperHit;

        [Header("PrisionEscape")]
        [SerializeField] AudioClip musicBG;
        [SerializeField] AudioClip alertSound;
        [SerializeField] AudioClip carSound;
        [SerializeField] AudioClip punchSound;
        [SerializeField] AudioClip laserSound;

        [Header("SquidGame")]
        [SerializeField] AudioClip meatSound;

        [Header("Marbles")]
        [SerializeField] AudioClip marbleSound;
        [SerializeField] AudioClip marbleHitGround;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        private bool isMuteMusic;
        public void SetMuteMusic(bool smooth)
        {
            isMuteMusic = Manager.Instance.IsMuteMusic;
            if (!smooth)
            {
                musicSource.mute = isMuteMusic;
                musicSource.volume = isMuteMusic ? 0f : 0.5f;
                return;
            }

            musicSource.DOFade(isMuteMusic ? 0f : 0.5f, 0.2f).OnComplete(() =>
            {
                musicSource.mute = isMuteMusic;
            }).SetUpdate(true);
        }

        public void SetMuteSounds()
        {
            if (Manager.Instance.IsMuteSound)
            {
                soundSource.mute = true;
                return;
            }
            soundSource.mute = false;
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

        public void PlaySoundMoneyDrop()
        {
            PlaySound(moneyDrop);
        }

        public void StopMusic()
        {
            musicSource.Stop();
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

        public void PlaySoundOpenBox()
        {
            PlaySound(openBox);
        }

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

        #region SixLegged

        public void PlaySoundFlipCard()
        {
            PlaySound(flipCard);
        }

        public void PlaySoundThrowStone()
        {
            PlaySound(throwStone);
        }

        public void PlaySoundPaperHit()
        {
            PlaySound(paperHit);
        }
        #endregion

        #region SquidGame
        public void PlaySoundEatMeat()
        {
            PlaySound(meatSound);
        }
        #endregion

        #region PrisionEscape
        public void PlayBGMusicPrisionEscape()
        {
            if (musicBG != null)
            {
                musicSource.clip = musicBG;
                musicSource.Play();
            }
        }

        public void PlaySoundAlert()
        {
            PlaySound(alertSound);
        }

        public void PlaySoundCar()
        {
            PlaySound(carSound);
        }

        public void PlaySoundPunch()
        {
            PlaySound(punchSound);
        }

        public void PlaySoundElectricDoor()
        {
            PlaySound(laserSound);
        }
        #endregion

        #region Marbles
        public void PlaySoundMarbleShoot()
        {
            PlaySound(marbleSound);
        }

        public void PlaySoundMarbleHitGround()
        {
            PlaySound(marbleHitGround);
        }
        #endregion

        public void PlayBGMusic4()
        {
            if (musicBG != null)
            {
                musicSource.clip = musicClip[0];
                musicSource.Play();
            }
        }

        public void PlayBGMusic5()
        {
            if (musicBG != null)
            {
                musicSource.clip = musicClip[1];
                musicSource.Play();
            }
        }

        public void PlayBGMusic6()
        {
            if (musicBG != null)
            {
                musicSource.clip = musicClip[2];
                musicSource.Play();
            }
        }

    }
}
