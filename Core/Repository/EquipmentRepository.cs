using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Core.Repository
{
    public class EquipmentRepository:IEquipmentRepository
    {
        private readonly IConfigurationRoot _configuration;

        public EquipmentRepository(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public List<Equipment> GetEquipment()
        {
            List<Equipment> listEquipments = new List<Equipment>()
            {
                new Equipment()
                {
                    IdEquipment = 1,
                    Outlet = "11",
                    SerialNumber = "111",
                    Model = _configuration["Model"],
                    MacAddress = "369785"
                },
                new Equipment()
                {
                    IdEquipment = 2,
                    Outlet = "Data",
                    SerialNumber = "222",
                    Model = "Dt",
                    MacAddress = "9654785"
                }
            };

            return listEquipments;
        }
    }
}
