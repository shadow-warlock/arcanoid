using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float defaultSpeed;
    private float _speed;
    private bool _leftWallTouch = false;
    private bool _rightWallTouch = false;
    private bool _start = true;
    public GameObject hpBar;
    public int hp;
    public GameObject ballPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        _speed = defaultSpeed;
        DrawHP();
        createBall();
    }

    private void DrawHP()
    {
        for (int i = 0; i < hpBar.transform.childCount; i++)
        {
            GameObject hpItem = hpBar.transform.GetChild(i).gameObject;
            hpItem.SetActive(i + 1 <= hp);
        }
    }

    private void createBall()
    {
        Transform ballsStartContainer = transform.GetChild(0);
        GameObject ball = Instantiate(ballPrefab, ballsStartContainer.transform.position, Quaternion.identity);
        ball.transform.SetParent(ballsStartContainer.transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.A) && !_leftWallTouch)
        {
            transform.Translate(Vector2.left * (_speed * Time.deltaTime));
        } else if (Input.GetKey(KeyCode.D) && !_rightWallTouch)
        {
            transform.Translate(Vector2.right * (_speed * Time.deltaTime));
        } else if (Input.GetKey(KeyCode.W) && _start)
        {
            _start = false;
            transform.GetChild(0).GetChild(0).GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1), ForceMode2D.Impulse);
            transform.GetChild(0).GetChild(0).SetParent(GameObject.FindWithTag("BallContainer").transform);
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        DrawHP();
        if (hp <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            _start = true;
            createBall();
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
