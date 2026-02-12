namespace Domain.Entities.Parametros
{
    public class Value
    {
        /// <summary>
        /// Gets or sets the ID of the value.
        /// </summary>
        /// <value>The identifier value.</value>
        public int IdValue { get; set; }

        /// <summary>
        /// Gets or sets the ID of the parameter.
        /// </summary>
        /// <value>The identifier parameter.</value>
        public int IdParameter { get; set; }

        /// <summary>
        /// Gets or sets the code of the value.
        /// </summary>
        /// <value>The code value.</value>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the value.
        /// </summary>
        /// <value>The name value.</value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the value.
        /// </summary>
        /// <value>The description value.</value>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated parameters.
        /// </summary>
        public virtual Parameter? Parameters { get; set; }
    }
}
