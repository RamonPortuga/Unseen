/* Código usado para os Menu's (para Teclado e para Joystick)
 * e também controla as áudio descrições de todas as cenas*/ 
using UnityEngine.SceneManagement;
using UnityEngine;

public class GeniusMenuScript : MonoBehaviour
{

    //int index = 0;
    public int totalButton = 4;

    //bool firstMoviment = true;
    //bool keypad = false;

    public AudioSource audioDescription;


    public AudioSource audioButtonGenius;
    public AudioSource audioButtonInstructions;
    public AudioSource audioButtonTester;
    public AudioSource audioButtonQuit;

    public static bool menuActived = false;

    public static bool pause = true;

    // Start is called before the first frame update
    void Start()
    {
        menuActived = false;
        //As condicionais abaixo servem para controlar as AudioDescrições
        if (SceneManager.GetActiveScene().name == "GeniusMenu")
        {
            audioDescription.Play();
            pause = false;
        }
        else if (SceneManager.GetActiveScene().name == "GeniusInstructions")
        {
            audioDescription.Play();
            pause = false;
        }
        else if (SceneManager.GetActiveScene().name == "GeniusTester")
        {
            audioDescription.Play();
            pause = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //As linhas abaixo servem para controlar as AudioDescrições
        //Debug.Log(pause);
        if (SceneManager.GetActiveScene().name == "GeniusMenu")
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
        else if (SceneManager.GetActiveScene().name == "GeniusInstructions")
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
        else if (SceneManager.GetActiveScene().name == "GeniusTester" )
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
