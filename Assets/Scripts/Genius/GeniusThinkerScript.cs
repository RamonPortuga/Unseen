/*Código principal que controla tudo relacionado ao jogo Genius,
 * presente nas Scene "Genius" e "GeniusJoystick"*/
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeniusThinkerScript : MonoBehaviour
{
    private readonly int NBUTTONS = 3; //Número de botões existentes no jogo
    public static int score = 0; //Pontuação do jogador
    public Text scoreText; //Pontuação do jogador, em texto
    private int i = 0; //Variável auxiliar
    private int rodadas = 0; //Variável auxiliar
    private int next = 0; //Variável auxiliar
    private int playerInput = 0; //Determina o que o jogador apertou
    private float countdown = 5; //Contador que servirá para "contar" os 5s máximos da resposta do usuário
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
    public bool end = false;
    public float countdownGameOver = 0f;

    //áudios da pontuação do jogador

    public AudioSource zeroPontos;
    public AudioSource umPonto;
    public AudioSource doisPontos;
    public AudioSource tresPontos;
    public AudioSource quatroPontos;
    public AudioSource cincoPontos;
    public AudioSource seisPontos;
    public AudioSource setePontos;
    public AudioSource oitoPontos;

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
            scoreText.text = " ";
            youWinText.SetActive(true);
            textInstructions.SetActive(true);
            restarter.SetActive(true);
            menu.SetActive(true);
            gameState = geniusState.IDLE;
            endGame = true;
            //Debug.Log("Player wins");
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
            if (score == 0)
            {
                zeroPontos.Play();
            }
            else if (score == 1)
            {
                umPonto.Play();
            }
            else if (score == 2)
            {
                doisPontos.Play();
            }
            else if (score == 3)
            {
                tresPontos.Play();
            }
            else if (score == 4)
            {
                quatroPontos.Play();
            }
            else if (score == 5)
            {
                cincoPontos.Play();
            }
            else if (score == 6)
            {
                seisPontos.Play();
            }
            else if (score == 7)
            {
                setePontos.Play();
            }
            else if (score == 8)
            {
                oitoPontos.Play();
            }
            scoreText.text = " ";
            //scoreText.text = "Pontuação final: " + score.ToString();
            //Colocar algumas condicionais para fazer o áudio da Pontuação Final
            gameOverText.SetActive(true);
            textInstructions.SetActive(true);
            restarter.SetActive(true);
            menu.SetActive(true);
            gameState = geniusState.IDLE;
            endGame = true;
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
        //zeroPontos.Play();
        gameState = geniusState.BUTTONPHASE;
        endGame = false;
        end = false;
        countdownGameOver = 0f;
    }

    //Chamado uma vez por frame
    void Update()
    {
        if (endGame == true)
        {
            countdownGameOver += Time.deltaTime;
        }
        //Se a qualquer momento o jogador apertar esc, o jogo finaliza
        if (Input.GetKeyDown("escape"))
        {
            //Debug.Log("Player says esc");
            SceneManager.LoadScene("ExitConfirmation");
            //Application.Quit();
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

            countdown -= Time.deltaTime;

            //Debug.Log(countdown);

            //Registra que o jogador escolheu o botão vermelho, o da esquerda

            if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerInput = 0;
                //Para propósitos de teste
                //Debug.Log("Player says red");
                check(playerInput);
                countdown = 5;
            }
            //Registra que o jogador escolheu o botão azul, o do centro
            else if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerInput = 1;
                //Para propósitos de teste
                //Debug.Log("Player says blue");
                check(playerInput);
                countdown = 5;
            }
            //Registra que o jogador escolheu o botão amarelo, o da direita
            else if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerInput = 2;
                //Para propósitos de teste
                //Debug.Log("Player says yellow");
                check(playerInput);
                countdown = 5;
            }


        }
        if (gameState == geniusState.PLAYERPHASE && countdown <= 0f)
        {
            gameOver.Play();
            //scoreText.text = "Pontuação final: " + score.ToString();
            //scoreboard.SetActive(false);
            gameOverText.SetActive(true);
            textInstructions.SetActive(true);
            restarter.SetActive(true);
            menu.SetActive(true);
            gameState = geniusState.IDLE;
            endGame = true;
        }
    }
}