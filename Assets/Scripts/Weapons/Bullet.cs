using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask collisionMask;
    public int damage;
    public float speed = 10;
    public float livingTime = 2f;
    float skinWidth = .1f;
    //Vector3 target;
    public GameObject impactEffect;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Start()
    {
        GameObject.Destroy(gameObject, livingTime);

        Collider2D initialCollisions = Physics2D.OverlapCircle(transform.position, .05f, collisionMask);

		if (initialCollisions != null) 
        {
            GameObject.Destroy(gameObject);
        }

    }

   void Update () 
    {

		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.right * moveDistance);
	}

    void CheckCollisions(float moveDistance) 
    {
		Ray ray = new Ray (transform.position, transform.TransformDirection(Vector3.right));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), moveDistance + skinWidth, collisionMask);

        if (hit.collider != null) 
        {
			OnHitObject(hit);
		}
    }

    void OnHitObject(RaycastHit2D hit) 
    {
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit(damage, hit.point, transform.right);
		}
        else
        {
            Destroy(Instantiate(impactEffect.gameObject, hit.point, Quaternion.FromToRotation(Vector3.down, transform.right)) as GameObject, 3f);
        }
		GameObject.Destroy (gameObject);
	}
}
