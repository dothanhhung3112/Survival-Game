using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantfood : MonoBehaviour
{
    Vector3 v;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(2, -4.5f), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
