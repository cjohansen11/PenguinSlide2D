using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSystem : MonoBehaviour
{
  public TMP_Text highScore;
  // Start is called before the first frame update
  void Start()
  {
    float prevHighscore = PlayerPrefs.GetFloat("Highscore");

    if (prevHighscore <= 0)
    {
      highScore.text = "No score saved";
    }
    else
    {
      highScore.text = prevHighscore.ToString("F0");
    }
  }

  public void SaveScore(float highScore)
  {
    PlayerPrefs.SetFloat("Highscore", highScore);
  }
}
