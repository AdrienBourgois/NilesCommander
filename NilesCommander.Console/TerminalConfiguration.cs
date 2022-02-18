using NilesCommander.Core.Components;

namespace NilesCommander.Terminal
{
    public class TerminalConfiguration : ComponentConfiguration
    {
        public int HistoryLength = -1;
        public int HistoryDisplayedHeight;
    }
}