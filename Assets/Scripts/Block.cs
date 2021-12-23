using System;
using Unit;
using UnitUI;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    private const int MAXHp = 6;
    private int _hp;
    private SpriteRenderer _spriteRenderer;
    public Sprite[] sprites;
    private Wizard _wizard;
    private ManaType? _type;
    private const int TouchManaCount = 15;
    private const int DestroyManaCount = 30;
    private const float EmptyChance = 0.2f;
    [SerializeField]
    private GameObject _particlePrefab;


    private void Start()
    {
        float emptyRand = Random.Range(0.0f, 1.0f);
        if (emptyRand <= EmptyChance)
        {
            _type = null;
        }
        else
        {
            float rand = Random.Range(0, 3);
            _type = (ManaType) rand;
        }

        _hp = MAXHp;
        _wizard = GameObject.FindWithTag("Wizard").GetComponent<Wizard>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[MAXHp - _hp];
        gameObject.GetComponent<SpriteRenderer>().color = WizardUIListener.GetColor(_type);
    }   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<Ball>().DamageAnimation();
            GameObject particle = Instantiate(_particlePrefab, transform.position, transform.rotation, transform.parent);
            ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = WizardUIListener.GetColor(_type);
            _hp--;
            _spriteRenderer.sprite = sprites[Math.Max(MAXHp - _hp, 0)];
            if (_type != null)
            {
                _wizard.AddMana((ManaType) _type, _hp == 0 ? DestroyManaCount : TouchManaCount);
            }

            if (_hp != 0) return;
            if (_type != null)
            {
                _wizard.GainExp(1);
            }
            Destroy(gameObject);
        }
    }
}
