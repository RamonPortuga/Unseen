using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{

    //Começa aqui as variáveis do MenuScript usado no Genius e Jogo da Memória
    bool pause = false;

    public AudioSource audioDescription;
    //Termina aqui as variáveis do MenuScript usado no Genius e Jogo da Memória

    [SerializeField] GameObject menuPanel, howToPanel; // Paineis do menu principal e das intruções

    // Objeto de texto dos indicadores de botão no menu
    [SerializeField] Text iniciarButtonText, instrucoesButtonText, testeButtonText, sairButtonText, voltarButtonText; 

    [SerializeField] AudioClip notaA, notaB, notaC, notaD; // Audios das notas musicais

    AudioSource audioSource; // Componente AudioSource onde os sons serão tocados

    public AudioSource audioSourceVoz; // Componente AudioSource de onde a voz sintetizada falará

    PlayerInput playerInput; // Componente PlayerInput para recebr entrada do teclado ou do controle

    // Objetos InputAction para cada ação que pode ser realizada no menu
    InputAction instrucoesVoltarAction, iniciarAction, instrucoesAction, testeAction, sairAction, repetirAction, repetirInstrucoesAction; 

    public AudioClip audioParaVozMenuInicial, audioParaVozInstrucoes; // Clipes de audio para a voz sintetizada

    void Start()
    {
        // Preenche o playerInput e as ações de input
        playerInput = GetComponent<PlayerInput>();
        instrucoesVoltarAction = playerInput.actions.FindAction("InstrucoesVoltar");
        iniciarAction = playerInput.actions.FindAction("Iniciar");
        instrucoesAction = playerInput.actions.FindAction("Instrucoes");
        testeAction = playerInput.actions.FindAction("Teste");
        sairAction = playerInput.actions.FindAction("Sair");
        repetirAction = playerInput.actions.FindAction("Repetir");
        repetirInstrucoesAction = playerInput.actions.FindAction("RepetirInstrucoes");

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pause == false)
                {
                    audioDescription.Pause();
                    pause = true;
                }
                else
                {
                    audioDescription.Play();
                    pause = false;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Instructions")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pause == false)
                {
                    audioDescription.Pause();
                    pause = true;
                }
                else
                {
                    audioDescription.Play();
                    pause = false;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Tester")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pause == false)
                {
                    audioDescription.Pause();
                    pause = true;
                }
                else
                {
                    audioDescription.Play();
                    pause = false;
                }
            }
        }
    }
}
