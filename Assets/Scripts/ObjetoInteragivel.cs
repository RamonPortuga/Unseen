using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoInteragivel : MonoBehaviour, IInteragivel
{
    // Classe de um objeto que mostra uma mensagem quando é interagido
    
    public string nome; // Nome da obra

    public AudioClip clipeDoNome; // Clipe do nome da obra para a voz sintetizada

    [TextArea(5,10)] public string mensagem; // Mensagem a ser mostrada

    public AudioClip clipeDaMensagem; // Clipe da mensagem para a voz sintetizada

    [Tooltip("Deixe vazio para não receber nenhum item.")]
    [SerializeField] private string itemRecebido; // Item que será recebido após a interação (Inútil por enquanto)

    // Método que será chamado quando o jogador interagir com o objeto
    public virtual void Interagir() {
        GameManager.GM.MandarMensagem( mensagem, clipeDaMensagem ); // Faz o GameManager mostrar a mensagem para o jogador
    }

    public void skip() {
        GameManager.GM.InterromperVoz();
    }
}
