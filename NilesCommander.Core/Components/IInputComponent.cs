namespace NilesCommander.Core.Components;

public interface IInputComponent : IComponent
{
    public event NilesCommander.CommandProvidedDelegate CommandProvided;
}