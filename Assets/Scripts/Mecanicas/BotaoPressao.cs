using UnityEngine;
using System.Collections.Generic;

// Botao de pressao SOLIDO: o personagem fica em pe sobre ele (nao e so uma zona de gatilho),
// igual um degrau. Detecta peso por colisao (mesmo criterio de "em cima" usado nas plataformas),
// nao por trigger, para o personagem realmente parar sobre o botao em vez de atravessar.
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BotaoPressao : MonoBehaviour
{
    [Header("Sprites (feedback visual)")]
    public Sprite spriteSolto;
    public Sprite spritePressionado;

    [Header("Alvos ativados enquanto o botao estiver pressionado")]
    [Tooltip("Arraste aqui qualquer componente que implemente IAtivavel (ex: ObjetoAtivavel, ObstaculoDeslizante, Porta). Componentes que nao implementam a interface sao ignorados (aviso no Console).")]
    public MonoBehaviour[] alvos;

    private SpriteRenderer sprite;
    private BoxCollider2D colisor;
    private readonly HashSet<Collider2D> pesosEmCima = new HashSet<Collider2D>();

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        colisor = GetComponent<BoxCollider2D>();
        AjustarColisorFixo();
    }

    void Start()
    {
        AtualizarSprite();
    }

    void OnCollisionEnter2D(Collision2D colisao) => AvaliarPeso(colisao.collider);
    void OnCollisionStay2D(Collision2D colisao) => AvaliarPeso(colisao.collider);

    void OnCollisionExit2D(Collision2D colisao)
    {
        bool tinhaPeso = pesosEmCima.Count > 0;
        pesosEmCima.Remove(colisao.collider);
        if (tinhaPeso && pesosEmCima.Count == 0) Desativar();
    }

    void AvaliarPeso(Collider2D outro)
    {
        if (!EhPeso(outro))
        {
            pesosEmCima.Remove(outro);
            return;
        }

        bool tinhaPeso = pesosEmCima.Count > 0;
        bool emCima = outro.bounds.min.y >= colisor.bounds.max.y - 0.1f;

        if (emCima)
        {
            pesosEmCima.Add(outro);
            if (!tinhaPeso) Ativar();
        }
        else
        {
            pesosEmCima.Remove(outro);
            if (tinhaPeso && pesosEmCima.Count == 0) Desativar();
        }
    }

    bool EhPeso(Collider2D outro)
    {
        return outro.CompareTag("Ignis") || outro.CompareTag("Aqua") || outro.GetComponent<CaixaEmpurravel>() != null;
    }

    void Ativar()
    {
        AtualizarSprite();
        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo is IAtivavel ativavel) ativavel.Ativar();
        }
    }

    void Desativar()
    {
        AtualizarSprite();
        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo is IAtivavel ativavel) ativavel.Desativar();
        }
    }

    void AtualizarSprite()
    {
        if (sprite == null) return;
        bool pressionado = pesosEmCima.Count > 0;
        Sprite alvo = pressionado ? spritePressionado : spriteSolto;
        if (alvo != null) sprite.sprite = alvo;
    }

    // O colisor fica FIXO na altura do sprite pressionado (o estado em que alguem realmente
    // esta apoiado em cima). Tentar acompanhar a troca de sprite em tempo real cria um loop:
    // encolhe -> personagem perde contato -> despressiona -> colisor cresce de volta -> encosta
    // de novo -> pressiona -> encolhe de novo... resultado era o tremor rapido reportado.
    // O sprite solto (mais alto) e so visual: como ninguem esta em cima nesse estado, a pequena
    // diferenca de altura entre os dois sprites nao aparece.
    void AjustarColisorFixo()
    {
        if (colisor == null) return;
        Sprite referencia = spritePressionado != null ? spritePressionado : spriteSolto;
        if (referencia == null) return;

        Bounds b = referencia.bounds;
        colisor.offset = b.center;
        colisor.size = b.size;
    }

    void OnValidate()
    {
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (colisor == null) colisor = GetComponent<BoxCollider2D>();
        AjustarColisorFixo();

        if (alvos == null) return;
        foreach (MonoBehaviour alvo in alvos)
        {
            if (alvo != null && !(alvo is IAtivavel))
                Debug.LogWarning($"[BotaoPressao] '{alvo.name}' foi arrastado em Alvos mas nao implementa IAtivavel. Ele nao sera ativado.", this);
        }
    }
}
