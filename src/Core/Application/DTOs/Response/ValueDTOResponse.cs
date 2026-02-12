public class ValueDTOResponse
{

    /// <summary>
    /// Gets or sets the ID of the value.
    /// </summary>
    /// <value>The identifier value.</value>
    public int IdValue { get; set; }

    /// <summary>
    /// Identificador único del valor.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Valor específico.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del valor.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    // Puedes añadir o quitar propiedades según las necesidades de tu aplicación.
}
