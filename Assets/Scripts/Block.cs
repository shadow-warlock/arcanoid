using System.Linq;
using Unit;
using UnityEditor;
using UnityEngine;

public class Block : MonoBehaviour
{
    private const int MAXHp = 6;
    private int _hp;
    public Texture2D texture;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites;
    private GameObject _wizard;
    private Wizard.ManaType _type;
    private const int TouchManaCount = 10;
    private const int DestroyManaCount = 30;


    private void Start()
    {
        float rand = Random.Range(0, 3);
            _type = (Wizard.ManaType) rand;
        _hp = MAXHp;
        _wizard = GameObject.FindWithTag("Wizard");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[MAXHp - _hp];
        gameObject.GetComponent<SpriteRenderer>().color = Wizard.GetColor(_type);
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<Ball>().DamageAnimation();
            _hp--;
            _spriteRenderer.sprite = sprites[MAXHp - _hp];
            if (_hp == 0)
            {
                _wizard.GetComponent<Wizard>().AddMana(_type, DestroyManaCount);
                Destroy(gameObject);
            }
            else
            {
                _wizard.GetComponent<Wizard>().AddMana(_type, TouchManaCount);
            }
        }
    }
}
