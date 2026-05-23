using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;


public class TraitsMapper
{
    public static ParrotTraits FromTraitsDTO(TraitsDTO tDTO)
    {
        return new ParrotTraits()
        {
            Size = tDTO.Size,
            NoiseLevel = tDTO.NoiseLevel,
            Sociability = tDTO.Sociability,
            Trainability = tDTO.Trainability,
            Talkativeness = tDTO.Talkativeness,
            ChewingRisk = tDTO.ChewingRisk,
            CareComplexity = tDTO.CareComplexity,
            KidSafety = tDTO.KidSafety
        };
    }
}