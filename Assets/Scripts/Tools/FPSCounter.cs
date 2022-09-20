using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
  public Text FPS;

  private float fpsCounter = 0;
  private float currentFpsTime = 0;
  private float fpsShowPeriod = 1;

  // Update is called once per frame
  void Update()
  {
    currentFpsTime = currentFpsTime + Time.deltaTime;
    fpsCounter = fpsCounter + 1;
    if (currentFpsTime > fpsShowPeriod)
    {
      FPS.text = fpsCounter.ToString();
      currentFpsTime = 0;
      fpsCounter = 0;
    }
  }
}
