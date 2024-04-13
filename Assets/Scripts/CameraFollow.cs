using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform Player;

    float camOffsetY;
    float camOffsetX;

    public float height = 13f;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        camOffsetY = gameObject.transform.position.y - Player.position.y;
        camOffsetX = gameObject.transform.position.x - Player.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 newPos = Player.position + camOffsetZ
        if(Player != null)
        {
            Vector3 m_cameraPos = new Vector3(Player.position.x + camOffsetX, Player.position.y + camOffsetY, -height);

            transform.position = Vector3.Lerp(transform.position, m_cameraPos, SmoothFactor);
        }
        
    }
}
