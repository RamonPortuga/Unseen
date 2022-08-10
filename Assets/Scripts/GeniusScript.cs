using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GeniusScript : MonoBehaviour, IMinigame {

    private const int NBUTTONS = 3; //Número de botões existentes no jogo
    private int i; //Variável auxiliar
    private ArrayList list; //Guarda a ordem dos botões durante o jogo
    private System.Random rand; //Usado em getRandom
    private AudioSource currentAudio; //Usado para tocar o som correto durante a execução
    private InputAction vermelhoAction, azulAction, amareloAction; // Componentes InputAction para cada botão do genius
    private bool jaVenceu; // Indica se o jogador já venceu esse minigame
    public geniusState gameState; //Guarda o estado atual do jogo
    public GameObject[] buttonArray; //Guarda os botões do jogo. Vermelho é 0, azul é 1, amarelo é 2
    public uint rodadasParaVencer = 5; // Quantas rodadas até o jogador vencer o minigame 
    [TextArea(5,10)] public string mensagemAoVencer; // Mensagem mostrada quando o jogador vence o minigame 
    public AudioClip clipeMensagemAoVencer; // Clipe de audio da mensagem mostrada quando o jogador vence
    public GameObject geniusPanel; // Painel contendo as instruções do minigame 
    public GameObject gameOver; // Objeto que faz o som de game over 
    public AudioClip clipeGeniusInstrucoes; // Clipe de audio para as instrucoes do minigame

    //Usado para determinar o estado atual de jogo; o Genius é tratado como uma máquina de estados
    public enum geniusState {
        BUTTONPHASE,
        PLAYERPHASE,
        IDLE,
        INSTRUCTIONS
    };

    void Start() {
        jaVenceu = false; // Marca que o jogador não venceu esse minigame ainda
    }

    // Inicia o jogo do Genius
    public void StartGame() {
        // Se o jogador já venceu esse minigame, mostra a mensagem e sai do minigame 
        if( jaVenceu ) {
            GameManager.GM.MandarMensagem( mensagemAoVencer, clipeMensagemAoVencer );
            QuitGame();
            return;
        }

        enabled = true; // Habilita esse script

        // Preenche os InputAction usados pelo script
        PlayerInput playerinput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        vermelhoAction = playerinput.actions.FindAction("Vermelho");
        azulAction = playerinput.actions.FindAction("Azul");
        amareloAction = playerinput.actions.FindAction("Amarelo");

        i = 0;
        list = new ArrayList();
        rand = new System.Random();
        geniusPanel.SetActive(true);
        gameState = geniusState.INSTRUCTIONS;

        // Faz a voz sintetizada ler as instruções do minigame
        GameManager.GM.FalarMensagem( clipeGeniusInstrucoes );        
    }

    // Sai do minigame e volta ao jogo normal
    public void QuitGame() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MudaMapaJogo();
        enabled = false;
    }

    // Retorna o nome do ActionMap usado pelo genius
    public string ActionMapName() {
        return "GeniusMinigame";
    }

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

            // Se o jogador ganhou a quantidade certa de rodadas, 
            // marca que ele venceu, mostra a mensagem e sai do minigame 
            if(i == (rodadasParaVencer-1)) {
                gameState = geniusState.IDLE;
                jaVenceu = true;
                GameManager.GM.MandarMensagem( mensagemAoVencer, clipeMensagemAoVencer );
                QuitGame();
            }

            i++;
            if(i >= list.Count && !jaVenceu) {
                i = 0;
                gameState = geniusState.BUTTONPHASE;
            }
        } else {
            makeSound( gameOver );
            QuitGame();
        }
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

    //Chamado uma vez por frame
    void Update() {
        //Durante a fase dos botões, o jogo calcula qual vai ser o próximo botão, e depois toca os sons
        if(gameState == geniusState.BUTTONPHASE) {
            int next = getRandom();
            list.Add(next);
            StartCoroutine(playSounds());
        }
        
        //Durante a fase do jogador, o input do jogador é registrado e o jogo confere se está correto. Se o jogador errar, ele perde
        if(gameState == geniusState.PLAYERPHASE) {
            //Registra que o jogador escolheu o botão vermelho, o da esquerda
            if( vermelhoAction.WasPressedThisFrame() ) {
                check(0);
            }

            //Registra que o jogador escolheu o botão azul, o do centro
            else if( azulAction.WasPressedThisFrame() ) {
                check(1);
            }

            //Registra que o jogador escolheu o botão amarelo, o da direita
            else if( amareloAction.WasPressedThisFrame() ) {
                check(2);
            }
        }
    }
}