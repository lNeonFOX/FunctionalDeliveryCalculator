using System;
using System.Globalization;

namespace FunctionalDeliveryCalculator;

enum DeliveryType
{
    Pickup,
    Courier,
    DoorToDoor
}

enum DeliveryZone
{
    City,
    OutsideCity,
    Remote
}

readonly record struct Order(
    decimal BasePrice,
    int Items,
    bool IsExpress,
    DeliveryType Type,
    DeliveryZone Zone);

static class Program
{
    static void Main()
    {
        Console.WriteLine("=== Functional Delivery Calculator ===");

        string? rawPrice   = Prompt("Base delivery price: ");
        string? rawItems   = Prompt("Number of items: ");
        string? rawExpress = Prompt("Express delivery (true/false): ");
        string? rawType    = Prompt("Delivery type (Pickup, Courier, DoorToDoor): ");
        string? rawZone    = Prompt("Delivery zone (City, OutsideCity, Remote): ");

        if (!TryParseOrder(rawPrice, rawItems, rawExpress, rawType, rawZone,
                           out Order order, out string error))
        {
            Console.WriteLine($"Error: {error}");
            return;
        }

        decimal finalPrice = CalculateFinalPrice(order);
        Console.WriteLine($"Final delivery price: {finalPrice.ToString("F2", CultureInfo.InvariantCulture)}");
    }

    static string? Prompt(string label)
    {
        Console.Write(label);
        return Console.ReadLine();
    }

    static bool IsMissing(string? text) => string.IsNullOrWhiteSpace(text);

    static bool TryParseEnum<TEnum>(string? raw, out TEnum value) where TEnum : struct, Enum
    {
        value = default;
        if (IsMissing(raw) || int.TryParse(raw, out _)) return false;
        return Enum.TryParse<TEnum>(raw, true, out value) && Enum.IsDefined(value);
    }

    static bool TryParseOrder(
        string? rawPrice, string? rawItems, string? rawExpress,
        string? rawType, string? rawZone,
        out Order order, out string error)
    {
        order = default;
        error = string.Empty;

        if (IsMissing(rawPrice))
        { error = "Base price is missing."; return false; }
        if (!decimal.TryParse(rawPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
        { error = $"'{rawPrice}' is not a valid price."; return false; }
        if (price < 0)
        { error = "Base price cannot be negative."; return false; }

        if (IsMissing(rawItems))
        { error = "Number of items is missing."; return false; }
        if (!int.TryParse(rawItems, NumberStyles.Integer, CultureInfo.InvariantCulture, out int items))
        { error = $"'{rawItems}' is not a valid whole number of items."; return false; }
        if (items < 1)
        { error = "Number of items must be at least 1."; return false; }

        if (IsMissing(rawExpress))
        { error = "Express status is missing."; return false; }
        if (!bool.TryParse(rawExpress, out bool express))
        { error = $"'{rawExpress}' is not valid. Use true or false."; return false; }

        if (!TryParseEnum(rawType, out DeliveryType type))
        { error = $"'{rawType}' is not a valid delivery type. Use Pickup, Courier or DoorToDoor."; return false; }

        if (!TryParseEnum(rawZone, out DeliveryZone zone))
        { error = $"'{rawZone}' is not a valid zone. Use City, OutsideCity or Remote."; return false; }

        order = new Order(price, items, express, type, zone);
        return true;
    }

    static decimal ApplyRule(decimal price, Func<decimal, decimal> rule) => rule(price);

    static Func<decimal, decimal> Percent(decimal percent) =>
        price => price * (1 + percent / 100m);

    static Func<decimal, decimal> GetItemsRule(int items) => items switch
    {
        >= 8 => Percent(20),
        >= 4 => Percent(10),
        _    => Percent(0)
    };

    static Func<decimal, decimal> GetTypeRule(DeliveryType type) => type switch
    {
        DeliveryType.Pickup     => Percent(-20),
        DeliveryType.Courier    => Percent(0),
        DeliveryType.DoorToDoor => Percent(15),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    static Func<decimal, decimal> GetZoneRule(DeliveryZone zone) => zone switch
    {
        DeliveryZone.City        => Percent(0),
        DeliveryZone.OutsideCity => Percent(25),
        DeliveryZone.Remote      => Percent(40),
        _ => throw new ArgumentOutOfRangeException(nameof(zone))
    };

    static decimal RoundFinal(decimal price) =>
        Math.Round(price, 2, MidpointRounding.AwayFromZero);

    static decimal CalculateFinalPrice(Order order)
    {
        Func<decimal, decimal> itemsRule   = GetItemsRule(order.Items);
        Func<decimal, decimal> typeRule    = GetTypeRule(order.Type);
        Func<decimal, decimal> zoneRule    = GetZoneRule(order.Zone);
        Func<decimal, decimal> expressRule = order.IsExpress ? Percent(30) : (price => price);


        decimal afterItems   = ApplyRule(order.BasePrice, itemsRule);
        decimal afterType    = ApplyRule(afterItems, typeRule);
        decimal afterZone    = ApplyRule(afterType, zoneRule);
        decimal afterExpress = ApplyRule(afterZone, expressRule);

        return RoundFinal(afterExpress);
    }
}