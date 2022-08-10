using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 initialPosition; // Posição inicial do jogador

    // Notas que serão tocadas com os movimentos do jogador
    [SerializeField] private AudioClip notaA, notaB, notaC, notaD, notaParede; 

    public AudioSource audioSource; // Componente AudioSource de onde serão tocadas as notas musicais

    int coordX, coordY; // Coordenadas atuais do jogador no grid

    public bool canMoveH, canMoveV; // Booleanas auxiliares que impedem o jogador de se mover rapidamente em sequência

    public PlayerInput playerInput; // Componente PlayerInput para receber o input

    InputAction interagirAction, movimentoAction, pauseAction, quitAction; // Ações de input do jogador

    GamepadRumble rumble; // Objeto auxiliar para a vibração do gamepad

    // Usados com o radar
    public GameObject[] interactableObj; // Guarda os objetos interatíveis para usar as suas audio sources no radar posicional
    public AudioSource radarSource; // Componente AudioSource para o radar não interferir com a AudioSource primária
    public AudioSource avisoSource; // Componente AudioSource para o aviso não interferir com as outras AudioSources
    public AudioSource stepSource; // Componente AudioSource para o som de passos não interferir com as outras AudioSources
    //bool radarIsRunning = false; // Guarda se o radar está ativo ou não
    public GameObject quitSphere; //Segura o script para fechar o jogo
    [SerializeField] private IScript currentPista; //Guarda a pista atual
    [SerializeField] private IInteragivel currentInteragivel; //Guarda o IInteragivel atual
    public static bool jaVenceuGenius = false;
    public static bool jaVenceuMemoria = false; //Usados para passar informação de uma cena para outra

    void Start()
    {
        // Preenche o objeto de vibração do controle e os objetos de input
        rumble = GetComponent<GamepadRumble>();
        playerInput = GetComponent<PlayerInput>();
        interagirAction = playerInput.actions.FindAction("Interagir");
        movimentoAction = playerInput.actions.FindAction("Movimento");
        pauseAction = playerInput.actions.FindAction("Pause");
        //radarAction = playerInput.actions.FindAction("Radar");
        quitAction = playerInput.actions.FindAction("Quit");

        transform.position = initialPosition; // Coloca o jogador na posição inicial

        // Inicias as booleanas auxiliares como true
        canMoveH = true;
        canMoveV = true;

        // Inicia as coordenadas atuais do jogador como (0, 0)
        coordX = 0;
        coordY = 0;
    }

    // Troca o mapa de input para o mapa "Jogo" e impede o jogador de se mover logo após a mudança
    public void MudaMapaJogo() {
        canMoveH = false;
        canMoveV = false;
        playerInput.SwitchCurrentActionMap("Jogo");
    }

    //Troca o mapa de input para o mapa "Quit" e impede o jogador de se mover logo após a mudança
    public void MudaMapaQuit() {
        canMoveH = false;
        canMoveV = false;
        playerInput.SwitchCurrentActionMap("Quit");
        quitSphere.SetActive(true);
    }

    // Trata do input de movimento
    public Vector2 TratarInput( Vector2 input ) {

        // No caso do input vir do analogico do controle, ele é float entre -1 e 1
        // Se o valor absoluto do input for muito próx. de 1 ( > 0.95f ) então 
        // transforma em 1 ou -1, dependendo do sinal do input original
        float hIn = Mathf.Abs(input.x) > 0.95f ? 1f * Mathf.Sign(input.x) : input.x;
        float vIn = Mathf.Abs(input.y) > 0.95f ? 1f * Mathf.Sign(input.y) : input.y;
        
        // Permite que o jogador possa mover denovo somente se os valores
        // absolutos dos inputs forem menores que 0.5
        // Isso é feito para evitar que o jogador consiga dar vários passos
        // rapidamente ao segurar o analogico em uma direção
        if(Mathf.Abs(hIn) < 0.5f) canMoveH = true;
        if(Mathf.Abs(vIn) < 0.5f) canMoveV = true;
        
        // Transforma os inputs em 0 caso o jogador não possa se mover
        hIn = canMoveH ? hIn : 0f;
        vIn = canMoveV ? vIn : 0f;

        return new Vector2(hIn, vIn);
    }

    /*
    //Executa o radar. É chamada quando o jogador aperta o botão do radar
    private IEnumerator radar() {
        radarIsRunning = true; //Tranca o radar para que ele não possa estar rodando mais de uma vez simultaneamente

        // Para cada objeto que não foi interagido ainda, sua AudioSource é tocada para indicar ao jogador que há algo naquela direção a ser feito
        for(int i = 0; i < interactableObj.Length; i++) {
            radarSource.transform.position = interactableObj[i].transform.position;
            radarSource.Play();
            while(radarSource.isPlaying) {
                yield return null;
            }
        }

        radarIsRunning = false; // Libera o radar para poder ser disparado de novo

        yield break;
    }
    */

    // Avisa ao jogador que há um objeto interatível logo à frente com um toque sonoro. É chamada quando o jogador se movimenta ou se vira
    private IEnumerator availableInteractionAhead() {
        if(checkForInteraction()) {
            avisoSource.Play();
        }
        yield break;
    }

    //Determina onde coloca a avisoSource para que o áudio saia do lugar certo
    private Vector3 avisoPosition(string direction) {
        float angle = transform.rotation.y;
        if(direction == "fwd") {
            switch (angle) {
                case 0: //Frente
                return new Vector3(0, 0, 2);
                case 0.7071068f: //Direita
                return new Vector3(2, 0, 0);
                case 1: //Trás
                return new Vector3(0, 0, -2);
                case -0.7071068f: //Esquerda
                return new Vector3(-2, 0, 0);
                default:
                break;
            }
        }
        else if(direction == "rgt") {
            switch (angle) {
                case 0:
                return new Vector3(2, 0, 0);
                case 0.7071068f:
                return new Vector3(0, 0, -2);
                case 1:
                return new Vector3(-2, 0, 0);
                case -0.7071068f:
                return new Vector3(0, 0, 2);
                default:
                break;
            }
        }
        else if(direction == "lft") {
            switch (angle) {
                case 0:
                return new Vector3(-2, 0, 0);
                case 0.7071068f:
                return new Vector3(0, 0, -2);
                case 1:
                return new Vector3(2, 0, 0);
                case -0.7071068f:
                return new Vector3(0, 0, 2);
                default:
                break;
            }
        }
        return new Vector3(0, 0, 0);
    }

    // Confere se há um objeto interatível perto do jogador
    private bool checkForInteraction() {
        // Usando Raycast, verifica se tem um objeto com uma componente que implementa a interface "IInteragivel" na frente do jogador.
        // Se tiver, avisa ao jogador com um toque sonoro
        RaycastHit hit;
        IInteragivel obj;
        IScript obj2;
        //TaskmasterScript obj3;
        if( Physics.Raycast(transform.position, transform.forward, out hit, 0.5f) &&
          (obj = hit.collider.GetComponent<IInteragivel>()) != null ) {
            avisoSource.transform.position = transform.position + avisoPosition("fwd");
            return true;
        }
        if( Physics.Raycast(transform.position, transform.right, out hit, 0.5f) &&
          (obj = hit.collider.GetComponent<IInteragivel>()) != null ) {
            avisoSource.transform.position = transform.position + avisoPosition("rgt");
            return true;
        }
        if( Physics.Raycast(transform.position, -1 * transform.right, out hit, 0.5f) &&
          (obj = hit.collider.GetComponent<IInteragivel>()) != null ) {
            avisoSource.transform.position = transform.position + avisoPosition("lft");
            return true;
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f) &&
           ((obj2 = hit.collider.GetComponent<IScript>()) != null)) {
            avisoSource.transform.position = transform.position + avisoPosition("fwd");
            return true;
        }
        if (Physics.Raycast(transform.position, transform.right, out hit, 0.5f) &&
           ((obj2 = hit.collider.GetComponent<IScript>()) != null)) {
            avisoSource.transform.position = transform.position + avisoPosition("rgt");
            return true;
        }
        if (Physics.Raycast(transform.position, -1 * transform.right, out hit, 0.5f) &&
           ((obj2 = hit.collider.GetComponent<IScript>()) != null)) {
            avisoSource.transform.position = transform.position + avisoPosition("lft");
            return true;
        }
        
        return false;
    }

    void Update() {
        //Evita imprecisões na posição
        transform.position = new Vector3(
            Mathf.Round(transform.position.x * 1000f)/1000f,
            Mathf.Round(transform.position.y * 1000f)/1000f,
            Mathf.Round(transform.position.z * 1000f)/1000f
        );

        // Se WasPressedThisFrame() estiver dando erro: 
        // Na tela do editor Window > Package Manager
        // Advanced > Show preview packages
        // Procure "input system" e instale a versão preview.2 - 1.1.0 do Input System

        // Se o botão de interagir for pressionado,
        // interage com o objeto a frente do jogador
        if(interagirAction.WasPressedThisFrame()) {
            // Usando Raycast, verifica se tem um objeto com uma componente que
            // implementa a interface "IInteragivel" na frente do jogador
            RaycastHit hit;
            IInteragivel obj;
            IScript obj2;
            if( Physics.Raycast(transform.position, transform.forward, out hit, 0.5f)
             && (obj = hit.collider.GetComponent<IInteragivel>()) != null) {
                //Se tenta interagir com um IInteragivel, então não está interagindo com uma pista e pode parar, se tiver uma rodando
                if(currentPista != null) {
                    currentPista.skip();
                }
                //Se o que estou acessando agora não é o antigo, interrompe o antigo
                if(currentInteragivel != obj) {
                    if(currentInteragivel != null) {
                        currentInteragivel.skip();
                    }
                    currentInteragivel = obj;
                    // Chama o método Interagir() do objeto
                    obj.Interagir();
                }
                //Se for, não faz nada
            }
            
            //Também chama lerDialogo para objetos que possuem essa função
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f)
             && ((obj2 = hit.collider.GetComponent<IScript>()) != null)) {
                //Se tenta interagir com uma pista, então não está interagindo com um IInteragivel e pode parar, se tiver um rodando
                if(currentInteragivel != null) {
                    currentInteragivel.skip();
                }
                //Se o que estou acessando agora não é o antigo, interrompe o antigo
                if(currentPista != obj2) {
                    if(currentPista != null) {
                        currentPista.skip();
                    }
                    currentPista = obj2;
                    obj2.interagir();
                }
                //Se for, não faz nada
            }
        }

        /*
        if(radarAction.WasPressedThisFrame()) {
            // Chama o radar
            if(!radarIsRunning) {
                StartCoroutine( radar() );
            }
        }
        */

        if(quitAction.WasPressedThisFrame()) {
            if(currentPista != null) {
                    currentPista.skip();
            }
            if(currentInteragivel != null) {
                currentInteragivel.skip();
            }
            MudaMapaQuit();
        }

        // Pega o input de movimento, que pode vir do teclado
        // ou do analógico de um controle, e trata ele
        Vector2 input = movimentoAction.ReadValue<Vector2>();
        Vector2 aux = TratarInput( input );

        // Floats para o input horizontal e input vertical
        float hIn = aux.x;
        float vIn = aux.y;

        if ( Mathf.Abs(hIn) == 1f ) {
        // Se o input horizontal for igual a -1 ou 1, rotaciona em uma direção

            canMoveH = false; // Não permite que o jogador se mova em sequência

            // Rotaciona o jogador em 90 graus em uma direção
            // dependendo do sinal de hIn (-1 ou 1)
            transform.Rotate(Vector3.up, 90f * hIn);

            // Se não houver um objeto interatível à frente após a rotação, toca a nota normalmente
            if(!checkForInteraction()) {
                // Toca uma nota dependendo do sinal de hIn (-1 ou 1)
                audioSource.clip = hIn > 0f ? notaD : notaA;
                audioSource.Play();
            } else { // Se houver, avisa o jogador com um toque sonoro
                StartCoroutine( availableInteractionAhead() );
            }

        } else if ( Mathf.Abs(vIn) == 1f ) {
            // Se o input vertical for igual a -1 ou 1, anda em uma direção

            canMoveV = false; // Não permite que o jogador se mova em sequência

            // Inicia variáveis auxiliares com os valores das coordenadas atuais do jogador
            int newCoordX = coordX;
            int newCoordY = coordY;

            // Se o jogador estiver alinhado com o eixo X 
            if( Mathf.Abs(transform.forward.x) >= 0.001f ) {
                // Dependendo do sentido do jogador em relação ao eixo X ( se forward.x é negativo ou positivo ),
                // adiciona ou subtrai vIn de newCoordX
                newCoordX += (int) (Mathf.Sign(transform.forward.x) * vIn);
            }
            
            // Se o jogador estiver alinhado com o eixo Z
            if( Mathf.Abs(transform.forward.z) >= 0.001f ) { 
                // Dependendo do sentido do jogador em relação ao eixo Z ( se forward.z é negativo ou positivo ),
                // adiciona ou subtrai vIn de newCoordZ
                newCoordY += (int) (Mathf.Sign(transform.forward.z) * vIn);
            }

            // Verifica se a nova coordenada do jogador esta dentro do grid da sala
            if (newCoordX >= 0 && newCoordY >= 0 && newCoordX < GameManager.GM.getGridSizeX && newCoordY < GameManager.GM.getGridSizeY) {
                // Se as novas coordenadas forem válidas

                transform.position += vIn * transform.forward; // Move o jogador

                // Atualiza as coordenadas do jogador
                coordX = newCoordX;
                coordY = newCoordY;

                // Faz o som de passos
                stepSource.Play();

                if(checkForInteraction()) { //Se houver um objeto interatível à frente, avisa ao jogador
                    StartCoroutine( availableInteractionAhead() );
                }
                

            } else {
                // Se as novas coordenadas forem inválidas
                
                // Se o jogador estiver usando um Gamepad, vibra ele
                if(playerInput.currentControlScheme == "Gamepad") rumble.Vibrate(2f, 0f, 0.5f);
                
                // Toca um som indicando que o jogador atngiu uma parede
                audioSource.clip = notaParede;
                audioSource.Play();
            }
        }
    }
}
