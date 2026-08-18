using UnityEngine;

public class Porta : MonoBehaviour, IAtivavel
{
    [Header("Qual tag abre esta porta? (Ignis ou Aqua)")]
    public string tagDoDono = "Ignis";

    [Header("Base da porta (este objeto)")]
    public Sprite baseFechada;
    public Sprite baseAberta;

    [Header("Topo da porta (o objeto filho)")]
    public SpriteRenderer topoRenderer;
    public Sprite topoFechado;
    public Sprite topoAberto;

    [Header("Contorno colorido (identifica o dono da porta)")]
    public SpriteRenderer contornoBase;
    public SpriteRenderer contornoTopo;

    public bool ocupada = false;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (baseFechada != null) sprite.sprite = baseFechada;
        if (topoRenderer != null && topoFechado != null) topoRenderer.sprite = topoFechado;

        AtualizarContorno();
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag(tagDoDono))
        {
            if (GameManager.instance.TodosCristaisColetados())
            {
                ocupada = true;

                if (baseAberta != null) sprite.sprite = baseAberta;
                if (topoRenderer != null && topoAberto != null) topoRenderer.sprite = topoAberto;

                AtualizarContorno();

                GerenciadorFase.instancia.VerificarVitoria();
            }
            else
            {
                Debug.Log("Ainda faltam cristais!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag(tagDoDono))
        {
            ocupada = false;

            if (baseFechada != null) sprite.sprite = baseFechada;
            if (topoRenderer != null && topoFechado != null) topoRenderer.sprite = topoFechado;

            AtualizarContorno();
        }
    }

    // O contorno usa sempre o mesmo desenho da peça correspondente (base/topo),
    // so que ampliado e tingido na cor do dono, entao acompanha automaticamente
    // a troca de sprite entre porta aberta/fechada.
    void AtualizarContorno()
    {
        if (contornoBase != null) contornoBase.sprite = sprite.sprite;
        if (contornoTopo != null && topoRenderer != null) contornoTopo.sprite = topoRenderer.sprite;
    }

    // Permite abrir/fechar a porta por um BotaoPressao ou Alavanca, alem do jeito normal
    // (personagem certo + todos os cristais coletados).
    public void Ativar()
    {
        if (ocupada) return;
        ocupada = true;

        if (baseAberta != null) sprite.sprite = baseAberta;
        if (topoRenderer != null && topoAberto != null) topoRenderer.sprite = topoAberto;
        AtualizarContorno();

        GerenciadorFase.instancia.VerificarVitoria();
    }

    public void Desativar()
    {
        if (!ocupada) return;
        ocupada = false;

        if (baseFechada != null) sprite.sprite = baseFechada;
        if (topoRenderer != null && topoFechado != null) topoRenderer.sprite = topoFechado;
        AtualizarContorno();
    }
}