# Maszyna stanu w praktyce

Kod źródłowy pochodzący z mojego wystąpienia 21.02.2019 na 123 spotkaniu **Warszawskiej Grupy .NET** (WG.NET). 


![Image of Marcin](wgnet.png)

## Instalacja biblioteki

~~~ bash
dotnet add package Stateless --version 4.2.1
~~~


## Definicja maszyny stanów
Określamy typ stanu (State), typ wyzwalacza (Trigger) oraz stan początkowy.
~~~ csharp
StateMachine<Status, Trigger> machine = new StateMachine<Status, Trigger>(Status.Off);
~~~


## Konfiguracja przejść
Za pomocą metody _Configure()_ oraz _Permit()_ definiujemy dopuszczalne przejścia. Dodatkowo za pomocą metod _OnEntry()_ i _OnExit()_ możemy zdefiniować dodatkowe akcje, które mają być uruchamianie przy wejściu lub po wyjściu ze stanu.

~~~ csharp
 machine.Configure(Status.Off)   
                .Permit(Trigger.Push, Status.On);

            machine.Configure(Status.On)
                .OnEntry(() => WriteLine("Pamiętaj o wyłączeniu światła."), "Powitanie")
                .Permit(Trigger.Push, Status.Blinking);

            machine.Configure(Status.Blinking)
                .Permit(Trigger.Push, Status.Off)
                .OnExit(()=>SendSms("Dziękuję za wyłączenie światła."), "Podziękowanie"); machine.Configure(Status.Off)   
                .Permit(Trigger.Push, Status.On);

            machine.Configure(Status.On)
                .OnEntry(() => WriteLine("Pamiętaj o wyłączeniu światła."), "Powitanie")
                .Permit(Trigger.Push, Status.Blinking);

            machine.Configure(Status.Blinking)
                .Permit(Trigger.Push, Status.Off)
                .OnExit(()=>SendSms("Dziękuję za wyłączenie światła."), "Podziękowanie");
~~~


## Uruchomienie wyzwalacza
Na podstawie wcześniej zdefiniowanej konfiguracji nastąpi przejście do kolejnego stanu.
Jeśli przejście nie zostało zdefiniowane pojawi się Exception.

~~~ csharp
machine.Fire(Trigger.Push)
~~~

## Pobranie bieżącego stanu
~~~ csharp
Console.WriteLine(machine.State)
~~~ 


## Sprawdzenie wyzwalacza
~~~ csharp
machine.CanFire(Trigger.Push)
~~~

## Śledzenie maszyny stanów   
~~~ csharp
 machine.OnTransitioned(t=> Console.WriteLine($"{t.Source} -> {t.Destination}"));
~~~        
            
## Wizualizacja grafu
~~~ csharp
Console.WriteLine(Stateless.Graph.UmlDotGraph.Format(machine.GetInfo()));
~~~

Zostanie wygenerowany graf w formacie [ DOT Graph](https://en.wikipedia.org/wiki/DOT_(graph_description_language)), który można wyświetlić na stronie:

http://www.webgraphviz.com
