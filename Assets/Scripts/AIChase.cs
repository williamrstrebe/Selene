using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{

    public GameObject player;
    public float speed;
    public Animator anim;
    //public float fixedY;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float playerX = player.transform.position.x;
        float enemyX = this.transform.position.x;
        bool facingRight = false;
        if (playerX > enemyX)
            facingRight = true;

            anim.SetInteger("transition", facingRight ? 1 : 0);

        //Debug.Log("Enemy X: "+this.transform.localPosition.x + " and Player X: "+player.transform.position.x);
        transform.position = Vector2.MoveTowards(this.transform.position,
            new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z), speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Does it enter?");
        if (collision.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().deathByZombie();
        }
        
    }
}
