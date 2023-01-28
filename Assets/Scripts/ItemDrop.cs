using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    public Sprite sprite;
    // Start is called before the first frame update
    void Start()
    {
        if (sprite != null)
        {
            this.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }
        else
        {
            Debug.Log("Prefab " + this.name + " sem imagem definida. Utilizando imagem padrão.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(collision);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().addMadeira();
            Destroy(gameObject);
        }
    }
}
