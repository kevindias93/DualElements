using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 pontoInicial;

    void Start()
    {
        pontoInicial = transform.position;
    }

    public void Morrer()
    {
        transform.position = pontoInicial;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    public void DefinirCheckpoint(Vector3 novaPosicao)
    {
        pontoInicial = novaPosicao;
    }
}