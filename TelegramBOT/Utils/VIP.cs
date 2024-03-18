using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBOT.Utils
{
    public class VIP
    {
        public string tgId { get; set; }
        public DateTime ExpirationDate { get; set; }

        public bool IsExpired()
        {
            return DateTime.Now > ExpirationDate;
        }
    }
}
