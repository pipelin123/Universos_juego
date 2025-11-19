using UnityEngine;

public class Statue : MonoBehaviour
{
    public GameObject victoryPanel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();

            if (pc.hasSword)
            {
                Debug.Log("Has ganado, tienes la espada.");
                victoryPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.Log("No puedes ganar. Te falta la espada.");
            }
        }
    }
}
