using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public GameObject[] waypoints;
    public float speed;
    int currentWP = 0;
    int currenttrackerWP = 0;
    public float rotSpeed = 1;
    public float lookAhead = 5;
    public float lookAheadtracker = 1;
    GameObject tracker;
    public float trackerspeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
        tracker.GetComponent<MeshRenderer>().enabled = false;
    }
    void Progresstracker()
    {
        float trackerDistance = Vector3.Distance(transform.position, tracker.transform.position);
        if (trackerDistance > lookAhead) return;
        float distance = Vector3.Distance(tracker.transform.position, waypoints[currenttrackerWP].transform.position);
        if (distance < lookAheadtracker)
        {
            currenttrackerWP++;
        }
        if (currenttrackerWP >= waypoints.Length)
        {
            currenttrackerWP = 0;
        }
        tracker.transform.LookAt(waypoints[currenttrackerWP].transform);
        tracker.transform.Translate(0, 0, trackerspeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Progresstracker();
        //float distance = Vector3.Distance(transform.position, waypoints[currentWP].transform.position);
        //if(distance < lookAhead)
        //{
        //    currentWP++;
        //}
        //if(currentWP >= waypoints.Length)
        //{
        //    currentWP = 0;
        //}
        Quaternion lookWP = Quaternion.LookRotation(tracker.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookWP, rotSpeed * Time.deltaTime);
        //transform.LookAt(waypoints[currentWP].transform);
        transform.Translate(0,0, speed * Time.deltaTime);
    }
}
