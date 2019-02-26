# Maszyna stanu w praktyce

Kod źródłowy pochodzący z mojego wystąpienia 21.02.2019 na 123 spotkaniu **Warszawskiej Grupy .NET** (WG.NET). 


## Instalacja biblioteki

~~~ bash
dotnet add package Stateless --version 4.2.1
~~~


## Definicja maszyny stanów

~~~ csharp
StateMachine<Status, Trigger> machine = new StateMachine<Status, Trigger>(Status.Off);
~~~


## Konfiguracja przejść
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
                .OnExit(()=>SendSms("Dziękuję za wyłączenie światła."), "Podziękowanie

- Wizualizacja grafu
http://www.webgraphviz.com
