using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Player : MonoBehaviour
{
    public bool facingRight = true;
    private bool isPaused;
    private string activeScene;
    public bool isAttacking;
    public string GetActiveScene() { return activeScene; }
    public string GetNextScene()
    {

        if (GameState.firstLevel)
            return "FirstLevel";

        if (GameState.secondLevel)
            return "SecondLevel";

        return "Main_Scene";
    }

    private int currentQuest = ChangeQuestAtHome.currentQuest;
    public int GetCurrentQuest() { return currentQuest; }
    public string currentText;

    [Header("Movement")]
    public float speed;
    public bool moveOnY;
    public float horizontal;

    private Rigidbody2D rb;
    [Header("Jump Options")]
    public int jumpPower;
    public float fallMultiplier;
    [SerializeField] public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 vecGravity;
    public int doubleJump = 2;

    public SucessController sucessController;

    public UnityEngine.UI.Text currentSelectTxt;


    public List<ChangeSprite> janelas;
    public List<GameObject> buracos;

    void Start()
    {

        currentText = ChangeQuestAtHome.objetivos[currentQuest];
        activeScene = SceneManager.GetActiveScene().name;
        Debug.Log("Active scene: " + activeScene);
        vecGravity = new Vector2(0, Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();

        if (GameState.buracosConserto)
        {
            foreach (GameObject obj in buracos)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }
        if (GameState.janelasConserto)
        {
            foreach (ChangeSprite chg in janelas)
            {
                chg.consertar();
            }
        }

        if (activeScene == "House_Scene")
        {

            currentSelectTxt.text = currentText;
        }

    }

    private void advanceQuest()
    {
        ChangeQuestAtHome.currentQuest += 1;
        currentQuest = ChangeQuestAtHome.currentQuest;
        currentText = ChangeQuestAtHome.objetivos[currentQuest];
        Debug.Log(currentText);
        currentSelectTxt.text = currentText;
    }

    private void MouseAction()
    {

        if (activeScene == "House_Scene")
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("CLICKED " + hit.collider.name);

                if (hit.collider.name == "Buraco")
                {

                    Debug.Log("Buraco consertado? " + GameState.buracosConserto);

                    if (!GameState.firstLevel)
                    {
                        GameState.buracos--;
                        Destroy(hit.collider.gameObject);

                        if (GameState.buracos == 0)
                        {
                            Debug.Log("zerou"); advanceQuest();
                            GameState.buracosConserto = true;
                            //terminou a quest

                        }
                    }
                }

                else if (hit.collider.name == "Janela")
                {
                    Debug.Log("Janela consertada? " + GameState.janelasConserto);
                    if (!GameState.secondLevel)
                    {
                        hit.transform.GetComponent<ChangeSprite>().consertar();

                        GameState.janelas--;

                        if (GameState.janelas == 0)
                        {
                            Debug.Log("zerou"); advanceQuest();
                            GameState.janelasConserto = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (activeScene == "SecondLevel")
                attack();
        }
    }

    [Header("Hit Detection")]
    public GameObject hitDetectLeft;
    public GameObject hitDetectRight;
    public LayerMask enemiesLayer;
    public float boxCollisionX;
    public float boxCollisionY;
    public GameObject enemy;

    private void attack()
    {
        Debug.Log("atacou!");
        //isAttacking = true;
        animator.SetTrigger(facingRight ? AnimEnum.ATTACK_RIGHT : AnimEnum.ATTACK_LEFT);
        //animator.SetInteger("transition", facingRight ? AnimEnum.ATTACK_RIGHT : AnimEnum.ATTACK_LEFT);
        GameObject obj = facingRight ? hitDetectRight : hitDetectLeft;

        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(obj.transform.position, new Vector2(boxCollisionX, boxCollisionY), enemiesLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log(enemy.name);
            if (enemy.name != "Player")
            {
                this.enemy = enemy.gameObject;

                //Destroy(enemy.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(hitDetectLeft.transform.position, new Vector3(boxCollisionX, boxCollisionY, 0f));
        Gizmos.DrawWireCube(hitDetectRight.transform.position, new Vector3(boxCollisionX, boxCollisionY, 0f));
    }

    private void enableHitCollision()
    {

        if (activeScene == "Second_Level")
        {
            GameObject obj = facingRight ? hitDetectRight : hitDetectLeft;

            obj.GetComponent<BoxCollider2D>().gameObject.SetActive(true);
        }
    }
    private void disableHitCollision()
    {
        if (activeScene == "Second_Level")
        {
            GameObject obj = facingRight ? hitDetectRight : hitDetectLeft;

            obj.GetComponent<BoxCollider2D>().gameObject.SetActive(false);
        }
    }

    public bool playerDeath = false;
    public void deathByZombie()
    {
        Debug.Log("Player Death");
        animator.SetTrigger(AnimEnum.DEATH);
    }



    private float vertical;
    private void pause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        Debug.Log("Game paused");
    }

    public bool readyToKill;
    void Update()
    {

        if (!playerDeath)
        {

            if (enemy != null && readyToKill)
            {
                if(enemy.name =="zombie")
                Destroy(enemy);
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                pause();
            }

            if (!isPaused)
            {

                if (moveOnY)
                {
                    vertical = Input.GetAxisRaw("Vertical");
                    horizontal = Input.GetAxisRaw("Horizontal");
                }
                else
                    horizontal = Input.GetAxisRaw("Horizontal");

                if (Input.GetMouseButtonDown(0))
                {
                    MouseAction();
                }


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isGrounded())
                    {
                        doubleJump = 2;
                    }

                    if (doubleJump > 0 && activeScene != "House_Scene")
                    {
                        //Debug.Log("Pulou?!");
                        //rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                        rb.AddForce(new Vector2(0f, (isGrounded() ? jumpPower : jumpPower / 2)), ForceMode2D.Impulse);
                        doubleJump -= 1;
                    }

                }

                // aumentar velocidade da queda
                if (rb.velocity.y < 0)
                {
                    rb.velocity += vecGravity * fallMultiplier * Time.deltaTime;
                }
                //(!isAttacking)
                SetAnimation();

            }
        }
    }

    public Animator animator;
    public void SetAnimation()
    {

        if (isGrounded() || activeScene == "House_Scene")
        {
            // Debug.Log("Vertical:" + (moveOnY && vertical != 0)+" Horizontal: "+horizontal);

            if (horizontal == 0)
            {
                animator.SetInteger("transition", facingRight ? AnimEnum.IDLE_RIGHT : AnimEnum.IDLE_LEFT);
            }
            else if (horizontal == 1)
            {
                animator.SetInteger("transition", AnimEnum.WALK_RIGHT);
                facingRight = true;
            }
            else if (horizontal == -1)
            {
                facingRight = false;
                animator.SetInteger("transition", AnimEnum.WALK_LEFT);
            }

            if (moveOnY && vertical != 0)
            {
                animator.SetInteger("transition", facingRight ? AnimEnum.WALK_RIGHT : AnimEnum.WALK_LEFT);
            }
        }
        else
        {

            animator.SetInteger("transition", facingRight ? AnimEnum.JUMP_RIGHT : AnimEnum.JUMP_LEFT);
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.1f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public void gameOver()
    {
        new ScenesController().LoadScene(this.activeScene);
    }

    private void FixedUpdate()
    {
        if (!playerDeath)
        {
            if (moveOnY)
                rb.MovePosition(rb.position + new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime);
            else
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    public void addMadeira()
    {
        this.sucessController.addMadeira();
    }

}
