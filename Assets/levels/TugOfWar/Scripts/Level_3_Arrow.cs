using UnityEngine;
using UnityEngine.UI;

namespace Hung.Gameplay.TugOfWar
{
    public class Level_3_Arrow : MonoBehaviour
    {

        public float arrow_back_speed, arrow_forward_speed;
        public float max_dist_arrow, min_dist_arrow;
        public float dist_between_arrows;
        public Transform pos_min_arrow;
        public Image filled_bg;
        public RectTransform target;
        public Color[] clrs;
        public RectTransform[] list_target_positions;
        public Image red_bg, white_bg;
        public int actual_pos;
        public float timer, max_time, speed_target;
        public bool can_move;
        Level_3_Conroller control_script;

        // Start is called before the first frame update
        void Start()
        {
            control_script = FindObjectOfType<Level_3_Conroller>();
        }

        // Update is called once per frame
        void Update()
        {
            float my_x_pos = transform.localPosition.x;

            if (Input.GetMouseButtonDown(0))
            {
                //SoundManager.instance.Play("tugclick");
                float inc = arrow_forward_speed * Time.deltaTime;

                Vector3 tmp = transform.localPosition;
                tmp.x += inc;

                tmp.x = Mathf.Clamp(tmp.x, min_dist_arrow, max_dist_arrow);

                transform.localPosition = tmp;
            }
            else
            {
                // decrease on x arrow
                decrease_on_x_arrow();
            }


            // filled the bar
            filled_bar();
            //check arrow if is in centre
            check_between_arrows();
            // change target everytime
            change_target_position();

            //if (my_x_pos <= min_dist_arrow)
            //{
            //    Vector3 tmp = transform.localPosition;
            //    tmp.x = min_dist_arrow;
            //    transform.localPosition = tmp;

            //}
            //else if(my_x_pos >= max_dist_arrow)
            //{
            //    Vector3 tmp = transform.localPosition;
            //    tmp.x = max_dist_arrow;
            //    transform.localPosition = tmp;
            //}



        }

        public void decrease_on_x_arrow()
        {
            float inc = arrow_back_speed * Time.deltaTime;

            Vector3 tmp = transform.localPosition;
            tmp.x -= inc;

            tmp.x = Mathf.Clamp(tmp.x, min_dist_arrow, max_dist_arrow);

            transform.localPosition = tmp;
        }

        public void filled_bar()
        {
            Vector2 posA = pos_min_arrow.GetComponent<RectTransform>().localPosition;
            Vector2 posB = GetComponent<RectTransform>().localPosition;
            float distance = Vector2.Distance(posA, posB);

            float percent = distance / 500;

            filled_bg.fillAmount = percent;

        }

        public void check_between_arrows()
        {
            Vector2 target_vr = target.GetComponent<RectTransform>().localPosition;
            Vector2 my_pos = GetComponent<RectTransform>().localPosition;

            if (my_pos.x >= (target_vr.x - dist_between_arrows) && my_pos.x <= (target_vr.x + dist_between_arrows))
            {
                red_bg.gameObject.SetActive(false);
                white_bg.gameObject.SetActive(true);

                filled_bg.color = clrs[1];
                control_script.can_pull = true;
            }
            else
            {
                red_bg.gameObject.SetActive(true);
                white_bg.gameObject.SetActive(false);

                filled_bg.color = clrs[0];
                control_script.can_pull = false;
            }
        }

        public void change_target_position()
        {
            Vector2 target_vr = list_target_positions[actual_pos].GetComponent<RectTransform>().localPosition;
            Vector2 my_pos = target.GetComponent<RectTransform>().localPosition;

            if (timer >= max_time)
            {
                can_move = true;
                timer = 0f;
                actual_pos++;
                if (actual_pos >= list_target_positions.Length - 1)
                {
                    actual_pos = 0;
                }
            }
            else
            {
                timer += Time.deltaTime;

                if (Vector2.Distance(target_vr, my_pos) <= .02f && can_move)
                {
                    can_move = false;
                    target.GetComponent<RectTransform>().localPosition = target_vr;
                }
                else if (can_move)
                {
                    my_pos = Vector2.MoveTowards(my_pos, target_vr, speed_target * Time.deltaTime);
                    target.GetComponent<RectTransform>().localPosition = my_pos;
                }
            }
        }
    }
}
