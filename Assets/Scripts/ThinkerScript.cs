using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThinkerScript : MonoBehaviour {

    private readonly int NBUTTONS = 3; //Número de botões existentes no jogo
    private readonly int winScore = 5; //Pontuação necessária para vencer
    private int score = 0; //Pontuação do jogador
    public Text scoreText; //Pontuação do jogador, em texto
    private int i = 0; //Variável auxiliar
    private int next = 0; //Variável auxiliar
    private int playerInput = 0; //Determina o que o jogador apertou
    private ArrayList list = new ArrayList(); //Guarda a ordem dos botões durante o jogo
    private System.Random rand = new System.Random(); //Usado em getRandom
    public geniusState gameState; //Guarda o estado atual do jogo
    public GameObject[] buttonArray; //Guarda os botões do jogo. Vermelho é 0, azul é 1, amarelo é 2
    private AudioSource currentAudio; //Usado para tocar o som correto durante a execução
    public AudioSource gameEndSource; //Usado para tocar um som que indica que o jogo terminou
    //public GameObject gameOverText; //Usado para indicar ao jogador que o jogo terminou
    //public GameObject restarter; //Botão que reinicia o jogo
    //public GameObject menu; //Botão que volta ao menu principal
    private PlayerInput input; //Guarda os inputs do jogador
    private InputAction vermelhoAction, azulAction, amareloAction; //Guarda as ações do jogador
    public AudioClip loseClip, pistaClip; //Guardam os clips de vitória, derrota, e a pista que o jogador recebe ao vencer

    //Usado para determinar o estado atual de jogo; o Genius é tratado como uma máquina de estados
    public enum geniusState {
        BUTTONPHASE,
        PLAYERPHASE,
        IDLE
    };

    //Retorna um inteiro de 0 a NBUTTONS, usado para determinar aleatoriamente qual vai ser a próxima cor
    private int getRandom() {
        return rand.Next(NBUTTONS);
    }

    //Usado durante a PLAYERPHASE para fazer o botão fazer o seu som
    private void makeSound(GameObject current) {
        currentAudio = current.GetComponent<AudioSource>();
        currentAudio.Play();
    }

    //Usado para conferir se o input do jogador é o correto e, caso necessário, realizar troca de estado
    private void check(int input) {
        makeSound(buttonArray[input]);
        if(input == (int) list[i]) {
            i++;
            score++;
            scoreText.text = "Pontuação: " + score.ToString();
            if(i >= list.Count) {
                if(list.Count >= winScore) {
                    win();
                }
                else {
                    i = 0;
                    gameState = geniusState.BUTTONPHASE;
                }
            } 
        } else {
            lose();
        }
    }

    //Funcionalizado para facilidade de leitura
    private void win() {
        gameState = geniusState.IDLE;
        gameEndSource.clip = pistaClip;
        StartCoroutine(endGame());
    }

    //Funcionalizado para facilidade de leitura
    private void lose() {
        gameState = geniusState.IDLE;
        gameEndSource.clip = loseClip;
        StartCoroutine(endGame());
    }

    //Termina o jogo
    private IEnumerator endGame() {
        //Toca o som do fim do jogo
        gameEndSource.Play();
        while(gameEndSource.isPlaying) {
            yield return null;
        }

        //Troca o input de volta para o jogo
        input.SwitchCurrentActionMap("Jogo");
        //Passa que o jogo foi vencido
        PlayerController.jaVenceuGenius = true;
        //Passa de volta para a cena principal
        SceneManager.LoadScene("Jogo");
        yield break;
    }

    //Usado durante a BUTTONPHASE para tocar os sons dos botões em ordem
    private IEnumerator playSounds() {

        gameState = geniusState.IDLE;

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i < list.Count; i++) {
            //Itera pela lista de botões que devem ser tocados. O atual passa qual som deve ser feito para a variável, que o executa
            currentAudio = buttonArray[ (int) list[i] ].GetComponent<AudioSource>();

            //Toca o som do botão
            currentAudio.Play();

            //Enquanto um botão está tocando, os outros devem esperar a vez
            while(currentAudio.isPlaying) {
                yield return null;
            }
        }

        gameState = geniusState.PLAYERPHASE;
        i = 0;

        yield break;
    }
    
    //Chamado exatamente uma vez durante a inicialização do script
    void Start() {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap("GeniusMinigame");
        vermelhoAction = input.actions.FindAction("Vermelho");
        azulAction = input.actions.FindAction("Azul");
        amareloAction = input.actions.FindAction("Amarelo");

        gameState = geniusState.BUTTONPHASE;
    }

    //Chamado uma vez por frame
    void Update() {
        //Durante a fase dos botões, o jogo calcula qual vai ser o próximo botão, e depois toca os sons
        if(gameState == geniusState.BUTTONPHASE) {
            next = getRandom();
            list.Add(next);
            StartCoroutine(playSounds());
        }
        
        //Durante a fase do jogador, o input do jogador é registrado e o jogo confere se está correto. Se o jogador errar, ele perde
        if(gameState == geniusState.PLAYERPHASE) {
            //Registra que o jogador escolheu o botão vermelho, o da esquerda
            if(vermelhoAction.WasPressedThisFrame()) {
                //Para propósitos de teste
                //Debug.Log("Player says red");
                playerInput = 0;
                check(playerInput);
            }

            //Registra que o jogador escolheu o botão azul, o do centro
            if(azulAction.WasPressedThisFrame()) {
                //Para propósitos de teste
                //Debug.Log("Player says blue");
                playerInput = 1;
                check(playerInput);
            }

            //Registra que o jogador escolheu o botão amarelo, o da direita
            if(amareloAction.WasPressedThisFrame()) {
                //Para propósitos de teste
                //Debug.Log("Player says yellow");
                playerInput = 2;
                check(playerInput);
            }
        }
    }
}