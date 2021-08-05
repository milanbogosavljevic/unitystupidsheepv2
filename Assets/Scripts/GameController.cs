using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int lives = 3;
    private int startCounter = 3;
    private float score = 0f;
    private float highScore = 0f;
    private float pauseLoadingBarLevel;
    private float pauseLoadingBarInterval;
    private float pauseSheepLoadingBarLevel;
    private float pauseSheepLoadingBarInterval;
    private string scoreCheckPoint;
    private bool countScore = false;
    private bool maxSpeedReached = false;
    public bool canPauseSheep = false;

    private SaveLoadSystem _saveLoadSystem;
    private GameData _data;
    private CameraSizeController _cameraController;
    private SoundController _soundController;

    [SerializeField] private Text ScoreText;
    [SerializeField] private Text HighScoreText;
    [SerializeField] private Text StartCounterText;
    [SerializeField] private List<string> scoreCheckPoints;
    [SerializeField] private List<Sheep> allSheeps;
    [SerializeField] private List<GameObject> lifeDots;
    [SerializeField] private List<float> speedLevels;
    [SerializeField] private List<Saw> allSaws;
    [SerializeField] private List<Button> allButtons;
    [SerializeField] private Image pauseButtonLoadingBar;
    [SerializeField] private Image pauseSheepLoadingBar;
    [SerializeField] int pauseFreezeTime;
    [SerializeField] int pauseSheepFreezeTime;
    [SerializeField] int pauseCredits;
    [SerializeField] Button pauseButton;
    [SerializeField] private Text PauseCreditsText;
    [SerializeField] private SpeedFinger SpeedFinger;
    //[SerializeField] private SoundsController SoundsController;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Tutorial;

    private void Start()
    {
        SetScoreCheckPoint();
        _soundController = GameObject.FindWithTag("SoundController").GetComponent<SoundController>();
        _cameraController = Camera.main.GetComponent<CameraSizeController>();

        _saveLoadSystem = GameObject.FindWithTag("SaveLoadSystem").GetComponent<SaveLoadSystem>();
        _data = _saveLoadSystem.GetGameData();

        _soundController.RestoreSoundsStates();
        SetHighscore(_data.highScore);

        StartCounterText.text = startCounter.ToString();
        InvokeRepeating("CountDownStartTime", 1, 1F);
        //InvokeRepeating("CountDownStartTime", 0.1f, 0.1F);

        pauseSheepLoadingBarInterval = 0.01f;
        pauseLoadingBarInterval = 0.05f;

        pauseLoadingBarLevel = (1f / pauseFreezeTime) * pauseLoadingBarInterval;
        pauseSheepLoadingBarLevel = (1f / pauseSheepFreezeTime) * pauseSheepLoadingBarInterval;

        UpdatePauseButtonCreditsText();
    }

    private void SetHighscore(float hs)
    {
        highScore = hs;
        HighScoreText.text = hs.ToString("F0");
    }

    public void StartCountDown()
    {
        InvokeRepeating("CountDownStartTime", 1, 1F);
    }

    private void UpdatePauseButtonCreditsText()
    {
        PauseCreditsText.text = pauseCredits.ToString();
    }

    private void Update()
    {
        this.updateScores();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScenesController.ShowHomeLevel();
            _soundController.StopBackgroundMusic();
        }
    }

    private void SetScoreCheckPoint()
    {
        if(scoreCheckPoints.Count > 0)
        {
            scoreCheckPoint = scoreCheckPoints[0];
            scoreCheckPoints.RemoveAt(0);
            SpeedFinger.SetFingerSpeed(scoreCheckPoint);
        }
    }

    private void CountDownStartTime()
    {
        startCounter--;
        StartCounterText.text = startCounter.ToString();
        if(startCounter == -1)
        {
            Destroy(StartCounterText);
            CancelInvoke();
            this.MoveSheeps(true);
            SpeedFinger.RunAnimation(true);
            canPauseSheep = true;
            _soundController.PlayBackgroundMusic();
        }
    }

    private void updateScores()
    {
        if (countScore == true)
        {
            score += Time.deltaTime;
            ScoreText.text = score.ToString("F0");

            if (this.maxSpeedReached == false)
            {
                if (ScoreText.text == scoreCheckPoint)
                {
                    this.SetScoreCheckPoint();
                    this.ChangeSheepsSpeed();
                }
            }


            if (score > highScore)
            {
                HighScoreText.text = score.ToString("F0");
            }
        }
    }

    /*    private IEnumerator WaitToUnfreezeSheeps()
        {
            yield return new WaitForSeconds(5);
            this.MoveSheeps(true);
        }*/

    public void OnParticlesAnimationDone()
    {
        if (this.lives > 0)
        {
            //StartCoroutine(WaitToUnfreezeSheeps());
            this.ReactivateSheep();
            this.MoveSheeps(true);
        }
        else
        {
            this.OnGameOver();
        }
    }

    private void OnGameOver()
    {
        _soundController.StopBackgroundMusic();
        this.countScore = false;
        if(score > highScore)
        {
            _saveLoadSystem.SaveScore(score);
        }
        PlayerPrefs.SetFloat("Score", score);
        ScenesController.ShowScore();
    }

    private void DisableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }

    private void MoveSheeps(bool move)
    {
        foreach (Sheep sheep in allSheeps)
        {
            sheep.SetCanMove(move);
        }
        countScore = move;
    }

    public void ReactivateSheep()
    {
        foreach (Sheep sheep in allSheeps)
        {
            if(sheep.gameObject.activeSelf == false)
            {
                sheep.gameObject.SetActive(true);
            }
        }
    }

    private void ChangeSheepsSpeed()
    {
        float newSpeed = speedLevels[0];
        speedLevels.RemoveAt(0);
        if(speedLevels.Count == 0)
        {
            this.maxSpeedReached = true;
        }
        foreach (Sheep sheep in allSheeps)
        {
            sheep.SetSpeed(newSpeed);
        }

        //SoundsController.SwitchBackgroundMusic();
    }

    public void SheepCollideWithSaw()
    {
        _soundController.PlaySheepCollideWithSaw();
        _cameraController.ShakeCamera();
        this.MoveSheeps(false);
        this.lives--;
        this.HideDot(this.lives);
        if(this.lives < 1)
        {
            this.DisableAllButtons();
        }
    }

    public float GetSecondSheepXPosition(string SheepTag, float FirstSheepXPosition)
    {
        float xPos = 0f;
        foreach (Sheep sheep in allSheeps)
        {
            if (sheep.CompareTag(SheepTag))
            {
                Vector3 pos = sheep.transform.position;
                if (pos.x != FirstSheepXPosition)
                {
                    xPos = pos.x;
                }
            }
        }
        return xPos;
    }

    private void HideDot(int ind)
    {
        this.lifeDots[ind].SetActive(false);
    }

    public void PauseButtonHandler()
    {
        _soundController.PlayClick();
        if(this.pauseCredits > 0)
        {
            pauseButton.interactable = false;
            this.pauseCredits--;
            this.UpdatePauseButtonCreditsText();
            this.PauseSaws(true);
            StartCoroutine(WaitToUnpauseSaw());
            this.StartPauseButtonLoadingAnimation();
        }
    }

    private IEnumerator WaitToUnpauseSaw()
    {
        yield return new WaitForSeconds(this.pauseFreezeTime);
        this.PauseSaws(false);
    }

    private void PauseSaws(bool pause)
    {
        foreach (Saw saw in this.allSaws)
        {
            saw.PauseSaw(pause);
        }
    }

    private void StartPauseButtonLoadingAnimation()
    {
        //pauseButtonLoadingBar.fillAmount = 0;
        InvokeRepeating("AnimatePauseButtonLoadingBar", 0f, pauseLoadingBarInterval);
    }

    private void AnimatePauseButtonLoadingBar()
    {
        if (pauseButtonLoadingBar.fillAmount == 1f)
        {
            pauseButtonLoadingBar.fillAmount = 0;
            CancelInvoke("AnimatePauseButtonLoadingBar");
            pauseButton.interactable = true;
            _soundController.PlayPauseEnabled();
            return;
        }
        pauseButtonLoadingBar.fillAmount += this.pauseLoadingBarLevel;
    }

    public void StartPauseSheepLoadingAnimation()
    {
        canPauseSheep = false;
        InvokeRepeating("AnimatePauseSheepLoadingBar", 0f, pauseSheepLoadingBarInterval);
    }

    private void AnimatePauseSheepLoadingBar()
    {
        if (pauseSheepLoadingBar.fillAmount == 1f)
        {
            pauseSheepLoadingBar.fillAmount = 0;
            CancelInvoke("AnimatePauseSheepLoadingBar");
            canPauseSheep = true;
            _soundController.PlayEnableEat();
            return;
        }
        pauseSheepLoadingBar.fillAmount += pauseSheepLoadingBarLevel;
    }

    public bool CanPauseSheep()
    {
        return canPauseSheep;
    }

    public void ShowPauseMenu()
    {
        TogglePauseGame();
        Menu.gameObject.SetActive(!Menu.gameObject.activeSelf);
        _soundController.PlayClick();
    }

    public void TogglePauseGame()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public void RestartGame()
    {
        TogglePauseGame();
        SceneManager.LoadScene(1);
    }

    public void ExitToMainMenu()
    {
        TogglePauseGame();
        SceneManager.LoadScene(0);
    }
}
