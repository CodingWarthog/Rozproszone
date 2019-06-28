using System;
using System.Collections.Generic;
using System.Text;

namespace Insertocepcja
{
    public class PaymentDTO
    {
        public int amount;
        public string speciality;

        public PaymentDTO(int amount, string speciality)
        {
            this.amount = amount;
            this.speciality = speciality;
        }
    }
}
