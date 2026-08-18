using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    [Header("Pulo")]
    public float forcaPulo = 11f;
    [Tooltip("Multiplica a gravidade durante a queda, para o pulo ficar mais responsivo e menos flutuante")]
    public float multiplicadorQueda = 2.5f;

    [Header("Controles")]
    public KeyCode teclaEsquerda = KeyCode.LeftArrow;
    public KeyCode teclaDireita = KeyCode.RightArrow;
    public KeyCode teclaPulo = KeyCode.UpArrow;

    [Header("Chao")]
    public Transform checkChao;
    public LayerMask oQueEChao;

    [Header("Som")]
    public AudioClip somPulo;

    [HideInInspector]
    public bool podeControlar = true;

    private Rigidbody2D rb;
    private bool noChao;
    private PersonagemAnimado animado;
    private AudioSource audioSource;

    // Confirmado por PlataformaMovel (via ConfirmarSuperficieInstavel) enquanto o personagem esta
    // sendo carregado por uma plataforma que se move na vertical: o contato fisico real com ela
    // mexe na velocidade vertical dele o tempo todo, entao a checagem normal de "esta parado"
    // (Mathf.Abs velocity.y < 0.05f) nunca fica satisfeita nesse caso. Usamos um instante em vez
    // de um bool resetado a cada frame porque Update() roda mais vezes por segundo do que
    // FixedUpdate (onde a plataforma confirma) em telas de taxa de atualizacao alta - um bool
    // resetado toda vez faria o pulo "piscar" disponivel/indisponivel entre frames.
    private float tempoConfirmadoInstavel = -10f;
    private const float JANELA_SUPERFICIE_INSTAVEL = 0.1f;

    public void ConfirmarSuperficieInstavel()
    {
        tempoConfirmadoInstavel = Time.time;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animado = GetComponentInChildren<PersonagemAnimado>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!podeControlar)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float x = 0f;
        if (Input.GetKey(teclaEsquerda)) x = -1f;
        if (Input.GetKey(teclaDireita)) x = 1f;

        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);

        if (x != 0f)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * Mathf.Sign(x);
            transform.localScale = escala;
        }

        noChao = Physics2D.OverlapCircle(checkChao.position, 0.12f, oQueEChao);

        // So pode pular estando no chao E (parado na vertical OU numa superficie instavel -
        // evita "reabastecer" o pulo no meio do ar e sair voando ao apertar a tecla repetidas
        // vezes, exceto quando esse teste de velocidade nao faz sentido, como em cima de uma
        // plataforma que sobe/desce).
        bool superficieInstavel = Time.time - tempoConfirmadoInstavel < JANELA_SUPERFICIE_INSTAVEL;
        bool paradoNoChao = noChao && (superficieInstavel || Mathf.Abs(rb.linearVelocity.y) < 0.05f);
        if (Input.GetKeyDown(teclaPulo) && paradoNoChao)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
            if (audioSource != null && somPulo != null)
                audioSource.PlayOneShot(somPulo);
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * multiplicadorQueda * Time.deltaTime;
        }

        if (animado != null)
            animado.DefinirMovimento(x != 0f, noChao, rb.linearVelocity.y);
    }

    void OnDrawGizmos()
    {
        if (checkChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checkChao.position, 0.12f);
        }
    }
}
