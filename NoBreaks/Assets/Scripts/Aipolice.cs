using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aipolice : MonoBehaviour
{
    public GameObject target;
    public ParticleSystem dirt;
    private bool ondirt;
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

    private int direcation=1;

    void Start()
    {
        rb = sphere.GetComponent<Rigidbody>();
        dirt.Stop();
    }

    void Update()
    {
        transform.position = sphere.transform.position - new Vector3(0, height, 0);
        //speed calculation
        speed = acculartion;
        //
        float an = Vector3.Angle(gameObject.transform.forward,target.transform.forward);
        if (direcation != 0)
        {
           
            int dir = direcation;
            float amount =Mathf.InverseLerp(0,30,an);
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

        modelheightmatcher();
        drift();
        steeringfront();
        dirtcheacker();
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
            if (!ondirt)
            {
                skids[0].emitting = true;
                skids[1].emitting = true;
            }

        }
        else
        {
            skids[0].emitting = false;
            skids[1].emitting = false;
        }
        //
        if (ondirt && rb.velocity.magnitude > 10)
        {
            dirt.Play();
        }
        else
        {
            dirt.Stop();
        }

    }

    public void modelheightmatcher()
    {
        RaycastHit hiton;
        Physics.Raycast(transform.position + new Vector3(0, height, 0), Vector3.down, out hiton, 5f);
        p.transform.up = Vector3.Lerp(gameObject.transform.up, hiton.normal, Time.deltaTime * 18f);
    }

    private void steeringfront()
    {
        Debug.Log(direcation);
        //front wheels steering 
        for (int i = 0; i < 2; i++)
        {
            whells[i].transform.localEulerAngles = Vector3.Lerp(whells[i].transform.localEulerAngles, new Vector3(0, Input.GetAxis("Horizontal") * 25, 0), 1);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            direcation = 1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            direcation = -1;
        }
    }
    private void dirtcheacker()
    {
        RaycastHit hiton;
        if (Physics.Raycast(transform.position + new Vector3(0, height, 0), Vector3.down, out hiton, 5f))
        {
            if (hiton.transform.tag == "dirt")
            {
                ondirt = true;
            }
            else
            {
                ondirt = false;
            }
        }

    }
}
