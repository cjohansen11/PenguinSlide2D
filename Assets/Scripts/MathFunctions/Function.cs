using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Function
{
    public float duration;

    public Vector2 start;
    public Vector2 end;

    public virtual void Generate(Vector2 startPoint, Vector2 endPoint)
    {
        start = startPoint;
        end = endPoint;
        duration = endPoint.x - startPoint.x;

        return;
    }
    
    public virtual float Use(float completion)
    {
        return 0;
    }

    public virtual Vector2 GetEnd()
    {
        return end;
    }

    public float GetX(float completion)
    {
        return Mathf.Lerp(start.x, end.x, completion);
    }

    public virtual float GetDerivative(float completion, int degree)
    {
        return 0.0f;
    }

    public virtual Vector2 GetTangent(float completion)
    {
        return new Vector2(1, GetDerivative(completion, 1)).normalized;
    }

    public virtual Vector2 GetNormal(float completion)
    {
        return new Vector2(-GetDerivative(completion, 1), 1).normalized;
    }

    public virtual float CurveRadius(float completion)
    {
        return GetDerivative(completion, 2) / Mathf.Pow(1 + Mathf.Pow(GetDerivative(completion, 1), 2), 2.0f/3.0f);
    }
}
