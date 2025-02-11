using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_4_GlassBreak : MonoBehaviour
{
    public Rigidbody[] list_glass_parts;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        break_glasses();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void break_glasses()
    {
        for (int i = 0; i < list_glass_parts.Length; i++)
        {
            Vector3 shoot = new Vector3(Random.Range(-2,2) , Random.Range(-2, -5) , Random.Range(-2, 2));
            list_glass_parts[i].AddForce(shoot * speed , ForceMode.Impulse);
        }
    }
}
