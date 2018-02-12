using System;
using System.ComponentModel.DataAnnotations;

namespace BlackJackWeb.Models
{
    internal class GreaterThanAttribute : ValidationAttribute
    {
        private int _playerChips;

        public GreaterThanAttribute(int playerChips)
        {
            _playerChips = playerChips;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int bet = (int)validationContext.ObjectInstance;

            if (bet > _playerChips)
            {
                return new ValidationResult(string.Format("Bet too high"));
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}