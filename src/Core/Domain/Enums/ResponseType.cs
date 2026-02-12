namespace Domain.Enums
{
    /// <summary>
    /// Defines the different types of responses that can be provided to a question in a reactive form.
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Text response type - allows free text input
        /// </summary>
        Text = 1,

        /// <summary>
        /// Numeric response type - allows only numeric input
        /// </summary>
        Number = 2,

        /// <summary>
        /// Boolean response type - checkbox or toggle (Si/No)
        /// </summary>
        CheckBox = 3,

        /// <summary>
        /// Multiple selection response type - dropdown select
        /// </summary>
        MultipleSelect = 4,

        /// <summary>
        /// SelectText response type - searchable selection (ICD, CUPS, Geographic)
        /// </summary>
        SelectText = 5,

        /// <summary>
        /// TextArea response type - multi-line text input
        /// </summary>
        TextArea = 6,

        /// <summary>
        /// ReadOnly response type - displays calculated or system values (non-editable)
        /// </summary>
        ReadOnly = 7
    }
}
