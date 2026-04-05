using System;
using Microsoft.AspNetCore.DataProtection;
using ErpShowroom.Application.Common.Interfaces;

namespace ErpShowroom.Infrastructure.Identity;

public class DataProtectionEncryptionService : IEncryptionService
{
    private readonly IDataProtector _protector;

    public DataProtectionEncryptionService(IDataProtectionProvider provider)
    {
        // "CustomerSensitiveData" purpose is used to separate high-security protectors.
        _protector = provider.CreateProtector("CustomerSensitiveData");
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;
        return _protector.Protect(plainText);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return string.Empty;
        try
        {
            return _protector.Unprotect(cipherText);
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            // Logging can be added here if needed to investigate potential tempering
            return "[Decryption Failed]";
        }
    }
}
