using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;

    public Transform target;

    public Vector3 offset;

    public float smoothDamp = 0.15f;

    public bool YMaxEnabled = false;
    public float YMaxValue = 0;

    public bool YMinEnabled = false;
    public float YMinValue = 0;

    public bool XMaxEnabled = false;
    public float XMaxValue = 0;

    public bool XMinEnabled = false;
    public float XMinValue = 0;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {
        //transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = target.position;

        if (YMinEnabled && YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(targetPos.y, YMinValue, YMaxValue);
        }
        else if (YMinEnabled)
        {
            targetPos.y = Mathf.Clamp(target.position.y, YMinValue, target.position.y);
        }
        else if (YMaxEnabled)
        {
            targetPos.y = Mathf.Clamp(target.position.y, target.position.y, YMaxValue);
        }

        if (XMinEnabled && XMaxEnabled)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, XMinValue, XMaxValue);
        }
        else if (XMinEnabled)
        {
            targetPos.x = Mathf.Clamp(target.position.x, XMinValue, target.position.x);
        }
        else if (XMaxEnabled)
        {
            targetPos.x = Mathf.Clamp(target.position.x, target.position.x, XMaxValue);
        }

        targetPos.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothDamp);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.position, 3f);
    }

    void LateUpdate()
    {
        //transform.position = new Vector3(
        //    Mathf.Clamp(transform.position.x, minClamp.x, maxClamp.x),
        //    Mathf.Clamp(transform.position.y, minClamp.y, maxClamp.y), 
        //    transform.position.z);
    }
}
