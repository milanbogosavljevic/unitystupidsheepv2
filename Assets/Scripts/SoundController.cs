using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource BackgroundMusic;
    [SerializeField] private AudioSource SoundsPlayer;

    [SerializeField] private AudioClip SheepCollideWithSaw;
    [SerializeField] private AudioClip Click;
    [SerializeField] private AudioClip PauseEnabled;
    [SerializeField] private AudioClip EnableEat;

    private bool _musicIsOn;
    private bool _soundIsOn;
    private SaveLoadSystem _saveLoadSystem;
    private GameData _data;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundController");

         if (objs.Length > 1)
         {
             Destroy(this.gameObject);
         }

        _saveLoadSystem = GameObject.FindWithTag("SaveLoadSystem").GetComponent<SaveLoadSystem>();
    }

    public void RestoreSoundsStates()
    {
        _data = _saveLoadSystem.GetGameData();
        _musicIsOn = _data.musicIsOn;
        _soundIsOn = _data.soundIsOn;
    }

    public void ToggleSound()
    {
        _soundIsOn = !_soundIsOn;
        _saveLoadSystem.SaveSoundState(_soundIsOn);
    }

    public void ToggleMusic()
    {
        _musicIsOn = !_musicIsOn;
        _saveLoadSystem.SaveMusicState(_musicIsOn);
    }

    public bool IsMusicOn()
    {
        return _musicIsOn;
    }

    public bool IsSoundOn()
    {
        return _soundIsOn;
    }

    public void PlayBackgroundMusic()
    {
        Debug.Log("music is on " + _musicIsOn);
        if (_musicIsOn)
        {
            BackgroundMusic.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        if (_musicIsOn)
        {
            BackgroundMusic.Stop();
        }
    }

    public void PlaySheepCollideWithSaw()
    {
        if (_soundIsOn)
        {
            SoundsPlayer.PlayOneShot(SheepCollideWithSaw, 1);// 1 = vol
        }
    }

    public void PlayClick()
    {
        if (_soundIsOn)
        {
            SoundsPlayer.PlayOneShot(Click, 1);// 1 = vol
        }
    }

    public void PlayPauseEnabled()
    {
        if (_soundIsOn)
        {
            SoundsPlayer.PlayOneShot(PauseEnabled, 1);// 1 = vol
        }
    }

    public void PlayEnableEat()
    {
        if (_soundIsOn)
        {
            SoundsPlayer.PlayOneShot(EnableEat, 1);// 1 = vol
        }
    }
}
