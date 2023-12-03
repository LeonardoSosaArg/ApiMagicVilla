using MagicVilla.Models.Dto;

namespace MagicVilla.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
             new VillaDto { Id = 1, Name = "Vista a la piscina", Capacity=500, Province = "Cordoba"},
             new VillaDto { Id = 2, Name = "Vista a la playa", Capacity=200, Province = "Mendoza"},
             new VillaDto { Id = 3, Name = "Vista al atardecer", Capacity=300, Province = "Salta"}
        };
    }
}
