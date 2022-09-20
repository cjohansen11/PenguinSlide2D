using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionReading : MonoBehaviour
{
    List<PixelData> listPixelData = new List<PixelData>();

    Function drawingFunction = new PolynomialFunction();

    List<QuadData> listQuad = new List<QuadData>();
    [SerializeField] FunctionDrawing quadRef;

    public uint drawingFunctionpixelAvancement = 0;

    public int valueToReadInShader = 1000;
    public float zoomFactor = 1.0f;

    [SerializeField] float minHeight = 0.001f;
    [SerializeField] float maxHeight = 0.003f;
    
    [SerializeField] float minForward = 0.01f;
    [SerializeField] float maxForward = 0.02f;


    float advancement = 0.0f;

    bool functionUp = true;

    public int nbCubeToGenerate = 2;

    public enum FunctionType { RANDOM, SINUS, SINUSN, POLYNOMIAL, ELLIPTIC, TANH};

    [SerializeField] FunctionType functionToGenerate = FunctionType.SINUSN;

    struct PixelData
    {
        public PixelData(Function _function, float _completion)
        {
            height = _function.Use(_completion);
            function = _function;
            completion = _completion;
        }

        public float height;
        public Function function;
        public float completion;
    }

    struct QuadData
    {
        public QuadData(int size, float offsetX, FunctionDrawing quadRef)
        {
            listHeight = new float[size];
            quad = Instantiate(quadRef);
            quad.transform.position += new Vector3(offsetX, 0, 0);
        }

        public QuadData(int size, FunctionDrawing quadRef)
        {
            listHeight = new float[size];
            quad = quadRef;
        }

        public void SetShader(int size)
        {
            quad.material.SetFloatArray("_DataArray", listHeight);
            quad.material.SetInt("valueToRead", size);
        }

        public FunctionDrawing quad;
        public float[] listHeight;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        listQuad.Add(new QuadData(valueToReadInShader, quadRef));
        
        for (int i = 1; i < nbCubeToGenerate; i++)
        {
            listQuad.Add(new QuadData(valueToReadInShader, i, quadRef));
        }

        drawingFunction.Generate(Vector2.zero, Vector2.one * 0.1f);

        for (uint totalPixel = 0; totalPixel < valueToReadInShader * nbCubeToGenerate; totalPixel++)
        {
            listPixelData.Add(new PixelData(drawingFunction, (float)(drawingFunctionpixelAvancement) / (uint)(drawingFunction.duration * valueToReadInShader)));

            CreateNewFunction();

            drawingFunctionpixelAvancement += 1;
        }
    }

    void CreateNewFunction()
    {
        if (drawingFunctionpixelAvancement >= (uint)(drawingFunction.duration * valueToReadInShader))
        {
            Function line = GetFunction();
            if (functionUp)
                line.Generate(new Vector2(0, drawingFunction.GetEnd().y), new Vector2(Random.Range(minForward, maxForward), Random.Range(minHeight, maxHeight)));
            else
                line.Generate(new Vector2(0, drawingFunction.GetEnd().y), new Vector2(Random.Range(minForward, maxForward), -Random.Range(minHeight, maxHeight)));

            functionUp = !functionUp;

            drawingFunctionpixelAvancement = 0;
            drawingFunction = line;
        }
    }
    
    public void Move(float distance)
    {
        if (advancement + distance < (1.0f / valueToReadInShader))
        {
            advancement += distance;
            SetCompletion(advancement / (1.0f / valueToReadInShader));
            return;
        }

        SetCompletion(0);
        distance -= (1f / valueToReadInShader) - advancement;
        advancement = 0;

        CreateNewFunction();
        drawingFunctionpixelAvancement += 1;

        for (int i = 1; i < listPixelData.Count; i++)
        {
            listPixelData[i - 1] = listPixelData[i];
        }

        listPixelData[(valueToReadInShader * listQuad.Count) - 1] = new PixelData(drawingFunction, (float)(drawingFunctionpixelAvancement) / (uint)(drawingFunction.duration * 1000));
        
        for (int i = 0; i < listQuad.Count; i++)
        {
            for (int e = 0; e < valueToReadInShader; e++)
            {
                listQuad[i].listHeight[e] = listPixelData[i * valueToReadInShader + e].height;
            }

            listQuad[i].SetShader(valueToReadInShader);
        }

        if (distance > 0)
        {
            Move(distance);
        }
    }

    void SetCompletion(float completion)
    {
        for (int i = 0; i < listQuad.Count; i++)
        {
            listQuad[i].quad.material.SetFloat("completion", completion);
        }
    }

    public int GetIndex(float factor)
    {
        return (int)(factor * valueToReadInShader);
    }

    Function GetFunction()
    {
        switch (functionToGenerate)
        {
            case FunctionType.RANDOM: return GetRandomFunction();
            case FunctionType.SINUS: return new SinusFunction();
            case FunctionType.SINUSN: return new SinusNFunction();
            case FunctionType.POLYNOMIAL: return new PolynomialFunction();
            case FunctionType.ELLIPTIC: return new EllipticFunction();
            case FunctionType.TANH: return new TanhFunction();
        }
        return new SinusFunction();
    }

    Function GetRandomFunction()
    {
        int random = Random.Range(0, 6);

        switch (random)
        {
            case 0: return new SinusFunction();
            case 1: return new SinusNFunction();
            case 2: return new PolynomialFunction();
            case 3: return new EllipticFunction();
            case 4: return new TanhFunction();
        }

        return new SinusFunction();
    }

    public float GetHeight(float indexFactor)
    {
        return listPixelData[(int)(valueToReadInShader * indexFactor)].height;
    }

    public float GetLerpHeight(float indexFactor)
    {
        int index = (int)(valueToReadInShader * indexFactor);
        return Mathf.Lerp(listPixelData[index].height, listPixelData[index + 1].height, advancement / (1f / valueToReadInShader));
    }

    public Vector2 GetVectorTangent(float indexFactor)
    {
        int index = (int)(valueToReadInShader * indexFactor);
        return listPixelData[index].function.GetTangent(listPixelData[index].completion);
    }

    public Vector2 GetVectorNormal(float indexFactor)
    {
        int index = (int)(valueToReadInShader * indexFactor);
        return listPixelData[index].function.GetNormal(listPixelData[index].completion);
    }

    public Vector2 GetLerpNormal(float indexFactor)
    {
        int index = (int)(valueToReadInShader * indexFactor);
     
        return Vector2.Lerp(listPixelData[index].function.GetNormal(listPixelData[index].completion),
            listPixelData[index + 1].function.GetNormal(listPixelData[index + 1].completion), advancement / (1f / valueToReadInShader));
    }

    public void SetFunctionGeneration(FunctionType type)
    {
        functionToGenerate = type;
    }

    public void SetFunctionGenerationRandom()
    {
        functionToGenerate = FunctionType.RANDOM;
    }

    public void SetFunctionGenerationSinus()
    {
        functionToGenerate = FunctionType.SINUS;
    }
    public void SetFunctionGenerationSinusN()
    {
        functionToGenerate = FunctionType.SINUSN;
    }
    public void SetFunctionGenerationPolynomial()
    {
        functionToGenerate = FunctionType.POLYNOMIAL;
    }
    public void SetFunctionGenerationElliptic()
    {
        functionToGenerate = FunctionType.ELLIPTIC;
    }
    public void SetFunctionGenerationTANH()
    {
        functionToGenerate = FunctionType.TANH;
    }

}
