using Domain.Entities.DNegocio;

namespace Domain.Entities
{
    /// <summary>
    /// Interfaz para entidades que pertenecen a un negocio (tenant).
    /// Permite tipado fuerte en Query Filters y SaveChangesAsync.
    /// </summary>
    public interface ITenantEntity
    {
        int NegocioId { get; set; }
        Negocio? Negocio { get; set; }
    }
}
