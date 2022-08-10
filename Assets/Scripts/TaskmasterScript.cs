/*Código usado no "jogo principal", sendo aplicado nas Scene
 * "Game" e "GameJoystick"
 */

//Teoricamente funcionando; falta testar (museu quebrado)

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TaskmasterScript : MonoBehaviour {
    private readonly int TOTALCARDS = 6; //Quantas cartas existem no jogo
    private int openCards = 0; //Guarda se existe uma carta já aberta ou não
    private int pairedCards = 0; //Guarda quantos pares já foram formados (número de cartas)
    private int score = 0; //Guarda quantos pares já foram formados (número de pares)
    private System.Random rand = new System.Random(); //Gera valores aleatórios
    public gameState state; //Estado atual do jogo
    public GameObject activeCard; //Guarda qual é a carta atual
    public GameObject potentialPair; //Guarda uma carta que foi aberta para poder testar se vai formar um par
    public GameObject[] allCards; //Guarda todas as cartas para permitir randomização
    public GameObject youWinText;  //Usado para indicar ao jogador que ele ganhou o jogo
//    public GameObject textInstructions; //Usado para orientar o jogador sobre as opções após a vitória
    public AudioClip[] cardSounds; //Guarda os áudios dos pares
    private CardScript activeScript; //Guarda o CardScript da carta atual
    public AudioSource victory; //Som que toca quando o jogador vence
    public Text scoreText; //Pontuação do jogador, em texto
    public Text timeText; //Tempo do jogador, em texto
//    public GameObject[] buttons; //Guarda os botões de quando o jogo terminar
    public AudioClip pistaClip; //Guardam os clips de vitória, derrota, e a pista que o jogador recebe ao vencer
    private InputAction upAction, downAction, leftAction, rightAction, interactAction, pauseAction; //Guarda as ações do jogador
    private PlayerInput input; //Guarda os inputs do jogador
    public AudioSource gameEndSource; //Toca as instruções para o jogador no final

    //Segue abaixo as variáveis referentes aos áudios do posiconamento das cartas
    public AudioSource audioInitialFirstCard;
    public AudioSource audioCardOne;
    public AudioSource audioCardTwo;
    public AudioSource audioCardThree;
    public AudioSource audioCardFour;
    public AudioSource audioCardFive;
    public AudioSource audioCardSix;

    public static bool gameEnd = false; //Armazena se o jogo chegou ao fim. True == Acabou, False == Ñ Acabou

    public float counter = 0f; //Contador que armazenará o tempo decorrido de jogo
    public bool pause = false; //Variável de controle da audiodescrição inicial

    //Enum com os estados possíveis que o jogo pode estar
    public enum gameState {
        ACTIVE,
        IDLE,
        END
    };

    //Chamada exatamente uma vez quando o script é inicializado
    void Start() {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap("MemoriaMinigame");
        upAction = input.actions.FindAction("Up");
        downAction = input.actions.FindAction("Down");
        leftAction = input.actions.FindAction("Left");
        rightAction = input.actions.FindAction("Right");
        interactAction = input.actions.FindAction("Interact");
        pauseAction = input.actions.FindAction("Pause");
        gameEnd = false;
        randomize();
        activeScript = activeCard.GetComponent<CardScript>();
        state = gameState.ACTIVE; 
    }
    
    //Randomiza os pares para que o jogo seja diferente toda vez
    private void randomize() {
        int i;
        int j = 0;
        int n = allCards.Length;
        int r;
        GameObject temp;

        for(i = 0; i < (n-1); i++) {
            r = i + rand.Next(n - i);
            temp = allCards[r];
            allCards[r] = allCards[i];
            allCards[i] = temp;
        }

        //Cada carta é pareada com a carta seguinte, após o array ser embaralhado
        for(i = 0; i < n; i++) {
            if(i%2 == 0) {
                allCards[i].GetComponent<CardScript>().Soulmate = allCards[i+1];
                allCards[i+1].GetComponent<CardScript>().Soulmate = allCards[i];
                allCards[i].GetComponent<AudioSource>().clip = cardSounds[j];
                allCards[i+1].GetComponent<AudioSource>().clip = cardSounds[j];
                j++;
            }
        }
    }

    //Testa se existe uma carta na direção escolhida. Se houver, move o "cursor". Se não, faz som indicando que não dá pra ir pra lá
    private void testCard(string direction) {
        state = gameState.IDLE;

        switch (direction) {
            case "left":
                if(activeScript.getLeft() != null) {
                    activeCard = activeScript.getLeft();
                    activeScript = activeCard.GetComponent<CardScript>();
                    gettingPosition();
                    //activeScript.getMovedSound().Play();
                    Debug.Log("Andou para a esquerda\n");
                } else {
                    activeScript.getBumpSound().Play();
                }
                break;

            case "right":
                if(activeScript.getRight() != null) {
                    activeCard = activeScript.getRight();
                    activeScript = activeCard.GetComponent<CardScript>();
                    gettingPosition();
                    //activeScript.getMovedSound().Play();
                    Debug.Log("Andou para a direita\n");
                } else {
                    activeScript.getBumpSound().Play();
                }
                break;
            
            case "up":
                if(activeScript.getUp() != null) {
                    activeCard = activeScript.getUp();
                    activeScript = activeCard.GetComponent<CardScript>();
                    gettingPosition();
                    //activeScript.getMovedSound().Play();
                    Debug.Log("Andou para cima\n");
                } else {
                    activeScript.getBumpSound().Play();
                }
                break;

            case "down":
                if(activeScript.getDown() != null) {
                    activeCard = activeScript.getDown();
                    activeScript = activeCard.GetComponent<CardScript>();
                    gettingPosition();
                    activeScript.getMovedSound().Play();
                    Debug.Log("Andou para baixo\n");
                } else {
                    //activeScript.getBumpSound().Play();
                }
                break;

            default:
            break;
        }

        state = gameState.ACTIVE;
    }

    public void gettingPosition()
    {
        int position;
        if(activeScript.getLeft() == null && activeScript.getUp() == null){
            position = 1;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() != null && activeScript.getRight() != null && activeScript.getUp() == null){
            position = 2;
            playPositionAudio(position);
        }
        else if (activeScript.getRight() == null && activeScript.getUp() == null){
            position = 3;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() == null && activeScript.getDown() == null){
            position = 4;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() != null && activeScript.getRight() != null && activeScript.getDown() == null){
            position = 5;
            playPositionAudio(position);
        }
        else if (activeScript.getRight() == null && activeScript.getDown() == null){
            position = 6;
            playPositionAudio(position);
        }
    }

    public void playPositionAudio(int positionAudio){
        if(positionAudio == 1){
            audioCardOne.Play();
        }
        else if (positionAudio == 2){
            audioCardTwo.Play();
        }
        else if (positionAudio == 3){
            audioCardThree.Play();
        }
        else if (positionAudio == 4){
            audioCardFour.Play();
        }
        else if (positionAudio == 5){
            audioCardFive.Play();
        }
        else if (positionAudio == 6){
            audioCardSix.Play();
        }
    }

    //Função que abre a carta. Existem algumas possibilidades:
    // A carta está aberta. Nesse caso, toca uma deixa indicando que
    // a carta está aberta.

    // A carta está fechada, e não há nenhuma outra carta aberta e
    // não pareada. Nesse caso, abre a carta e toca o som dela.
    
    // A carta está fechada, e há outra carta aberta e não pareada.
    // Nesse caso, abre a carta e toca o som dela. Se a outra carta
    // aberta for o par da
    
    // carta atual, forma um par e toca a deixa de cartas pareadas.
    // Caso contrário, toca a deixa de erro e fecha as duas cartas.
    
    //Se as cartas forem pareadas, incrementa o valor de cartas
    //pareadas. Depois, caso todas as cartas tenham sido pareadas,
    //o jogo acaba.

    private void openCard() {
        AudioSource audio;

        //Verifica se a carta selecionada está aberta
        if(activeScript.getOpen()) {
            activeScript.getOpenSound().Play();
        } else {
            //Se não estiver, verifica se há outra carta aberta
            if(openCards == 0) {
                openCards = 1;
                audio = activeCard.GetComponent<AudioSource>();
                Debug.Log(audio);
                audio.Play();
                potentialPair = activeCard;
                activeScript.setOpen();
            }
            //Se houver, verifica se forma par. Se formar, entra nesse if
            else if(openCards == 1 && potentialPair == activeScript.getPair()) {
                StartCoroutine(playPair());
                pairedCards += 2;
                score += 1;
                scoreText.text = "Pares formados: " + score.ToString();
                openCards = 0;
                activeScript.setOpen();
                potentialPair.GetComponent<CardScript>().setOpen();
                potentialPair = null;
                //Colocar aqui o áudio informando que 1 par foi formado
            }
            //Se não formar, entra nesse if
            else if(openCards == 1 && potentialPair != activeScript.getPair()) {
                StartCoroutine(playNotPair());
                openCards = 0;
                activeScript.setClosed();
                potentialPair.GetComponent<CardScript>().setClosed();
                potentialPair = null;
                //Colocar aqui o áudio informando que o par está incorreto
            }
        }
    }

    //Função que toca os sons da carta sendo aberta e depois que o par foi formado
    private IEnumerator playPair() {
        AudioSource audio;
        state = gameState.IDLE;
        audio = activeCard.GetComponent<AudioSource>();
        audio.Play();

        while (audio.isPlaying) {
            yield return null;
        }

        activeScript.getCorrectSound().Play();

        while(audio.isPlaying) {
            yield return null;
        }

        //Vitória!
        if(pairedCards == TOTALCARDS) {
            victory.Play();
            state = gameState.END;
            //Colocar aqui o áudio da Vitória
            yield break;
        }

        state = gameState.ACTIVE;

        yield break;
    }

    //Função que toca os sons da carta sendo aberta e depois que o par não foi formado
    private IEnumerator playNotPair() {
        AudioSource audio;
        state = gameState.IDLE;
        audio = activeCard.GetComponent<AudioSource>();
        audio.Play();

        while (audio.isPlaying) {
            yield return null;
        }

        activeScript.getIncorrectSound().Play();

        state = gameState.ACTIVE;

        yield break;
    }

    private void win() {
        state = gameState.IDLE;
        scoreText.text = "Pares formados: " + score.ToString();
        //float auxCounter = Math.Round(counter);
        timeText.text = "Seu tempo foi de: " + Math.Round(counter, 1).ToString() + "s";
        youWinText.SetActive(true);
//        textInstructions.SetActive(true);
        gameEndSource.clip = pistaClip;
        StartCoroutine(endGame());
        gameEnd = true;

        /*scoreText.text = "Pares formados: " + score.ToString() + "\n\n\nParabéns! Você Ganhou!" +
            "\nAperte 1 para jogar novamente. Caso queira retornar ao Menu Principal, aperte 2.";*/
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
        PlayerController.jaVenceuMemoria = true;
        //Passa de volta para a cena principal
        SceneManager.LoadScene("Jogo");
        yield break;
    }

    //Chamada uma vez por frame
    void Update() {
        //Se estiver ativo, permite ao jogador escolher a carta e virar uma que esteja fechada
        if (state == gameState.ACTIVE)
        {
            if(gameEnd == false)
            {
                counter += Time.deltaTime;
            }
            //Movimentação do jogador pelas cartas
            if (leftAction.WasPressedThisFrame())
            {
                testCard("left");
            }

            if (rightAction.WasPressedThisFrame())
            {
                testCard("right");
            }

            if (upAction.WasPressedThisFrame())
            {
                testCard("up");
            }

            if (downAction.WasPressedThisFrame())
            {
                testCard("down");
            }

            //Quando o jogador quer selecionar uma carta
            if (interactAction.WasPressedThisFrame())
            {
                Debug.Log("Carta foi selecionada\n");
                openCard();
            }
        }

        if (pauseAction.WasPressedThisFrame() && pause == false){
            audioInitialFirstCard.Pause();
            pause = true;
        }
        if (pauseAction.WasPressedThisFrame() && pause == true)
        {
            audioInitialFirstCard.Play();
            pause = false;
        }

        //Se estiver esperando
        if (state == gameState.IDLE) {
        //não faz nada
        }

        //Se tiver terminado (separado de IDLE por legibilidade)
        if(state == gameState.END) {
            win();
        }
    }
}