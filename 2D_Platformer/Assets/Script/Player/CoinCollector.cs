using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            AudioManager.instance.PlaySFX("Collect_Coin");
            FindObjectOfType<GameManager>().AddScore(200);
            FindObjectOfType<GameManager>().AddCoin();
            Destroy(collision.gameObject);
        }
    }
}
