using UnityEngine;

// Ao contrario da lava (mata so Aqua) e da agua (mata so Ignis), o acido mata os dois
// personagens, independente de qual elemento tocar nele.
public class AcidoColider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Ignis") || outro.CompareTag("Aqua"))
        {
            GerenciadorFase.instancia.MorreramOsDois();
        }
    }
}
