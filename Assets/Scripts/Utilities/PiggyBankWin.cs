using Cinemachine;
using System.Collections;
using UnityEngine;

public class PiggyBankWin : MonoBehaviour
{
    public static PiggyBankWin Instance;
    [SerializeField] Transform moneyParent;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] GameObject camPiggy;
    [SerializeField] int moneyAmount;
    [SerializeField] CinemachineBrain brain;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartSpawnMoney();
    }

    public void StartSpawnMoney()
    {
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        camPiggy.SetActive(true);
        StartCoroutine(SpawnMoney());
    }

    IEnumerator SpawnMoney()
    {
        while(moneyAmount < 15)
        {
            moneyAmount++;
            GameObject money = Instantiate(moneyPrefab, moneyParent);
            money.transform.localPosition = Vector3.zero;
            money.transform.localRotation = Quaternion.Euler(new Vector3(90 + Random.Range(-20,20),0,0));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
