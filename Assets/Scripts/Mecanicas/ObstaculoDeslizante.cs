using UnityEngine;

// Bloco solido que desliza pra cima, baixo, esquerda ou direita quando ativado por um
// BotaoPressao ou Alavanca, liberando uma passagem bloqueada.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class ObstaculoDeslizante : MonoBehaviour, IAtivavel
{
    public enum Direcao { Cima, Baixo, Esquerda, Direita }

    [Header("Movimento ao ativar")]
    public Direcao direcao = Direcao.Cima;
    [Tooltip("Distancia que o obstaculo se desloca ao ser ativado, liberando a passagem.")]
    public float distancia = 2f;
    [Tooltip("Velocidade do deslocamento.")]
    public float velocidade = 3f;

    [Header("Comportamento")]
    [Tooltip("Marcado: volta a bloquear quando Desativar for chamado (ex: botao de pressao, que desativa ao sair de cima). Desmarcado: uma vez liberado, fica liberado ate a fase reiniciar (ex: alavanca).")]
    public bool podeFecharDeNovo = true;

    private Rigidbody2D rb;
    private Vector2 posicaoFechada;
    private Vector2 posicaoAberta;
    private Vector2 alvoAtual;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        posicaoFechada = rb.position;
        Vector2 dir = direcao switch
        {
            Direcao.Cima => Vector2.up,
            Direcao.Baixo => Vector2.down,
            Direcao.Esquerda => Vector2.left,
            _ => Vector2.right,
        };
        posicaoAberta = posicaoFechada + dir * distancia;
        alvoAtual = posicaoFechada;
    }

    void OnEnable() { GameManager.AoReiniciarFase += Reiniciar; }
    void OnDisable() { GameManager.AoReiniciarFase -= Reiniciar; }

    void FixedUpdate()
    {
        Vector2 novaPos = Vector2.MoveTowards(rb.position, alvoAtual, velocidade * Time.fixedDeltaTime);
        rb.MovePosition(novaPos);
    }

    public void Ativar()
    {
        alvoAtual = posicaoAberta;
    }

    public void Desativar()
    {
        if (podeFecharDeNovo)
            alvoAtual = posicaoFechada;
    }

    // Sempre volta a bloquear quando a fase reinicia, independente de podeFecharDeNovo,
    // para o puzzle nao ficar destravado permanentemente so porque os personagens morreram.
    void Reiniciar()
    {
        alvoAtual = posicaoFechada;
        rb.position = posicaoFechada;
    }
}
