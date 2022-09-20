using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusFunction : Function
{
    public float amplitude = 0f;
    public float pulsation = 0f;
    public float phase = 0f;
    public float offset = 0f;

    private Vector2 endMinusStart;
    
    public override void Generate(Vector2 startPoint, Vector2 endPoint)
    {
        base.Generate(startPoint, endPoint);

        endMinusStart = end - start;
        pulsation = Mathf.PI / endMinusStart.x;
    }

    public override float Use(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);

        phase = Mathf.PI / 2 - pulsation * end.x;
        amplitude = (end.y - start.y) / 2;
        offset = end.y - amplitude;

        return offset + amplitude * Mathf.Sin(pulsation * x + phase);
    }

    public override Vector2 GetEnd()
    {
        return end;
    }

    public override float GetDerivative(float completion, int degree)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);
        if (degree == 0)
            return Use(completion);

        return amplitude * Mathf.Pow(pulsation, degree) * Mathf.Sin(pulsation * x + phase + degree * Mathf.PI * 0.5f);
    }
}
