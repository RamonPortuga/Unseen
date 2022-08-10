using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {
    public GameObject Left = null; //Guarda qual carta está à esquerda dessa carta
    public GameObject Right = null; //Guarda qual carta está à direita dessa carta
    public GameObject Up = null; //Guarda qual carta está acima dessa carta
    public GameObject Down = null; //Guarda qual carta está abaixo dessa carta
    public GameObject Soulmate; //Guarda qual carta é o par dessa carta
    public bool isOpen = false; //Guarda se a carta está aberta
    public AudioSource bump; //Som que toca quando o cursor não pode ir nessa direção
    public AudioSource moved; //Som que toca quando o cursor selecionou uma nova carta
    public AudioSource cardIsOpen; //Som que toca quando o jogador tenta abrir uma carta que já está aberta
    public AudioSource incorrectPair; //Som que toca quando as cartas que o jogador abriu não são um par
    public AudioSource correctPair; //Som que toca quando as cartas que o jogador abriu SÃO um par
    private int position; //Determina a posição da carta no tabuleiro;

    //Se a carta possui alguma carta na direção indicada, retorna essa carta
    public GameObject getLeft() {
        if(this.Left != null) {
            return this.Left;
        }
        return null;
    }

    //Se a carta possui alguma carta na direção indicada, retorna essa carta
    public GameObject getRight() {
        if(this.Right != null) {
            return this.Right;
        }
        return null;
    }

    //Se a carta possui alguma carta na direção indicada, retorna essa carta
    public GameObject getUp() {
        if(this.Up != null) {
            return this.Up;
        }
        return null;
    }

    //Se a carta possui alguma carta na direção indicada, retorna essa carta
    public GameObject getDown() {
        if(this.Down != null) {
            return this.Down;
        }
        return null;
    }

    //Define um par novo para a carta
    public void setPair(GameObject newPair) {
        this.Soulmate = newPair;
    }

    //Retorna qual carta é o par dessa carta
    public GameObject getPair() {
        return this.Soulmate;
    }
    
    //Retorna se essa carta está aberta
    public bool getOpen() {
        return this.isOpen;
    }

    //Faz a carta estar aberta
    public void setOpen() {
        this.isOpen = true;
    }

    //Faz a carta estar fechada
    public void setClosed() {
        this.isOpen = false;
    }

    //Retorna um dos áudios de movimento para fácil acesso.
    public AudioSource getBumpSound() {
        return bump;
    }

    //Retorna um dos áudios de movimento para fácil acesso.
    public AudioSource getMovedSound() {
        return moved;
    }

    //Retorna um dos áudios de movimento para fácil acesso.
    public AudioSource getOpenSound() {
        return cardIsOpen;
    }

    //Retorna um dos áudios de movimento para fácil acesso.
    public AudioSource getCorrectSound() {
        return correctPair;
    }

    //Retorna um dos áudios de movimento para fácil acesso.
    public AudioSource getIncorrectSound() {
        return incorrectPair;
    }

    public int getPosition(){
        return position;
    }

    public void setPosition(int position){
        this.position = position;
    }
}