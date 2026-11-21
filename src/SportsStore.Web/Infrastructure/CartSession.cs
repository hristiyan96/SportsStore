using System.Text.Json;
using SportsStore.Services.Cart;

namespace SportsStore.Web.Infrastructure;

public static class CartSession
{
    private const string Key = "sportsstore.cart";

    public static IReadOnlyList<StoredCartLine> Read(ISession session)
    {
        var json = session.GetString(Key);
        if (string.IsNullOrWhiteSpace(json))
        {
            return Array.Empty<StoredCartLine>();
        }

        return JsonSerializer.Deserialize<List<StoredCartLine>>(json) ?? new List<StoredCartLine>();
    }

    public static void Write(ISession session, IReadOnlyList<StoredCartLine> lines)
    {
        session.SetString(Key, JsonSerializer.Serialize(lines));
    }
}
