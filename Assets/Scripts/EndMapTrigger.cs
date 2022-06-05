using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMapTrigger : MonoBehaviour
{
    public int NextMapIndex = 0;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(NextMapIndex);
        }
    }
}
