namespace NilesCommander.Core.Components;

public interface IComponent
{
    public ComponentConfiguration Configuration { get; set; }

    public void SetComponentConfiguration(ComponentConfiguration _configuration);

    public bool Initialize();

    public bool Close();
}