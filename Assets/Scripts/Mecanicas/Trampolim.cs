using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Trampolim : MonoBehaviour
{
    [Header("Impulso")]
    [Tooltip("Velocidade vertical aplicada ao personagem (maior que um pulo normal).")]
    public float forcaImpulso = 18f;

    [Header("Sprites (feedback visual)")]
    public Sprite spriteNormal;
    public Sprite spriteAcionado;
    public float duracaoAnimacao = 0.15f;

    private Collider2D colisor;
    private SpriteRenderer sprite;
    private bool animando;

    void Awake()
    {
        colisor = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (sprite != null && spriteNormal != null) sprite.sprite = spriteNormal;
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (!colisao.collider.CompareTag("Ignis") && !colisao.collider.CompareTag("Aqua")) return;

        Rigidbody2D rbJogador = colisao.collider.attachedRigidbody;
        if (rbJogador == null) return;

        // So ativa quem esta pousando em cima, descendo (evita ativar ao encostar de lado ou por baixo,
        // e evita reativar no meio do impulso, ja que a velocidade positiva alta desqualifica esta checagem).
        if (rbJogador.linearVelocity.y > 0.5f) return;
        bool emCima = colisao.collider.bounds.min.y >= colisor.bounds.max.y - 0.15f;
        if (!emCima) return;

        rbJogador.linearVelocity = new Vector2(rbJogador.linearVelocity.x, forcaImpulso);

        if (!animando)
            StartCoroutine(Animar());
    }

    IEnumerator Animar()
    {
        animando = true;
        if (sprite != null && spriteAcionado != null) sprite.sprite = spriteAcionado;
        yield return new WaitForSeconds(duracaoAnimacao);
        if (sprite != null && spriteNormal != null) sprite.sprite = spriteNormal;
        animando = false;
    }
}
