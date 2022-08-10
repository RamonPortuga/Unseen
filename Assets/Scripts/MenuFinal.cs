using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuFinal : MonoBehaviour
{   
    public Text textObraFalsa, textResultado; // Textos usados pelo menu de fim de jogo
    public AudioClip clipeFimDeJogo, clipeDeAcerto, clipeDeErro; // Clipes de audio para a voz sintetizada
    PlayerInput playerInput; // Componente PlayerInput que pega a entrada
    InputAction replayAction, menuAction; // Componentes InputAction para os controles do menu de fim de jogo

    // Chamado para iniciar o menu de fim de jogo
    public void StartMenu( string nome_obra_falsa, AudioClip clipe_nome_obra_falsa, bool acertou ) {
        // Preenche o playerInput
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        
        // Muda o ActionMap atual para o ActionMap do menu de saída
        playerInput.SwitchCurrentActionMap("MenuFinal");

        // Preenche os InputAction usados pelo script
        replayAction = playerInput.actions.FindAction("Replay");
        menuAction = playerInput.actions.FindAction("Menu");

        // Habilita o painel do menu
        gameObject.SetActive(true);

        // Atualiza os textos do menu
        textObraFalsa.text = "A obra falsa era: " + nome_obra_falsa;
        GameManager.GM.InterromperVoz();
        GameManager.GM.EnfileirarMensagem( clipeFimDeJogo );
        GameManager.GM.EnfileirarMensagem( clipe_nome_obra_falsa );
        if( acertou ) { 
            textResultado.text = "Parabéns! Você acertou!";
            GameManager.GM.EnfileirarMensagem( clipeDeAcerto ); 
        } else {
            textResultado.text = "Infelizmente, você errou.";
            GameManager.GM.EnfileirarMensagem( clipeDeErro );
        }
    }

    void Update() {
        // Verifica os ações de input e chama a função correspondente
        if( replayAction.WasPressedThisFrame() ) {
            // Reinicia o jogo
            SceneManager.LoadScene("Jogo");
        } else if( menuAction.WasPressedThisFrame() ) {
            // Volta ao menu principal
            SceneManager.LoadScene("Menu");
        }
    }
}
