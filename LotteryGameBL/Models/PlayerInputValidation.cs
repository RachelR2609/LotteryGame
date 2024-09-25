namespace LotteryGameBL.Models
{
    public class PlayerInputValidation
    {
        /// <summary>
        /// Gets or sets whether the validation checks passed.
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Gets or sets the validation error/success message.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Gets or sets the converted input value entered by the user.
        /// </summary>
        public int Value { get; set; }

        public PlayerInputValidation(bool isValid, string message, int value = 0)
        {
            IsValid = isValid;
            Message = message;
            Value = value;
        }
    }
}
