using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Game Manager feito com design de Singleton, isto é, a classe não é estática, mas só pode haver um Game Manager instanciado 
    
    private static GameManager _gm; // Instancia desse GameManager

    public static GameManager GM { get {return _gm;} } // Getter para a instancia desse GM

    [SerializeField] private int gridSizeX, gridSizeY; // Dimensões da grid onde o jogador pode navegar

    public Text mensagemText; // Painel de texto onde as mesagens seram apresentadas

    public ObjetoInteragivel[] obras; // As obras contidas na sala
    public ObjetoInteragivel[] objetos; //Objetos contidos na sala. Podem ser pessoas ou objetos mesmo

    public int indexObraFalsa; // Indice da obra falsificada

    //private IEnumerator coroutineFadeMsg; // Objeto para a corotina de fade-out das mensagens
    
    // Getters para as dimensões do grid da sala
    public int getGridSizeX { get {return gridSizeX;}}
    public int getGridSizeY { get {return gridSizeY;}}

    public AudioSource audioSourceVoz; // Componente AudioSource de onde a voz sintetizada falará

    Queue<AudioClip> filaClipes; // Fila de clipes para a voz sintetizada tocar em sequência

    private System.Random rand = new System.Random(); //Gera valores aleatórios

    public GameObject player; //Guarda o jogador
    PlayerController controller; //Guarda o script do jogador, para evitar sobreposição de áudios
    PlayerInput playerInput; //Guarda os inputs do jogador
    InputAction interagirAction, skipAction; //Guarda ações do jogador
    [SerializeField] bool isPaused = false; //Guarda se está pausado
    [SerializeField] bool canPause = false; //Guarda se pode pausar

    void Awake() {
        // Grante que exista apenas um objeto instanciado da classe
        if(_gm != null && _gm != this) Destroy(this.gameObject);
        else _gm = this;
    }

    void Start() {
        controller = player.GetComponent<PlayerController>();
        playerInput = player.GetComponent<PlayerInput>();
        interagirAction = playerInput.actions.FindAction("Interagir");
        skipAction = playerInput.actions.FindAction("Skip");

        // Inicia o objeto de corotina como null
        //coroutineFadeMsg = null;

        // Preenche o AudioSource para a voz sintetizada
        audioSourceVoz = GetComponent<AudioSource>();

        // Inicia a fila de clipes
        filaClipes = new Queue<AudioClip>();

        // Inicia a corrotina para tocar os clipes enfileirados
        StartCoroutine( "TocaFilaDeClipes" ); 

    }

    //DRY Principle
    private void togglePause() {
        canPause = false;
        isPaused = !isPaused;
        StartCoroutine(delayCanPause());
    }

    //Permite a troca de pausado para despausado e vice versa
    private IEnumerator delayCanPause() {
        yield return new WaitForSeconds(0.1f);
        canPause = true;
    }

    void Update() {
        if(interagirAction.WasPressedThisFrame() && !canPause) {
            StartCoroutine(delayCanPause());
        }
        if(interagirAction.WasPressedThisFrame() && !isPaused && canPause) {
            togglePause();
            audioSourceVoz.Pause();
        }
        if(interagirAction.WasPressedThisFrame() && isPaused && canPause) {
            togglePause();
            audioSourceVoz.UnPause();
        }
        if(audioSourceVoz.isPlaying && skipAction.WasPressedThisFrame()) {
            InterromperVoz();
        }
    }

    //Garante que não vai tentar pausar e despausar na mesma frame
    private IEnumerator delayPause() {
        yield return new WaitForSeconds(0.1f);
        isPaused = !isPaused;
    }

    /*
    Comentado por não servir propósito prático por enquanto
    //Randomiza as posições dos objetos presentes na sala. Randomiza a partir de um array no qual os objetos a serem espalhados estão contidos.
    //Guarda as posições iniciais de cada objeto, e depois randomiza os objetos no vetor. Então, os objetos no vetor recebem novas posições
    //que foram guardadas. Essas posições guardadas não necessariamente são as que os objetos possuíam antes por conta do vetor ter sido
    //randomizado
    private void shuffle(GameObject[] arr) {
        GameObject temp;
        Transform[] positions;
        int i;
        int j = 0;
        int n = arr.Length;
        int r;

        //Guarda as posições para a randomização
        for(i = 0; i < n ; i++) {
            positions[i] = arr[i].transform.position;
        }

        //Realiza a randomização
        for(i = 0; i < (n-1) ; i++) {
            r = rand.Next(n - i);
            temp = arr[r];
            arr[r] = arr[i];
            arr[i] = temp;
        }

        //Randomiza as posições dos objetos no array
        for(i = 0 ; i < n ; i++) {
            arr[i].transform.position = positions[i];
        }
    }
    */

    // Interrompe a voz sintetizada e esvazia a fila de clipes
    public void InterromperVoz() {
        canPause = false;
        isPaused = false;
        filaClipes.Clear();
        audioSourceVoz.Stop();
    }

    // Método para a voz sintetizada falar um clipe de audio
    public void FalarMensagem( AudioClip clip ) {
        InterromperVoz();
        audioSourceVoz.clip = clip;
        audioSourceVoz.Play();
    }

    // Método para enfileirar mensagem na fila de clipes
    public void EnfileirarMensagem( AudioClip clip ) {
        filaClipes.Enqueue( clip );
    }

    // Método para a voz sintetizada fala um clipe e mostrar 
    // uma mensagem na tela por um tempo determinado
    public void MandarMensagem( string mensagem, AudioClip clip, float tempo ) {   
        
        // Faz a voz sintetizada ler a mensagem
        FalarMensagem( clip );

        /*
        // Altera o texto da caixa da mensagem e muda a opacidade para 1
        mensagemText.text = mensagem;
        mensagemText.color = new Color(mensagemText.color.r, mensagemText.color.g, mensagemText.color.b, 1f);
        
        // Se já existe uma corotina de fade-out rodando, para ela 
        if(coroutineFadeMsg != null) StopCoroutine(coroutineFadeMsg);

        // Chama uma nova corotina para a mensagem desaparecer 
        // com um fade-out após o tempo determinado
        coroutineFadeMsg = MensagemFade(tempo);
        StartCoroutine(coroutineFadeMsg);
        */
    }

    // Método para mostrar mensagem na tela por um tempo 
    // calculado baseado no tamanho do clipe
    public void MandarMensagem( string mensagem, AudioClip clip ) {
        MandarMensagem( mensagem, clip, clip.length );
    }

    // Corrotina para o efeito de fade-out da mensagem após certo tempo
    IEnumerator MensagemFade( float tempo ) {
        yield return new WaitForSeconds(tempo);

        float timer = 0;
        while(timer < 0.5f)
        {
            mensagemText.color = new Color(mensagemText.color.r, mensagemText.color.g, mensagemText.color.b, 1f - (timer)/0.5f);
            timer += Time.deltaTime;
            yield return null;
        }
        mensagemText.color = new Color(mensagemText.color.r, mensagemText.color.g, mensagemText.color.b, 0f);
    }

    // Corrotina para tocar os clipes enfileirados em sequência
    IEnumerator TocaFilaDeClipes() {

        while( true ) {
            if( !audioSourceVoz.isPlaying && filaClipes.Count != 0 ) {
                AudioClip prox_clipe = filaClipes.Dequeue();
                audioSourceVoz.clip = prox_clipe;
                audioSourceVoz.Play();
            }
            yield return null;
        }
    }
}
