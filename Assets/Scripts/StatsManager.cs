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
    public TotalElectricity totalElectricity;


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


    private void Start()
    {

        totalPopulation = new TotalPopulation { Current = 0, Space = 0 };
        totalJobs = new TotalJobs { Taken = 0, Available = 0 };
        totalElectricity = new TotalElectricity { Available = 0, Cost = 0 };

        Monthly();
        InvokeRepeating(nameof(Monthly), tickRate, tickRate);
    }
    private void Monthly()
    {
        UpdatePopulation();
        if (month >= 12)
        {
            month = 1;
            year++;
            Yearly();
            UpdateDisplay();
            return;
        }
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



    void UpdatePopulation()
    {
        if (totalJobs.Available > 0 && totalPopulation.Space > totalPopulation.Current)
        {
            int peopleMovingIn = Mathf.Min(totalJobs.Available, totalPopulation.Space - totalPopulation.Current);

            totalPopulation.Current += peopleMovingIn;

            int peopleWithoutJobs = totalPopulation.Current - totalJobs.Taken;

            if (peopleWithoutJobs > 0)
            {
                int availableJobsForPeople = Mathf.Min(peopleWithoutJobs, totalJobs.Available);
                totalJobs.Taken += availableJobsForPeople;
            }
        }
    }




}
