using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    Vector3 rot = Vector3.zero;

    float degpersec = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rot.x = degpersec * Time.deltaTime;
        transform.Rotate(rot, Space.World);
    }
}
