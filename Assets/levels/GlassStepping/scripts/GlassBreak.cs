using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public Rigidbody[] list_glass_parts;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        break_glasses();
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
