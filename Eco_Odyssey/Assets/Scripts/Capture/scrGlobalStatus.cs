using UnityEngine;

public class scrGlobalStatus : MonoBehaviour
{
    // Party de Ecos
    public scrEcoFather[] ecoParty = new scrEcoFather[7];

    // Eco atualmente selecionado
    public int currentEcoIndex = 0;

    // Eco atual
    public scrEcoFather currentEco;

    // Stats atuais do Eco
    public float vidaC;
    public float atkC;
    public float defC;
    public float veloC;
    public int levelC;
    public RuntimeAnimatorController animControllerC;
    public bool usingEco = false;
    public scrEcoFather ecoInicial;

    public void Start()
    {
        if (ecoInicial != null)
        {
            AddEco(ecoInicial);
        }
    }
    
    // Adicionar Eco na party
    public void AddEco(scrEcoFather eco)
    {
        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] == null)
            {
                ecoParty[i] = eco;

                Debug.Log("Eco adicionado no slot " + (i + 1));

                // Se for o primeiro Eco capturado
                if (currentEco == null)
                {
                    EquipEco(i);
                }

                return;
            }
        }

        Debug.Log("Party cheia!");
    }

    // Equipar Eco
    public void EquipEco(int index)
    {
        if (ecoParty[index] == null)
        {
            Debug.Log("Slot vazio");
            return;
        }

        currentEcoIndex = index;

        currentEco = ecoParty[index];

        vidaC = currentEco.Vida;
        atkC = currentEco.Ataque;
        defC = currentEco.Defesa;
        veloC = currentEco.Velocidade;
        levelC = currentEco.Level;
        animControllerC = currentEco.animControllerEco;
        Debug.Log("Eco equipado: " + currentEco.name);
    }
}