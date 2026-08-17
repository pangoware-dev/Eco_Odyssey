using UnityEngine;

public class scrGlobalStatus : MonoBehaviour{

    // PARTY DE ECOS
   public scrEcoFather[] ecoParty = new scrEcoFather[7];


    // HP ATUAL DE CADA ECO
   public float[] vidaAtualParty = new float[7];


    // ECO ATUALMENTE SELECIONADO
   public int currentEcoIndex = 0;

    public scrEcoFather currentEco;


    // STATUS DO ECO ATUAL
    public float vidaC;
    public float vidaAtual;

    public float atkC;
    public float defC;
    public float veloC;

    public int levelC;

    public RuntimeAnimatorController animControllerC;

    public bool usingEco = false;

    public scrEcoFather ecoInicial;


    // START
    public void Start(){
        if (ecoInicial != null){
            AddEco(ecoInicial);
        }
    }


    // ADICIONAR ECO
    public void AddEco(scrEcoFather eco){
        for (int i = 0; i < ecoParty.Length; i++){
            if (ecoParty[i] == null){

                ecoParty[i] = eco;

                // O Eco começa com HP cheio
                vidaAtualParty[i] = eco.Vida;

                Debug.Log("Eco adicionado no slot " + (i + 1));

                // Se for o primeiro Eco
                if (currentEco == null){
                    EquipEco(i);
                }

                return;
            }
        }

        Debug.Log("Party cheia!");
    }


    // EQUIPAR ECO
    public bool EquipEco(int index){
        // Verifica se o índice é válido
        if (index < 0 || index >= ecoParty.Length){
            Debug.Log("Índice de Eco inválido.");
            return false;
        }


        // Verifica se existe Eco nesse slot
        if (ecoParty[index] == null){
            Debug.Log("Slot vazio.");
            return false;
        }


        // Não permite usar Eco morto
        if (vidaAtualParty[index] <= 0){
            Debug.Log("O Eco do slot " + (index + 1) + " está morto.");

            return false;
        }


        // Define o índice atual
        currentEcoIndex = index;


        // Define o Eco atual
        currentEco = ecoParty[index];


        // Carrega os status do Eco
        vidaC = currentEco.Vida;

        // IMPORTANTE:
        // pega o HP salvo daquele SLOT
        vidaAtual = vidaAtualParty[index];

        atkC = currentEco.Ataque;
        defC = currentEco.Defesa;
        veloC = currentEco.Velocidade;

        levelC = currentEco.Level;

        animControllerC = currentEco.animControllerEco;


        //Debug.Log("Eco equipado: " + currentEco.name + " | HP: " + vidaAtual + "/" + vidaC);

        return true;
    }


    // SALVAR HP DO ECO ATUAL
    public void SaveCurrentEcoHealth(float hp){
        if (currentEco == null){
            return;
        }

        // Garante que o HP fique entre 0 e o máximo
        hp = Mathf.Clamp(hp, 0, vidaC);

        // Salva no slot correspondente
        vidaAtualParty[currentEcoIndex] = hp;

        // Atualiza também a variável do Eco atual
        vidaAtual = hp;
    }


    // PEGAR HP DO ECO ATUAL
    public float GetCurrentEcoHealth(){
        if (currentEco == null){
            return 0;
        }

        return vidaAtualParty[currentEcoIndex];
    }


    // ENCONTRAR PRÓXIMO ECO VIVO
    public int GetNextAliveEco(){
        // Primeiro procura nos slots seguintes
        for(int i = currentEcoIndex + 1; i < ecoParty.Length; i++){
            if (ecoParty[i] != null && vidaAtualParty[i] > 0){
                return i;
            }
        }


        // Se não encontrou, procura nos slots anteriores
        for(int i = 0; i < currentEcoIndex; i++){
            if (ecoParty[i] != null && vidaAtualParty[i] > 0){
                return i;
            }
        }


        // Nenhum Eco vivo
        return -1;
    }

    public void HealParty(){
        for (int i = 0; i < ecoParty.Length; i++){
            if (ecoParty[i] != null){
                vidaAtualParty[i] = ecoParty[i].Vida;
            }
        }

        if (currentEco != null){
            vidaC = currentEco.Vida;
            vidaAtual = vidaAtualParty[currentEcoIndex];
        }
    }
}