using UnityEngine;
using TMPro;

public class StatsManager : MonoBehaviour
{
    public int TotalMoney = 10000;
    public int TotalPopulation;
    public int TotalPopulationSpace;
    public int TotalJobs;
    public int TotalJobPositions;
    public int TotalElectricityCosts;
    public int TotalElectricity;
    public int TotalLandValue;
    public int TotalCrimeRate;
    public int TotalPollution;
    public int TotalFireCoverage;
    public int TotalExpenses;

    public float residentialDemand;
    public float commercialDemand;
    public float industrialDemand;

    public float residentialTaxRate = 0.05f;
    public float commercialTaxRate = 0.05f;
    public float industrialTaxRate = 0.05f;
    public float tickRate = 5f;
    public int year = 2000;

    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI yearDisplay;
    public TextMeshProUGUI demandDisplay;

    private void Start()
    {
        Yearly();
        InvokeRepeating(nameof(Yearly), tickRate, tickRate);

    }

    private void Yearly()
    {
        ProcessIncome();
        CalculateDemand();
        UpdatePopulation();
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        moneyDisplay.text = $"Money: ${TotalMoney}";
        CalculateDemand();

    }
    void ProcessIncome()
    {
        int residentialIncome = Mathf.RoundToInt(TotalPopulation * residentialTaxRate * 10);
        int commercialIncome = Mathf.RoundToInt(TotalJobPositions * commercialTaxRate * 15);
        int industrialIncome = Mathf.RoundToInt(TotalJobPositions * industrialTaxRate * 20);

        int totalIncome = residentialIncome + commercialIncome + industrialIncome;
        int expenses = TotalExpenses;

        TotalMoney += totalIncome - expenses;
        year++;

        yearDisplay.text = $"Year: {year}";
    }

    void CalculateDemand()
    {


        demandDisplay.text = $"R: {residentialDemand}% C: {commercialDemand}% I: {industrialDemand}%";

    }


    void UpdatePopulation()
    {

    }


}
