using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace Igtampe.ChopoTTS {
    public class Word {

        private readonly List<byte[]> Graphemes = new List<byte[]>();

        public void AddGrapheme(byte[] Grapheme) => Graphemes.Add(Grapheme);

        public byte[] GenerateWave() {

            if(Graphemes.Count == 0) return null;
            
            WaveFileWriter WaveRider = null;

            using (MemoryStream OutStream = new MemoryStream()) {

                foreach (var Grapheme in Graphemes) {

                    using (WaveFileReader reader = new WaveFileReader(new MemoryStream(Grapheme))) {

                        if (WaveRider == null) { WaveRider = new WaveFileWriter(OutStream, reader.WaveFormat); } 
                        else if (!reader.WaveFormat.Equals(WaveRider.WaveFormat)) { throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format"); }

                        WaveRider.Write(Grapheme, 0, Grapheme.Length);
                    }

                }

                if (WaveRider != null) { WaveRider.Dispose(); }
                
                return OutStream.ToArray();
            
            }        
        }
    }
}
