using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISeparation : MonoBehaviour
{
    // Start is called before the first frame update
    public float spaceBetween = 1f;
    private Collider2D[] neighboors;
    public LayerMask mask;
    // Update is called once per frame

    void FixedUpdate()
    {
        neighboors = Physics2D.OverlapCircleAll(transform.position, spaceBetween, mask);
        foreach(Collider2D c in neighboors)
        {
            GameObject go = c.gameObject;
            if(go != gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, this.transform.position);
                if(distance <= spaceBetween)
                {
                    Vector3 direction = transform.position - go.transform.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
    }
}
