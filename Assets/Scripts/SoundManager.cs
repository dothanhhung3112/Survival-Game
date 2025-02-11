using UnityEngine;

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
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip enemySound;
    [SerializeField] AudioClip enemySearching;
    [SerializeField] AudioClip[] gunShooting;
    [SerializeField] AudioClip maleHited;
    [SerializeField] AudioClip femaleHited;

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

    public void PlaySound(AudioClip clip,float volume = 1f)
    {
        if (GameManager.Instance.IsMuteSound) return;

        if(clip != null)
        soundSource.PlayOneShot(clip,volume);
    }

    public void StopSound()
    {
        if (soundSource.isPlaying)
        {
            if(soundSource.loop == true)
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

    public void PlaySoundEnemy()
    {
        PlaySound(enemySound);
    }

    public void PlaySoundEnemySearching()
    {
        PlaySound(enemySearching);
    }

    public void PlaySoundGunShooting()
    {
        int a = Random.Range(0,gunShooting.Length);
        PlaySound(gunShooting[a],0.5f);
    }

    public void PlaySoundMaleHited()
    {
        PlaySound(maleHited,0.5f);
    }
    public void PlaySoundFemaleHited()
    {
        PlaySound(femaleHited, 0.5f);
    }
    #endregion
}
