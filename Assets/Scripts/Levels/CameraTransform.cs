using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    [SerializeField] private float borderX;
    [SerializeField] private float speed = 0.125F;
    private float x;

    void Update()
    {        
        float tempX = GetX();

        transform.position = Vector3.Lerp(transform.position, new Vector3(borderX * tempX, transform.position.y, transform.position.z), speed);

        if (tempX > 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(borderX, transform.position.y, transform.position.z), speed);
        }
        else if (tempX == 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, transform.position.y, transform.position.z), speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(-borderX, transform.position.y, transform.position.z), speed);
        }
    }

    private float GetX()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            x = 0;
            return Input.GetAxis("Horizontal");
        }
        else
            return x;
    }
    public void SetX(float deltaX)
    {
        if (Mathf.Clamp(deltaX, -1, 1) != x && x != 0)
            x = 0;
        else
            x = Mathf.Clamp(deltaX, -1,1);        
    }
}
