using UnityEngine;

// Camera que acompanha os dois personagens ao mesmo tempo: fica centralizada no ponto medio
// entre eles e ajusta o zoom automaticamente para os dois sempre caberem na tela.
//
// Por que um script simples em vez de Cinemachine: o comportamento pedido (2 alvos, zoom
// automatico por distancia, limites retangulares por fase) e direto o suficiente pra implementar
// sem adicionar uma dependencia nova ao projeto. Cinemachine vale a pena se no futuro quiser coisas
// como camera shake, zonas de camera ou confiner com formas irregulares.
[RequireComponent(typeof(Camera))]
public class CameraSeguidor : MonoBehaviour
{
    [Header("Alvos (deixe vazio para buscar automaticamente pelas tags Ignis/Aqua)")]
    public Transform alvo1;
    public Transform alvo2;

    [Header("Zoom (tamanho ortografico da camera)")]
    public float zoomMinimo = 3f;
    public float zoomMaximo = 8f;
    [Tooltip("Espaco extra ao redor dos dois personagens, em unidades do mundo.")]
    public float margem = 2f;

    [Header("Suavidade (menor = mais responsivo, maior = mais suave)")]
    public float suavidadePosicao = 0.25f;
    public float suavidadeZoom = 0.35f;

    [Header("Limites da fase (area visivel maxima, em coordenadas do mundo)")]
    [Tooltip("Veja o retangulo amarelo na Scene view para ajustar visualmente.")]
    public Vector2 limiteMin = new Vector2(-10, -5);
    public Vector2 limiteMax = new Vector2(10, 5);

    private Camera cam;
    private Vector3 velocidadePosicao;
    private float velocidadeZoom;
    private bool forcarInstantaneo;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        if (alvo1 == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Ignis");
            if (go != null) alvo1 = go.transform;
        }
        if (alvo2 == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Aqua");
            if (go != null) alvo2 = go.transform;
        }

        AtualizarCamera(instantaneo: true);
    }

    void OnEnable() { GameManager.AoReiniciarFase += MarcarRespawn; }
    void OnDisable() { GameManager.AoReiniciarFase -= MarcarRespawn; }

    // No respawn os personagens sao teleportados de volta ao inicio. Se a camera continuasse
    // seguindo suavemente ela ia arrastar por cima do cenario inteiro ate chegar la - em vez
    // disso, no proximo frame ela salta direto pra posicao certa, sem o arrasto.
    void MarcarRespawn()
    {
        forcarInstantaneo = true;
    }

    void LateUpdate()
    {
        AtualizarCamera(forcarInstantaneo);
        forcarInstantaneo = false;
    }

    void AtualizarCamera(bool instantaneo)
    {
        if (alvo1 == null || alvo2 == null) return;

        Vector3 centro = (alvo1.position + alvo2.position) / 2f;

        float distanciaX = Mathf.Abs(alvo1.position.x - alvo2.position.x) + margem;
        float distanciaY = Mathf.Abs(alvo1.position.y - alvo2.position.y) + margem;

        float zoomParaCaberX = distanciaX / (2f * cam.aspect);
        float zoomParaCaberY = distanciaY / 2f;
        float zoomAlvo = Mathf.Clamp(Mathf.Max(zoomParaCaberX, zoomParaCaberY), zoomMinimo, zoomMaximo);

        float novoZoom = instantaneo
            ? zoomAlvo
            : Mathf.SmoothDamp(cam.orthographicSize, zoomAlvo, ref velocidadeZoom, suavidadeZoom);
        cam.orthographicSize = novoZoom;

        float meiaAltura = novoZoom;
        float meiaLargura = novoZoom * cam.aspect;

        float minX = limiteMin.x + meiaLargura;
        float maxX = limiteMax.x - meiaLargura;
        float minY = limiteMin.y + meiaAltura;
        float maxY = limiteMax.y - meiaAltura;

        // Se a fase for menor que a area visivel atual (zoom maximo numa fase pequena),
        // centraliza em vez de inverter o clamp.
        float alvoX = minX <= maxX ? Mathf.Clamp(centro.x, minX, maxX) : (limiteMin.x + limiteMax.x) / 2f;
        float alvoY = minY <= maxY ? Mathf.Clamp(centro.y, minY, maxY) : (limiteMin.y + limiteMax.y) / 2f;

        Vector3 posicaoAlvo = new Vector3(alvoX, alvoY, transform.position.z);

        transform.position = instantaneo
            ? posicaoAlvo
            : Vector3.SmoothDamp(transform.position, posicaoAlvo, ref velocidadePosicao, suavidadePosicao);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 centro = new Vector3((limiteMin.x + limiteMax.x) / 2f, (limiteMin.y + limiteMax.y) / 2f, 0);
        Vector3 tamanho = new Vector3(limiteMax.x - limiteMin.x, limiteMax.y - limiteMin.y, 0);
        Gizmos.DrawWireCube(centro, tamanho);
    }
}
