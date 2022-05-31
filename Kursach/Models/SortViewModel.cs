using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models
{
    public class SortViewModel
    {
        public StateSort IdSort { get; private set; }
        public StateSort EmailSort { get; private set; }
        public StateSort Current { get; private set; }

        public SortViewModel(StateSort sortOrder)
        {
            IdSort = sortOrder == StateSort.IdAsc ?
                StateSort.IdDesc : StateSort.IdAsc;
            EmailSort = sortOrder == StateSort.EmailAsc ?
                StateSort.EmailDesc : StateSort.EmailAsc;
            Current = sortOrder;
        }

    }
}
