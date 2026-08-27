using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Eco", menuName ="Eco/Create new eco")]
public class scrEcoFather : ScriptableObject
{
<<<<<<< Updated upstream
    public float Ataque, Defesa, Vida, Velocidade;
    public int Level;
=======
    public float Ataque, Defesa, Vida, Velocidade, VidaAtual;
>>>>>>> Stashed changes
    public RuntimeAnimatorController animControllerEco;
    private scrGlobalStatus globalStatus;
 
    //Nome
    [SerializeField] string nome;

    [TextArea]
    [SerializeField] string description;

    //Elementos
    [SerializeField] EcoElement Element1;
    [SerializeField] EcoElement Element2;

}

public enum EcoElement
{
    Water,
    Fire,
    Plant,
    Toxic,
    Earth,
    Wind,
	None
}