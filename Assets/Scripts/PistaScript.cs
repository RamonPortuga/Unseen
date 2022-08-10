using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO
//Por algum motivo, o jogo da memória não está registrando como uma coisa com a qual o jogador pode interagir; se consertar isso, o resto dos problemas likely se resolvem
//First off: colocar o jogador onde ele interagiu com o Genius, em vez de reiniciar a cena completamente
//Second off: replicar o sucesso com o jogo da memória
//Third off: fazer o pedestal pingar quando o jogador está perto de novo

public class PistaScript : MonoBehaviour, IScript {

    public AudioClip dialogo; //Guarda a fala do objeto
    public AudioSource dialogoSource; // Componente AudioSource do próprio objeto
    public GameObject player; //Guarda o jogador para poder receber inputs dele
    PlayerController controller; //Guarda o script PlayerController do jogador
    PlayerInput playerInput; //Guarda os inputs do jogador
    InputAction interagirAction, skipAction; //Ações do jogador
    [SerializeField] private bool isPaused = false; //Guarda se o áudio está pausado ou não
    [SerializeField] private bool canPause = false; //Guarda se o áudio pode pausar ou não

    public IEnumerator lerDialogo() {
        //Evita que o jogador pause o áudio imediatamente por acidente
        StartCoroutine(delayCanPause());
        
        // Lê a fala do diálogo
        dialogoSource.clip = dialogo;
        dialogoSource.Play();

        //Só avança quando o diálogo não está mais falando
        while (dialogoSource.isPlaying || isPaused) {
            yield return null;
        }

        canPause = false;
        yield break;
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

    //Inicia a interação do jogador
    public void interagir() {
        StartCoroutine(lerDialogo());
    }

    //Funcionalizado para poder ser chamado por PlayerController
    public void skip() {
        StopCoroutine(lerDialogo());
        dialogoSource.Stop();
        isPaused = false;
        canPause = false;
    }

    void Start() {
        playerInput = player.GetComponent<PlayerInput>();
        interagirAction = playerInput.actions.FindAction("Interagir");
        skipAction = playerInput.actions.FindAction("Skip");
        controller = player.GetComponent<PlayerController>();
    }

    void Update() {
        if(interagirAction.WasPressedThisFrame() && !isPaused && canPause) {
            togglePause();
            dialogoSource.Pause();
        }
        if(interagirAction.WasPressedThisFrame() && isPaused && canPause) {
            togglePause();
            dialogoSource.UnPause();
        }
        if(skipAction.WasPressedThisFrame()) {
            skip();
        }
    }
}
