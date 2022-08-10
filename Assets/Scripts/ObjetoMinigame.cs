using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoMinigame : MonoBehaviour, IInteragivel
{   
    // Classe de um objeto que inicia um minigame quando é interagido

    PlayerInput playerInput; // Componente PlayerInput que pega a entrada
    public IMinigame minigameScript; //Componente IMinigame que guarda o script do minigame

    // Roda uma vez no inicio do jogo
    void Start() {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // Método que será chamado quando o jogador interagir com o objeto
    public void Interagir() {

        // Muda o ActionMap atual para o ActionMap usado pelo minigame
        playerInput.SwitchCurrentActionMap(minigameScript.ActionMapName());

        // Inicia o minigame
        minigameScript.StartGame();
    }

    //Não faz nada
    public void skip() {}
}
