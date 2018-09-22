using System;
using System.Collections.Generic;
using System.Text;

using Core.Repository;


namespace Core.Handler
{
    public class EquipmentsHandler: IEquipmentsHandler
    {
        IEquipmentRepository _equipmentRepo;

        public EquipmentsHandler(IEquipmentRepository equipmentRepo)
        {
            _equipmentRepo = equipmentRepo;
        }

        public List<Equipment> getListOfEquipments()
        {
            return _equipmentRepo.GetEquipment();
        }
    }
}
