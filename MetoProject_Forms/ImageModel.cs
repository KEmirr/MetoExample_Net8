using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetoFirstExample_v4_WinFormMain
{
    public class ImageModel
    {
        
        //Image Model sınıfı API tarafına aktarıldı
        public string ImagePath { get; set; }
        public byte[] ImageData { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
