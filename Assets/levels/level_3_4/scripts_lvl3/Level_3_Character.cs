using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_3_Character : MonoBehaviour
{
    public bool active;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        active = true;

        anim = GetComponent<Animator>();

    }

   
}
