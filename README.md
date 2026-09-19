# FunctionalDeliveryCalculator

## Run
dotnet run   (requires .NET 6+)
Enter: price, items, express (true/false), type, zone.

## Answers
1. Input/output: Main and the Prompt method only. Parsing methods and calculation methods never use Console.
<img width="1241" height="892" alt="изображение" src="https://github.com/user-attachments/assets/2bedb63b-133c-40ff-8353-f6e34e765099" />

2. Pure calculation: ApplyRule, Percent, GetItemsRule, GetTypeRule, GetZoneRule, RoundFinal, CalculateFinalPrice.
<img width="1232" height="935" alt="изображение" src="https://github.com/user-attachments/assets/f56e73c7-96b3-499e-8b87-fd5dcffa52cf" />


3. Func<decimal, decimal> represents one pricing rule. Each rule is created by a method or lambda, stored in a Func variable, and applied by ApplyRule in the required order.
<img width="1045" height="156" alt="изображение" src="https://github.com/user-attachments/assets/59806fb6-aeff-4eac-9d62-6f5f7d26e082" />


4. TryParse returns false instead of throwing an exception, so invalid user input produces a clear message and the program does not crash.

<img width="1398" height="862" alt="изображение" src="https://github.com/user-attachments/assets/3605e0d8-705c-4f90-9273-e1e87df0e627" />


## Tests
<img width="587" height="163" alt="изображение" src="https://github.com/user-attachments/assets/e7caa783-ca8e-4f49-b040-51901c70a09c" />

Here we can see that price not changed, because all our options are not adding or reducing price

<img width="637" height="155" alt="изображение" src="https://github.com/user-attachments/assets/65cfc6a2-2116-47f9-8526-8f324ed9e69c" />

here we can see that our price is as high as possible, because all our options increased price

<img width="527" height="163" alt="изображение" src="https://github.com/user-attachments/assets/4dfb74c3-9051-4729-813c-c2f32211b944" />

<img width="566" height="160" alt="изображение" src="https://github.com/user-attachments/assets/1c0a0eb9-538d-4a13-8c6b-80c5336b2d94" />

<img width="786" height="172" alt="изображение" src="https://github.com/user-attachments/assets/e033aba2-06da-449c-a787-8ab41334e89f" />


and here we see the different errors
