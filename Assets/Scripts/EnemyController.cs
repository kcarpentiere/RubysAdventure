using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;




    
    
    // Start is called before the first frame update
   public class EnemyController : MonoBehaviour
{
    
  
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    Rigidbody2D rb;
    float timer;
    int direction = 1;
    public ParticleSystem smokeEffect;
    bool broken = true;
    Animator animator;
    public int botCount;
    public TextMeshProUGUI collected;
    
   
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
               
    }

    void Update()
    {
        if(!broken)
        {
            return;
            botCount = botCount+1;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        SetBotCount();
    }
    
    void FixedUpdate()
    {
        if(!broken)
        {
            return;
            botCount = botCount + 1;
        }
        Vector2 position = rb.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
        animator.SetFloat("Move X",0);
        animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
           animator.SetFloat("Move X", direction);
           animator.SetFloat("Move Y", 0);
        }
        
        rb.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController>();
        if (controller != null){
            controller.ChangeHealth (- 1);
        }
    }
    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        botCount = botCount + 1;
        SetBotCount();

    }
  public void SetBotCount()
  {

  collected.text = "Bots Fixed: "+ botCount;
  }
    void SceneChange()
    {
if (botCount>= 4)
{
if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level1Ruby"))
            {
          SceneManager.LoadScene(sceneName:"Level2Ruby");
            }
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level2Ruby")){
                SceneManager.LoadScene(sceneName:"Level2Ruby");
            }
}
    }
}

 

