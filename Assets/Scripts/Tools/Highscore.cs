using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
  public ScoreCalculator scoreCalculator;
  // Update is called once per frame
  void Update()
  {
    bool newHighscore = scoreCalculator.newHighscore;

    GetComponent<TMP_Text>().enabled = newHighscore;
  }
}
