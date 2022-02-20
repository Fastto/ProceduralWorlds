using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    [SerializeField] private StageController stageController;
    // Start is called before the first frame update

    private float targetY = 0;
    void Start()
    {
        transform.position = new Vector3(-stageController.WorldSize.x / 10, stageController.WorldSize.y * 2,
            -stageController.WorldSize.z / 10);
        transform.RotateAround(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2), Vector3.up, 110f);
        transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up / 4;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.up / 4;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += transform.forward/2;
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= transform.forward/2;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            targetY -= .5f;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            targetY += .5f;
        }
        
        transform.RotateAround(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2), Vector3.up, .25f);
        transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2));


 
    }
}
