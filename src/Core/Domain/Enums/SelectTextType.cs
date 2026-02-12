namespace Domain.Enums
{
    /// <summary>
    /// Defines the different types of searchable text selections used in SelectText response types.
    /// These correspond to medical classification systems and geographic data.
    /// </summary>
    public enum SelectTextType
    {
        /// <summary>
        /// ICD (International Classification of Diseases) - for diagnoses
        /// </summary>
        ICD = 1,

        /// <summary>
        /// CUPS (Clasificación Única de Procedimientos en Salud) - for medical procedures
        /// </summary>
        CUPS = 2,

        /// <summary>
        /// Geographic - for regions, cities, or locations
        /// </summary>
        Geographic = 3
    }
}
