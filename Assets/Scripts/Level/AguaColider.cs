using UnityEngine;

public class AguaColider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Ignis"))
        {
            GerenciadorFase.instancia.MorreramOsDois();
        }
    }
}