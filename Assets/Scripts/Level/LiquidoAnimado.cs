using UnityEngine;

// Da vida ao lava/agua/acido sem precisar de shader customizado: pulsa a cor levemente
// (brilho/respiracao) e balanca a superficie para cima/baixo (ondulacao).
//
// Nao rola a textura (tecnica comum de "agua fluindo"): o sprite do lava/agua fica dentro de um
// atlas compartilhado com varios outros sprites do mesmo pacote, e o shader padrao de Sprite nao
// resolve esse deslocamento de forma segura dentro do recorte do atlas - o resultado seria vazar
// pixels de sprites vizinhos em vez de "fluir". Prefiro os dois efeitos abaixo, que sao garantidos
// de funcionar sem eu poder testar visualmente, a arriscar quebrar a arte.
[RequireComponent(typeof(SpriteRenderer))]
public class LiquidoAnimado : MonoBehaviour
{
    [Header("Pulso de brilho")]
    public float intensidadePulso = 0.12f;
    public float velocidadePulso = 1.5f;

    [Header("Ondulacao da superficie")]
    public float alturaOndulacao = 0.04f;
    public float velocidadeOndulacao = 1.2f;

    private SpriteRenderer sprite;
    private Color corBase;
    private Vector3 posicaoBase;
    private float deslocamentoTempo;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        corBase = sprite.color;
        posicaoBase = transform.localPosition;
        deslocamentoTempo = Random.Range(0f, 100f); // evita varios liquidos pulsando em sincronia
    }

    void Update()
    {
        float t = Time.time + deslocamentoTempo;

        float pulso = 1f + Mathf.Sin(t * velocidadePulso) * intensidadePulso;
        sprite.color = new Color(
            Mathf.Clamp01(corBase.r * pulso),
            Mathf.Clamp01(corBase.g * pulso),
            Mathf.Clamp01(corBase.b * pulso),
            corBase.a);

        float onda = Mathf.Sin(t * velocidadeOndulacao) * alturaOndulacao;
        transform.localPosition = posicaoBase + new Vector3(0, onda, 0);
    }
}
