using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Informante : MonoBehaviour {
    private AudioClip[] perguntas = new AudioClip[4]; //Guarda as perguntas que o jogador pode fazer; 4 é sair
    private AudioClip[] respostas = new AudioClip[5]; //Guarda as respostas que o jogo pode dar; 4 é o jogo perguntando se tem mais alguma pergunta,
                                                      //5 é a saudação inicial
    public AudioSource fonte; //Alto-falante que toca os sons
    private int pergunta = -1; //Guarda qual pergunta o jogador quer fazer. Começa em -1, e depois varia de 0 a 4
    private informanteState currentState; //Guarda o estado atual do informante

    //Enum que contém os estados possíveis do informante
    private enum informanteState {
        PERGUNTANDO,
        RESPONDENDO,
    };

    //Toca a saudação inicial quando o script é disparado
    private IEnumerator playGreeting() {
        //Repostas[5] guarda a saudação inicial que o informante dá ao jogador quando abordado pela primeira vez
        fonte.clip = respostas[5];

        fonte.Play();
        while (fonte.isPlaying) {
            yield return null;
        }

        currentState = informanteState.PERGUNTANDO;
        yield break;
    }

    //O jogador faz uma pergunta, e esse IEnumerator responde. O jogador não pode perguntar mais coisas enquanto o jogo estiver falando
    private IEnumerator playAnswer() {
        currentState = informanteState.RESPONDENDO;

        //Responde à pergunta feita
        fonte.clip = respostas[pergunta];
        fonte.Play();
        while (fonte.isPlaying) {
            yield return null;
        }

        //Pergunta se há mais alguma dúvida
        fonte.clip = respostas[4];
        fonte.Play();
        while (fonte.isPlaying) {
            yield return null;
        }

        //Devolve a vez para o jogador, para este poder escolher a próxima pergunta
        currentState = informanteState.PERGUNTANDO;
        yield break;
    }

    //Chamada uma vez quando o script é inicializado
    private void Start() {
        currentState = informanteState.RESPONDENDO;
        StartCoroutine(playGreeting());
    }

    //Função que interrompe o script quando o jogador quiser sair
    private void stop() {
        this.enabled = false;
    }

    //Chamada uma vez por frame
    private void Update() {
        //Se o jogo não estiver explicando alguma coisa, o jogador pode fazer uma nova pergunta
        if (currentState == informanteState.PERGUNTANDO) {
            //As perguntas são organizadas num array de 4 posições. Para selecionar qual pergunta o jogador quer fazer, usam-se as teclas
            //para cima e para baixo
            if (Input.GetKeyDown("down") && pergunta <= 4) {
                pergunta++;
                fonte.clip = perguntas[pergunta];
                fonte.Play();
            }

            if (Input.GetKeyDown("up") && pergunta >= 0) {
                pergunta--;
                fonte.clip = perguntas[pergunta];
                fonte.Play();
            }

            //Quando o jogador faz uma pergunta, se for de fato uma pergunta, o jogo responde. Isso porque a posição 4 do array guarda a
            //opção de sair do diálogo com o informante
            if (Input.GetKeyDown("return") && pergunta != 4) {
                StartCoroutine(playAnswer());
            }

            //Ao escolher a pergunta 4, o jogador sai do diálogo com  informante
            if (Input.GetKeyDown("return") && pergunta == 4) {
                stop();
            }
        }
        
        //Enquanto o jogo está explicando alguma coisa para o jogador, este não pode fazer mais perguntas
        if (currentState == informanteState.RESPONDENDO) {
            //não faz nada
        }
    }
}