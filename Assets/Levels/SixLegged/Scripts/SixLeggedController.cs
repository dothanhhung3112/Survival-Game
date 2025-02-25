using UnityEngine;
using UnityEngine.AI;

public class SixLeggedController : MonoBehaviour
{
    public NavMeshAgent nav;
    NavMeshPath path;
    
    public void StartMove()
    {
        path = GetComponent<NavMeshPath>();
    }
}
