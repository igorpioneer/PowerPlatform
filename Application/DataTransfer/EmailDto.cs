using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class EmailDto
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public byte[] Attachment { get; set; }
        public string SendTo { get; set; }

    }
}
