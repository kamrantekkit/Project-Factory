using Godot;

public class PowerOverview : Label
{
    private TextureProgress EnergyBar;

    public override void _Ready()
    {
        EnergyBar = GetNode("../CenterContainer/EnergyBar") as TextureProgress;
    }
    public void UpdatePower(float PowerConsumption, float PowerGeneration)
    {
        string FormattedPowerText = $"Usage: {PowerConsumption}KW/{PowerGeneration}KW";
        float PercentUsed = PowerConsumption / PowerGeneration * 100;
        UpdateProgressBar(PercentUsed);
        this.Text = FormattedPowerText;
    }

    private void UpdateProgressBar(float PercentUsed)
    {
        EnergyBar.Value = 100 - PercentUsed;
    }
}
