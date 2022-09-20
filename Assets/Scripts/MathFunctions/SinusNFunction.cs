using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusNFunction : Function
{
    public float amplitude = 0f;
    public float pulsation = 0f;
    public float phase = 0f;
    public float offset = 0f;
    public int n = 5;
    public int nMin = 1;
    public int nMax = 10;

    private Vector2 endMinusStart;

    private bool isNPair = false;
    private bool isWithBinomialCoef = false;// put this boolean at true if you want to use the function with the binomial

    public override void Generate(Vector2 startPoint, Vector2 endPoint)
    {
        n = Random.Range(nMin,nMax);

        base.Generate(startPoint, endPoint);
        endMinusStart = end - start;

        //Check if the function is pair or impair
        if (n % 2 == 0)
        {
            isNPair = true;

            amplitude = endMinusStart.y;
            offset = start.y;
            pulsation = Mathf.PI / (endMinusStart.x * 2f);
            phase = Mathf.PI * 0.5f - end.x * pulsation;
        }
        else
        {
            isNPair = false;

            amplitude = endMinusStart.y * 0.5f;
            offset = start.y + amplitude;
            pulsation = Mathf.PI / endMinusStart.x;
            phase = Mathf.PI * 0.5f - end.x * pulsation;
        }
    }

    public override float Use(float completion)
    {
        float x = Mathf.Lerp(start.x, end.x, completion);

        if (!isWithBinomialCoef)
            return offset + amplitude * Mathf.Pow(Mathf.Sin(pulsation * x + phase), n);

        float sum = 0;

        if (isNPair)
        {
            for (int k = 0; k<= (n-2)*0.5f; k++)
                sum += Mathf.Pow(-1, k) * MathTools.BinomialCoefficient(k, n) * Mathf.Cos((n - k * 2) * (pulsation * x + phase));
            
            return offset + amplitude * (Mathf.Pow(-1, n * 0.5f) / Mathf.Pow(2, n - 1)) * (sum + 0.5f * Mathf.Pow(-1, n * 0.5f) * MathTools.BinomialCoefficient(n / 2, n));
        }
        else
        {
            for (int k = 0; k <= ((float)n - 1f)*0.5f; k++)
                sum += Mathf.Pow(-1, k) * MathTools.BinomialCoefficient(k, n) * Mathf.Sin((n-k*2) * (pulsation * x + phase));
            
            return offset + amplitude * (Mathf.Pow(-1, (n - 1) * 0.5f) / Mathf.Pow(2, n - 1)) * sum;
        }
    }

    public override float GetDerivative(float completion, int degree)
    {
        if (degree < 0)
            return 0;
        float x = Mathf.Lerp(start.x, end.x, completion);

        float sum = 0;

        if (isNPair)
        {
            for (int k = 0; k <= (n - 2) * 0.5f; k++)
                sum += Mathf.Pow(-1, k) * MathTools.BinomialCoefficient(k, n) * Mathf.Pow((n - k * 2) * pulsation, degree) * Mathf.Cos((n - k * 2) * (pulsation * x + phase) + degree * Mathf.PI * 0.5f);

            return offset + amplitude * (Mathf.Pow(-1, n * 0.5f) / Mathf.Pow(2, n - 1)) * (sum + 0.5f * Mathf.Pow(-1, n * 0.5f) * MathTools.BinomialCoefficient(n / 2, n));
        }
        else
        {
            for (int k = 0; k <= ((float)n - 1f) * 0.5f; k++)
                sum += Mathf.Pow(-1, k) * MathTools.BinomialCoefficient(k, n) * Mathf.Pow((n - k * 2) * pulsation, degree) * Mathf.Sin((n - k * 2) * (pulsation * x + phase) + degree * Mathf.PI * 0.5f);

            return offset + amplitude * (Mathf.Pow(-1, (n - 1) * 0.5f) / Mathf.Pow(2, n - 1)) * sum;
        }
    }
}
