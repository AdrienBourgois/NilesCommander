namespace NilesCommander.Core.Components
{
    public interface IOutputComponent : IComponent
    {
        protected internal void LogProvided(Log _log);
    }
}