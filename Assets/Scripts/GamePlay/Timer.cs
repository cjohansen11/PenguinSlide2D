using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
  public float timeStart = 120;
  public TMP_Text textBox;
  public GameObject gameOverUI;

  // Start is called before the first frame update
  void Start()
  {
    gameOverUI.SetActive(false);
    textBox.text = timeStart.ToString("F2");
  }

  // Update is called once per frame
  void Update()
  {
    if (timeStart > 0)
    {
      timeStart -= Time.deltaTime;
      textBox.text = timeStart.ToString("F2");
    }
    else if (timeStart <= 0)
    {
      Time.timeScale = 0.2f;
      gameOverUI.SetActive(true);
    }
  }
}
