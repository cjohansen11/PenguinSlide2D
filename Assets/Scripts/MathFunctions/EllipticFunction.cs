using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipticFunction : Function
{
    float amplitude = 0;
    float pulsation = 0;

    public override void Generate(Vector2 startPoint, Vector2 endPoint)
    {
        base.Generate(startPoint, endPoint);

        float u = 1 - Mathf.Pow((end.y - start.y) / (2 * (end.y - start.y)), 2);
        float v = (start.x - end.x) * 0.5f;
        pulsation = Mathf.Sqrt(u) / v;
        amplitude = end.y - start.y;
    }

    public override float Use(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);

        if (x / (end.x - start.x) > 0.5f)
            return start.y + amplitude * Mathf.Sqrt(1 - Mathf.Pow(pulsation * (x - end.x), 2));
        else 
            return end.y - amplitude * Mathf.Sqrt(1 - Mathf.Pow(pulsation * (x - start.x), 2));
    }

    public float UseDerivative(float x)
    {
        if (end.x - x == 0 || start.x + x == 0)
            return 0;

        if (x / (end.x - start.x) > 0.5f)
            return (amplitude * Mathf.Pow(pulsation, 2) * (end.x - x)) / Mathf.Sqrt(1 - Mathf.Pow(pulsation, 2) * Mathf.Pow(end.x - x, 2));

        else
            return (amplitude * Mathf.Pow(pulsation, 2) * (start.x + x)) / Mathf.Sqrt(1 - Mathf.Pow(pulsation, 2) * Mathf.Pow(start.x + x, 2));
    }

    public float UseDerivativeSecond(float x)
    {
        if (end.x + x == 0 || start.x + x == 0)
            return 0;

        if (x / (end.x - start.x) > 0.5f)
            return (Mathf.Pow(pulsation, 4) * amplitude * Mathf.Pow(x + end.x, 2)) / ((1 - Mathf.Pow(pulsation, 2) * Mathf.Pow(Mathf.Pow(x + end.x, 2), ((3) / (2))))) + (Mathf.Pow(pulsation, 2) * amplitude) / (Mathf.Sqrt(1 - Mathf.Pow(pulsation, 2) * Mathf.Pow(x + end.x, 2)));

        else
            return (Mathf.Pow(pulsation, 4) * amplitude * Mathf.Pow(x + start.x, 2))/ ((1 - Mathf.Pow(pulsation, 2) * Mathf.Pow(Mathf.Pow(x + start.x, 2), ((3) / (2))))) + (Mathf.Pow(pulsation, 2)* amplitude) / (Mathf.Sqrt(1 - Mathf.Pow(pulsation , 2) * Mathf.Pow(x + start.x,2)));
    }

    public override Vector2 GetEnd()
    {
        return end;
    }

    public override Vector2 GetTangent(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        return new Vector2(1, UseDerivative(x)).normalized;
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
