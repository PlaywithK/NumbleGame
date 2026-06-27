public static class GameModeSelection
{
    private static int _selectedIndex = 0;

    public static int SelectedIndex
    {
        get => _selectedIndex;
        set => _selectedIndex = value >= 0 ? value : 0;
    }
}
