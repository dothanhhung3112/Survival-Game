using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_3_DeadZone : MonoBehaviour
{
    Level_3_Conroller control_script;

    private void Start()
    {
        control_script = FindObjectOfType<Level_3_Conroller>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "lvl3_pl" && control_script.game_run)
        {
            if (other.GetComponent<Level_3_Character>().active)
            {
                other.GetComponent<Level_3_Character>().active = false;
                other.GetComponent<Level_3_Character>().anim.Play("fall");
                control_script.players.Remove(other.gameObject);
                other.GetComponent<Rigidbody>().isKinematic = false;

                control_script.rope_back_speed -= .25f;
                control_script.rope_forward_speed += .25f;

                Destroy(other.gameObject, 4f);
                control_script.check_lose();
            }

        }
        else if(other.tag == "lvl3_enm" && control_script.game_run)
        {

            if (other.GetComponent<Level_3_Character>().active)
            {
                other.GetComponent<Level_3_Character>().active = false;
                other.GetComponent<Level_3_Character>().anim.Play("fall");
                control_script.enemies.Remove(other.gameObject);
                other.GetComponent<Rigidbody>().isKinematic = false;

                control_script.rope_back_speed += .25f;
                control_script.rope_forward_speed -= .25f;

                Destroy(other.gameObject, 4f);

                control_script.check_win();
            }
                
        }
    }
}
