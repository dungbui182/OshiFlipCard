namespace GrandDreams.Core.Components
{
    public interface IGDToggle
    {
        bool IsOn { get; set; }

        void RegisterToggleGroup(IGDToggleGroup toggleGroup);
    }
}