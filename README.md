# FunctionalDeliveryCalculator

## Run
dotnet run   (requires .NET 6+)
Enter: price, items, express (true/false), type, zone.

## Answers
1. Input/output: Main and the Prompt method only. Parsing methods and calculation methods never use Console.
2. Pure calculation: ApplyRule, Percent, GetItemsRule, GetTypeRule, GetZoneRule, RoundFinal, CalculateFinalPrice.
3. Func<decimal, decimal> represents one pricing rule. Each rule is created by a method or lambda, stored in a Func variable, and applied by ApplyRule in the required order.
4. TryParse returns false instead of throwing an exception, so invalid user input produces a clear message and the program does not crash.

## Tests
<img width="587" height="163" alt="изображение" src="https://github.com/user-attachments/assets/e7caa783-ca8e-4f49-b040-51901c70a09c" />

Here we can see that price not changed, because all our options are not adding or reducing price

<img width="637" height="155" alt="изображение" src="https://github.com/user-attachments/assets/65cfc6a2-2116-47f9-8526-8f324ed9e69c" />

here we can see that our price is as high as possible, because all our options increased price

<img width="527" height="163" alt="изображение" src="https://github.com/user-attachments/assets/4dfb74c3-9051-4729-813c-c2f32211b944" />

and here we see the error
