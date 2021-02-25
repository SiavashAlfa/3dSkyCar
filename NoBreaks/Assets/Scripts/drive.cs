using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drive : MonoBehaviour
{
    public float acculartion;
    public GameObject sphere;
    private float speed;
    private Rigidbody rb;
    private float rotate;
    public float steering;
    public int downforce;
    private float curruntspeed;
    private float curruntrotate;
    public GameObject p;
    [Range(0, 4)]
    public float height;
    [Range(0, 1f)]
    public float dritangel;
    private float curruntdrift;
    public float maxspeed;
    [Header("Whells")]
    public GameObject[] whells;
    public int rpm;
    public TrailRenderer[] skids;

    void Start()
    {
        rb = sphere.GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.position = sphere.transform.position - new Vector3(0, height, 0);
        //speed calculation
        speed = acculartion;
        //
        if (Input.GetAxis("Horizontal") != 0)
        {
            int dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(Input.GetAxis("Horizontal"));
            steer(dir, amount);
        }
        curruntspeed = Mathf.SmoothStep(curruntspeed, speed, Time.deltaTime * 12f); speed = 0f;
        curruntrotate = Mathf.Lerp(curruntrotate, rotate, Time.deltaTime * 4f); rotate = 0f;

        //wheel rotation
        for (int i = 0; i < whells.Length; i++)
        {
            whells[i].transform.Rotate(curruntspeed * rpm * Time.deltaTime, 0, 0);
        }

    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < maxspeed)
        {
            rb.AddForce(gameObject.transform.forward * curruntspeed, ForceMode.Acceleration);
        }
        //
        rb.AddForce(Vector3.down * downforce, ForceMode.Acceleration);
        //
        float amountofsteer = Mathf.InverseLerp(0, 8, rb.velocity.magnitude);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, transform.localEulerAngles.y + curruntrotate, 0), Time.deltaTime * 5f * amountofsteer);

        drift();
        steeringfront();
    }

    public void steer(int direction, float amount)
    {
        rotate = (steering * (direction * amount));
    }

    public void drift()
    {
        // drifting
        curruntdrift = Vector3.Angle(rb.velocity.normalized, gameObject.transform.forward);
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(), Time.deltaTime * curruntdrift * dritangel);
        //skids
        if (Mathf.Abs(curruntdrift) > 30)
        {
            skids[0].emitting = true;
            skids[1].emitting = true;

        }
        else
        {
            skids[0].emitting = false;
            skids[1].emitting = false;
        }

    }
    private void steeringfront()
    {
        //front wheels steering 
        for (int i = 0; i < 2; i++)
        {
            whells[i].transform.localEulerAngles = Vector3.Lerp(whells[i].transform.localEulerAngles, new Vector3(0, Input.GetAxis("Horizontal") * 25, 0), 1);
        }

    }

}
