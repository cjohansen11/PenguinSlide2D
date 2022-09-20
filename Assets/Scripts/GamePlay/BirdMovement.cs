using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdMovement : MonoBehaviour
{

    public FunctionReading funcReading;

    [SerializeField] Camera activeCamera;

    [SerializeField] float minCameraSize = 0.02f;
    [SerializeField] float verticalOffsetFactor = 1.25f;

    [SerializeField] [Range(0,1)] float pixelPosition = 0.2f;
    [SerializeField] Vector2 velocity = new Vector2();
    [SerializeField] float height = 0;

    [SerializeField] float minSpeed = 1.0f;

    [SerializeField] float gravity = 1.0f;
    [SerializeField] float groundedFactorDown = 1.0f;
    [SerializeField] float groundedFactorUp = 1.0f;
    [SerializeField] float flyingUpFactor = 1.0f;
    [SerializeField] float flyingDownFactor = 1.0f;

    public float groundedErrorMarge = 0.001f;
    public float animationGroundedErrorMarge = 0.01f;

    [SerializeField] float downRush = 1.0f;

    [SerializeField] CharacterAnimation characterAnimation;

    SaveSystem.Score scoreData;

    [SerializeField] Text heightText;
    [SerializeField] Text speedText;

    [SerializeField] Text bestHeightText;
    [SerializeField] Text bestSpeedText;


    // Start is called before the first frame update
    void Start()
    {
        scoreData = SaveSystem.Load();

        if (Application.isMobilePlatform)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 60;
        }
        Application.targetFrameRate = 1000;

        Physics.autoSimulation = false;
    }

    // Update is called once per frame
    void Update()
    {
        funcReading.Move(velocity.x * Time.deltaTime);
        height += velocity.y * Time.deltaTime;
     
        if (Input.GetKeyDown(KeyCode.E))
        {
            velocity.x += 1;
        }


        if (Input.GetKey(KeyCode.Space) /*|| Input.touchCount > 1*/)
        {
            velocity.y -= downRush * Time.deltaTime;
            characterAnimation.Set(false);
        }
        else
        {
            if (height <= funcReading.GetLerpHeight(pixelPosition) + animationGroundedErrorMarge)
                characterAnimation.Set(false);
            else
                characterAnimation.Set(true);
        }

        if (height <= funcReading.GetLerpHeight(pixelPosition))
        {
            height = funcReading.GetLerpHeight(pixelPosition);
            transform.up = funcReading.GetLerpNormal(pixelPosition);
            
            Vector2 normal = funcReading.GetVectorNormal(pixelPosition);
            Vector2 tangent = funcReading.GetVectorTangent(pixelPosition);

            velocity = tangent * (Vector2.Dot(tangent, velocity));

            if (tangent.y > 0)
            {
                velocity.y -= (gravity * normal.y * Time.deltaTime) * groundedFactorUp;
            }
            else
            {
                velocity.y -= (gravity * normal.y * Time.deltaTime) * groundedFactorDown;
            }
        }
        else
        {
            if (velocity.y >= 0)
                velocity.y -= (gravity * Time.deltaTime) * flyingUpFactor;
            else
                velocity.y -= (gravity * Time.deltaTime) * flyingDownFactor;

        }

        velocity.x = Mathf.Max(velocity.x, minSpeed);

        float maxCameraSize = (1f / activeCamera.aspect);
        activeCamera.orthographicSize = Mathf.Clamp(Mathf.Abs(height) * verticalOffsetFactor, minCameraSize, maxCameraSize);

        if (activeCamera.orthographicSize <= maxCameraSize - 0.001f)
            activeCamera.transform.position = new Vector3((activeCamera.orthographicSize), Mathf.Lerp(0f, 0.5f, height / maxCameraSize), -5);
        else
            activeCamera.transform.position = new Vector3((activeCamera.orthographicSize), (height * verticalOffsetFactor) - maxCameraSize + Mathf.Lerp(0f, 0.5f, height / maxCameraSize), -5);


        transform.localPosition = new Vector3(0, height, -1);

        if (scoreData.height < height)
        {
            scoreData.height = height;
            SaveSystem.Save(scoreData);
        }
        if (scoreData.speed < velocity.x)
        {
            scoreData.speed = velocity.x;
            SaveSystem.Save(scoreData);
        }

        heightText.text = (height * 100f).ToString();
        speedText.text = (velocity.x * 100f).ToString();

        bestHeightText.text = (scoreData.height * 100f).ToString();
        bestSpeedText.text = (scoreData.speed * 100f).ToString();
    }
}