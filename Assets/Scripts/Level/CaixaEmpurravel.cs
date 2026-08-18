using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class CaixaEmpurravel : MonoBehaviour
{
    [Header("Peso da caixa")]
    [Tooltip("Massa da caixa. Mais alta = mais dificil de empurrar (trava). Mais baixa = sai voando ao ser tocada.")]
    public float massa = 4f;

    [Tooltip("Reduz o deslizamento: quanto maior, mais rapido a caixa para depois que o personagem para de empurrar.")]
    public float amortecimentoLinear = 0.5f;

    [Tooltip("Multiplicador de gravidade da caixa (mesmo padrao usado pelos personagens: 3).")]
    public float escalaGravidade = 3f;

    [Header("Atrito / quique (opcional)")]
    [Tooltip("Physics Material 2D para ajustar atrito e quique da caixa. Deixe vazio para usar o padrao do projeto. Sugestao: atrito por volta de 0.6-0.8 e quique 0 para um empurrao natural.")]
    public PhysicsMaterial2D materialFisico;

    private Rigidbody2D rb;
    private Vector3 posicaoInicial;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        AplicarConfiguracao();
    }

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void OnEnable() { GameManager.AoReiniciarFase += Reiniciar; }
    void OnDisable() { GameManager.AoReiniciarFase -= Reiniciar; }

    // Chamado quando os personagens morrem e a fase reinicia: a caixa volta pra onde comecou,
    // igual os cristais, em vez de ficar largada onde foi empurrada.
    void Reiniciar()
    {
        transform.position = posicaoInicial;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    // Aplica os valores do Inspector tambem no Editor (fora do Play Mode), para o ajuste ficar visivel na hora.
    void OnValidate()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        AplicarConfiguracao();
    }

    void AplicarConfiguracao()
    {
        if (rb == null) return;

        rb.mass = massa;
        rb.linearDamping = amortecimentoLinear;
        rb.gravityScale = escalaGravidade;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.sharedMaterial = materialFisico;
    }
}
