namespace Application.DTOs.Response
{
    /// <summary>
    /// DTO for parameter responses.
    /// </summary>
    public class ParameterDTOResponse
    {
        /// <summary>
        /// Gets or sets the identifier of the parameter.
        /// </summary>
        /// <value>The identifier of the parameter.</value>
        public int IdParameter { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public string TypeParameter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the parameter.
        /// </summary>
        /// <value>The creation date of the parameter.</value>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the modification date of the parameter.
        /// </summary>
        /// <value>The modification date of the parameter.</value>
        public DateTime ModifierDate { get; set; }

        /// <summary>
        /// Gets or sets the user who created the parameter.
        /// </summary>
        /// <value>The user who created the parameter.</value>
        public string CreationUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user who made the modification to the parameter.
        /// </summary>
        /// <value>The user who made the modification.</value>
        public string ModifierUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of values associated with the parameter.
        /// </summary>
        /// <value>The collection of values.</value>
        public ICollection<ValueDTOResponse>? Values { get; set; }
    }
}
