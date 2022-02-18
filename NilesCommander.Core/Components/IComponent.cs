using System;

namespace NilesCommander.Core.Components
{
    public interface IComponent : IDisposable
    {
        public ComponentConfiguration Configuration { get; set; }

        public void SetComponentConfiguration(ComponentConfiguration _configuration);

        public bool Initialize();
    }
}