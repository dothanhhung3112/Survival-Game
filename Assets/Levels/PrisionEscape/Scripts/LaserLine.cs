using UnityEngine;

namespace Hunter
{
    public class LaserLine : MonoBehaviour
    {
        public ParticleSystem laserDamage;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                laserDamage.Play();
                PrisionEscapeController.instance.Lose();
            }else if (other.CompareTag("Bot"))
            {
                laserDamage.Play();
                BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
                bot.Die();
            }
        }
    }
}
