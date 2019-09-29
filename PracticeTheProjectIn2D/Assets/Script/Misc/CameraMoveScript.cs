using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{

    public Vector3 minClamp;
    public Vector3 maxClamp;

    public Transform player;
    public Vector3 offset;

    void Start()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minClamp.x, maxClamp.x),
            Mathf.Clamp(transform.position.y, minClamp.y, maxClamp.y), 
            transform.position.z);
    }
}
