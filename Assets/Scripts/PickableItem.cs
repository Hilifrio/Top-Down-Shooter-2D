using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour
{
    bool isPlayerEntered = false;
    public bool isPickedUp = false;
    public float radius;
    // Start is called before the first frame update
    [SerializeField] Canvas generalInfos;
    public Text itemInfos;

    public Player player{get; protected set;}

    public event System.Action OnPickUp;
    public event System.Action OnArea;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisons();
        DisplayInfos();
        
        if(isPlayerEntered)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                OnPickUp();
            }
            OnArea();
        }
    }

    void CheckCollisons()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        isPlayerEntered = false;
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.tag == "Player")
            {
                isPlayerEntered = true;
                player = hitCollider.gameObject.GetComponent<Player>();
                if(player == null)
                {
                    player = GameObject.FindWithTag("Player").GetComponent<Player>();
                }
            }
        }
    }

    void DisplayInfos()
    {
        if(!isPickedUp)
        {
            generalInfos.gameObject.SetActive(isPlayerEntered);
        }
        
    } 
}
