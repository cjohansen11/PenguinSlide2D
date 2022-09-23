using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
  public bool isCalculating = true;
  public TMP_Text textBox;
  public TMP_Text gameOverScore;
  public TMP_Text gameOverDistance;
  public bool newHighscore = false;
  public float totalScore = 0f;
  public float distanceTravelled = 0f;
  public Timer timerScript;
  public BirdMovement birdScript;
  public SaveSystem saveScript;

  // Update is called once per frame
  void Update()
  {
    if (isCalculating)
    {

      totalScore += ((timerScript.timeStart - timerScript.timeRemaining) * (birdScript.velocity.x));

      if (birdScript.height * 10 > 1f)
      {
        totalScore += 10;
      }
      else if (birdScript.height * 10 > 1.5f)
      {
        totalScore += 50;
      }

      distanceTravelled += ((timerScript.timeStart - timerScript.timeRemaining) * (birdScript.velocity.x));

      if (PlayerPrefs.GetFloat("Highscore") < totalScore)
      {
        saveScript.SaveScore(totalScore);
        newHighscore = true;
      }

      textBox.text = totalScore.ToString("F0");
      gameOverScore.text = totalScore.ToString("F0");
      gameOverDistance.text = distanceTravelled.ToString("F1") + "M";
    }
  }
}
