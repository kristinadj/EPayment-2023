﻿using System.ComponentModel.DataAnnotations;
using System.Net;

namespace WebShop.DTO.Input
{
    public class UserIDTO
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(25)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(25)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }

        [Required]
        [StringLength(25)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public UserIDTO()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Address = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        public UserIDTO(string firstName, string lastName, string email, string? phoneNumber, string address, string password, string confirmPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
