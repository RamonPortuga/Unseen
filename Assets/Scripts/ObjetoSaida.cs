using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ObjetoSaida : MonoBehaviour, IInteragivel
{
    public GameObject painelSaida; // Painel do menu da porta de saída
    public GameObject confirmacaoSaida;
    public MenuFinal menuFinal; // Script que lida com o menu de fim de jogo
    public Text textSelecaoDeObra; // Texto da seleção de obras
    public AudioClip clipeMenuSaida; // Clipe do menu de saída para a voz sintetizada
    PlayerInput playerInput; // Componente PlayerInput que pega a entrada
    InputAction selecUpAction, selecDownAction, confirmAction, backAction; // Componentes InputAction para os controles do menu da saída
    int selectedIndex; // Indice da obra selecionada atualmente
    bool avancaParaOFinal = false;

    public bool continuar()
    {
        avancaParaOFinal = true;
        return avancaParaOFinal;
    }

    public bool voltar()
    {
        avancaParaOFinal = false;
        return avancaParaOFinal;
    }

    public void controle()
    {
        confirmacaoSaida.SetActive(true);
    }

    // Método que será chamado quando o jogador interagir com o objeto
    public void Interagir() {

        confirmacaoSaida.SetActive(true);

        if (avancaParaOFinal)
        {
            confirmacaoSaida.SetActive(false);
            // Ativa o painel do menu da saída
            painelSaida.SetActive(true);

            // Habilita esse script
            enabled = true;

            // Inicia com a primeira obra sendo selecionada
            selectedIndex = 0;

            // Preenche o playerInput
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

            // Muda o ActionMap atual para o ActionMap do menu de saída
            playerInput.SwitchCurrentActionMap("MenuSaida");

            // Preenche os InputAction usados pelo script
            selecUpAction = playerInput.actions.FindAction("SelectionUp");
            selecDownAction = playerInput.actions.FindAction("SelectionDown");
            confirmAction = playerInput.actions.FindAction("Confirm");
            backAction = playerInput.actions.FindAction("Back");

            // Faz a voz sintetizada falar o clipe do menu da saída
            GameManager.GM.FalarMensagem(clipeMenuSaida);
        }
        else
        {
            confirmacaoSaida.SetActive(false);
        }
    }

    public void Update() {
        // Atualiza o texto que indica a obra selecionada
        textSelecaoDeObra.text = GameManager.GM.obras[selectedIndex].nome;

        // Verifica os ações de input e chama a função correspondente
        if( selecUpAction.WasPressedThisFrame() ) {
            SelectionUp();
        } else if( selecDownAction.WasPressedThisFrame() ) {
            SelectionDown();
        } else if( confirmAction.WasPressedThisFrame() ) {
            ConfirmarSelecao();
        } else if( backAction.WasPerformedThisFrame() ) {
            Voltar();
        }
    }

    // Seleciona a próxima obra
    public void SelectionUp() {
        // Aumenta o índice da obra selecionada e aplica a função módulo para que
        // o índice fique no intervalo [0, n), sendo n o número total de obras
        selectedIndex = aux_mod(selectedIndex + 1, GameManager.GM.obras.Length);
        GameManager.GM.FalarMensagem(GameManager.GM.obras[selectedIndex].clipeDoNome ); // Faz a voz dizer o nome da obra selecionada
    }

    // Seleciona a obra anterior
    public void SelectionDown() {
        // Diminui o índice da obra selecionada e aplica a função módulo para que
        // o índice fique no intervalo [0, n), sendo n o número total de obras
        selectedIndex = aux_mod(selectedIndex - 1, GameManager.GM.obras.Length);
        GameManager.GM.FalarMensagem(GameManager.GM.obras[selectedIndex].clipeDoNome ); // Faz a voz dizer o nome da obra selecionada
    }

    // Confirma seleção da obra
    public void ConfirmarSelecao() {
        // Guarda uma booleana indicando se o jogador 
        // acertou ou não e o nome e clipe da obra falsa 
        string nome_obra_falsa = GameManager.GM.obras[GameManager.GM.indexObraFalsa].nome;
        AudioClip clipe_nome_obra_falsa = GameManager.GM.obras[GameManager.GM.indexObraFalsa].clipeDoNome;
        bool acertou = (selectedIndex == GameManager.GM.indexObraFalsa);
        
        // Desabilita o painel do menu de saída
        painelSaida.SetActive(false);

        // Desabilita esse script
        enabled = false;

        // Inicia o menu de fim de jogo
        menuFinal.StartMenu(nome_obra_falsa, clipe_nome_obra_falsa, acertou);

    }

    //Não faz nada
    public void skip() {}

    public void Voltar() {
        // Desabilita o painel do menu de saída
        painelSaida.SetActive(false);

        // Muda o ActionMap atual para o ActionMap do jogo
        playerInput.SwitchCurrentActionMap("Jogo");

        // Desabilita esse script
        enabled = false;
    }

    // Função módulo auxiliar. Retorna número positivo com entrada negativa.
    int aux_mod(int x, int m) { 
        return (x % m + m) % m; 
    }
}
