/*Esse código é utilizado para rodar os casos de teste 
 * presentes na scene Tester no jogo */

using System.Collections;
using UnityEngine;

public class GeniusTesterScript : MonoBehaviour {

    public GameObject[] testerArray; //Guarda os testadores
    private int[] testOrder = new int[] {0, 2, 2, 1, 1, 1}; //Guarda a ordem do teste
    private AudioSource currentAudio; //Usado para tocar os sons
    public bool pause = true;
    public bool end = false;
    float countdown = 0f;

    //Chamada exatamente uma vez ao iniciar o script
    void Start() {
        
    }
    //Função responsável por executar a sequência de sons
    private IEnumerator testerSounds() {

        for (int i = 0; i < 6; i++) {
            currentAudio = testerArray[ testOrder[i] ].GetComponent<AudioSource>();

            currentAudio.Play();

            while (currentAudio.isPlaying) {
                yield return null;
            }
        }

        yield break;

    }
    private void Update()
    {
        //O countdown serve para cronometrar o tempo em ordem descrescente.
        //Logo, ao passar dos 28s, ele entrará no segundo if e executará
        //a sequência de testes.

        //OBS: O motivo para ser 28s é o fato da audiodescrição durar 26s
        //Afinal, a sequência de testes só executará após o término da mesma
        pause = GeniusMenuScript.pause;
        //Debug.Log(countdown);
        if (pause == false)
        {
            countdown += Time.deltaTime;
        }

        if (countdown >= 28 && end == false)
        {
            StartCoroutine(testerSounds());
            end = true;
        }
        
    }

}
