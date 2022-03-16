using Godot;
using System.Collections.Generic;

public class PowerGridNetwork : Node2D
{
    private float CurrentPowerGeneration = 0;
    private float CurrentPowerConsumption = 0;
    public bool IsOverloaded { get; private set; }
    public List<BaseMultiGenerator> ConnectedGenerators = new List<BaseMultiGenerator>();
    public List<BaseMachine> ConnectedConsumers = new List<BaseMachine>();

    public override void _Ready()
    {
        Timer WorldTimer = (Timer)GetNode("/root/SceneNode/WorldTimer");
        WorldTimer.Connect("timeout", this, "On_Timer_Timeout");
    }

    public void On_Timer_Timeout()
    {
        CheckPowerGridState();
        UpdateGUI();
    }

    private void CheckPowerGridState()
    {
        if (CurrentPowerConsumption <= CurrentPowerGeneration)
        {
            IsOverloaded = false; //Normal Operation
            return;
        }
        else
        {
            IsOverloaded = true; //If more power is being consumed then produced.
        }
    }

    public void UpdatePowerOutput()
    {
        float TotalGenOutput = 0;
        foreach (BaseMultiGenerator gen in ConnectedGenerators)
        {
            TotalGenOutput += gen.CurrentPowerOutput;

        }
        CurrentPowerGeneration = TotalGenOutput;
    }

    public void UpdatePowerConsumption()
    {
        float TotalPowerCosumtion = 0;
        foreach (BaseMachine machine in ConnectedConsumers)
        {
            TotalPowerCosumtion += machine.PowerConsumption;
        }
        CurrentPowerConsumption = TotalPowerCosumtion;
    }
    private void UpdateGUI()
    {
        HUDManager.PowerOverviewHUD.UpdatePower(CurrentPowerConsumption, CurrentPowerGeneration);
    }
}
