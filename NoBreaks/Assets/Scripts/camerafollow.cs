using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        Vector3 pose = target.transform.position - offset;
        pose.y = 0f;
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pose, 0.05f);
    }

}
