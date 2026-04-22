namespace ErpShowroom.Application.Common.Constants;

public static class CacheKeys
{
    public const string EMI_CUSTOMER_PREFIX = "emi:customer:";
    public const string PRODUCT = "product:{0}";
    public const string USER_PERMISSIONS = "user:perms:{0}";

    public static string EmiCustomer(long customerId) => $"{EMI_CUSTOMER_PREFIX}{customerId}";
}
