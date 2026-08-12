using UnityEngine;

public class Porta : MonoBehaviour
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

    public bool ocupada = false;

    private SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (baseFechada != null) sprite.sprite = baseFechada;
        if (topoRenderer != null && topoFechado != null) topoRenderer.sprite = topoFechado;
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
        }
    }
}