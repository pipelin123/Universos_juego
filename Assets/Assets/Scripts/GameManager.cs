using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int collected = 0;       // Cuántos has recolectado
    public int total = 13;          // Los que necesitas

    public GameObject sword;        // La espada que aparece al final

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Asegura que la espada NO aparece al inicio
        if (sword != null)
            sword.SetActive(false);
    }

    public void AddCollectible()
    {
        collected++;

        if (collected >= total)
        {
            // Activa la espada cuando completes los 13 coleccionables
            if (sword != null)
                sword.SetActive(true);
        }
    }
}
