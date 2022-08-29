/*Código usado no "jogo principal", sendo aplicado nas Scene
 * "Game" e "GameJoystick"
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardTaskmasterScript : MonoBehaviour {
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
    public GameObject youLostText; //Usado pra indicar ao jogador que ele perdeu o jogo
    public GameObject textInstructions; //Usado para orientar o jogador sobre as opções após a vitória
    public AudioClip[] cardSounds; //Guarda os áudios dos pares
    private CardScript activeScript; //Guarda o CardScript da carta atual
    public AudioSource victory; //Som que toca quando o jogador vence
    public AudioSource Defeat; //Som que toca quando o jogador perde
    public Text scoreText; //Pontuação do jogador, em texto
    public Text timeText; //Tempo do jogador, em texto
    public GameObject[] buttons; //Guarda os botões de quando o jogo terminar

    //Segue abaixo as variáveis referentes aos áudios do posiconamento das cartas
    public AudioSource audioInitialFirstCard;
    public AudioSource audioCardOne;
    public AudioSource audioCardTwo;
    public AudioSource audioCardThree;
    public AudioSource audioCardFour;
    public AudioSource audioCardFive;
    public AudioSource audioCardSix;


    //segue abaixo as variáveis referentes aos áudios do tempo do jogador
    //áudios especiais
    public AudioSource minutoSound;
    public AudioSource pontoSound;
    public AudioSource segundosSound;
    public AudioSource tempoSound;
    //áudio das unidades
    public AudioSource umSound;
    public AudioSource doisSound;
    public AudioSource tresSound;
    public AudioSource quatroSound;
    public AudioSource cincoSound;
    public AudioSource seisSound;
    public AudioSource seteSound;
    public AudioSource oitoSound;
    public AudioSource noveSound;
    public AudioSource dezSound;
    //áudio das dezenas 
    public AudioSource vinteSound;
    public AudioSource trintaSound;
    public AudioSource quarentaSound;
    public AudioSource cinquentaSound;
    public AudioSource vinteESound;
    public AudioSource trintaESound;
    public AudioSource quarentaESound;
    public AudioSource cinquentaESound;
    //áudios entre 10 e 20
    public AudioSource onzeSound;
    public AudioSource dozeSound;
    public AudioSource trezeSound;
    public AudioSource quatorzeSound;
    public AudioSource quinzeSound;
    public AudioSource dezesseisSound;
    public AudioSource dezesseteSound;
    public AudioSource dezoitoSound;
    public AudioSource dezenoveSound;
    //áudio dos milésimos
    public AudioSource umMISound;
    public AudioSource doisMISound;
    public AudioSource tresMISound;
    public AudioSource quatroMISound;
    public AudioSource cincoMISound;
    public AudioSource seisMISound;
    public AudioSource seteMISound;
    public AudioSource oitoMISound;
    public AudioSource noveMISound;


    public static bool endGame = false; //Armazena se o jogo chegou ao fim. True == Acabou, False == Ñ Acabou

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
        endGame = false;
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

        for (i = 0; i < (n - 1); i++) {
            r = i + rand.Next(n - i);
            temp = allCards[r];
            allCards[r] = allCards[i];
            allCards[i] = temp;
        }

        //Cada carta é pareada com a carta seguinte, após o array ser embaralhado
        for (i = 0; i < n; i++) {
            if (i % 2 == 0) {
                allCards[i].GetComponent<CardScript>().Soulmate = allCards[i + 1];
                allCards[i + 1].GetComponent<CardScript>().Soulmate = allCards[i];
                allCards[i].GetComponent<AudioSource>().clip = cardSounds[j];
                allCards[i + 1].GetComponent<AudioSource>().clip = cardSounds[j];
                j++;
            }
        }
    }

    //Testa se existe uma carta na direção escolhida. Se houver, move o "cursor". Se não, faz som indicando que não dá pra ir pra lá
    private void testCard(string direction) {
        state = gameState.IDLE;

        switch (direction) {
            case "left":
                if (activeScript.getLeft() != null) {
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
                if (activeScript.getRight() != null) {
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
                if (activeScript.getUp() != null) {
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
                if (activeScript.getDown() != null) {
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
        if (activeScript.getLeft() == null && activeScript.getUp() == null) {
            position = 1;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() != null && activeScript.getRight() != null && activeScript.getUp() == null) {
            position = 2;
            playPositionAudio(position);
        }
        else if (activeScript.getRight() == null && activeScript.getUp() == null) {
            position = 3;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() == null && activeScript.getDown() == null) {
            position = 4;
            playPositionAudio(position);
        }
        else if (activeScript.getLeft() != null && activeScript.getRight() != null && activeScript.getDown() == null) {
            position = 5;
            playPositionAudio(position);
        }
        else if (activeScript.getRight() == null && activeScript.getDown() == null) {
            position = 6;
            playPositionAudio(position);
        }
    }

    public void playPositionAudio(int positionAudio) {
        if (positionAudio == 1) {
            audioCardOne.Play();
        }
        else if (positionAudio == 2) {
            audioCardTwo.Play();
        }
        else if (positionAudio == 3) {
            audioCardThree.Play();
        }
        else if (positionAudio == 4) {
            audioCardFour.Play();
        }
        else if (positionAudio == 5) {
            audioCardFive.Play();
        }
        else if (positionAudio == 6) {
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
        if (activeScript.getOpen()) {
            activeScript.getOpenSound().Play();
        } else {
            //Se não estiver, verifica se há outra carta aberta
            if (openCards == 0) {
                openCards = 1;
                audio = activeCard.GetComponent<AudioSource>();
                Debug.Log(audio);
                audio.Play();
                potentialPair = activeCard;
                activeScript.setOpen();
            }
            //Se houver, verifica se forma par. Se formar, entra nesse if
            else if (openCards == 1 && potentialPair == activeScript.getPair()) {
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
            else if (openCards == 1 && potentialPair != activeScript.getPair()) {
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

        while (audio.isPlaying) {
            yield return null;
        }

        //Vitória!
        if (pairedCards == TOTALCARDS) {
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

    private void lost()
    {
        state = gameState.IDLE;
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
        scoreText.text = "Pares formados: " + score.ToString();
        //float auxCounter = Math.Round(counter);
        timeText.text = "Tempo limite estourado!";
        youLostText.SetActive(true);
        textInstructions.SetActive(true);
        endGame = true;
        /*scoreText.text = "Pares formados: " + score.ToString() + "\n\n\nParabéns! Você Ganhou!" +
            "\nAperte 1 para jogar novamente. Caso queira retornar ao Menu Principal, aperte 2.";*/
    }

    //toca o áudio da pontuação se o jogador vencer o jogo
    private IEnumerator playTime()
    {
        int parteInt = (int)counter; //parte inteira
        float parteDec = (float) Math.Round(counter - parteInt, 1) ; //parte decimal arredondada para 1 casa
        float milesimo = parteDec * 10;

        //casas decimais da esquerda para a direita
        int pCasa = parteInt / 100; //primeira casa
        int sCasa = (parteInt - (pCasa*100)) / 10; //segunda casa 
        int tCasa = parteInt - ((pCasa * 100) + (sCasa * 10)); //terceira casa

        //casas decimais dos segundos
        int segundos = Math.Abs((int)counter - 60); //pega a diferença para falar os segundos //módolo
        int psegundos = segundos / 10; //primeira casa dos segundos (dezena)
        int ssegundos = segundos - (psegundos * 10); //segunda casa dos segundos (unidade)

        yield return new WaitForSeconds(11f);
        tempoSound.Play();
        //para o caso de um minuto ou mais
        if (sCasa >= 6)
        {
            yield return new WaitForSeconds(1.5f);
            minutoSound.Play();
            yield return new WaitForSeconds(1f);
            //se a segunda casa dos segundos (unidade) for 0, toca audio das dezenas
            if (ssegundos == 0)
            {
                switch (psegundos)
                {
                    case 1:
                        dezSound.Play();
                        break;
                    case 2:
                        vinteSound.Play();
                        break;
                    case 3:
                        trintaSound.Play();
                        break;
                    case 4:
                        quarentaSound.Play();
                        break;
                    case 5:
                        cinquentaSound.Play();
                        break;
                    case 0:
                        //toca nada
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1f);
                segundosSound.Play();
            }
            //senão a segunda casa (unidade) não for 0, toca os audios das dezenas e unidades 
            else
            {
                switch (psegundos)
                {
                    case 1:
                        if (ssegundos == 1) onzeSound.Play();
                        else if (ssegundos == 2) dozeSound.Play();
                        else if (ssegundos == 3) trezeSound.Play();
                        else if (ssegundos == 4) quatorzeSound.Play();
                        else if (ssegundos == 5) quinzeSound.Play();
                        else if (ssegundos == 6) dezesseisSound.Play();
                        else if (ssegundos == 7) dezesseteSound.Play();
                        else if (ssegundos == 8) dezoitoSound.Play();
                        else if (ssegundos == 9) dezenoveSound.Play();
                        break;
                    case 2:
                        vinteESound.Play();
                        if (ssegundos == 1) umSound.Play();
                        else if (ssegundos == 2) doisSound.Play();
                        else if (ssegundos == 3) tresSound.Play();
                        else if (ssegundos == 4) quatroSound.Play();
                        else if (ssegundos == 5) cincoSound.Play();
                        else if (ssegundos == 6) seisSound.Play();
                        else if (ssegundos == 7) seteSound.Play();
                        else if (ssegundos == 8) oitoSound.Play();
                        else if (ssegundos == 9) noveSound.Play();
                        break;
                    case 3:
                        if (ssegundos == 1) umSound.Play();
                        else if (ssegundos == 2) doisSound.Play();
                        else if (ssegundos == 3) tresSound.Play();
                        else if (ssegundos == 4) quatroSound.Play();
                        else if (ssegundos == 5) cincoSound.Play();
                        else if (ssegundos == 6) seisSound.Play();
                        else if (ssegundos == 7) seteSound.Play();
                        else if (ssegundos == 8) oitoSound.Play();
                        else if (ssegundos == 9) noveSound.Play();
                        break;
                    case 4:
                        quarentaESound.Play();
                        if (ssegundos == 1) umSound.Play();
                        else if (ssegundos == 2) doisSound.Play();
                        else if (ssegundos == 3) tresSound.Play();
                        else if (ssegundos == 4) quatroSound.Play();
                        else if (ssegundos == 5) cincoSound.Play();
                        else if (ssegundos == 6) seisSound.Play();
                        else if (ssegundos == 7) seteSound.Play();
                        else if (ssegundos == 8) oitoSound.Play();
                        else if (ssegundos == 9) noveSound.Play();
                        break;
                    case 5:
                        cinquentaESound.Play();
                        if (ssegundos == 1) umSound.Play();
                        else if (ssegundos == 2) doisSound.Play();
                        else if (ssegundos == 3) tresSound.Play();
                        else if (ssegundos == 4) quatroSound.Play();
                        else if (ssegundos == 5) cincoSound.Play();
                        else if (ssegundos == 6) seisSound.Play();
                        else if (ssegundos == 7) seteSound.Play();
                        else if (ssegundos == 8) oitoSound.Play();
                        else if (ssegundos == 9) noveSound.Play();
                        break;
                    case 0:
                        if (ssegundos == 1) umSound.Play();
                        else if (ssegundos == 2) doisSound.Play();
                        else if (ssegundos == 3) tresSound.Play();
                        else if (ssegundos == 4) quatroSound.Play();
                        else if (ssegundos == 5) cincoSound.Play();
                        else if (ssegundos == 6) seisSound.Play();
                        else if (ssegundos == 7) seteSound.Play();
                        else if (ssegundos == 8) oitoSound.Play();
                        else if (ssegundos == 9) noveSound.Play();
                        break;
                    default:
                        break;
                }
                yield return new WaitForSeconds(1f);
                segundosSound.Play();
            }

        }

        //se for menos que 1 minuto
        else
        {
            yield return new WaitForSeconds(2f);
            switch (sCasa)
            {
                case 1:
                    if (tCasa == 0) dezSound.Play();
                    else if (tCasa == 1) onzeSound.Play();
                    else if (tCasa == 2) dozeSound.Play();
                    else if (tCasa == 3) trezeSound.Play();
                    else if (tCasa == 4) quatorzeSound.Play();
                    else if (tCasa == 5) quinzeSound.Play();
                    else if (tCasa == 6) dezesseisSound.Play();
                    else if (tCasa == 7) dezesseteSound.Play();
                    else if (tCasa == 8) dezoitoSound.Play();
                    else if (tCasa == 9) dezenoveSound.Play();
                    break;
                case 2:
                    if (tCasa == 0) vinteSound.Play();
                    else
                    {
                        vinteESound.Play();
                        yield return new WaitForSeconds(1f);
                        if (tCasa == 1) umSound.Play();
                        else if (tCasa == 2) doisSound.Play();
                        else if (tCasa == 3) tresSound.Play();
                        else if (tCasa == 4) quatroSound.Play();
                        else if (tCasa == 5) cincoSound.Play();
                        else if (tCasa == 6) seisSound.Play();
                        else if (tCasa == 7) seteSound.Play();
                        else if (tCasa == 8) oitoSound.Play();
                        else if (tCasa == 9) noveSound.Play();
                    }
                    break;
                case 3:
                    if (tCasa == 0) trintaSound.Play();
                    else
                    {
                        trintaESound.Play();
                        yield return new WaitForSeconds(1f);
                        if (tCasa == 1) umSound.Play();
                        else if (tCasa == 2) doisSound.Play();
                        else if (tCasa == 3) tresSound.Play();
                        else if (tCasa == 4) quatroSound.Play();
                        else if (tCasa == 5) cincoSound.Play();
                        else if (tCasa == 6) seisSound.Play();
                        else if (tCasa == 7) seteSound.Play();
                        else if (tCasa == 8) oitoSound.Play();
                        else if (tCasa == 9) noveSound.Play();
                    }
                    break;
                case 4:
                    if (tCasa == 0) quarentaSound.Play();
                    else
                    {
                        quarentaESound.Play();
                        yield return new WaitForSeconds(1f);
                        if (tCasa == 1) umSound.Play();
                        else if (tCasa == 2) doisSound.Play();
                        else if (tCasa == 3) tresSound.Play();
                        else if (tCasa == 4) quatroSound.Play();
                        else if (tCasa == 5) cincoSound.Play();
                        else if (tCasa == 6) seisSound.Play();
                        else if (tCasa == 7) seteSound.Play();
                        else if (tCasa == 8) oitoSound.Play();
                        else if (tCasa == 9) noveSound.Play();
                    }
                    break;
                case 5:
                    if (tCasa == 0) cinquentaSound.Play();
                    else
                    {
                        cinquentaESound.Play();
                        yield return new WaitForSeconds(1f);
                        if (sCasa == 1) umSound.Play();
                        else if (sCasa == 2) doisSound.Play();
                        else if (sCasa == 3) tresSound.Play();
                        else if (sCasa == 4) quatroSound.Play();
                        else if (sCasa == 5) cincoSound.Play();
                        else if (sCasa == 6) seisSound.Play();
                        else if (sCasa == 7) seteSound.Play();
                        else if (sCasa == 8) oitoSound.Play();
                        else if (sCasa == 9) noveSound.Play();
                    }
                    break;
                case 0:
                    if (tCasa == 1) umSound.Play();
                    else if (tCasa == 2) doisSound.Play();
                    else if (tCasa == 3) tresSound.Play();
                    else if (tCasa == 4) quatroSound.Play();
                    else if (tCasa == 5) cincoSound.Play();
                    else if (tCasa == 6) seisSound.Play();
                    else if (tCasa == 7) seteSound.Play();
                    else if (tCasa == 8) oitoSound.Play();
                    else if (tCasa == 9) noveSound.Play();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1f);
            segundosSound.Play();
        }

        //parte decimal para os milésimos
        if(milesimo != 0)
        {
            yield return new WaitForSeconds(1f);
            pontoSound.Play(); //toca audio "ponto"
            yield return new WaitForSeconds(1f);
            switch (milesimo)
            {
                case 1:
                    umMISound.Play();
                    break;
                case 2:
                    doisMISound.Play();
                    break;
                case 3:
                    tresMISound.Play();
                    break;
                case 4:
                    quatroMISound.Play();
                    break;
                case 5:
                    cincoMISound.Play();
                    break;
                case 6:
                    seisMISound.Play();
                    break;
                case 7:
                    seteMISound.Play();
                    break;
                case 8:
                    oitoMISound.Play();
                    break;
                case 9:
                    noveMISound.Play();
                    break;
                default:
                    break;
            }
        }

    }

    private void win() {
        state = gameState.IDLE;
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
        scoreText.text = "Pares formados: " + score.ToString();
        //float auxCounter = Math.Round(counter);
        timeText.text = "Seu tempo foi de: " + Math.Round(counter, 1).ToString() + "s";
        youWinText.SetActive(true);
        textInstructions.SetActive(true);
        endGame = true;
        /*scoreText.text = "Pares formados: " + score.ToString() + "\n\n\nParabéns! Você Ganhou!" +
            "\nAperte 1 para jogar novamente. Caso queira retornar ao Menu Principal, aperte 2.";*/
    }

    //Chamada uma vez por frame
    void Update() {
        //Se a qualquer momento o jogador apertar esc, o jogo fecha
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }

        //Se estiver ativo, permite ao jogador escolher a carta e virar uma que esteja fechada
        if (state == gameState.ACTIVE)
        {
            if (endGame == false)
            {
                counter += Time.deltaTime;
            }
            if (SceneManager.GetActiveScene().name == "CardGame")
            {
                //Movimentação do jogador pelas cartas
                if (Input.GetKeyDown("a") || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    testCard("left");
                }

                if (Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    testCard("right");
                }

                if (Input.GetKeyDown("w") || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    testCard("up");
                }

                if (Input.GetKeyDown("s") || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    testCard("down");
                }

                //Quando o jogador quer selecionar uma carta
                if (Input.GetKeyDown("return"))
                {
                    Debug.Log("Carta foi selecionada\n");
                    openCard();
                }
            }
        }

        /*ALTERAR PARA BARRA DE ESPAÇO*/
        if (Input.GetKeyDown(KeyCode.Space) && pause == false) {
            audioInitialFirstCard.Pause();
            pause = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && pause == true)
        {
            audioInitialFirstCard.Play();
            pause = false;
        }

        //Se estiver esperando
        if (state == gameState.IDLE) {
            //não faz nada
        }

        //Se tiver terminado (separado de IDLE por legibilidade)
        if (state == gameState.END) {
            win();
            StartCoroutine(playTime());
            //playTime();
        }


        //se passar do teto de tempo (2min)
        if (counter >= 120)
        {
            lost();
        }

    }

}