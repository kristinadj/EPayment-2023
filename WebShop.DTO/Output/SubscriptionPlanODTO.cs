﻿namespace WebShop.DTO.Output
{
    public class SubscriptionPlanODTO
    {
        public int SubscriptionPlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CurrencyId { get; set; }
        public int DurationInDays { get; set; }
        public bool AutomaticRenewel { get; set; }

        public SubscriptionPlanODTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}