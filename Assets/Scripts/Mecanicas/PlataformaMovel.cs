using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlataformaMovel : MonoBehaviour
{
    public enum Eixo { Horizontal, Vertical }

    [Header("Trajeto")]
    [Tooltip("Direcao do movimento, a partir da posicao onde a plataforma foi colocada na fase.")]
    public Eixo eixo = Eixo.Horizontal;
    [Tooltip("Distancia percorrida a partir da posicao inicial, em unidades do mundo.")]
    public float distancia = 3f;
    [Tooltip("Velocidade de deslocamento (unidades por segundo).")]
    public float velocidade = 2f;
    [Tooltip("Espera parada em cada ponta do trajeto antes de voltar (segundos).")]
    public float esperaNasPontas = 0f;

    private Rigidbody2D rb;
    private Collider2D colisor;
    private Vector2 pontoA;
    private Vector2 pontoB;
    private Vector2 alvoAtual;
    private Vector2 deltaFrame;
    private float tempoEsperando;
    private readonly HashSet<Transform> passageiros = new HashSet<Transform>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        colisor = GetComponent<Collider2D>();
    }

    void Start()
    {
        pontoA = rb.position;
        Vector2 direcao = eixo == Eixo.Horizontal ? Vector2.right : Vector2.up;
        pontoB = pontoA + direcao * distancia;
        alvoAtual = pontoB;
    }

    void FixedUpdate()
    {
        Vector2 posAnterior = rb.position;

        if (tempoEsperando > 0f)
        {
            tempoEsperando -= Time.fixedDeltaTime;
            deltaFrame = Vector2.zero;
        }
        else
        {
            Vector2 novaPos = Vector2.MoveTowards(rb.position, alvoAtual, velocidade * Time.fixedDeltaTime);
            rb.MovePosition(novaPos);
            deltaFrame = novaPos - posAnterior;

            if (Vector2.Distance(novaPos, alvoAtual) < 0.01f)
            {
                alvoAtual = alvoAtual == pontoA ? pontoB : pontoA;
                tempoEsperando = esperaNasPontas;
            }
        }

        if (deltaFrame == Vector2.zero) return;

        foreach (Transform passageiro in passageiros)
        {
            if (passageiro == null) continue;
            Rigidbody2D rbPassageiro = passageiro.GetComponent<Rigidbody2D>();
            if (rbPassageiro != null)
            {
                rbPassageiro.position += deltaFrame;

                // Avisa o personagem que ele esta sobre uma superficie que mexe na velocidade
                // vertical dele via contato fisico direto (plataforma subindo/descendo), pra ele
                // nao exigir velocidade perto de zero pra liberar o pulo (ver
                // PlayerMovement.ConfirmarSuperficieInstavel). Tentar zerar a velocidade aqui no
                // FixedUpdate nao funciona: o motor de fisica resolve o contato real entre a
                // plataforma e o personagem DEPOIS do FixedUpdate rodar, entao qualquer correcao
                // feita aqui e sobrescrita antes do Update() conseguir ler.
                PlayerMovement movimento = passageiro.GetComponent<PlayerMovement>();
                if (movimento != null) movimento.ConfirmarSuperficieInstavel();
            }
            else
                passageiro.position += (Vector3)deltaFrame;
        }
    }

    void OnCollisionEnter2D(Collision2D colisao) => AvaliarPassageiro(colisao);
    void OnCollisionStay2D(Collision2D colisao) => AvaliarPassageiro(colisao);
    void OnCollisionExit2D(Collision2D colisao) => passageiros.Remove(colisao.transform);

    // So carrega quem esta em pe em cima (base do personagem no topo da plataforma),
    // nao quem apenas esbarra do lado ou por baixo.
    void AvaliarPassageiro(Collision2D colisao)
    {
        if (!colisao.collider.CompareTag("Ignis") && !colisao.collider.CompareTag("Aqua"))
        {
            passageiros.Remove(colisao.transform);
            return;
        }

        bool emCima = colisao.collider.bounds.min.y >= colisor.bounds.max.y - 0.1f;
        if (emCima)
            passageiros.Add(colisao.transform);
        else
            passageiros.Remove(colisao.transform);
    }

    void OnDrawGizmos()
    {
        Vector2 origem = Application.isPlaying ? pontoA : (Vector2)transform.position;
        Vector2 direcao = eixo == Eixo.Horizontal ? Vector2.right : Vector2.up;
        Vector2 destino = origem + direcao * distancia;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origem, destino);
        Gizmos.DrawWireSphere(origem, 0.12f);
        Gizmos.DrawWireSphere(destino, 0.12f);
    }
}
