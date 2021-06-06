using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float defaultSpeed;
    private float _speed;
    private bool _leftWallTouch = false;
    private bool _rightWallTouch = false;
    private bool _magnet = false;
    public GameObject hpBar;
    public int hp;
    public GameObject ballPrefab;
    private GameObject _gameController;

    // Start is called before the first frame update
    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController");
        _speed = defaultSpeed;
        DrawHp();
        CreateBall();
    }

    private void DrawHp()
    {
        hpBar.transform.GetChild(0).GetComponent<Text>().text = hp.ToString();
    }

    private void CreateBall()
    {
        Transform ballsStartContainer = transform.GetChild(0);
        GameObject ball = Instantiate(ballPrefab, ballsStartContainer.position, Quaternion.identity);
        ball.transform.SetParent(ballsStartContainer);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && !_leftWallTouch)
        {
            transform.Translate(Vector2.left * (_speed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.D) && !_rightWallTouch)
        {
            transform.Translate(Vector2.right * (_speed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.W))
        {
            foreach (Transform ball in transform.GetChild(0))
            {
                ball.GetComponent<Ball>().Pulse();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            if (_magnet)
            {
                _magnet = false;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
            else
            {
                _magnet = true;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 1);
            }


        }
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
        DrawHp();
        if (hp <= 0)
        {
            _gameController.GetComponent<GameController>().GameOver("Закончились светлячки");
        }
        else
        {
            CreateBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool leftCollision = collision.gameObject.CompareTag("LeftWall");
        bool rightCollision = collision.gameObject.CompareTag("RightWall");
        if (leftCollision)
        {
            _leftWallTouch = true;
        }

        if (rightCollision)
        {
            _rightWallTouch = true;
        }

        bool ballCollision = collision.gameObject.CompareTag("Ball");
        if (ballCollision && _magnet)
        {
            collision.gameObject.GetComponent<Ball>().Magnet(transform.GetChild(0));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        bool leftCollision = collision.gameObject.CompareTag("LeftWall");
        bool rightCollision = collision.gameObject.CompareTag("RightWall");
        if (leftCollision)
        {
            _leftWallTouch = false;
        }

        if (rightCollision)
        {
            _rightWallTouch = false;
        }
    }
}