using UnityEngine;

namespace Hung.Gameplay.TugOfWar
{

    public class TugOfWarDeadZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "lvl3_pl" && TugOfWarController.Instance.runnedGame)
            {
                if (other.GetComponent<TugOfWarCharacter>().active)
                {
                    other.GetComponent<TugOfWarCharacter>().active = false;
                    other.GetComponent<TugOfWarCharacter>().canFall = true;
                    TugOfWarController.Instance.players.Remove(other.gameObject);
                    other.GetComponent<Rigidbody>().isKinematic = false;

                    TugOfWarController.Instance.ropeBackSpeed -= .25f;
                    TugOfWarController.Instance.ropeForwardSpeed += .25f;

                    Destroy(other.gameObject, 4f);
                    TugOfWarController.Instance.check_lose();
                }

            }
            else if (other.tag == "lvl3_enm" && TugOfWarController.Instance.runnedGame)
            {

                if (other.GetComponent<TugOfWarCharacter>().active)
                {
                    other.GetComponent<TugOfWarCharacter>().active = false;
                    other.GetComponent<TugOfWarCharacter>().canFall = true;
                    TugOfWarController.Instance.enemies.Remove(other.gameObject);
                    other.GetComponent<Rigidbody>().isKinematic = false;

                    TugOfWarController.Instance.ropeBackSpeed += .25f;
                    TugOfWarController.Instance.ropeForwardSpeed -= .25f;

                    Destroy(other.gameObject, 4f);

                    TugOfWarController.Instance.check_win();
                }

            }
        }
    }
}
