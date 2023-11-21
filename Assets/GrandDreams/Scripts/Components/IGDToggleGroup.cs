namespace GrandDreams.Core.Components
{
    public interface IGDToggleGroup
    {
        bool AllowSelectMultiple { get; set; }

        void AddToggle(IGDToggle toggle);

        void BroadcastToggleState(IGDToggle toggleSender);
    }
}
