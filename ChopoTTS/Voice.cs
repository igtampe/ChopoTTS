using System;
using System.Media;
using System.Collections.Generic;
using System.Threading;
using Igtampe.CabHandler;
using System.IO;

namespace Igtampe.ChopoTTS {

    /// <summary>Holds a ChopoTTS Voice, generated from a ChopoTTS Phoneme Pack (PPACK)</summary>
    public class Voice {

        /// <summary>Dictionary of all Graphemes to Phonemes.</summary>
        private readonly Dictionary<string,byte[]> Graphemes = new Dictionary<string, byte[]>();

        /// <summary>Default list of base graphemes</summary>
        private static readonly string[] BaseGraphemes = {"a","ah","ai","air","ao","aow","aw","ay","b","ch","d","e","ear","ee","er","f","g","h","i","ir","j","k","l","m","n","ng","o","oh","oi","oo","p","r","s","sh","sz","t","th","u","ure","v","w","y","z"};

        /// <summary>Creates a voice from a ChopoTTS phoneme pack</summary>
        /// <param name="VoicePack"></param>
        public Voice(string VoicePack) {
            if(!VoicePack.ToUpper().EndsWith(".PPACK")) { throw new ArgumentException("Provided file doesn't appear to be a Phoneme Pack"); }

            string OpenDIR = "OpenVoicePack";

            if(Directory.Exists(OpenDIR)) { Directory.Delete(OpenDIR,true); }

            Console.WriteLine("Loading " + VoicePack);
            CabHandlerUtils.CabToFolder(VoicePack,OpenDIR);
            foreach(string BP in BaseGraphemes) {Graphemes.Add(BP,File.ReadAllBytes(Path.Combine(OpenDIR,BP+".wav")));}

            //Now we're going to link the BPs to their other graphemes
            Graphemes.Add("bb",Graphemes["b"]);
            
            Graphemes.Add("dd",Graphemes["d"]);
            Graphemes.Add("ed",Graphemes["d"]);

            Graphemes.Add("ff",Graphemes["f"]);
            Graphemes.Add("ph",Graphemes["f"]);

            Graphemes.Add("wh",Graphemes["h"]);

            Graphemes.Add("ge",Graphemes["j"]);
            Graphemes.Add("dge",Graphemes["j"]);
            Graphemes.Add("dj",Graphemes["j"]);
            Graphemes.Add("gg",Graphemes["j"]);

            Graphemes.Add("c",Graphemes["k"]);
            Graphemes.Add("cc",Graphemes["k"]);
            Graphemes.Add("q",Graphemes["k"]);
            Graphemes.Add("ck",Graphemes["k"]);

            Graphemes.Add("ll",Graphemes["l"]);

            Graphemes.Add("mm",Graphemes["m"]);

            Graphemes.Add("nn",Graphemes["n"]);
            Graphemes.Add("kn",Graphemes["n"]);

            Graphemes.Add("pp",Graphemes["p"]);

            Graphemes.Add("rr",Graphemes["r"]);
            Graphemes.Add("rh",Graphemes["r"]);

            Graphemes.Add("ss",Graphemes["s"]);
            Graphemes.Add("ps",Graphemes["s"]);

            Graphemes.Add("tt",Graphemes["t"]);

            Graphemes.Add("zz",Graphemes["z"]);
            Graphemes.Add("x",Graphemes["z"]);
            
            Graphemes.Add("tch",Graphemes["ch"]);
            
            Graphemes.Add("ngue",Graphemes["ng"]);

            Graphemes.Add("eigh",Graphemes["ay"]);
            Graphemes.Add("aigh",Graphemes["ay"]);
            Graphemes.Add("ey",Graphemes["ay"]);
            Graphemes.Add("ea",Graphemes["ay"]);

            Graphemes.Add("ae",Graphemes["e"]);
            
            Graphemes.Add("ie",Graphemes["ee"]);

            Graphemes.Add("igh",Graphemes["ai"]);

            Graphemes.Add("ough",Graphemes["aw"]);

            Graphemes.Add("oa",Graphemes["oh"]);

            Graphemes.Add("ou",Graphemes["oo"]);

            Graphemes.Add("oy",Graphemes["oi"]);
            Graphemes.Add("uoy",Graphemes["oi"]);

            Graphemes.Add("yr",Graphemes["ir"]);

            Graphemes.Add("eer",Graphemes["ear"]);
            Graphemes.Add("ere",Graphemes["ear"]);
            Graphemes.Add("ier",Graphemes["ear"]);
        }

        /// <summary>Speaks out the provided string with this voice's graphemes, with the specified display option, and a 125ms dellay between each grapheme</summary>
        /// <param name="Something">thing to say</param>
        /// <param name="display">Whether or not to display the text as its said</param>
        public void Say(string Something, bool display) => Say(Something, display, 125);

        /// <summary>Speaks out the provided string with this voice's graphemes, with the specified display option, and delay between each grapheme</summary>
        /// <param name="Something">thing to say</param>
        /// <param name="display">Whether or not to display the text as its said</param>
        /// <param name="Delay">delay in milliseconds for each grapheme</param>
        public void Say(string Something, bool display, int Delay) {

            if(File.Exists(Something)) { Something = File.ReadAllText(Something); }

            Something = Something.ToLower();

            Word W = null;

            while(!string.IsNullOrWhiteSpace(Something)) {

                //take the first 4 characters
                string PotentialGrapheme = Something.Substring(0,Math.Min(4,Something.Length));

                //Keep analyzing the potential grapheme until we find a match, or it's only one character in length
                while(PotentialGrapheme.Length!=1 && !Graphemes.ContainsKey(PotentialGrapheme)) {
                    PotentialGrapheme = PotentialGrapheme.Substring(0,PotentialGrapheme.Length - 1); //keep trimming the last character out
                }

                //If we have to display then display
                if(display) { Console.Write(PotentialGrapheme); }

                //If we have a match, then say it
                if(Graphemes.ContainsKey(PotentialGrapheme)) {
                    if (W == null) { W = new Word(); }
                    W.AddGrapheme(Graphemes[PotentialGrapheme]);
                    //PlayPhoneme(PotentialGrapheme, Delay); 
                }

                //Special cases
                switch(PotentialGrapheme) {
                    case ".":
                    case "?":
                    case "!":
                        if (W != null) { SayWord(W); }
                        W = null;
                        Thread.Sleep(1000);
                        break;
                    case ",":
                        if (W != null) { SayWord(W); }
                        W = null;
                        Thread.Sleep(500);
                        break;
                    case " ":
                        if (W != null) { SayWord(W); }
                        W = null;
                        break;
                    case "1":
                        Say("won",false) ;
                        break;
                    case "2":
                        Say("too",false);
                        break;
                    case "3":
                        Say("three",false);
                        break;
                    case "4":
                        Say("for",false);
                        break;
                    case "5":
                        Say("faiv",false);
                        break;
                    case "6":
                        Say("sicks",false);
                        break;
                    case "7":
                        Say("seven",false);
                        break;
                    case "8":
                        Say("ayt",false);
                        break;
                    case "9":
                        Say("nain",false);
                        break;
                    case "0":
                        Say("zeeroh",false);
                        break;
                    default:
                        //If we have nothing then we make no sound
                        break;
                }

                //Remove the bit of the text we're supposed to say that we've already said
                Something = Something.Remove(0,PotentialGrapheme.Length);
            }
            
            if (W != null) { SayWord(W); }
        }

        /// <summary>Plays one phoneme out of the set of graphemes for this voice.</summary>
        /// <param name="Grapheme"></param>
        /// <param name="Delay"></param>
        public void PlayPhoneme(string Grapheme, int Delay) {
            SoundPlayer S = new SoundPlayer {Stream = new MemoryStream(Graphemes[Grapheme])};
            S.Play();
            Thread.Sleep(Delay);
        }

        public void SayWord(Word W) {
            using (SoundPlayer S = new SoundPlayer { Stream = new MemoryStream(W.GenerateWave()) }) {
                S.PlaySync();
            }
        }
    }
}
