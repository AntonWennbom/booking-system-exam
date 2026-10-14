# Debuggfall – Buggrapport
---

## 🐞 Vilket fel undersökte du?
004 - Programmet kraschar vid felaktigt deltagarantal 

---

## 🔁 Hur återskapade du felet?
1. Starta applikationen.
2. Välj alternativ 3 i menyn [Create booking]
3. Mata in efterfrågad information fram till participant
4. Skriv in "five"
---

## Screenshot - Före
Här kastas exceptiono och applikationen kraschar. 
![Bokningarna visas inte korrekt](./Images/bug-004-before.png)  

## 📌 Vilket resultat fick du?  
Applikationen kastar ett ``` FormatException ``` och stänger ner.

---

## 🎯 Vilket resultat skulle du ha fått?
Användaren skulle ha fått information om vad som gick fel och fick försöka igen och skriva in en siffra nästa försök.

---

## 🧩 Vilken del av koden orsakade felet?
- I filen ```BookingConsole.cs``` kan vi se att värdet vi skriver in i konsolen för participants parsas direkt utan att kolla om en parse är möjlig. ```int.Parse()``` 
behöver en sträng i format av ett heltal för att fungera t.ex. ```"5"``` därför krashar det eftersom att vi skickar in ```"five"```.  
```csharp
        Console.Write("Participants: ");
        var participants = int.Parse(Console.ReadLine() ?? string.Empty);
```

---

## 🧩 Hur fixades buggen?
- Genom att skapa en metod för att validera och hantera Participant inputen och i den lägga ```int.TryParse(input, out participants)```
istället för att köra ```int.Parse()``` så kan vi också skriva ut ett meddelande till användaren att det måste vara ett nummer.
För att underlätta lade jag också till så att vi visar och kollar att värdet är gilltigt för det valda rummet redan här. 
Så att vi får snabb feedback på om det är ett giltigt nummer.
```csharp
      private int InputParticipants(string roomName)
    {
        int participants;
        var maxNumberInRoom = service.MaxParticipantsInRoom(roomName);

        while (true)
        {
            Console.WriteLine($"(For {roomName} enter a number between 1 and {maxNumberInRoom})");
            Console.WriteLine("Enter number of participants: ");
            var input = Console.ReadLine();

            if (int.TryParse(input, out participants) && participants > 0 && participants <= maxNumberInRoom)
                break;

            Console.WriteLine($"Participants must be a number greater than 0 and maximum {maxNumberInRoom}.");
        }

        return participants;
    }
```   
-  Jag kan inte se att något av testen hittade den här buggen då testet för participants ```Create_WhenParticipantCountExceedsCapacity_ReturnsFailure()```
tar en int när den skapar testbokningen och kollar så att man inte kan ha för många deltagare för det valda mötesrummet. 
Och alla metoder i ```BookingConsole.cs``` är privata och har inga tester. 