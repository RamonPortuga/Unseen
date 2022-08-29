/*Código usado para fazer o "controle" dos botões em todas
 * as Scene.
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardButtonScript : MonoBehaviour {

    public GameObject[] buttons;

    //public bool joystick = true;
    public bool endGame = false;

    public bool repeatAudio = true;


    //Reinicia a cena atual
    public void restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Retorna o jogador para o menu principal
    public void selectMenu() {
        SceneManager.LoadScene("CardMenu");
    }

    //Inicia o jogo
    public void startGame() {
        SceneManager.LoadScene("CardGame");
    }

    //Mostra as Instruções
    public void selectInstructions(){
        SceneManager.LoadScene("CardInstructions");
    }

    //Testa o fone do usuário
    public void selectPreTester()
    {
        SceneManager.LoadScene("CardPreTester");
    }

    //Testa o fone do usuário
    public void selectTester() {
        SceneManager.LoadScene("CardTester");
    }

    
    public void quitGame() {
        SceneManager.LoadScene("CardExitConfirmation");
    }

    private void Start(){

    }

    private void Update()
    {
        //endGame = thinkerScript.GetComponent<ThinkerScript>().endGame;
        endGame = CardTaskmasterScript.endGame;
        //joystick = SelectControlScript.joystick;
        //Debug.Log(endGame);
        //Essas condicionais servem para que, quando o usuário apertar uma tecla no teclado ou joystick
        //Mude de Menu. Como podemos perceber, ele faz a comparação segundo  cada Scene. Além disso, vale
        //destacar que para o joystick, estou tomando como referência as inputs que coloquei presentes em 
        //Project Settings.

       
        if (SceneManager.GetActiveScene().name == "CardInstructions")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectMenu();
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectInstructions();
            }
        }
        else if (SceneManager.GetActiveScene().name == "CardPreTester")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectTester();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                selectPreTester();
            }
        }
        else if (SceneManager.GetActiveScene().name == "CardTester")
        {
            if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            {
                selectMenu();
            }
        }
        else if (SceneManager.GetActiveScene().name == "CardGame")
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
        else if (SceneManager.GetActiveScene().name == "CardMenu")
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
                selectPreTester();
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                quitGame();
            }
        }

        //Essas condicionais serão utilizadas quando o usuário apertar
        //o botão ou tecla para sair do jogo
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Entrou");
            if (SceneManager.GetActiveScene().name == "CardMenu")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "CardInstructions")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "CardPreTester")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "CardTester")
            {
                quitGame();
            }
            else if (SceneManager.GetActiveScene().name == "CardGame")
            {
                quitGame();
            }
        }
    }
}
