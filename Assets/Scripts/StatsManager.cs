using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public int TotalMoney = 10000;

    [System.Serializable]
    public struct TotalPopulation
    {
        public int Current;
        public int Space;
    }
    [System.Serializable]
    public struct TotalElectricity
    {
        public int Available;
        public int Cost;
    }
    [System.Serializable]
    public struct TotalJobs
    {
        public int Taken;
        public int Available;
    }
    public TotalPopulation totalPopulation;
    public int Happiness;
    public TotalElectricity totalElectricity;

    public int TotalHappiness = 0;
    public int HappinessThresholdForLeaving = 30;
    public int LeavingRate = 10;
    public int HappinessThresholdForMovingIn = 50;
    public int PopulationGrowthRate = 5;
    public TotalJobs totalJobs;
    [Header("Etc")]
    public int TotalLandValue;
    public int TotalCrimeRate;
    public int TotalPollution;
    public int TotalFireCoverage;
    public int TotalExpenses;

    public float taxRate = 0.05f;
    public float tickRate = 5f;
    public int year = 2000;
    public int month = 0;

    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI yearDisplay;

    public TextMeshProUGUI statsDisplay;

    private void Start()
    {

        totalPopulation = new TotalPopulation { Current = 0, Space = 0 };
        totalJobs = new TotalJobs { Taken = 0, Available = 0 };
        totalElectricity = new TotalElectricity { Available = 0, Cost = 0 };

        Monthly();
        InvokeRepeating(nameof(Monthly), tickRate, tickRate);
    }
    void Update()
    {
        if (totalPopulation.Current > totalPopulation.Space)
        {
            totalPopulation.Current = totalPopulation.Space;
        }
        if (totalJobs.Taken > totalJobs.Available)
        {
            totalJobs.Taken = totalJobs.Available;
        }
    }
    private void Monthly()
    {
        CalculateHappiness();
        HandlePopulationLeaving();
        UpdatePopulation();
        if (month >= 12)
        {
            month = 1;
            year++;
            Yearly();
            UpdateDisplay();
            return;
        }
        ProcessIncome();
        month++;
        UpdateDisplay();
    }
    private void Yearly()
    {
        ProcessIncome();
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        moneyDisplay.text = $"Money: ${TotalMoney}";
        yearDisplay.text = $"Year: {year}\nMonth: {month}";
        statsDisplay.text = $"Population: {totalPopulation.Current}\nHappiness: {TotalHappiness}";

    }

    void ProcessIncome()
    {
        int residentialIncome = Mathf.RoundToInt(totalPopulation.Current * taxRate * 10);
        int commercialIncome = Mathf.RoundToInt(totalJobs.Taken * taxRate * 15);
        int industrialIncome = Mathf.RoundToInt(totalJobs.Taken * taxRate * 20);

        int totalIncome = residentialIncome + commercialIncome + industrialIncome;
        int expenses = TotalExpenses;

        TotalMoney += totalIncome - expenses;

    }
    private void CalculateHappiness()
    {
        int unemployed = totalPopulation.Current - totalJobs.Taken;
        int happinessChange = 0;


        if (unemployed > 0)
        {
            happinessChange -= unemployed;
        }

        if (totalElectricity.Available < totalElectricity.Cost)
        {
            happinessChange -= 5;
        }
        else
        {
            happinessChange += 2;
        }


        if (totalJobs.Available <= 0)
        {
            happinessChange -= 3;
        }
        else
        {
            happinessChange += 1;
        }


        TotalHappiness += happinessChange;

        TotalHappiness = Mathf.Clamp(TotalHappiness, 0, 100);
    }

    private void HandlePopulationLeaving()
    {
        if (TotalHappiness <= HappinessThresholdForLeaving)
        {
            int peopleLeaving = Mathf.Min(LeavingRate, totalPopulation.Current);
            totalPopulation.Current -= peopleLeaving;


            totalJobs.Taken = Mathf.Min(totalJobs.Taken, totalPopulation.Current);
        }
    }


    void UpdatePopulation()
    {

        if (TotalHappiness > HappinessThresholdForMovingIn)
        {

            int availableSpace = totalPopulation.Space - totalPopulation.Current;
            int availableJobs = totalJobs.Available - totalJobs.Taken;


            if (availableSpace > 0 && availableJobs > 0)
            {

                int peopleMovingIn = Mathf.Min(PopulationGrowthRate, availableSpace, availableJobs);


                totalPopulation.Current += peopleMovingIn;


                totalJobs.Taken += peopleMovingIn;
            }
        }
    }



}
