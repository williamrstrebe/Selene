using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {

            new ScenesController().LoadScene(collision.GetComponent<Player>().GetNextScene());
        }
    }
}
