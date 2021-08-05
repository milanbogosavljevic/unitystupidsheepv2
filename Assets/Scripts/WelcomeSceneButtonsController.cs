using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeSceneButtonsController : MonoBehaviour
{
    [SerializeField] Button PlayButton;
    [SerializeField] Button MusicButton;
    [SerializeField] Button SoundButton;
    private SaveLoadSystem _saveLoadSystem;
    private GameData _data;

    void Start()
    {
        _saveLoadSystem = GameObject.FindWithTag("SaveLoadSystem").GetComponent<SaveLoadSystem>();
        _data = _saveLoadSystem.GetGameData();

        SetMusicButtonAlpha();
    }

    public void PlayGame()
    {
        PlayButton.interactable = false;
        ScenesController.StartGame();
    }

    public void MusicButtonHandler()
    {
        _saveLoadSystem.SaveMusicState(!_data.musicIsOn);
        SetMusicButtonAlpha();
    }

    private void SetMusicButtonAlpha()
    {
        float musicAlpha = _data.musicIsOn ? 1 : 0.3f;
        Color musicColor = MusicButton.image.color;
        musicColor.a = musicAlpha;
        MusicButton.image.color = musicColor;
    }

    public void SoundButtonHandler()
    {
        _saveLoadSystem.SaveSoundState(!_data.soundIsOn);
        SetSoundButtonAlpha();
    }

    private void SetSoundButtonAlpha()
    {
        float soundAlpha = _data.soundIsOn ? 1 : 0.3f;
        Color soundColor = SoundButton.image.color;
        soundColor.a = soundAlpha;
        SoundButton.image.color = soundColor;
    }

    public void QuitButtonHandler()
    {
        ScenesController.QuitGame();
    }
}
