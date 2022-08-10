/*Esse código é utilizado principalmente para "controlar" os
 * menus (botões) que aparecem no decorrer do jogo */

using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject instructions;
    public GameObject thinkerScript;

    public AudioSource IniciarJogo;

    public bool joystick = true;
    public bool endGame = true;
    //public bool endGame = false;

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
        SceneManager.LoadScene("Menu");
    }

    //Inicia o jogo
    public void startGame()
    {
        SceneManager.LoadScene("Jogo");
    }

    //Mostra as instruções
    public void selectInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    //Testa o fone do usuário
    public void selectTester()
    {
        SceneManager.LoadScene("TesteFone");
    }

    public void quitGame()
    {
        SceneManager.LoadScene("ExitConfirmation");
    }

    public void exit()
    {
        Application.Quit();
    }

    public void Start()
    {

    }

    private void Update()
    {
        //endGame = thinkerScript.GetComponent<ThinkerScript>().endGame;
        //endGame = ThinkerScript.endGame;
        //Essas condicionais servem para que, quando o usuário apertar uma tecla no teclado ou joystick
        //Mude de Menu. Como podemos perceber, ele faz a comparação segundo  cada Scene. Além disso, vale
        //destacar que para o joystick, estou tomando como referência as inputs que coloquei presentes em 
        //Project Settings.
        //PARA O TECLADO
        if (SceneManager.GetActiveScene().name == "Instructions" || SceneManager.GetActiveScene().name == "Tester")
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
        else if (SceneManager.GetActiveScene().name == "Menu")
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
        else if (SceneManager.GetActiveScene().name == "ExitConfirmation")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectMenu();
            }
        }

        //Essas condicionais serão utilizadas quando o usuário apertar
        //o botão ou tecla para sair do jogo.
        if (Input.GetKeyDown(KeyCode.Escape)){
            //Debug.Log("Entrou");
            if (SceneManager.GetActiveScene().name == "Menu")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "Instructions")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "Tester")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "Genius")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "Jogo")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "ExitConfirmation")
            {
                Application.Quit();
            }
        }
    }
}
