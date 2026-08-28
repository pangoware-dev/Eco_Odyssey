using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

[CreateAssetMenu(fileName = "Eco", menuName ="Eco/Create new eco")]
public class scrEcoFather : ScriptableObject
{
    public float Ataque, Defesa, Vida, Velocidade, VidaAtual;
    public int Level;
    public RuntimeAnimatorController animControllerEco;
 
    //Nome
    [SerializeField] string nome;

    [TextArea]
    [SerializeField] string description;

    //Elementos
    public EcoElement Element1;
    public EcoElement Element2;

    public static float ElementEffectiveness(EcoElement attack1, EcoElement attack2, EcoElement defense1, EcoElement defense2)
    {
        Dictionary<EcoElement, Dictionary<EcoElement, float>> multiplier = new Dictionary<EcoElement, Dictionary<EcoElement, float>>()
        {
            {EcoElement.Fire, new Dictionary<EcoElement, float>()},
            {EcoElement.Water, new Dictionary<EcoElement, float>()},
            {EcoElement.Plant, new Dictionary<EcoElement, float>()},
            {EcoElement.Toxic, new Dictionary<EcoElement, float>()},
            {EcoElement.Earth, new Dictionary<EcoElement, float>()},
            {EcoElement.Wind, new Dictionary<EcoElement, float>()},
            {EcoElement.None, new Dictionary<EcoElement, float>()}
        };

        //super efetivo
        multiplier[EcoElement.Fire][EcoElement.Plant]=2;
        multiplier[EcoElement.Fire][EcoElement.Wind]=2;
        multiplier[EcoElement.Water][EcoElement.Fire]=2;
        multiplier[EcoElement.Water][EcoElement.Earth]=2;
        multiplier[EcoElement.Plant][EcoElement.Wind]=2;
        multiplier[EcoElement.Plant][EcoElement.Water]=2;
        multiplier[EcoElement.Wind][EcoElement.Toxic]=2;
        multiplier[EcoElement.Wind][EcoElement.Earth]=2;
        multiplier[EcoElement.Toxic][EcoElement.Plant]=2;
        multiplier[EcoElement.Toxic][EcoElement.Water]=2;
        multiplier[EcoElement.Earth][EcoElement.Toxic]=2;
        multiplier[EcoElement.Earth][EcoElement.Fire]=2;

        //pouco efetivo
        multiplier[EcoElement.Fire][EcoElement.Water]=0.5f;
        multiplier[EcoElement.Fire][EcoElement.Earth]=0.5f;
        multiplier[EcoElement.Water][EcoElement.Plant]=0.5f;
        multiplier[EcoElement.Water][EcoElement.Toxic]=0.5f;
        multiplier[EcoElement.Plant][EcoElement.Fire]=0.5f;
        multiplier[EcoElement.Plant][EcoElement.Toxic]=0.5f;
        multiplier[EcoElement.Wind][EcoElement.Fire]=0.5f;
        multiplier[EcoElement.Wind][EcoElement.Plant]=0.5f;
        multiplier[EcoElement.Toxic][EcoElement.Earth]=0.5f;
        multiplier[EcoElement.Toxic][EcoElement.Wind]=0.5f;
        multiplier[EcoElement.Earth][EcoElement.Water]=0.5f;
        multiplier[EcoElement.Earth][EcoElement.Wind]=0.5f;
        multiplier[EcoElement.Earth][EcoElement.Earth]=0.5f;
        multiplier[EcoElement.Fire][EcoElement.Fire]=0.5f;
        multiplier[EcoElement.Water][EcoElement.Water]=0.5f;
        multiplier[EcoElement.Plant][EcoElement.Plant]=0.5f;
        multiplier[EcoElement.Wind][EcoElement.Wind]=0.5f;
        multiplier[EcoElement.Toxic][EcoElement.Toxic]=0.5f;

        foreach (EcoElement atk in multiplier.Keys)
        {
            foreach (EcoElement def in multiplier.Keys)
            {
                if (!multiplier[atk].ContainsKey(def))
                {
                    multiplier[atk][def] = 1f;
                }
            }
        }

        float finalMultiplier = 1f;

        finalMultiplier *= multiplier[attack1][defense1];

        if (attack2 != EcoElement.None)
        {
            finalMultiplier *= multiplier[attack2][defense1];
        }

        if (defense2 != EcoElement.None)
        {
            finalMultiplier *= multiplier[attack1][defense2];

            if (attack2 != EcoElement.None)
            {
                finalMultiplier *= multiplier[attack2][defense2];
            }
        }

        return finalMultiplier;
        }

}