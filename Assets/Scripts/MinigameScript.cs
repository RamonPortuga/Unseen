using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameScript : MonoBehaviour, IScript {
    public string minigameName, minigameMapName; //Guarda o nome da cena do minigame e do mapa de input para serem trocados
    public bool jaVenceu = false; //Guarda se o jogador já venceu; se tiver vencido, apenas devolve a pista em vez de carregar o minigame
    public PistaScript pista; //Guarda a pista que o jogador recebe ao vencer o jogo
    public GameObject player; //Guarda o jogador para poder receber inputs dele
    PlayerInput playerInput; //Guarda os inputs do jogador

    //Chamada uma vez na inicialização do script
    void Start() {
        playerInput = player.GetComponent<PlayerInput>();
    }

    void Update()
    {

    }

    //Não faz nada
    public void skip() {

    }

    //Se o jogador já venceu o minigame, ele é tratado como uma pista. Dispara a pista associada. Caso contrário, inicia o jogo
    public void interagir() {

        
        jaVenceu = false;
        if(minigameName == "Genius") {
            SceneManager.LoadScene("Menu");
            //jaVenceu = PlayerController.jaVenceuGenius;
        }
        if(minigameName == "Memoria") {
            SceneManager.LoadScene("Menu");
            //jaVenceu = PlayerController.jaVenceuMemoria;
        }
        if(jaVenceu) {
            pista.interagir();
            return;
        }
        playerInput.SwitchCurrentActionMap(minigameMapName);
        SceneManager.LoadScene(minigameName);
        
    }
}
