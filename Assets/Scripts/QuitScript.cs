using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitScript : MonoBehaviour {
    public GameObject player; //Guarda o jogador
    PlayerInput playerInput; //Guarda os inputs do jogador
    PlayerController controller; //Guarda o controlador do jogador
    InputAction quitAction, cancelAction; //Guarda as ações possíveis
    public AudioSource quitSource; //AudioSource do próprio script
    public GameObject dis; //Guarda a própria quitSphere para poder ser desabilitada caso necessário

    // Start is called before the first frame update
    void Start() {
        playerInput = player.GetComponent<PlayerInput>();
        quitAction = playerInput.actions.FindAction("Quit");
        cancelAction = playerInput.actions.FindAction("Cancel");
        controller = player.GetComponent<PlayerController>();
        quitSource.Play();
    }

    // Update is called once per frame
    void Update() {
        if(quitAction.WasPressedThisFrame()) {
            Application.Quit();
        }
        if(cancelAction.WasPressedThisFrame()) {
            controller.canMoveH = true;
            controller.canMoveV = true;
            playerInput.SwitchCurrentActionMap("Jogo");
            dis.SetActive(false);
        }
    }
}
