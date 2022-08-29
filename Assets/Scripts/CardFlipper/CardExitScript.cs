/*Código usado na Scene "ExitConfirmation" 
 * e "ExitConfirmationJoystick"*/
using UnityEngine.SceneManagement;
using UnityEngine;

public class CardExitScript : MonoBehaviour
{

    public void selectMenu()
    {
        SceneManager.LoadScene("CardMenu");
    }

    // Sai do Jogo
    public void quitGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CardExitConfirmation")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                quitGame();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Debug.Log("Entrou");
                selectMenu();
            }
        }
    }
}
