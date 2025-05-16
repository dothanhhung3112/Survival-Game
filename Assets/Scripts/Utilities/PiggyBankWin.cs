using Cinemachine;
using Hung;
using System.Collections;
using UnityEngine;

public class PiggyBankWin : MonoBehaviour
{
    public static PiggyBankWin Instance;
    [SerializeField] CinemachineBrain brain;
    [SerializeField] Transform moneyParent;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] GameObject camPiggy;
    int moneyAmount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void Start()
    {
        StartCoroutine(SpawnMoney(50));
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(SpawnMoney(30));
        //}
    }

    public void StartSpawnMoney()
    {
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        camPiggy.SetActive(true);
        SoundManager.Instance.PlaySoundMoneyDrop();
        StartCoroutine(SpawnMoney(30));
    }

    public void ResetPiggy()
    {
        camPiggy.SetActive(false);
    }

    IEnumerator SpawnMoney(int amount)
    {
        moneyAmount = 0;
        while (moneyAmount < amount)
        {
            moneyAmount++;
            GameObject money = Instantiate(moneyPrefab, moneyParent);
            money.transform.localPosition = Vector3.zero;
            money.transform.localRotation = Quaternion.Euler(new Vector3(90 + Random.Range(-20, 20), 0, 0));
            yield return new WaitForSeconds(0.1f);
        }
        SoundManager.Instance.StopSound();
    }
}
