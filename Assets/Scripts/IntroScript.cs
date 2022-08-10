using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IntroScript : MonoBehaviour {

    public AudioClip[] intro; // Vetor com os diálogos do objeto
    public AudioClip falaAtual; // Fala que o objeto está lendo atualmente
    public AudioSource introSource; // Componente AudioSource do próprio objeto
    public GameObject player; //Guarda o jogador para poder receber inputs dele
    PlayerInput playerInput; //Guarda os inputs do jogador
    InputAction interagirAction, skipAction; //Ações do jogador
    public GameObject introSphere; //Guarda a própria IntroSphere para deleção, caso necessário
    [SerializeField] private bool isPaused = false; //Guarda se o áudio está pausado
    [SerializeField] private bool canPause = false; //Guarda se o áudio pode ser pausado
    PlayerController controller; //Guarda o script PlayerController do jogador

    void Start() {
        playerInput = player.GetComponent<PlayerInput>();
        interagirAction = playerInput.actions.FindAction("Interagir");
        skipAction = playerInput.actions.FindAction("Skip");
        controller = player.GetComponent<PlayerController>();
        if(!PlayerController.jaVenceuGenius && !PlayerController.jaVenceuMemoria) {
            StartCoroutine(lerIntro());
        }
    }

    private IEnumerator lerIntro() {
        StartCoroutine(delayCanPause());
        
        // Lê as falas da intro, uma de cada vez
        for(int i = 0; i < intro.Length; i++) {
            falaAtual = intro[i];
            introSource.clip = falaAtual;
            introSource.Play();

            while(introSource.isPlaying || isPaused) {
                yield return null;
            }
        }

        yield break;
    }
    
    //Garante que não vai tentar pausar e despausar na mesma frame
    private IEnumerator delayCanPause() {
        yield return new WaitForSeconds(0.1f);
        canPause = true;
    }

    //DRY Principle
    private void togglePause() {
        canPause = false;
        isPaused = !isPaused;
        StartCoroutine(delayCanPause());
    }

    void Update() {
        if(interagirAction.WasPressedThisFrame() && !isPaused && canPause) {
            togglePause();
            introSource.Pause();
        }
        if(interagirAction.WasPressedThisFrame() && isPaused && canPause) {
            togglePause();
            introSource.UnPause();
        }
        if(skipAction.WasPressedThisFrame()) {
            Destroy(introSphere);
        }
    }
}
