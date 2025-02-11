using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level_5_cam : MonoBehaviour
{

    public Transform win_cam_pos, lose_cam_pos;
    Sequence sequence;
    public Ease ease;
    public Animator anim_player, anim_enemy;

    private void Start()
    {
        anim_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        anim_enemy = GameObject.FindGameObjectWithTag("enemy").GetComponent<Animator>();
    }

    public void win_move()
    {
        Quaternion qt = win_cam_pos.transform.rotation;

        Vector3 vt = qt.eulerAngles;

        sequence = DOTween.Sequence();

        sequence

                    .Append(transform.DOMove(win_cam_pos.position, 1f).SetEase(ease))
                    .Join(transform.DORotate(vt, .5f).SetEase(ease))
                    .OnComplete(() => animate_player());

        anim_enemy.gameObject.SetActive(false);
    }

    public void lose_move()
    {
        Quaternion qt = lose_cam_pos.transform.rotation;

        Vector3 vt = qt.eulerAngles;

        sequence = DOTween.Sequence();

        sequence

                    .Append(transform.DOMove(lose_cam_pos.position, 1f).SetEase(ease))
                    .Join(transform.DORotate(vt, .5f).SetEase(ease))
                    .OnComplete(() => animate_enemy());

        anim_player.gameObject.SetActive(false);
    }

    public void animate_enemy()
    {
        //SoundManager.instance.Play("fire2");
        anim_enemy.Play("Shooting");
    }

    public void animate_player()
    {
        anim_player.Play("happy");
    }
}
