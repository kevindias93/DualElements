using UnityEngine;

public class PersonagemAnimado : MonoBehaviour
{
    public enum Estado { Idle, Andando, Pulando, Caindo, Vitoria, Dano }

    [Header("Quadros de animacao")]
    public Sprite[] idle;
    public Sprite[] andando;
    public Sprite[] pulando;
    public Sprite[] caindo;
    public Sprite[] vitoria;
    public Sprite[] dano;

    [Header("Velocidade da animacao")]
    public float quadrosPorSegundo = 10f;
    public float quadrosPorSegundoAndando = 8f;
    public float quadrosPorSegundoDano = 10f;

    public float DuracaoDano => dano != null && dano.Length > 0 ? dano.Length / Mathf.Max(quadrosPorSegundoDano, 0.01f) : 0f;

    private SpriteRenderer sr;
    private Estado estado = Estado.Idle;
    private int frame;
    private float timer;
    private bool tocandoUnico;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Aplicar();
    }

    void Update()
    {
        Sprite[] quadrosAtuais = Quadros(estado);
        if (quadrosAtuais == null || quadrosAtuais.Length == 0) return;

        timer += Time.deltaTime;
        float fps = quadrosPorSegundo;
        if (estado == Estado.Dano) fps = quadrosPorSegundoDano;
        else if (estado == Estado.Andando) fps = quadrosPorSegundoAndando;
        float duracao = 1f / Mathf.Max(fps, 0.01f);
        if (timer < duracao) return;
        timer -= duracao;

        frame++;
        if (frame >= quadrosAtuais.Length)
        {
            if (tocandoUnico)
            {
                tocandoUnico = false;
                estado = Estado.Idle;
            }
            frame = 0;
        }
        Aplicar();
    }

    // Chamado todo frame pelo PlayerMovement para escolher Idle/Andando/Pulando/Caindo
    public void DefinirMovimento(bool andandoAgora, bool noChao, float velocidadeY)
    {
        if (tocandoUnico) return;

        Estado novo;
        if (!noChao)
            novo = velocidadeY > 0f ? Estado.Pulando : Estado.Caindo;
        else
            novo = andandoAgora ? Estado.Andando : Estado.Idle;

        if (novo == estado) return;

        estado = novo;
        frame = 0;
        timer = 0;
        Aplicar();
    }

    public void TocarVitoria() => TocarUnico(Estado.Vitoria);
    public void TocarDano() => TocarUnico(Estado.Dano);

    void TocarUnico(Estado novoEstado)
    {
        Sprite[] quadros = Quadros(novoEstado);
        if (quadros == null || quadros.Length == 0) return;

        estado = novoEstado;
        frame = 0;
        timer = 0;
        tocandoUnico = true;
        Aplicar();
    }

    Sprite[] Quadros(Estado e)
    {
        switch (e)
        {
            case Estado.Andando: return andando;
            case Estado.Pulando: return pulando;
            case Estado.Caindo: return caindo;
            case Estado.Vitoria: return vitoria;
            case Estado.Dano: return dano;
            default: return idle;
        }
    }

    void Aplicar()
    {
        Sprite[] quadros = Quadros(estado);
        if (sr != null && quadros != null && quadros.Length > 0)
            sr.sprite = quadros[Mathf.Clamp(frame, 0, quadros.Length - 1)];
    }
}
