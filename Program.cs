using System;
using Stateless;
using static System.Console;

namespace wg_net_state_machine
{
    class Program
    {
        static void Main(string[] args)
        {       
            LampTest();
        }

        private static void LampTest()
        {
            Lamp lamp = new Lamp();

            BackgroundColor = ConsoleColor.Blue;
            ForegroundColor = ConsoleColor.White;
            WriteLine(lamp.Diagram);
            ResetColor();

            Dump(lamp.Status);

            lamp.PushButton();

            Dump(lamp.Status);

            if(lamp.CanPushButton)
               lamp.PushButton();

            Dump(lamp.Status);

            if (lamp.CanPushButton)
              lamp.PushButton();

            Dump(lamp.Status);

            ResetColor();
        }

        static ConsoleColor[] colors = { ConsoleColor.DarkGray, ConsoleColor.Red, ConsoleColor.Green };
        
        private static void Dump(Status status)
        {
            ForegroundColor = colors[(int) status]; 

            System.Console.WriteLine(status);

            ResetColor();
        }
       
    }

    class Lamp
    {
        private StateMachine<Status, Trigger> machine = new StateMachine<Status, Trigger>(Status.Off);

        public Lamp()
        {
            machine.Configure(Status.Off)   
                .Permit(Trigger.Push, Status.On);

            machine.Configure(Status.On)
                .OnEntry(() => WriteLine("Pamiętaj o wyłączeniu światła."), "Powitanie")
                .Permit(Trigger.Push, Status.Blinking);

            machine.Configure(Status.Blinking)
                .Permit(Trigger.Push, Status.Off)
                .OnExit(()=>SendSms("Dziękuję za wyłączenie światła."), "Podziękowanie");


            // śledzenie maszyny stanów         
            machine.OnTransitioned(t=> WriteLine($"{t.Source} -> {t.Destination}"));

            

        }

        public void SendSms(string message)
        {
            WriteLine($"SMS: {message}");
        }


        public string Diagram => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());
        public Status Status => machine.State;

    

        public void PushButton() => machine.Fire(Trigger.Push);

        public bool CanPushButton => machine.CanFire(Trigger.Push);
    }

    enum Status
    {
        On,
        Off,
        Blinking
    }

    enum Trigger
    {
        Push
    }
}
