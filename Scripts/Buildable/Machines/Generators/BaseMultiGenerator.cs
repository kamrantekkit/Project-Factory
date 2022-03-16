public class BaseMultiGenerator : BaseMachineMultiTile
{
    public float MaxPowerOutput;
    public float CurrentPowerOutput;

    public BaseMultiGenerator()
    {
        IsConsumer = false;
    }

    public override void _Ready()
    {
        base._Ready();
        PowerGridNetwork.ConnectedGenerators.Add(this);
    }

    public override void _ExitTree()
    {
        PowerGridNetwork.ConnectedGenerators.Remove(this);
        UpdatePowerStatus();
    }
    protected new virtual void UpdatePowerStatus()
    {
        PowerGridNetwork.UpdatePowerOutput();
    }
}
