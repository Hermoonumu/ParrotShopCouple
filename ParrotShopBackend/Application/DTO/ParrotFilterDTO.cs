using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.DTO;
public class ParrotFilterDTO
{
    public Species? Species {set;get;}
    public Gender? Gender {set;get;}
    public List<Color>? Color {set;get;}
    public int? PriceFrom {set;get;}
    public int? PriceTo {set;get;}
    public bool? AscendingPrice {set;get;}
    public Size? Size {set;get;}
    public Level? NoiseLevel {set;get;}
    public Level? Sociability {set;get;}
    public Level? Talkativeness {set;get;}
    public Level? Trainability {set;get;}
    public Level? ChewingRisk {set;get;}
    public Level? CareComplexity {set;get;}
    public KidSafety? KidSafety {set;get;}
}