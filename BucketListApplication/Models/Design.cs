using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class Design
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public Color BorderColor { get; set; }
        public Color BackgroundColor { get; set; }

        public BorderType BorderType { get; set; }

    }
}
