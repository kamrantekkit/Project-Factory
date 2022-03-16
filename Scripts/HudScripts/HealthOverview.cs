using Godot;

public class HealthOverview : Label
{
    public void UpdateHealth(float MaxHealth, float CurrentHealth)
    {
        string FormattedPowerText = $"{CurrentHealth}/{MaxHealth}HP";
        this.Text = FormattedPowerText;
    }
}
