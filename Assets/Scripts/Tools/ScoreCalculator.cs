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
  public float totalScore = 0f;
  public Timer timerScript;
  public BirdMovement birdScript;

  // Update is called once per frame
  void Update()
  {
    if (isCalculating)
    {

      totalScore += ((timerScript.timeStart - timerScript.timeRemaining) * (birdScript.velocity.x));

      textBox.text = totalScore.ToString("F0");
      gameOverScore.text = totalScore.ToString("F0");
      gameOverDistance.text = totalScore.ToString() + "M";
    }
  }
}
