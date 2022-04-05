using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    [SerializeField] private StageController stageController;
    // Start is called before the first frame update

    private float targetY = 0;

    private bool isRotating;
    void Start()
    {
        transform.position = new Vector3(-stageController.WorldSize.x / 10, stageController.WorldSize.y * 2,
            -stageController.WorldSize.z / 10);
        transform.RotateAround(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2), Vector3.up, 110f);
        transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2));

        isRotating = true;
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
            targetY -= 1f;
            transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
                stageController.WorldSize.z / 2));
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            targetY += 1f;
            transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
                stageController.WorldSize.z / 2));
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            isRotating = !isRotating;
        }
        
        if (Input.GetKey(KeyCode.Z))
        {
            Rotate(2f);
        }
        
        if (Input.GetKey(KeyCode.C))
        {
            Rotate(-2f);
        }

        if (isRotating)
        {
            Rotate();
        }

    }

    private void Rotate(float angle = .25f)
    {
        transform.RotateAround(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2), Vector3.up, angle);
        transform.LookAt(new Vector3(stageController.WorldSize.x / 2, targetY,
            stageController.WorldSize.z / 2));
    }
}
