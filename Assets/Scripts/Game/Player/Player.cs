using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float heightJump = 2f;
    public float JumpSmooth = 2f;
    public float GravityScale = 5f;
    public bool IsJumping = false;
    public bool isMoving = true;
    public Vector3 m;
    public bool Grounded = true;
    private CharacterController C;
    public int hp = 1000;
    public int armor = 0;
    public GameObject gameOverscreen;
    public Collider Sword;
    public float rotationSpeed = 5f;
    public Animator anim;
    public float AttackTimer;
    public float JumpAnimTime = 0f;
    public float HitTime = 0;
    public int Coin = 2000;
    public bool InventoryOpen = false;
    public UIManager UI;
    public GameManager GM;
    public float Weight;
    public GameObject LootBox = null;
    public Dialoguemanager DM;
    public int Armor
    {
        get
        {
            return armor;
        }
        set
        {
            armor = value;
            UI.ChangeArmor(armor);
        }
    }
    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (hp < value)
            {
                if (value < 100)
                {
                    hp = value;
                }
                else
                {
                    hp = 100;
                }
                UI.healthTxT.text = hp.ToString();
                UI.hpSlider.value = (float)hp / 100;
                return;
            }
            int blockedDamage = armor / 5;
            int ActHP = value + blockedDamage;
            if (ActHP >= 0)
            {
                hp = ActHP;
                UI.healthTxT.text = hp.ToString();
                UI.hpSlider.value = (float)hp / 100;
            }
            else
            {
                return;
            }
        }
    }
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        UI = GameObject.Find("Hud").GetComponent<UIManager>();
        anim = GetComponent<Animator>();
        C = GetComponent<CharacterController>();
        Grounded = C.isGrounded;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameOverscreen.SetActive(false);
        Sword.enabled = false;
        DM = GameObject.Find("Player").GetComponent<Dialoguemanager>();
    }
    void Update()
    {
        if (HitTime - Time.time < -1f)
        {
            anim.SetBool("GetHit", false);
            HitTime = 0;
        }
        if (Input.GetMouseButtonDown(0) && !InventoryOpen && anim.GetBool("Attack") == false)
        {
            AttackTimer = Time.time;
            anim.SetBool("Attack", true);
            isMoving = false;
            Sword.enabled = true;
        }
        if (Time.time - AttackTimer > 1.25f)
        {
            anim.SetBool("Attack", false);
            isMoving = true;
            Sword.enabled = false;
        }
        if (Grounded && m.y < 0)
        {
            m.y = 0;
        }
        if (isMoving)
        {
            Move();
        }
        else
        {
            m.x = 0;
            m.z = 0;
        }
        if (Time.time - JumpAnimTime > 0.2f)
        {
            anim.SetBool("Jump", false);
        }
        m.y += Physics.gravity.y * Time.deltaTime * GravityScale;
        C.Move(m * Time.deltaTime);
        Grounded = C.isGrounded;
        Rotate();
    }
    public void Move()
    {
        float CurrentHeight = m.y;
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        m = transform.forward * verticalInput + transform.right * horizontalInput;
        m.Normalize();
        m *= moveSpeed;
        m.y = CurrentHeight;
        if (Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            m.y = heightJump;
            anim.SetBool("Jump", true);
            JumpAnimTime = Time.time;
        }
        if (verticalInput != 0 || horizontalInput != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        else
        {
            anim.SetBool("IsRunning", false);
        }
    }
    public void Rotate()
    {
        transform.Rotate(Vector3.up * Info.Sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
    }

    public GameObject FindLootBox()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 20f);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.name.Contains("Trash"))
            {
                return cols[i].gameObject;
            }
        }
        return null;
    }
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            anim.SetBool("GetHit", true);
            HitTime = Time.time;
            if (!col.gameObject.GetComponent<Enemy>())
            {
                Hp -= col.gameObject.GetComponent<EnemyDamage>().Damage;
            }
            else
            {
                Hp -= col.gameObject.GetComponent<Enemy>().damage;

            }
        }
        if (Hp <= 0 && GM.PlayerDead == false)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Attack", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("GetHit", false);
            anim.SetBool("Death", true);
            gameOverscreen.SetActive(true);
            GM.PlayerDead = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public IEnumerator Immobilized()
    {
        isMoving = false;
        yield return new WaitForSeconds(5f);
        isMoving = true;
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Trash")
        {
            LootBox = col.gameObject;
            UI.Interact.enabled = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Trash")
        {
            LootBox = null;
            UI.Interact.enabled = false;
        }
    }

}
