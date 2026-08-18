using UnityEngine;

// Componente generico: coloque em QUALQUER GameObject (uma plataforma extra, uma parede, um
// obstaculo) para torna-lo controlavel por um BotaoPressao ou Alavanca, sem precisar de um
// script proprio pra cada coisa nova que precisar ser ligada/desligada.
public class ObjetoAtivavel : MonoBehaviour, IAtivavel
{
    public enum Acao { MostrarEsconder, LigarDesligarColisor }

    [Tooltip("MostrarEsconder: ativa/desativa o GameObject inteiro (visual + colisao).\nLigarDesligarColisor: mantem o visual, so liga/desliga a colisao.")]
    public Acao acao = Acao.MostrarEsconder;

    [Tooltip("Marcado: 'Ativar' faz o objeto aparecer/colidir (ex: uma ponte que surge). Desmarcado: 'Ativar' faz o objeto sumir/parar de colidir (ex: uma parede que abre passagem).")]
    public bool ativarMostra = true;

    private Collider2D colisor;

    void Awake()
    {
        colisor = GetComponent<Collider2D>();
    }

    public void Ativar() => Aplicar(ativarMostra);
    public void Desativar() => Aplicar(!ativarMostra);

    void Aplicar(bool mostrar)
    {
        if (acao == Acao.MostrarEsconder)
        {
            gameObject.SetActive(mostrar);
        }
        else if (colisor != null)
        {
            colisor.enabled = mostrar;
        }
    }
}
