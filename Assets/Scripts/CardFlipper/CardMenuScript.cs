/*Código usado majoritariamente para controlar as 
 * audiodescrições das Scenes.
 */
using UnityEngine.SceneManagement;
using UnityEngine;

public class CardMenuScript : MonoBehaviour
{

    public int totalButton = 4;

    bool pause = false;

    public AudioSource audioDescription;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "CardMenu")
        {
            audioDescription.Play();
        }
        else if (SceneManager.GetActiveScene().name == "CardInstructions")
        {
            audioDescription.Play();
        }
        else if (SceneManager.GetActiveScene().name == "CardPreTester")
        {
            audioDescription.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CardMenu")
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
        else if (SceneManager.GetActiveScene().name == "CardInstructions")
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
        else if (SceneManager.GetActiveScene().name == "CardPreTester")
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
        
        else if (SceneManager.GetActiveScene().name == "CardTester")
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
