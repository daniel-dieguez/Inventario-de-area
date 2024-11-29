using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.models;


[Table("Equipos")]
public class Productos
{
    [Key]
    [Column("codigoInventario")]
    public int codigoInventario { get; set; }
    [Column("Marca")]
    public string Marca { get; set; }
    [Column("TipoEquipo")]
    public string TipoEquipo { get; set; }
    [Column("Departamento")]
    public string Departamento { get; set; }
    [Column("FechaAsignacion")]
    public DateOnly FechaAsignacion { get; set; }
    [Column("PersonaResponsable")]
    public string PersonaResponsable { get; set; }
    [Column("FechaIngreso")]
    public DateOnly FechaIngreso { get; set; }
    
}