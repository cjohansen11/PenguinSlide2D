using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathTools
{
    public static long BinomialCoefficient(long k, long n)
    {
        long r = 1;
        long d;

        if (k > n)
            return 0;

        for (d = 1; d <= k; d++)
        {
            r *= n--;
            r /= d;
        }
        return r;
    }

    public static float HornerAlgorythm(float[] poly, int n, float x)
    {
        float result = poly[0];

        for (int i = 1; i < n + 1; i++)
            result = result * x + poly[i];

        return result;
    }

    public static float HornerAlgorythmDerivative(float[] polynom, int degree, int derivative, float x)
    {
        float result = 0;

        for (int i = 0; i < (degree - derivative) + 1; i++)
        {
            result = result * x + polynom[i] * (degree - i);
        }

        return result;
    }
}