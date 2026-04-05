namespace ErpShowroom.Application.Common.Interfaces;

public interface IOcrService
{
    string ExtractText(string imagePath);
}
