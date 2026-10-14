## Buggar

# Debuggfall – Buggrapport

Nedan följer dokumentation av ett specifikt debuggarbete.  
Strukturen är avsedd att vara enkel, konsekvent och lätt att följa.

---

## 🐞 Vilket fel undersökte du?
001 - Visa alla aktiva bokningar.  

---

## 🔁 Hur återskapade du felet?
1. Starta applikationen.
2. Välj alternativ 1 i menyn [Show active bookings]
3. Bokningarna visas

---

## Screenshot  
Här visas inaktiva bokningar och fel sortering.  
![Crash vid kundlista](./Images/bug-001-before.png)  

## 📌 Vilket resultat fick du?
Jag fick en lista på alla boknginarna som finns i systemet, den visade även bokningar som inte är aktiva, samt att den inte listade boknignana i rätt följd.

---

## 🎯 Vilket resultat skulle du ha fått?
Jag skulle ha fått en lista på de bokningarna som är aktiva i systemet, dvs inte avbokade möten. De skulle även listas efter datum ordning 
och vid flera bokningar samma dag ska de boknigarna listas efter starttid.

---

## 🧩 Vilken del av koden orsakade felet?
```csharp


```

---

## 🧩 Hur fixades buggen?