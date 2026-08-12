using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleFase : MonoBehaviour
{
    public void JogarDeNovo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}