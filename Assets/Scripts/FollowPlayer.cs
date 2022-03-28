using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]private GameObject player;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            return;
        }
        else 
        {
            transform.position = player.transform.position + offset;
        }
    }
}


