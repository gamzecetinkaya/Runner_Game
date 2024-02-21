using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    Rigidbody rb;
    public float Speed = 5f;
    public float moveSpeed = 5f;
 

    bool isGrounded = true;
    bool isJumping = false;
    bool isRolling = false;
    bool isHit = false;
    
    Animator anim;
    public CharacterController PlayerController;

    float road = 0f;
    public GameObject newroad;

    public int maxCollisionCount = 3;
    private int currentCollisionCount = 0;
    public float deathHeight = -5f;

    public GameObject gameOverPanel;
    private int collisionCount = 0;
    private bool isGamePaused = false;

    [SerializeField] TMP_Text scoreText;
    public TMP_Text highScoreText;
    private float score = 0f;
    private float highScore = 0f;
    private int health = 3;
    public TMP_Text healthText;
    float currentSpeed = 0f;
    int skor;
    int coinScore = 0;
    public TMP_Text coinScoreText;
    
    void Update()
    {
        coinScoreText.text = "Coin: " + coinScore.ToString();
        //Debug.Log(score.ToString());
        moveSpeed +=Time.deltaTime/10;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0f, 0f) * Speed * Time.deltaTime;
        transform.Translate(movement);
        
        transform.Translate(moveSpeed * Vector3.forward * Time.deltaTime);

        if (transform.position.y < deathHeight)
        {
            PauseGame();
            ShowGameOverPanel();
            Vector3 my = new Vector3(horizontalInput, 0f, 0f) * Speed * Time.deltaTime;
            transform.Translate(my);

            
        }
       

        RollInput();

        if (!isGamePaused)
        {
           // currentSpeed = Mathf.Abs(GetComponent<Rigidbody>().velocity.x)*1000;
            score +=  Time.deltaTime;
            UpdateScoreUI();
        }

    }
    void ReduceHealth()
    {
        health--;
        healthText.text = "Health: " + health.ToString();

        if (health <= 0)
        {
            health = 0;
            ShowGameOverPanel();
            UpdateHealthUI();
        }
        
        
    }

    void UpdateScoreUI()
    {
        
        scoreText.text = "Score: " + Mathf.Floor (score).ToString();
        UpdateHighScoreUI();
    }

    void UpdateHighScoreUI()
    {
        if(score > highScore)
        {
            highScore = (int)score;
            
            PlayerPrefs.SetInt("highScore", ((int)highScore));
            PlayerPrefs.Save();
        }
        highScoreText.text = "High Score: " + Mathf.Round(highScore).ToString();
        
    }
    void PauseGame()
    {
        Time.timeScale = 0f; 
        isGamePaused = true;
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f; 
        isGamePaused = false;
    }
    
    private void FixedUpdate()
    {
        //JumpInput();
        if (Input.GetKeyDown(KeyCode.UpArrow) && this.gameObject.transform.position.y<3f)
        {
            
            isJumping = true;
            anim.SetBool("IsJumping", true);
            
            rb.AddForce(Vector3.up * 300f);
        }
        else
        {
            this.anim.SetBool("IsJumping", false);
        }
    }
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
        PlayerController = GetComponent<CharacterController>();
        InvokeRepeating("CreateNewObject", 5f,8f);

        //highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        UpdateHighScoreUI();
        health = 3;
        UpdateHealthUI();
        
        highScore = PlayerPrefs.GetInt("highScore");
        skor = (int)Mathf.Floor(score);

       
    }

   
    void CreateNewObject()
    {
        road -= 90;
        GameObject newroad1 = Instantiate(newroad, new Vector3(road, 3.212864f, 5.947651f), newroad.transform.rotation);
    }
    void JumpInput()
    {
       
        if (PlayerController.isGrounded && isJumping)
        {
          //  isJumping = false;
            //anim.SetBool("IsJumping", false);
          //  anim.SetBool("IsGrounded", true);
        }

    }

    private void ShowGameOverPanel()
    {
       
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
       

    }

    void UpdateHealthUI()
    {
        
        healthText.text = "Health: " + health.ToString();
    }


    void RollInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
            isRolling = true;
            anim.SetBool("IsRolling", true);
        }
        else
        {
            anim.SetBool("IsRolling", false);

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "death")
        {
            currentCollisionCount++;
            ReduceHealth();


            if (currentCollisionCount >= maxCollisionCount)
            {

                PauseGame();
                ShowGameOverPanel();


            }

            isHit = true;
            anim.SetBool("IsHit", true);




        }
        else
        {
            anim.SetBool("IsHit", false);

        }

        if (collision.gameObject.tag == "coin")
        {
            coinScore++;
        }


    }

}
