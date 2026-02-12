namespace Domain.Entities.Parametros
{
    public class Parameter
    {
        /// <summary>
        /// Gets or sets the IdParameter.
        /// </summary>
        /// <value>The identifier parameter.</value>
        public int IdParameter { get; set; }

        /// <summary>
        /// Gets or sets the TypeParameter.
        /// </summary>
        /// <value>The type of the parameter.</value>
        public string TypeParameter { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the CreationDate.
        /// </summary>
        /// <value>The creation date.</value>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the ModifierDate.
        /// </summary>
        /// <value>The modification date.</value>
        public DateTime ModifierDate { get; set; }

        /// <summary>
        /// Gets or sets the CreationUser.
        /// </summary>
        /// <value>The user who created the parameter.</value>
        public string CreationUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ModifierUser.
        /// </summary>
        /// <value>The user who made the modification.</value>
        public string ModifierUser { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Values.
        /// </summary>
        /// <value>The collection of values associated with the parameter.</value>
        public virtual List<Value> Values { get; set; } = [];
    }
}
 