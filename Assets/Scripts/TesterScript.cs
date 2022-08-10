/*Código usado para auxiliar nas devidas execuções dos testes
 * de áudio. Logo, ele é utilizado em "Tester" e "TesterJoystick"
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class TesterScript : MonoBehaviour {

    public GameObject[] testerArray; //Guarda as cartas
    public GameObject activeCard; //Guarda a carta ativa
    
    public AudioSource testerBump; //Guarda o som do limite
    public AudioSource testerIsOpen; //Guarda o som de quando a carta já está aberta
    public AudioSource testerCorrectPair; //Guarda o som de quando o par está correto
    public AudioSource testerNewCard; //Guarda o som de quando uma nova carta é aberta
    public AudioSource testerIncorrectPair; //Guarda o som de quando um par está incorreto
    public AudioSource testerVictory; //Guarda o som da vitória
    
    public AudioSource victory; //Guarda o som da vitória
    private System.Random rand = new System.Random(); //Gera valores aleatórios

    public bool startNext = false; //Variável de controle, evita que um áudio seja executado
                                   //enquanto outro já esteja em execução

    public bool play = true; //Variável de controle, evita que um áudio seja repetido diversas
                             //vezes sem ser por comando do usuário

    public int totalButtons = 6; //Armazena a quantidade total de botões que devem ser ouvidos
                                 //no Tester

    public int index = 0; //Devido a grande quantidade de botões, quando o usuário estiver
                          //navegando utilizando o Joystick, basta ele apertar o  botão "RT"
                          //(para frente) ou "LT" (para trás) que ele poderá ouvir os sons.
                          //O index serve justamente como variável de controle, para
                          //possibilitar a devida execução ds áudios junto com suas
                          //audiodescrições

    private void shuffle() {
        int i = rand.Next(6);
        activeCard = testerArray[i];
    }

    public void playBump() {
        shuffle();
        activeCard.GetComponent<CardScript>().getBumpSound().Play();
    }

    public void playMoved() {
        shuffle();
        activeCard.GetComponent<CardScript>().getMovedSound().Play();
    }

    public void playPair() {
        shuffle();
        activeCard.GetComponent<CardScript>().getCorrectSound().Play();
    }

    public void playNotPair() {
        shuffle();
        activeCard.GetComponent<CardScript>().getIncorrectSound().Play();
    }

    public void playOpen() {
        shuffle();
        activeCard.GetComponent<CardScript>().getOpenSound().Play();
    }

    public void playWin() {
        victory.Play();
    }

    public bool audioIsPlaying()
    {
        if (testerBump.isPlaying == false && testerIsOpen.isPlaying == false && testerCorrectPair.isPlaying == false
                && testerNewCard.isPlaying == false && testerIncorrectPair.isPlaying == false && testerVictory.isPlaying == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //AQUI COMEÇA O CÓDIGO PARA O TESTER
    public void playTesterBump(){
        testerBump.Play();
    }

    public void playTesterIsOpen(){
        testerIsOpen.Play();
    }

    public void playTesterCorrectPair(){
        testerCorrectPair.Play();
    }

    public void playTesterNewCard(){
        testerNewCard.Play();
    }

    public void playTesterIncorrectPair(){
        testerIncorrectPair.Play();
    }

    public void playTesterVictory(){
        testerVictory.Play();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Tester")
        {
            startNext = audioIsPlaying();
            if ((Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1)) && startNext == true)
            {
                playTesterBump();
            }
            else if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) && startNext == true)
            {
                playTesterIsOpen();
            }
            else if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) && startNext == true)
            {
                playTesterCorrectPair();
            }
            else if ((Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4)) && startNext == true)
            {
                playTesterNewCard();
            }
            else if ((Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5)) && startNext == true)
            {
                playTesterIncorrectPair();
            }
            else if ((Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6)) && startNext == true)
            {
                playTesterVictory();
            }
        }
    }
}
