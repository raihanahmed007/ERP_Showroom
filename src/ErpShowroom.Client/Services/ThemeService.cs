using MudBlazor;

namespace ErpShowroom.Client.Services;

public class ThemeService
{
    public enum AppTheme
    {
        Normal,
        Dark,
        Purple
    }

    public AppTheme CurrentTheme { get; private set; } = AppTheme.Normal;

    public MudTheme MudTheme { get; private set; } = new();

    public event Action? OnThemeChanged;

    public ThemeService()
    {
        UpdateTheme();
    }

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        UpdateTheme();
        OnThemeChanged?.Invoke();
    }

    private void UpdateTheme()
    {
        MudTheme = CurrentTheme switch
        {
            AppTheme.Dark => GetDarkTheme(),
            AppTheme.Purple => GetPurpleTheme(),
            _ => GetNormalTheme()
        };
    }

    private MudTheme GetNormalTheme() => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#2563eb", // Blue 600
            Secondary = "#475569", // Slate 600
            AppbarBackground = "#ffffff",
            AppbarText = "#1e293b", // Slate 800
            Background = "#f8fafc", // Slate 50
            Surface = "#ffffff",
            DrawerBackground = "#ffffff",
            DrawerText = "#475569",
            Success = "#10b981",
            Error = "#ef4444",
            Warning = "#f59e0b",
            Info = "#3b82f6"
        }
    };

    private MudTheme GetDarkTheme() => new MudTheme()
    {
        PaletteDark = new PaletteDark()
        {
            Primary = "#3b82f6", // Blue 500
            Secondary = "#94a3b8", // Slate 400
            AppbarBackground = "#0f172a", // Slate 900
            AppbarText = "#f8fafc",
            Background = "#020617", // Slate 950
            Surface = "#0f172a",
            DrawerBackground = "#0f172a",
            DrawerText = "#94a3b8",
            Success = "#10b981",
            Error = "#f43f5e",
            Warning = "#fbbf24",
            Info = "#60a5fa"
        }
    };

    private MudTheme GetPurpleTheme() => new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = "#7c3aed", // Purple 600
            Secondary = "#d946ef", // Fuchsia 600
            AppbarBackground = "#7c3aed",
            AppbarText = "#ffffff",
            Background = "#f5f3ff", // Violet 50
            Surface = "#ffffff",
            DrawerBackground = "#1e1b4b", // Indigo 950
            DrawerText = "#e9d5ff",
            Success = "#059669",
            Error = "#e11d48",
            Warning = "#d97706",
            Info = "#2563eb"
        }
    };
}
