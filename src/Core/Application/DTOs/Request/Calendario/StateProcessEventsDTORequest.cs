using Domain.Entities.DCalendario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Calendario
{
    public class StateProcessEventsDTORequest
    {

        /// <summary>
        /// Gets or sets the state of the process event.
        /// </summary>
        [Required(ErrorMessage = "El campo State es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo State debe tener un máximo de 50 caracteres.")]
        public string State { get; set; } = string.Empty;

    }
}
