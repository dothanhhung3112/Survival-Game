using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using MoreMountains.NiceVibrations;

public class Level_4_Controller : MonoBehaviour
{
    public Level_4_Glass[] list_glasses;
    public LayerMask glass_layer, finish_layer;
    public List<MeshRenderer> list_true_glasses;
    public Material[] mats;
    public float tm;
    public Camera cam;
    public int actual_step;
    public bool game_run , active_finish , show_start , start_game;
    public float power_jump , duration , fall_power;

    public Vector3 mypos;

    Rigidbody rb;
    public Animator anim;
    public GameObject finish_pos;

    //camera
    public Transform my_cam , cam_pos_1 , cam_pos_finish;
    Sequence sequence;
    public Ease ease;

    Level_4_cam cam_follow;

    public GameObject Level_4_start_panel , win_panel , InGame_Panel , lose_panel;
    public ParticleSystem confetti;


    bool canClick = true;

    // Start is called before the first frame update
    void Start()
    {
        cam_follow = FindObjectOfType<Level_4_cam>();

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        mypos = transform.position;

        StartCoroutine(manage_glasses(tm));
    }

    // Update is called once per frame
    void Update()
    {
        if (!game_run || !start_game)
            return;

        if (Input.GetKeyUp(KeyCode.Z))
        {
            //transform.position = mypos;
            on_final();
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine(manage_glasses(tm));
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            get_next_step();
        }

        if (Input.GetMouseButtonDown(0))
        {
            //ray cast
            choose_glass();
        }
    }

    IEnumerator manage_glasses(float tm)
    {
        actual_step = 0;
        list_true_glasses.Clear();
        // fill all glasses
        for (int i = 0; i < list_glasses.Length; i+=2)
        {
            int rdm = Random.Range(0, 2);

            if(rdm == 0)
            {
                list_glasses[i].type = glass_type.true_glass;
                list_glasses[i+1].type = glass_type.wrong_glass;

                // add to list true glasses
                MeshRenderer ms = list_glasses[i].GetComponent<MeshRenderer>();

                list_true_glasses.Add(ms);

            }
            else
            {
                if (rdm == 1)
                {
                    list_glasses[i].type = glass_type.wrong_glass;
                    list_glasses[i + 1].type = glass_type.true_glass;

                    // add to list true glasses
                    MeshRenderer ms = list_glasses[i+1].GetComponent<MeshRenderer>();

                    list_true_glasses.Add(ms);
                }
            }
        }

        
        // make hint by animation

        for (int i = 0; i < list_true_glasses.Count; i++)
        {
            list_true_glasses[i].material = mats[2];

            if (i != 0)
            {
                list_true_glasses[i-1].material = mats[0];
            }

            yield return new WaitForSeconds(tm);
            if(i == list_true_glasses.Count - 1)
            {
                list_true_glasses[i].material = mats[0];
            }
        }

        // show all answer and hide them again
        //show
        for (int i = 0; i < list_true_glasses.Count; i++)
        {
            list_true_glasses[i].material = mats[2];
            
        }
        yield return new WaitForSeconds(1f);
        //hide
        for (int i = 0; i < list_true_glasses.Count; i++)
        {
            list_true_glasses[i].material = mats[0];

        }

        // run the game
        //game_run = true;

        

        // camera 
        cam_to_player_pos();
        // start steps 
        //get_next_step();

    }

    public void get_next_step()
    {
        if (!game_run)
            return;

        if (!show_start)
        {
            show_start = true;
            Level_4_start_panel.SetActive(true);
        }
        canClick = true;


        //active follow cam
        if (!cam_follow.is_active)
            cam_follow.start_follow();


        if (actual_step ==10)
        {
            active_finish = true;
            finish_pos.SetActive(true);

            //reset all glasses material
            for (int i = 0; i < list_glasses.Length; i++)
            {
                list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                list_glasses[i].is_active = false;
            }
        }
        else
        {
            // idle animation
            anim.Play("idle_glass");

            //reset all glasses material
            for (int i = 0; i < list_glasses.Length; i++)
            {
                list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                list_glasses[i].is_active = false;
            }

            // change materials of next step
            int step = actual_step * 2;
            int cnt = 0;
            for (int i = step; i < list_glasses.Length; i++)
            {
                list_glasses[i].GetComponent<MeshRenderer>().material = mats[1];
                list_glasses[i].is_active = true;
                cnt++;
                if (cnt >= 2)
                    break;
            }

            actual_step++;
        }
        
    }

    public void choose_glass()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray , out hit , Mathf.Infinity , glass_layer))
        {
            Level_4_Glass glass = hit.collider.GetComponent<Level_4_Glass>();
            //SoundManager.instance.Play("jumping");
            if (glass.is_active)
            {
                canClick = false;
                if (glass.type == glass_type.true_glass)
                {
                    //get_next_step();
                    //get jump position
                    Vector3 jump_pos = glass.pos_jumping.transform.position;

                    jumping(jump_pos , true);
                }
                else if (glass.type == glass_type.wrong_glass)
                {
                    //get jump position
                    Vector3 jump_pos = glass.pos_jumping.transform.position;
                    jumping(jump_pos , false,glass);

                }
            }
            
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, finish_layer))
        {
            //SoundManager.instance.Play("jumping");
            // get pos
            Vector3 my_pos = hit.collider.transform.GetChild(0).position;

            finish_pos.SetActive(false);
            //jump
            transform.DOJump(my_pos, power_jump, 1, duration).OnComplete(() => on_final());



        }
    }

    public void jumping(Vector3 vec , bool bl= false , Level_4_Glass glass_sc = null)
    {
        // jump animation
        anim.Play("jump");

        if (bl)
        {
            transform.DOJump(vec, power_jump, 1, duration).OnComplete(() => on_success());
        }
        else
        {
            transform.DOJump(vec, power_jump, 1, duration).OnComplete(() => on_player_fall(glass_sc));
        }
        
    }

    public void on_player_fall(Level_4_Glass glass_sc)
    {
        print("lose worng");
        StartCoroutine(lose_panel_wait());
        //inactive timer
        Level_4_Timer timer_script = FindObjectOfType<Level_4_Timer>();
        timer_script.active = false;

        // failure vibrate
        //MMVibrationManager.Haptic(HapticTypes.Failure, true, this);

        //reset all glasses material
        for (int i = 0; i < list_glasses.Length; i++)
        {
            list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
            list_glasses[i].is_active = false;
        }
        //SoundManager.instance.Play("glassbreak");
        glass_sc.break_glass.SetActive(true);
        glass_sc.GetComponent<MeshRenderer>().enabled = false;
        glass_sc.GetComponent<MeshCollider>().enabled = false;

        // game run

        game_run = false;

        // fall animation
        anim.Play("fall");

        //addforce
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * fall_power , ForceMode.Impulse);

    }

    public void on_final()
    {
        //play confetti
        confetti.Play();

        game_run = false;
        

        cam_follow.is_active = false;


        //animation win
        anim.Play("happy");

        Quaternion qt1 = Quaternion.Euler(0, -180, 0);

        Vector3 vt1 = qt1.eulerAngles;

        transform.DORotate(vt1, .5f).SetEase(ease);

        //cam_pos_finish

        Quaternion qt = cam_pos_finish.transform.rotation;

        Vector3 vt = qt.eulerAngles;

        sequence = DOTween.Sequence();

        sequence

                    .Append(my_cam.transform.DOMove(cam_pos_finish.position, .6f).SetEase(ease))
                    .Append(my_cam.transform.DORotate(vt, .5f).SetEase(ease))
                    .OnComplete(() => break_all_glasses_finish());


        // win panel 
        StartCoroutine(win_panel_wait());
    }

    public void cam_to_player_pos()
    {
        //game_run = false;
        

        Quaternion qt = cam_pos_1.transform.rotation;

        Vector3 vt = qt.eulerAngles;

        sequence = DOTween.Sequence();

        sequence

                    .Append(my_cam.transform.DOMove(cam_pos_1.position, 1f).SetEase(ease))
                    .Join(my_cam.transform.DORotate(vt, 1f).SetEase(ease))
                    .OnComplete(() => get_next_step());

    }

    public void on_success()
    {
        //success vibrate
        //MMVibrationManager.Haptic(HapticTypes.HeavyImpact, true, this);

        get_next_step();
    }

    
    public void break_all_glasses_finish()
    {
        Level_4_Glass[] all_glasses = FindObjectsOfType<Level_4_Glass>();

        for (int i = 0; i < all_glasses.Length; i++)
        {
            //MMVibrationManager.Haptic(HapticTypes.Failure, true, this);

            all_glasses[i].break_glass.SetActive(true);
            all_glasses[i].GetComponent<MeshRenderer>().enabled = false;
            all_glasses[i].GetComponent<MeshCollider>().enabled = false;
        }
        
    }

    public void break_all_glasses_and_player_fall_timeOut()
    {
        Level_4_Glass[] all_glasses = FindObjectsOfType<Level_4_Glass>();

        //reset all glasses material
        for (int i = 0; i < list_glasses.Length; i++)
        {
            list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
            list_glasses[i].is_active = false;
        }

        //break all glasses
        for (int i = 0; i < all_glasses.Length; i++)
        {
            //MMVibrationManager.Haptic(HapticTypes.Failure, true, this);

            all_glasses[i].break_glass.SetActive(true);
            all_glasses[i].GetComponent<MeshRenderer>().enabled = false;
            all_glasses[i].GetComponent<MeshCollider>().enabled = false;
        }

        //animation lose
        anim.Play("fall");

        //addforce to player 
        rb.isKinematic = false;
        rb.AddForce(Vector3.down * fall_power, ForceMode.Impulse);

    }


    IEnumerator win_panel_wait()
    {
        print("wiin");
        //inactive timer
        //SoundManager.instance.Play("win");
        Level_4_Timer timer_script = FindObjectOfType<Level_4_Timer>();
        timer_script.active = false;

        yield return new WaitForSeconds(4f);
        //InGame_Panel.SetActive(false);
        //BridgeController.instance.rewardedCountOnPlay++;
        //BridgeController.instance.ShowBannerCollapsible();
        //if (BridgeController.instance.rewardedCountOnPlay >= 3)
        //{
        //    if (BridgeController.instance.IsRewardReady())
        //    {
        //        BridgeController.instance.rewardedCountOnPlay = 0;
        //    }
        //    win_panel.SetActive(true);
           
        //    BridgeController.instance.ShowRewarded("win_level_5", null, null);

        //}
        //else
        //{
        //    UnityEvent e = new UnityEvent();
        //    e.AddListener(() =>
        //    {
        //        // luồng game sau khi tắt quảng cáo
        //        win_panel.SetActive(true);
        //    });
        //    BridgeController.instance.ShowInterstitial("win_level_5", e);

        //}

    }

    IEnumerator lose_panel_wait()
    {
       // SoundManager.instance.Play("lose");
        yield return new WaitForSeconds(4f);
        InGame_Panel.SetActive(false);
        //BridgeController.instance.rewardedCountOnPlay++;
        //BridgeController.instance.ShowBannerCollapsible();
        //if (BridgeController.instance.rewardedCountOnPlay >= 3)
        //{
        //    if (BridgeController.instance.IsRewardReady())
        //    {
        //        BridgeController.instance.rewardedCountOnPlay = 0;
        //    }
        //    lose_panel.SetActive(true);
           
        //    BridgeController.instance.ShowRewarded("lose_level_5", null, null);

        //}
        //else
        //{
        //    UnityEvent e = new UnityEvent();
        //    e.AddListener(() =>
        //    {
        //        // luồng game sau khi tắt quảng cáo
        //        lose_panel.SetActive(true);
        //    });
        //    BridgeController.instance.ShowInterstitial("lose_level_5", e);

        //}
        
    }
}
