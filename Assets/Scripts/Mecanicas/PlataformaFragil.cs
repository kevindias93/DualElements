using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlataformaFragil : MonoBehaviour
{
    [Header("Tempos")]
    [Tooltip("Tempo tremendo antes de cair, em segundos.")]
    public float atrasoParaCair = 1f;
    [Tooltip("Tempo que fica sumida antes de reaparecer, em segundos.")]
    public float tempoParaReaparecer = 4f;

    [Header("Tremor (aviso visual)")]
    public float intensidadeTremor = 0.05f;

    [Header("Queda")]
    [Tooltip("Duracao da animacao de queda (desce e desaparece).")]
    public float duracaoQueda = 0.35f;
    [Tooltip("Quanto a plataforma desce, visualmente, antes de sumir.")]
    public float distanciaQueda = 1.5f;

    private Vector3 posicaoInicial;
    private Collider2D colisor;
    private SpriteRenderer sprite;
    private Color corOriginal;
    private bool ativada;
    private Coroutine rotinaAtual;

    void Awake()
    {
        colisor = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        posicaoInicial = transform.position;
        corOriginal = sprite.color;
    }

    void OnEnable() { GameManager.AoReiniciarFase += Reiniciar; }
    void OnDisable() { GameManager.AoReiniciarFase -= Reiniciar; }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (ativada) return;
        if (!colisao.collider.CompareTag("Ignis") && !colisao.collider.CompareTag("Aqua")) return;

        bool emCima = colisao.collider.bounds.min.y >= colisor.bounds.max.y - 0.1f;
        if (!emCima) return;

        ativada = true;
        rotinaAtual = StartCoroutine(SequenciaQueda());
    }

    IEnumerator SequenciaQueda()
    {
        float tempo = 0f;
        Vector3 posBase = transform.position;
        while (tempo < atrasoParaCair)
        {
            float deslocX = Random.Range(-intensidadeTremor, intensidadeTremor);
            transform.position = posBase + new Vector3(deslocX, 0, 0);
            tempo += Time.deltaTime;
            yield return null;
        }
        transform.position = posBase;

        float t = 0f;
        Vector3 inicio = transform.position;
        Vector3 fim = inicio + Vector3.down * distanciaQueda;
        while (t < duracaoQueda)
        {
            t += Time.deltaTime;
            float progresso = t / duracaoQueda;
            transform.position = Vector3.Lerp(inicio, fim, progresso);
            Color c = corOriginal;
            c.a = Mathf.Lerp(corOriginal.a, 0f, progresso);
            sprite.color = c;
            yield return null;
        }

        colisor.enabled = false;
        sprite.enabled = false;

        yield return new WaitForSeconds(tempoParaReaparecer);

        Reaparecer();
    }

    void Reaparecer()
    {
        transform.position = posicaoInicial;
        sprite.color = corOriginal;
        colisor.enabled = true;
        sprite.enabled = true;
        ativada = false;
        rotinaAtual = null;
    }

    // Chamado tambem quando os personagens morrem e a fase reinicia, para o puzzle poder
    // ser tentado de novo mesmo que a plataforma ainda estivesse caida ou tremendo.
    void Reiniciar()
    {
        if (rotinaAtual != null)
        {
            StopCoroutine(rotinaAtual);
            rotinaAtual = null;
        }
        Reaparecer();
    }
}
