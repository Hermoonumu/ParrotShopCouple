using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.DTO;



public class TraitsDTO
{
    public Size? Size {get; set;}
    public Level? NoiseLevel {set; get;}
    public Level? Sociability {set;get;}
    public Level? Trainability {set;get;}
    public Level? Talkativeness{set;get;}
    public Level? ChewingRisk {set;get;}
    public Level? CareComplexity {set;get;}
    public KidSafety? KidSafety {set;get;}
}