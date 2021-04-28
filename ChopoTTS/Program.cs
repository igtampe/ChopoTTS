using System;

namespace Igtampe.ChopoTTS {
    class Program {

        /// <summary>Runs the program</summary>
        /// <param name="args"></param>
        static void Main(string[] args) {

            //Define our voice V
            Voice V;

            //If the args include a PPAck, use it instead, otherwise use the default one
            if(args.Length == 1 && args[0].ToUpper().EndsWith(".PPACK")) { V = new Voice(args[0]); }
            else { V = new Voice("Chopo.ppack"); }

            //Write down from the initialization thing
            Console.WriteLine();
            Console.WriteLine();

            //If there's something to say, say it and return
            if(args.Length == 2) {V.Say(args[1],true); return; }

            //Otherwise
            string ToSpeak;

            //Keep asking the user for something to speak, until they want to exit.
            while(true) {
                Console.Write("> ");
                ToSpeak = Console.ReadLine();
                Console.WriteLine();
                if(ToSpeak.ToLower() == "exit") { return; }
                V.Say(ToSpeak,true);
                Console.WriteLine(); Console.WriteLine();
            }
        }
    }
}
