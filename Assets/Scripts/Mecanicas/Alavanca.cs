using UnityEngine;
using System.Collections;

// Alavanca fisica e empurravel: comeca encostada a esquerda (solta) e o personagem precisa
// empurra-la fisicamente (como uma caixa, mas presa a um trilho horizontal) ate o fim do curso,
// a direita, para ativar. Uma vez ativada, trava no lugar (nao volta ao ser empurrada de novo).
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Alavanca : MonoBehaviour
{
    [Header("Sprites (posicao da alavanca)")]
    public Sprite spriteEsquerda;
    public Sprite spriteCentro;
    public Sprite spriteDireita;

    [Header("Curso")]
    [Tooltip("Distancia que a alavanca percorre da posicao inicial (esquerda, solta) ate ativar (direita).")]
    public float cursoTotal = 0.5f;

    [Header("Peso (resistencia ao empurrao)")]
    public float massa = 3f;
    public float amortecimentoLinear = 2f;

    [Header("Alvos ativados quando a alavanca chega no fim do curso")]
    [Tooltip("Arraste aqui qualquer componente que implemente IAtivavel (ex: ObjetoAtivavel, ObstaculoDeslizante, Porta). Componentes que nao implementam a interface sao ignorados (aviso no Console).")]
    public MonoBehaviour[] alvos;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float posicaoInicialX;
    private bool ativada;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        AplicarFisica();
    }

    void Start()
    {
        posicaoInicialX = rb.position.x;
        AtualizarSprite(0f);
    }

    void OnEnable() { GameManager.AoReiniciarFase += Reiniciar; }
    void OnDisable() { GameManager.AoReiniciarFase -= Reiniciar; }

    void FixedUpdate()
    {
        if (ativada) return;

        // trava o curso: nao deixa a alavanca ser empurrada alem da direita (ativa) nem da esquerda (solta)
        float xLimitado = Mathf.Clamp(rb.position.x, posicaoInicialX, posicaoInicialX + cursoTotal);
        if (!Mathf.Approximately(xLimitado, rb.position.x))
        {
            rb.position = new Vector2(xLimitado, rb.position.y);
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        float progresso = Mathf.Clamp01((rb.position.x - posicaoInicialX) / cursoTotal);
        AtualizarSprite(progresso);

        if (progresso >= 0.999f)
            Ativar();
    }

    void AtualizarSprite(float progresso)
    {
        if (sprite == null) return;
        Sprite alvoSprite;
        if (progresso < 0.15f) alvoSprite = spriteEsquerda;
        else if (progresso > 0.85f) alvoSprite = spriteDireita;
        else alvoSprite = spriteCentro;
        if (alvoSprite != null) sprite.sprite = alvoSprite;
    }

    void Ativar()
    {
        ativada = true;
        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        AtualizarSprite(1f);
        StartCoroutine(Sinalizar());

        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo is IAtivavel ativavel) ativavel.Ativar();
        }
    }

    IEnumerator Sinalizar()
    {
        Color original = sprite.color;
        sprite.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        sprite.color = original;
    }

    // Chamado quando os personagens morrem e a fase reinicia: a alavanca destrava e volta pra
    // posicao inicial (solta), e os alvos sao desativados de novo, igual o resto do puzzle.
    void Reiniciar()
    {
        StopAllCoroutines();
        ativada = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        rb.position = new Vector2(posicaoInicialX, rb.position.y);
        rb.linearVelocity = Vector2.zero;
        if (sprite != null) sprite.color = Color.white;
        AtualizarSprite(0f);

        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo is IAtivavel ativavel) ativavel.Desativar();
        }
    }

    void AplicarFisica()
    {
        rb.gravityScale = 0f;
        rb.mass = massa;
        rb.linearDamping = amortecimentoLinear;
        if (!ativada)
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    void OnValidate()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb != null) AplicarFisica();

        if (alvos == null) return;
        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo != null && !(alvo is IAtivavel))
                Debug.LogWarning($"[Alavanca] '{alvo.name}' foi arrastado em Alvos mas nao implementa IAtivavel. Ele nao sera ativado.", this);
        }
    }
}
