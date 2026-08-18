using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Som")]
    public AudioClip somMorte;

    private Vector3 pontoInicial;
    private PersonagemAnimado animado;
    private PlayerMovement movimento;
    private AudioSource audioSource;

    void Start()
    {
        pontoInicial = transform.position;
        animado = GetComponentInChildren<PersonagemAnimado>();
        movimento = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Morrer()
    {
        StopAllCoroutines();
        StartCoroutine(SequenciaMorte());
    }

    IEnumerator SequenciaMorte()
    {
        if (movimento != null) movimento.podeControlar = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (audioSource != null && somMorte != null)
            audioSource.PlayOneShot(somMorte);

        float duracao = 0f;
        if (animado != null)
        {
            animado.TocarDano();
            duracao = animado.DuracaoDano;
        }

        yield return new WaitForSeconds(duracao);

        transform.position = pontoInicial;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (movimento != null) movimento.podeControlar = true;
    }

    public void TocarVitoria()
    {
        if (animado != null)
            animado.TocarVitoria();
    }

    public void DefinirCheckpoint(Vector3 novaPosicao)
    {
        pontoInicial = novaPosicao;
    }
}