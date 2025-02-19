using UnityEngine;

public enum glass_type
{
    true_glass,
    wrong_glass
}

public class Glass : MonoBehaviour
{
    public glass_type type;
    public bool is_active;
    public GameObject pref_glass , pref_pos_jump;
    public GameObject break_glass , pos_jumping;

    void Start()
    {

        instantiate_breakGlass();
    }

    public void instantiate_breakGlass()
    {
        //break glass
        break_glass = Instantiate(pref_glass, transform);
        Vector3 new_pos = break_glass.transform.localPosition;
        new_pos.x = 0f;
        new_pos.y = -1f;
        new_pos.z = 0f;
        break_glass.transform.localPosition = new_pos;

        // pos jumping
        pos_jumping = Instantiate(pref_pos_jump, transform);
        new_pos = pos_jumping.transform.localPosition;
        new_pos.x = 0f;
        new_pos.y = -.48f;
        new_pos.z = 0f;
        pos_jumping.transform.localPosition = new_pos;
    }
}
