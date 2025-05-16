using Cinemachine;
using Hung.Gameplay.GreenRedLight;
using UnityEngine;

public class GreenRedLightController : MonoBehaviour
{
    public static GreenRedLightController Instance;
    public PlayerController player;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera loseCam;
    private void Awake()
    {
        if(Instance == null) Instance = this;

        player = FindAnyObjectByType<PlayerController>();
        playerCam.m_LookAt = player.transform;
        playerCam.m_Follow = player.transform;
    }
}
