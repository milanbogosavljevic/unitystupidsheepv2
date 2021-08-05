using UnityEngine;
using UnityEngine.UI;

public class ScoreTextSetter : MonoBehaviour
{
    public Text Score;
    private float score;
    private int counter;

    void Start()
    {
        counter = 0;
        score = PlayerPrefs.GetFloat("Score");
        InvokeRepeating("CountScore", 0f, 0.05f);
    }

    private void CountScore()
    {
        counter++;
        if (Score.text == score.ToString("F0"))
        {
            CancelInvoke("CountScore");
            return;
        }
        Score.text = counter.ToString("F0");
    }
}
