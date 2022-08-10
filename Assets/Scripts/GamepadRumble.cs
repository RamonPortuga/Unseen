using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumble : MonoBehaviour
{   
    // Classe auxiliar para a vibração do gamepad

    // Vibra o controle
    // Recebe a intensidade da vibração para os dois motores do controle e o tempo de vibração    
    public void Vibrate(float leftMotor, float rightMotor, float time){
        // Inicia uma corotina para vibrar o controle por uma certa quantidade de tempo
        IEnumerator coroutine = VibrateCoroutine(leftMotor, rightMotor, time);
        StartCoroutine(coroutine);
    }

    // Vibra o controle com intensidade definida e por uma certa quantidade de tempo
    private IEnumerator VibrateCoroutine(float leftMotor, float rightMotor, float time)
    {
        float timer = 0f;
        while(timer < time)
        {
            if(Gamepad.current != null)
            {
                float timeLeft = (time - timer)/time;
                Gamepad.current.SetMotorSpeeds(timeLeft * leftMotor, timeLeft * rightMotor);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
