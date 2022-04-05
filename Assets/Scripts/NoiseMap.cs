using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{

    [SerializeField] private GameObject _rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.localScale = new Vector3(.5f, 0.5f, 0.5f);
        }
        
        if (Input.GetKey(KeyCode.Alpha2))
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
