using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class StageController : MonoBehaviour
{
    [SerializeField] private float GenerationSpeed = .5f;
    public GameObject WaterPrefab;
    public GameObject GroundPrefab;
    public GameObject SandPrefab;
    public GameObject GrassPrefab;
    public GameObject RockPrefab;
    public GameObject SnowPrefab;
    
    [SerializeField] private Toggle InstantGenerationToggle;
    
    [SerializeField] private InputField WorldX;
    [SerializeField] private InputField WorldY;
    [SerializeField] private InputField WorldZ;
    
    [SerializeField] private InputField WaterLevelI;
    [SerializeField] private InputField SandLevelI;
    [SerializeField] private InputField GrassLevelI;
    [SerializeField] private InputField RockLevelI;
    [SerializeField] private InputField SnowLevelI;
    
    [SerializeField] private Dropdown NoiseTypeD;
    [SerializeField] private Dropdown NoiseVarietyTypeD;
    
    [SerializeField] private Slider NoiseScaleS;
    [SerializeField] private Slider NoiseVarietyScaleS;
    [SerializeField] private Slider NoiseVarietyVolumeS;
    
    [SerializeField] private Image NoiseMap1;
    [SerializeField] private Image NoiseMap2;
    
    
    public bool InstantGeneration
    {
        get { return InstantGenerationToggle.isOn; }
    }

    public Vector3Int WorldSize
    {
        get
        {
            //Debug.Log(new Vector3Int(int.Parse(WorldX.text), int.Parse(WorldY.text), int.Parse(WorldZ.text)));
            return new Vector3Int(int.Parse(WorldX.text), int.Parse(WorldY.text), int.Parse(WorldZ.text));
        }
    }

    public int WaterLevel  
    {
        get
        {
            return (int) (float.Parse(WaterLevelI.text) * .01f * WorldSize.y);
        }
    }
    
    public int SandLevel
    {
        get { return (int) (float.Parse(SandLevelI.text) * .01f * WorldSize.y); }
    }
    
    public int GrassLevel
    {
        get { return (int) (float.Parse(GrassLevelI.text) * .01f * WorldSize.y);}
    }
    
    public int RockLevel
    {
        get { return (int) (float.Parse(RockLevelI.text) * .01f * WorldSize.y); }
    }
    
    public int SnowLevel
    {
        get {return (int) (float.Parse(SnowLevelI.text) * .01f * WorldSize.y); }
    }
    
    public int NoiseScale
    {
        get { return (int)NoiseScaleS.value; }
    }
    
    public int NoiseVarietyScale
    {
        get { return (int)NoiseVarietyScaleS.value; }
    }
    
    public int NoiseVarietyVolume
    {
        get {return (int) ((NoiseVarietyVolumeS.value) * .01f * WorldSize.y); }
    }
    
    public int NoiseType
    {
        get { return (int)NoiseTypeD.value + 1; }
    }
    
    public int NoiseVarietyType
    {
        get { return (int)NoiseVarietyTypeD.value; }
    }

    //private GameObject[,,] World;

    private Queue<KeyValuePair<Vector3, GameObject>> _queue;

    private FastNoiseLite.NoiseType GetNoiseType(int type)
    {
        return type switch
        {
            1 => FastNoiseLite.NoiseType.Cellular,
            2 => FastNoiseLite.NoiseType.Perlin,
            3 => FastNoiseLite.NoiseType.Value,
            4 => FastNoiseLite.NoiseType.ValueCubic,
            5 => FastNoiseLite.NoiseType.OpenSimplex2,
            6 => FastNoiseLite.NoiseType.OpenSimplex2S,
            _ => FastNoiseLite.NoiseType.Perlin
        };
    }

    void Start()
    {
        OnRestart();
    }

    //todo:
    // Option: FVP Game ?????
    // camera control - Y of point of view

    private void Update()
    {
        if (!InstantGeneration)
        {
            if (_queue.Count > 0)
            {
                for (int i = 0; i < WorldSize.x * GenerationSpeed; i++)
                {
                    if(_queue.Count == 0) break;
                    
                    KeyValuePair<Vector3, GameObject> _pair = _queue.Dequeue();
                    Instantiate(_pair.Value, _pair.Key, Quaternion.identity);
                }
            }
        }
        else
        {
            while (_queue.Count > 0)
            {
                KeyValuePair<Vector3, GameObject> _pair = _queue.Dequeue();
                Instantiate(_pair.Value, _pair.Key, Quaternion.identity);
            }
        }
    }

    private FastNoiseLite noise;
    private FastNoiseLite noiseVariety;

    private void Generate()
    {
       // World = new GameObject[WorldSize.x, WorldSize.y, WorldSize.z];

        noise = new FastNoiseLite();
        noise.SetNoiseType(GetNoiseType(NoiseType));

        if (NoiseVarietyType > 0)
        {
            noiseVariety = new FastNoiseLite();
            noiseVariety.SetNoiseType(GetNoiseType(NoiseVarietyType));
        }

        
        Texture2D texture = new Texture2D(WorldSize.x, WorldSize.z);
        //NoiseMap1.GetComponentInParent<CanvasRenderer>().material.mainTexture = texture;

        for (int x = 0; x < WorldSize.x; x++)
        {
            for (int z = 0; z < WorldSize.z; z++)
            {
                float noiseValue = noise.GetNoise(x * NoiseScale, z * NoiseScale);
                int yVol = (int) ((noiseValue + 1f) * WorldSize.y/2);
                FillRow(x, yVol, z);

                float cv = (noiseValue + 1f)/2f;
                //Debug.Log(cv);
                texture.SetPixel(x, z, new Color(cv,cv,cv));
                //texture.SetPixel(x, z, new Color(,cv,cv));
                //texture.SetPixel(x, z, Color.red);
            }
        }
      
        NoiseMap1.material.mainTexture = texture;
        texture.Apply();
        NoiseMap1.enabled = false;
        NoiseMap1.enabled = true;


    }

    private void FillRow(int x, int yVol, int z)
    {
        Debug.Log(yVol);
        int heightLimit = yVol > WaterLevel ? yVol : WaterLevel;

        for (int i = 0; i <= heightLimit; i++)
        {
            if (i == yVol && i == 0)
            {
                _queue.Enqueue(new KeyValuePair<Vector3, GameObject>(new Vector3(x, i, z), GroundPrefab));
            }
            else if (i > yVol && WaterLevel > 0)
            {
                _queue.Enqueue(new KeyValuePair<Vector3, GameObject>(new Vector3(x, i, z), WaterPrefab));
            }
            else if (i == yVol)
            {
                _queue.Enqueue(new KeyValuePair<Vector3, GameObject>(new Vector3(x, i, z), GetPrefab(x, i, z)));
            }
        }
    }

    private GameObject GetPrefab(int x, int yVol, int z)
    {
        int deltaY = 0;
        if (NoiseVarietyType > 0)
        {
            deltaY = (int)(noiseVariety.GetNoise(x * NoiseVarietyScale, z * NoiseVarietyScale) * NoiseVarietyVolume);
        }
        
        GameObject prefab;
        if (yVol + deltaY >= SnowLevel)
        {
            prefab = SnowPrefab;
        }
        else if (yVol + deltaY >= RockLevel)
        {
            prefab = RockPrefab;
        }
        else if (yVol + deltaY >= GrassLevel)
        {
            prefab = GrassPrefab;
        }
        else if (yVol + deltaY >= SandLevel)
        {
            prefab = SandPrefab;
        }
        else
        {
            prefab = GroundPrefab;
        }

        return prefab;
    }
    
    public float Sigmoid(double value) {
        return 1.0f / (1.0f + (float) Math.Exp(-value));
    }

    public void OnRestart()
    {
        _queue = new Queue<KeyValuePair<Vector3, GameObject>>();

        GameObject[] list = GameObject.FindGameObjectsWithTag("World");
        foreach (var item in list)
        {
            Destroy(item);
        }
        
        Generate();
    }
}