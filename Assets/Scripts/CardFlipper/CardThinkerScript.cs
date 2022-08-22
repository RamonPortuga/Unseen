using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ThinkerScript : MonoBehaviour
{

    private readonly int NBUTTONS = 3; //Número de botões existentes no jogo
    private int score = 0; //Pontuação do jogador
    public Text scoreText; //Pontuação do jogador, em texto
    private int i = 0; //Variável auxiliar
    private int rodadas = 0; //Variável auxiliar
    private int next = 0; //Variável auxiliar
    private int playerInput = 0; //Determina o que o jogador apertou
    private ArrayList list = new ArrayList(); //Guarda a ordem dos botões durante o jogo
    private System.Random rand = new System.Random(); //Usado em getRandom
    public geniusState gameState; //Guarda o estado atual do jogo
    public GameObject[] buttonArray; //Guarda os botões do jogo. Vermelho é 0, azul é 1, amarelo é 2
    private AudioSource currentAudio; //Usado para tocar o som correto durante a execução
    public AudioSource gameOver; //Usado para tocar um som que indica que o jogador perdeu
    public AudioSource vitoria; //Usado para tocar um som que indica que o jogador perdeu
    public GameObject gameOverText; //Usado para indicar ao jogador que o jogo terminou
    public GameObject youWinText; //Usado para indicar ao jogador que ele ganhou o jogo
    public GameObject textInstructions; //Usado para orientar o jogador sobre as opções após a vitória
    public GameObject restarter; //Botão que reinicia o jogo
    public GameObject menu; //Botão que volta ao menu principal

    public static bool endGame = false; //Armazena se o jogo chegou ao fim. True == Acabou, False == Ñ Acabou
    //public bool joystick = true;

    //Usado para determinar o estado atual de jogo; o Genius é tratado como uma máquina de estados
    public enum geniusState
    {
        BUTTONPHASE,
        PLAYERPHASE,
        IDLE
    };

    //Retorna um inteiro de 0 a NBUTTONS, usado para determinar aleatoriamente qual vai ser a próxima cor
    private int getRandom()
    {
        return rand.Next(NBUTTONS);
    }

    //Usado durante a PLAYERPHASE para fazer o botão fazer o seu som
    private void makeSound(GameObject current)
    {
        currentAudio = current.GetComponent<AudioSource>();
        currentAudio.Play();
    }

    //Usado para conferir se o input do jogador é o correto e, caso necessário, realizar troca de estado
    private void check(int input)
    {
        makeSound(buttonArray[input]);
        rodadas++;
        if (rodadas == 36)
        {
            vitoria.Play();
            score++;
            scoreText.text = "Pontuação final: 8";
            youWinText.SetActive(true);
            textInstructions.SetActive(true);
            restarter.SetActive(true);
            menu.SetActive(true);
            gameState = geniusState.IDLE;
            endGame = true;
            Debug.Log("Player wins");
        }
        else if (input == (int)list[i])
        {
            i++;
            if (i >= list.Count)
            {
                i = 0;
                score++;
                scoreText.text = "Pontuação: " + score.ToString();
                gameState = geniusState.BUTTONPHASE;
            }
        }
        else if (input != (int)list[i])
        {
            gameOver.Play();
            scoreText.text = "Pontuação final: " + score.ToString();
            //Colocar algumas condicionais para fazer o áudio da Pontuação Final
            gameOverText.SetActive(true);
            textInstructions.SetActive(true);
            restarter.SetActive(true);
            menu.SetActive(true);
            gameState = geniusState.IDLE;
            endGame = true;
            Debug.Log("Player lose");
        }
    }

    //Usado durante a BUTTONPHASE para tocar os sons dos botões em ordem
    private IEnumerator playSounds()
    {

        gameState = geniusState.IDLE;

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < list.Count; i++)
        {
            //Itera pela lista de botões que devem ser tocados. O atual passa qual som deve ser feito para a variável, que o executa
            currentAudio = buttonArray[(int)list[i]].GetComponent<AudioSource>();

            //Toca o som do botão
            currentAudio.Play();

            //Enquanto um botão está tocando, os outros devem esperar a vez
            while (currentAudio.isPlaying)
            {
                yield return null;
            }
        }

        gameState = geniusState.PLAYERPHASE;
        i = 0;

        yield break;
    }

    //Chamado exatamente uma vez durante a inicialização do script
    void Start()
    {
        gameState = geniusState.BUTTONPHASE;
        endGame = false;
    }

    //Chamado uma vez por frame
    void Update()
    {
        //joystick = SelectControlScript.joystick;

        //Se a qualquer momento o jogador apertar esc, o jogo finaliza
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Player says esc");
            Application.Quit();
        }

        //Durante a fase dos botões, o jogo calcula qual vai ser o próximo botão, e depois toca os sons
        if (gameState == geniusState.BUTTONPHASE)
        {
            next = getRandom();
            list.Add(next);
            StartCoroutine(playSounds());
        }

        //Durante a fase do jogador, o input do jogador é registrado e o jogo confere se está correto.
        //Se o jogador errar, ele perde
        if (gameState == geniusState.PLAYERPHASE)
        {
            //Registra que o jogador escolheu o botão vermelho, o da esquerda
         
            if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerInput = 0;
                //Para propósitos de teste
                Debug.Log("Player says red");
                check(playerInput);
            }

            //Registra que o jogador escolheu o botão azul, o do centro
            if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerInput = 1;
                //Para propósitos de teste
                Debug.Log("Player says blue");
                check(playerInput);
            }

            //Registra que o jogador escolheu o botão amarelo, o da direita
            if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerInput = 2;
                //Para propósitos de teste
                Debug.Log("Player says yellow");
                check(playerInput);
            }
        }
    }
}