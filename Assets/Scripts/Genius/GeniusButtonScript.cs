/*Esse código é utilizado principalmente para "controlar" os
 * menus (botões) que aparecem no decorrer do jogo */

using UnityEngine;
using UnityEngine.SceneManagement;

public class GeniusButtonScript : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject instructions;
    public GameObject thinkerScript;

    public AudioSource IniciarJogo;

    public bool joystick = true;
    public bool endGame = false;

    public static bool instructionsActived = false;
    public static bool testerActived = false;
    public static bool geniusActived = false;

    //Reinicia a cena atual
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Retorna o jogador para o menu principal
    public void selectMenu()
    {
        SceneManager.LoadScene("GeniusMenu");
    }

    //Inicia o jogo
    public void startGame()
    {
        SceneManager.LoadScene("Genius");
    }

    //Mostra as instruções
    public void selectInstructions()
    {
        SceneManager.LoadScene("GeniusInstructions");
    }

    //Testa o fone do usuário
    public void selectTester()
    {
        SceneManager.LoadScene("GeniusTester");
    }

    public void quitGame()
    {
        SceneManager.LoadScene("Jogo");
        //SceneManager.LoadScene("GeniusExitConfirmation");
        //Application.Quit();
    }



    private void Update()
    {
        //endGame = thinkerScript.GetComponent<ThinkerScript>().endGame;
        endGame = GeniusThinkerScript.endGame;
        //Essas condicionais servem para que, quando o usuário apertar uma tecla no teclado ou joystick
        //Mude de Menu. Como podemos perceber, ele faz a comparação segundo  cada Scene. Além disso, vale
        //destacar que para o joystick, estou tomando como referência as inputs que coloquei presentes em 
        //Project Settings.

        //PARA O TECLADO
        if (SceneManager.GetActiveScene().name == "GeniusInstructions" || SceneManager.GetActiveScene().name == "GeniusTester")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectMenu();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Genius")
        {
            if (endGame == true)
            {
                if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                {
                    startGame();
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                {
                    selectMenu();
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "GeniusMenu")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                startGame();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectInstructions();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectTester();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                quitGame();
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Entrou");
            if (SceneManager.GetActiveScene().name == "GeniusMenu")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "GeniusInstructions")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "GeniusTester")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "Genius")
            {
                quitGame();
            }
        }
    }
}
