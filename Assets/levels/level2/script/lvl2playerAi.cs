using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class lvl2playerAi : MonoBehaviour
{
    lvl2playerctr pc;
    public Vector3 offset;
    public float speed;
    NavMeshAgent ag;
    bool stay;
    public Image img;
    public bool die;
    public GameObject[] allplayer;
    public GameObject mainplayer;
    float dist;
    bool findit;
    bool checkdie;
    // Start is called before the first frame update
    void Start()
    {
        int bb = Random.Range(1, 6);
        GetComponent<Animator>().Play("idle" + bb.ToString());
        pc = FindObjectOfType<lvl2playerctr>();
        ag = GetComponent<NavMeshAgent>();
        img = transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>();
        allplayer = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        if(pc.GmRun && !die )//&& FindObjectOfType<UiManager>().t>=0)
        {
            
            if(mainplayer!=null)
            {
                
                if(mainplayer.GetComponent<lvl2playerAi>() == null)
                {
                    checkdie = mainplayer.GetComponent<lvl2playerctr>().die;
                }
                else
                {
                    checkdie = mainplayer.GetComponent<lvl2playerAi>().die;
                }
                ag.SetDestination(mainplayer.transform.position);

                if(!checkdie)
                {
                    if (ag.remainingDistance > 1f)
                    {
                        stay = false;
                        GetComponent<Animator>().Play("run");
                        GetComponent<Animator>().speed = 1;
                    }
                    else
                    {
                        if (!stay)
                        {
                            stay = true;
                            StartCoroutine(keppkicking());

                        }
                        transform.LookAt(mainplayer.transform);

                    }
                }
                else
                {
                    findit = false;
                    findplayer();
                }
                
            }
            else
            {
                findit = false;
                findplayer();
            }
            

            if(img.fillAmount<=0)
            {
                die = true;
                GetComponent<Animator>().Play("die1");
                Destroy(gameObject, 1f);
            }

            
        }
        
    }

    IEnumerator keppkicking()
    {
        //float a=0;
        do
        {
            int bb = Random.Range(1, 4);
            GetComponent<Animator>().Play("kick" + bb.ToString());
            if (mainplayer != null)
            {
                if (mainplayer.GetComponent<lvl2playerctr>() == null)
                {
                    mainplayer.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().fillAmount -= Random.Range(0.01f, 0.03f);
                }
                else
                {
                    mainplayer.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Image>().fillAmount -= Random.Range(0.005f, 0.025f);
                }
            }


            GetComponent<Animator>().speed = 2;
            yield return new WaitForSeconds(0.6f);
        } while (stay && !die && !checkdie); //&& FindObjectOfType<UiManager>().t >= 0);
       // GetComponent<Animator>().Play("die1");
       if(die)
       {

       }
    }


    public void findplayer()
    {
        if(!findit)
        {
            findit = true;
            allplayer = GameObject.FindGameObjectsWithTag("Player");
            dist =1000f;
            for (int i = 0; i < allplayer.Length; i++)
            {
                if (Vector3.Distance(transform.position, allplayer[i].transform.position) < dist && transform.name!= allplayer[i].transform.name)
                {
                    dist = Vector3.Distance(transform.position, allplayer[i].transform.position);
                    mainplayer = allplayer[i];
                }
            }
        }
        
    }

   
}
