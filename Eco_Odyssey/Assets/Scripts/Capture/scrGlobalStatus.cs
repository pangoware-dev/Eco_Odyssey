using UnityEngine;

public class scrGlobalStatus : MonoBehaviour
{
<<<<<<< Updated upstream
    // Party de Ecos
    public scrEcoFather[] ecoParty = new scrEcoFather[7];

    // Eco atualmente selecionado
    public int currentEcoIndex = 0;

    // Eco atual
    public scrEcoFather currentEco;

    // Stats atuais do Eco
=======
    // PARTY DE ECOS
    public scrEcoFather[] ecoParty = new scrEcoFather[7];

    // HP ATUAL DE CADA ECO
    public float[] vidaAtualParty = new float[7];

    // ECO ATUALMENTE SELECIONADO
    public int currentEcoIndex = 0;
    public scrEcoFather currentEco;

    // STATUS DO ECO ATUAL
    public float vidaAtual;
>>>>>>> Stashed changes
    public float vidaC;
    public float atkC;
    public float defC;
    public float veloC;
    public int levelC;
<<<<<<< Updated upstream
=======
    public int quantEco;

>>>>>>> Stashed changes
    public RuntimeAnimatorController animControllerC;
    public bool usingEco = false;
    public scrEcoFather ecoInicial;

<<<<<<< Updated upstream
=======

    // =========================
    // START
    // =========================

>>>>>>> Stashed changes
    public void Start()
    {
        if (ecoInicial != null)
        {
            AddEco(ecoInicial);
        }
    }
<<<<<<< Updated upstream
    
    // Adicionar Eco na party
    public void AddEco(scrEcoFather eco)
    {
=======


    // =========================
    // CALCULAR VIDA MÁXIMA
    // =========================

    private float CalcularVidaMaxima(scrEcoFather eco)
    {
        if (eco == null)
        {
            return 0;
        }

        return (eco.Vida + 2 * Mathf.Sqrt(levelC)) * 10;
    }


    // =========================
    // CALCULAR ATAQUE
    // =========================

    private float CalcularAtaque(scrEcoFather eco)
    {
        if (eco == null)
        {
            return 0;
        }

        return (eco.Ataque + 2 * Mathf.Sqrt(levelC)) * 10;
    }


    // =========================
    // CALCULAR DEFESA
    // =========================

    private float CalcularDefesa(scrEcoFather eco)
    {
        if (eco == null)
        {
            return 0;
        }

        return (eco.Defesa + 2 * Mathf.Sqrt(levelC)) * 10;
    }


    // =========================
    // ADICIONAR ECO
    // =========================

    public void AddEco(scrEcoFather eco)
    {
        if (eco == null)
        {
            Debug.Log("Tentativa de adicionar um Eco nulo.");
            return;
        }

>>>>>>> Stashed changes
        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] == null)
            {
<<<<<<< Updated upstream
                ecoParty[i] = eco;

                Debug.Log("Eco adicionado no slot " + (i + 1));

                // Se for o primeiro Eco capturado
                if (currentEco == null)
                {
                    EquipEco(i);
=======
                // Adiciona o Eco
                ecoParty[i] = eco;

                // Aumenta a quantidade de Ecos
                quantEco++;

                Debug.Log("Eco adicionado no slot " + (i + 1));

                // Se for o primeiro Eco,
                // define ele como o Eco atual
                if (currentEco == null)
                {
                    currentEco = eco;
                    currentEcoIndex = i;
>>>>>>> Stashed changes
                }

                // Atualiza o nível baseado na quantidade de Ecos
                ChangeLevel();

                // O Eco começa com HP cheio
                vidaAtualParty[i] = CalcularVidaMaxima(eco);

                // Se esse for o Eco atual,
                // atualiza suas informações
                if (currentEcoIndex == i)
                {
                    vidaC = CalcularVidaMaxima(currentEco);
                    vidaAtual = vidaAtualParty[i];

                    atkC = CalcularAtaque(currentEco);
                    defC = CalcularDefesa(currentEco);

                    animControllerC = currentEco.animControllerEco;
                }

                Debug.Log(
                    "Eco " + (i + 1) +
                    " | HP: " + vidaAtualParty[i] +
                    " | Level: " + levelC
                );

                return;
            }
        }

        Debug.Log("Party cheia!");
    }

<<<<<<< Updated upstream
    // Equipar Eco
    public void EquipEco(int index)
    {
        if (ecoParty[index] == null)
        {
            Debug.Log("Slot vazio");
=======

    // =========================
    // ALTERAR LEVEL
    // =========================

    public void ChangeLevel()
    {
        levelC = quantEco * 5;

        if (currentEco == null)
        {
            return;
        }

        vidaC = CalcularVidaMaxima(currentEco);
        atkC = CalcularAtaque(currentEco);
        defC = CalcularDefesa(currentEco);
        veloC = currentEco.Velocidade;
    }


    // =========================
    // EQUIPAR ECO
    // =========================

    public bool EquipEco(int index)
    {
        // Índice inválido
        if (index < 0 || index >= ecoParty.Length)
        {
            Debug.Log("Índice de Eco inválido.");
            return false;
        }

        // Slot vazio
        if (ecoParty[index] == null)
        {
            Debug.Log("Slot vazio.");
            return false;
        }

        // Eco morto
        if (vidaAtualParty[index] <= 0)
        {
            Debug.Log(
                "O Eco do slot " +
                (index + 1) +
                " está morto."
            );

            return false;
        }

        // Define o índice atual
        currentEcoIndex = index;

        // Define o Eco atual
        currentEco = ecoParty[index];

        // Atualiza o nível e atributos
        ChangeLevel();

        // Calcula a vida máxima do Eco
        vidaC = CalcularVidaMaxima(currentEco);

        // Recupera o HP salvo daquele slot
        vidaAtual = vidaAtualParty[index];

        // Outros atributos
        atkC = CalcularAtaque(currentEco);
        defC = CalcularDefesa(currentEco);

        // Velocidade não escala com levelC
        veloC = currentEco.Velocidade;

        // Animação
        animControllerC = currentEco.animControllerEco;

        Debug.Log(
            "Eco equipado: " +
            currentEco.name +
            " | HP: " +
            vidaAtual +
            "/" +
            vidaC +
            " | Level: " +
            levelC
        );

        return true;
    }


    // =========================
    // SALVAR HP DO ECO ATUAL
    // =========================

    public void SaveCurrentEcoHealth(float hp)
    {
        if (currentEco == null)
        {
>>>>>>> Stashed changes
            return;
        }

        currentEcoIndex = index;

        currentEco = ecoParty[index];

<<<<<<< Updated upstream
        vidaC = currentEco.Vida;
        atkC = currentEco.Ataque;
        defC = currentEco.Defesa;
        veloC = currentEco.Velocidade;
        levelC = currentEco.Level;
        animControllerC = currentEco.animControllerEco;
        Debug.Log("Eco equipado: " + currentEco.name);
=======
        // Atualiza também a variável do Eco atual
        vidaAtual = hp;
    }


    // =========================
    // PEGAR HP DO ECO ATUAL
    // =========================

    public float GetCurrentEcoHealth()
    {
        if (currentEco == null)
        {
            return 0;
        }

        return vidaAtualParty[currentEcoIndex];
    }


    // =========================
    // ENCONTRAR PRÓXIMO ECO VIVO
    // =========================

    public int GetNextAliveEco()
    {
        // Primeiro procura nos slots seguintes
        for (int i = currentEcoIndex + 1; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }

        // Depois procura nos slots anteriores
        for (int i = 0; i < currentEcoIndex; i++)
        {
            if (ecoParty[i] != null && vidaAtualParty[i] > 0)
            {
                return i;
            }
        }

        // Nenhum Eco vivo
        return -1;
    }


    // =========================
    // CURAR TODA A PARTY
    // =========================

    public void HealParty()
    {
        for (int i = 0; i < ecoParty.Length; i++)
        {
            if (ecoParty[i] != null)
            {
                vidaAtualParty[i] =
                    CalcularVidaMaxima(ecoParty[i]);
            }
        }

        // Atualiza o Eco atual
        if (currentEco != null)
        {
            vidaC = CalcularVidaMaxima(currentEco);

            vidaAtual =
                vidaAtualParty[currentEcoIndex];

            atkC = CalcularAtaque(currentEco);
            defC = CalcularDefesa(currentEco);
            veloC = currentEco.Velocidade;
        }
>>>>>>> Stashed changes
    }
}