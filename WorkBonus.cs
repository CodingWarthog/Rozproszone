using System;
using System.Collections.Generic;
using System.Text;

namespace Insertocepcja
{
    public class WorkBonus
    {
        public int amount;
        public string bonusName;

        public WorkBonus(int amount, string bonusName)
        {
            this.amount = amount;
            this.bonusName = bonusName;
        }
    }
}
