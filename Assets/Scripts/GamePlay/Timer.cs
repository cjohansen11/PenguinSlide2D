using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
  public float timeStart = 120;
  public float timeRemaining;
  public TMP_Text pauseMenuTimer;
  public TMP_Text textBox;
  public GameObject gameOverUI;
  public ScoreCalculator scoreCalculator;

  private void Awake()
  {
    timeRemaining = timeStart;
  }

  // Start is called before the first frame update
  void Start()
  {
    gameOverUI.SetActive(false);
    textBox.text = timeRemaining.ToString("F2");
    pauseMenuTimer.text = timeRemaining.ToString("F2");
  }

  // Update is called once per frame
  void Update()
  {
    if (timeRemaining > 0)
    {
      timeRemaining -= Time.deltaTime;
      textBox.text = timeRemaining.ToString("F2") + "s";
      pauseMenuTimer.text = timeRemaining.ToString("F2");
    }
    else if (timeRemaining <= 0)
    {
      textBox.text = "Game Over";
      scoreCalculator.isCalculating = false;
      Time.timeScale = 0f;
      gameOverUI.SetActive(true);
    }
  }
}
