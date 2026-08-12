using UnityEngine;

public class lavacolider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Aqua"))
        {
            GerenciadorFase.instancia.MorreramOsDois();
        }
    }
}