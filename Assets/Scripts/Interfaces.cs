using UnityEngine;
using System.Collections;

// Algumas interfaces que são usadas para o desenvolvimento do jogo

public interface IInteragivel {

    void Interagir();

    void skip();
    
}

public interface IScript {
    void interagir();

    void skip();
}

public interface IMinigame {

    string ActionMapName();

    void StartGame();


}
