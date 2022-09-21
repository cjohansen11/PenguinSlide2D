using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
  private float totalDistance = 0f;
  private Vector2 previousPosition;
  // Start is called before the first frame update
  public GameObject player;
  public TMP_Text textBox;
  public float totalScore = 0f;

  void Start()
  {
    previousPosition = player.transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    float distanceThisFrame = Vector2.Distance(player.transform.position, previousPosition);
    totalDistance += distanceThisFrame;

    totalScore = Mathf.Round(totalDistance * 1000f);

    textBox.text = totalScore.ToString();

    previousPosition = player.transform.position;
  }
}
