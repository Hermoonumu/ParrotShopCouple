using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParrotShopBackend.Domain;


public class Parrot : Item
{
    [Range(0, 100)]
    public byte Age { get; set; }
    public Color ColorType { get; set; }
    public Gender GenderType { get; set; }
    public Species SpeciesType { get; set; }
    public ParrotTraits? Traits{set;get;}
    public long? TraitsId{set;get;}
}

[Flags]
public enum Color
{
    Black = 1,
    White = 1 << 1,
    Red = 1 << 2,
    Green = 1 << 3,
    Blue = 1 << 4,
    Grey = 1 << 5,
    Colourful = 1 << 6,
    Pastel = 1 << 7,
    Muted = 1 << 8,
    Yellow = 1 << 9,
    Orange = 1 << 10,
    Brown = 1 << 11,
    Pink = 1 << 12,
    Purple = 1 << 13
}
public enum Gender
{
    Male,
    Female
}
public enum Species
{
    // Small Parrots
    Budgerigar = 1,
    Cockatiel = 2,
    Lovebird = 3,
    Parrotlet = 4,

    // Medium Parrots
    Conure = 10,
    Caique = 11,
    Ringneck = 12,
    Lorikeet = 13,
    Quaker = 14,

    // Large Parrots
    AfricanGrey = 20,
    AmazonParrot = 21,
    Eclectus = 22,
    Cockatoo = 23,
    Macaw = 24,
    Unknown = 100
}
