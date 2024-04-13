using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAiming : MonoBehaviour
{
    public int rotationOffset = 90;
    private Transform playerTransform;
    // Update is called once per frame
    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }
    void Update()
    {
        Vector3 difference = playerTransform.position - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
        if (Mathf.Abs(rotZ) > 90)
        {
            transform.rotation = Quaternion.Euler(180f, 0f, -rotZ);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
        }
    }
}
