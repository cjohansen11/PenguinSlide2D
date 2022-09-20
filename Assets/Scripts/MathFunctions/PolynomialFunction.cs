using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolynomialFunction : Function
{
     float[] polynomialValue = new float [4];

    public override void Generate(Vector2 startPoint, Vector2 endPoint)
    {

        base.Generate(startPoint, endPoint);

        Matrix4x4 A = new Matrix4x4();

        A.SetRow(0, new Vector4(Mathf.Pow(startPoint.x, 3), Mathf.Pow(startPoint.x, 2), startPoint.x, 1));
        A.SetRow(1, new Vector4(Mathf.Pow(endPoint.x, 3), Mathf.Pow(endPoint.x, 2), endPoint.x, 1));
        A.SetRow(2, new Vector4(3 * Mathf.Pow(startPoint.x, 2), 2 * startPoint.x, 1, 0));
        A.SetRow(3, new Vector4(3 * Mathf.Pow(endPoint.x, 2), 2 * endPoint.x, 1, 0));

        Vector4 poly = A.inverse * new Vector4(startPoint.y, endPoint.y, 0, 0);

        polynomialValue[0] = poly.x;
        polynomialValue[1] = poly.y;
        polynomialValue[2] = poly.z;
        polynomialValue[3] = poly.w;

    }

    public override float Use(float completion)
    {
        return MathTools.HornerAlgorythm(polynomialValue, 3, Mathf.Lerp(start.x, end.x, completion));
    }

    public override Vector2 GetEnd()
    {
        return end;
    }

    public override float GetDerivative(float completion, int degree)
    {
        return MathTools.HornerAlgorythmDerivative(polynomialValue, 3, degree, Mathf.Lerp(start.x, end.x, completion));
    }

    public override Vector2 GetTangent(float completion)
    {
  
        float df = MathTools.HornerAlgorythmDerivative(polynomialValue, 3, 1, Mathf.Lerp(start.x, end.x, completion));

        return new Vector2(1, df).normalized;
    }

    

   
}
