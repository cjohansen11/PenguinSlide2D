using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanhFunction : Function
{
    float alpha = 3;

    float a = 0;
    float b = 0;
    float k1 = 0;
    float k2 = 0;

    float alphaMin = 1f;
    float alphaMax = 10f;


    public override void Generate(Vector2 startPoint, Vector2 endPoint)
    {
        alpha = Random.Range(alphaMin, alphaMax);
        base.Generate(startPoint, endPoint);

        a = (start.y + end.y) * 0.5f;
        b = (start.x + end.x) * 0.5f;
        k1 = 2f / (end.x - start.x);
        k2 = ((end.y - start.y) * 0.5f) / ((1f - Mathf.Exp(alpha)) / (1f + Mathf.Exp(alpha)));
    }

    public override float Use(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        return a + k2 * (1 - Mathf.Exp(alpha * k1 * (x - b))) / (1 + Mathf.Exp(alpha * k1 * (x - b)));
    }

    float UseDerivative(float x)
    {
        return -(2 * alpha * k1 * k2 * Mathf.Exp(alpha * k1 * (x - b))) / Mathf.Pow(Mathf.Exp(alpha * k1 * (x - b)) + 1, 2);
    }

    float UseDerivativeSecond(float x)
    {
        return (2 * Mathf.Pow(alpha, 2) * Mathf.Pow(k1, 2) * k2 * Mathf.Pow(Mathf.Exp(alpha * k1 * (x-b)), 2) - 2 * Mathf.Pow(alpha, 2)* Mathf.Pow(k1, 2) * k2 * Mathf.Exp(alpha * k1 * (x - b))) / (Mathf.Pow(Mathf.Exp(alpha * k1 * (x - b)), 3) + 3* Mathf.Pow(Mathf.Exp(alpha * k1 * (x - b)), 2) + 3 * Mathf.Exp(alpha * k1 * (x - b)) +1);
    }
    public override Vector2 GetEnd()
    {
        return end;
    }

    public override Vector2 GetTangent(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        return (new Vector2(1, UseDerivative(x))).normalized;
    }

    public override Vector2 GetNormal(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        return new Vector2(-UseDerivative(x), 1).normalized;
    }
    public override float GetDerivative(float completion, int degree)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        switch (degree)
        {
            case 0:
                return Use(completion);
       
            case 1:
                return UseDerivative(x);

            case 2:
                return UseDerivativeSecond(x);

            default:
                return UseDerivative(x);
        }
    }
}
