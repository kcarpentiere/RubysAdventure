using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;



public class RubyController : MonoBehaviour
{

    public ParticleSystem Hit;
    public float speed = 3.0f;
    public int maxHealth = 5;
    public GameObject projectilePrefab;
    public int health { get { return currentHealth; } }
    public float timeInvincible = 2.0f;
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rb;
    float horizontal;
    float vertical;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public AudioClip throwSound;
    public AudioClip hitSound;
    AudioSource audioSource;
    public ParticleSystem PowerUp;
    public bool power;
    public TextMeshProUGUI cogCount;
    public bool scene;
    public int Cogs;
    public int maxCogs = 10;
    public int currentCogs;
    int minCogs = 0;
    public GameObject resourceCubePrefab;

    public ParticleSystem Open;
    public int currentBots;
    public int maxBots = 5;
    public int minBots = 0;

    public bool disabled;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        power = false;
        currentCogs = maxCogs;
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();

        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
            updateCogs(-1);


        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                if (character != null)
                {
                    character.DisplayDialog();
                }
                else
                {
                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2Ruby"))
                    { SceneManager.LoadScene(sceneName: "Level2Ruby"); }


                }

            }
        }
    }

    void FixedUpdate()
    {

        Vector2 position = rb.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rb.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            Hit.Play();


            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            if (power == true)
            {
                PowerUp.Play();
            }

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        if (currentHealth <= 0)
        {
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1Ruby"))
            {
                SceneManager.LoadScene(sceneName: "Level1Ruby");
            }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2Ruby"))
            {
                SceneManager.LoadScene(sceneName: "Level2Ruby");
            }
        }
        if (power == true)
        {
            PowerUp.Play();
        }

    }
    void Launch()
    {
        if (currentCogs >= 1)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);
            animator.SetTrigger("Launch");

        }

    }


    void SceneChange()
    {
        if (scene == true)
        {
            SceneManager.LoadScene("Level2Ruby");
        }

    }
    public void cogTextUpdate()
    {
        cogCount.text = currentCogs.ToString();
    }
    public void updateCogs(int amount)
    {
        cogTextUpdate();

        if (currentCogs <= minCogs)
        {
            cogCount.text = "Reload";

        }
        else
        {
            cogTextUpdate();
        }
        currentCogs = Mathf.Clamp(currentCogs + amount, 0, maxCogs);


    }

    void OnCollisionEnter2D(Collision2D other)
    {

        ResourceController b = other.collider.GetComponent<ResourceController>();
        if (b != null)
        {
            Open.Play();
            b.GiveResource();
        }
    }


}
