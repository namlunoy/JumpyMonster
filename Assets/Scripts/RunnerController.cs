using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RunnerController : MonoBehaviour
{
    //Đối tượng quản lý game
    private GameController controller;

    //Phần âm thanh
    public AudioClip au_die;
    public AudioClip au_jump;
    public AudioClip au_click;

    //Phần dừng để uer chạm vào mới bắt đầu
    private bool isStart = false;


    //Thông tin người dùng
    private float counter = 0;
    private int score;
    private int currentHighScore;
    private int currentCoin;
    private int HightScore
    {
        get { return PlayerPrefs.GetInt(Config.HIGHSCORE, 0); }
        set { PlayerPrefs.SetInt(Config.HIGHSCORE, value); }
    }
    private int Coin
    {
        get { return PlayerPrefs.GetInt(Config.COIN, 0); }
        set { PlayerPrefs.SetInt(Config.COIN, value); }
    }

    //Kiểm tra xem có đang ở đất hay khong
    private bool isGrounded = false;
    //Kiểm tra đang nhảy hay không
    private bool isJump = false;
    private bool isDoubleJump = false;
    //Lực nhảy
    public float force;

    //
    private bool _isAlive = true;
    public bool IsAlive { get { return _isAlive; } }

    //
    private Animator animator;
    //Bộ đếm thời gian giành riêng cho việc touch
    private float touchTimer = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        Time.timeScale = 0;


        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        currentCoin = Coin;
        currentHighScore = HightScore;
        controller.Update_Coin(currentCoin);
        controller.Update_HighScore(currentHighScore);
        controller.Update_Score(0);


    }

    void OnDestroy()
    {
        Save();
    }

    //Lưu điểm và xu lại

    void Save()
    {
        Coin = currentCoin;
        if (score > currentHighScore)
            HightScore = score;
    }

    void Update()
    {
        //Chạm để bắt đầu
        if (isStart == false && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)))
        {
            isStart = true;
            Time.timeScale = 1;
            controller.batdau();
            if (animator == null)
                animator = GetComponent<Animator>();
            animator.SetTrigger("Run");
        }

        if (_isAlive)
        {
            touchTimer += Time.deltaTime;
            XuLyInput();
        }
    }

    void XuLyInput()
    {
        //Xử lý với bàn phím
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((isGrounded || !isDoubleJump) && !isJump)
                isJump = true;
        }

        //Xử lý với Touch
        if (Input.touchCount > 0 && touchTimer > 0.2f)
        {
            if ((isGrounded || !isDoubleJump) && !isJump)
                isJump = true;
        }
    }




    void FixedUpdate()
    {
        audio.volume = Config.Sound_On ? 1 : 0;
        if (_isAlive && isStart == true)
        {
            //---------------  Xử lý vụ nhảy  ---------------  
            if (isJump)
            {
                audio.clip = au_jump;
                audio.Play();

                animator.speed = 0;
                isJump = false;
                rigidbody2D.velocity = Vector2.zero;

                //Nếu nó nhảy khi đang ko ở dưới đất tức là nó đang thực hiện double jump
                if (isGrounded == false)
                    isDoubleJump = true;
                else
                    touchTimer = 0;

                rigidbody2D.AddForce(Vector3.up * force);
            }

            //---------------  Xử lý tính điểm  ---------------  
            counter += 0.2f;
            score = (int)counter;
            controller.Update_Score(score);
            if (score > currentHighScore && daKeuHighScore == false)
            {
                daKeuHighScore = true;
                controller.GetHighScore();
            }
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Star")
        {
            counter += 100;
            currentCoin++;
            Coin = currentCoin;
        }
        else if (other.tag == "Bird" || other.tag == "Obstacle")
        {
            Chet();
        }
    }

    private void Chet()
    {
        audio.clip = au_die;
        audio.Play();
        _isAlive = false;
        animator.speed = 1;
        animator.SetTrigger("Die");
        if (score > currentHighScore)
            controller.Show_Winpanel(score);
        else
            controller.Show_LossPanel(score);

        Save();

        print("Chết!");
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            if (_isAlive)
            {
                isGrounded = true;
                isDoubleJump = false;
                animator.speed = 1;
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            isGrounded = false;
    }

    private bool daKeuHighScore = false;
}
