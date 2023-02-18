using System;

namespace ClientApp.Models
{
    public class FormModel
    {
        public string FileName { get; } = string.Empty;
        public string FileExtension { get; } = string.Empty;
        public byte[] FileBytes { get; } = Array.Empty<byte>();
        


        public FormModel()
        {

        }

        public FormModel(string fn,string fe, byte[] fb)
        {
            FileName = fn;
            FileExtension = fe;
            FileBytes = fb;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
