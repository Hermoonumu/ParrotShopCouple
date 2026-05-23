using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;



public class ParrotTraits
{
    [Key]
    public long Id { get; set; }
    public long ParrotId{set;get;}
    public Size? Size {get; set;}
    public Level? NoiseLevel {set; get;}
    public Level? Sociability {set;get;}
    public Level? Trainability {set;get;}
    public Level? Talkativeness{set;get;}
    public Level? ChewingRisk {set;get;}
    public Level? CareComplexity {set;get;}
    public KidSafety? KidSafety {set;get;}

}

public enum Size
{
    Small = 1,
    Medium = 2,
    Large = 3
}

public enum KidSafety
{
    Yes=0,
    No=2,
    Cautious=1
}

public enum Level
{
    Low, 
    Mid,
    High
}

public enum Traits
{
    Size,
    NoiseLevel,
    Sociability,
    Trainability,
    Talkativeness,
    ChewingRisk,
    CareComplexity,
    LifespanMin,
    LifespanMax,
    KidSafety
}
