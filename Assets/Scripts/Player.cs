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
    // Start is called before the first frame update
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

    public void deathByZombie()
    {
        Debug.Log("Player Death");
        isPaused = true;
        Time.timeScale = 0;
    }

    private float vertical;
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            Debug.Log("Game paused");
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

            SetAnimation();

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
                animator.SetInteger("transition", facingRight ? 0 : 1);
            }
            else if (horizontal == 1)
            {
                animator.SetInteger("transition", 2);
                facingRight = true;
            }
            else if (horizontal == -1)
            {
                facingRight = false;
                animator.SetInteger("transition", 3);
            }

            if (moveOnY && vertical != 0)
            {
                animator.SetInteger("transition", facingRight ? 2 : 3);
            }
        }
        else
        {

            animator.SetInteger("transition", facingRight ? 4 : 5);
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.1f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private void FixedUpdate()
    {
        if (moveOnY)
            rb.MovePosition(rb.position + new Vector2(horizontal, vertical) * speed * Time.fixedDeltaTime);
        else
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);



    }

    public void addMadeira()
    {
        this.sucessController.addMadeira();
    }

}
