using System;
using UnityEngine;
using UnityEngine.UI;

public class Platform : MonoBehaviour
{
    public float defaultSpeed;
    private float _speed;
    private bool _leftWallTouch = false;
    private bool _rightWallTouch = false;
    public GameObject hpBar;
    protected int hp = 3;
    public GameObject ballPrefab;
    public GameObject ballContainer;
    private GameObject _gameController;

    private Vector2? moveDirection;

    private IController controller;

    public int Hp => hp + ShopStore.GetInstance().GetProductCount(ShopStore.Product.Balls);

    // Start is called before the first frame update
    private void Start()
    {
#if UNITY_ANDROID
        controller = new Touch2Controller();
#else
        controller = new TouchController();
#endif
        _gameController = GameObject.FindWithTag("GameController");
        _speed = defaultSpeed;
        DrawHp();
        CreateBall();
    }

    private void DrawHp()
    {
        hpBar.transform.GetChild(0).GetComponent<Text>().text = Hp.ToString();
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
        Vector2 direction = controller.Update(this);
        if (direction == Vector2.up)
        {
            foreach (Transform ball in transform.GetChild(0))
            {
                ball.GetComponent<Ball>().Pulse();
            }
            foreach (Transform ball in ballContainer.transform)
            {
                ball.GetComponent<Ball>().Haste();
            }
        }
        if (direction == Vector2.down)
        {
            foreach (Transform ball in ballContainer.transform)
            {
                ball.GetComponent<Ball>().Slow();
            }
        }
        if (direction.x < - 0.1f && !_leftWallTouch)
        {
            transform.Translate(new Vector3(Math.Abs(direction.x) > _speed * Time.deltaTime ? -_speed * Time.deltaTime : direction.x, 0) );
        }
        else if (direction.x > 0.1f && !_rightWallTouch)
        {
            transform.Translate(new Vector3(Math.Abs(direction.x) > _speed * Time.deltaTime ? _speed * Time.deltaTime : direction.x, 0) );
        }
    }


    public void TakeDamage(int damage)
    {
        hp -= damage;
        DrawHp();
        if (Hp <= 0)
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