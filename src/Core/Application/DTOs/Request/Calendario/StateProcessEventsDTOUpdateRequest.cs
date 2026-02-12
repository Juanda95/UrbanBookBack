using Domain.Entities.DCalendario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Calendario
{
    public class StateProcessEventsDTOUpdateRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the state process event.
        /// </summary>
        public int StateProcessEventsId { get; set; }

        /// <summary>
        /// Gets or sets the state of the process event.
        /// </summary>
        public string State { get; set; } = string.Empty;

    }
}
