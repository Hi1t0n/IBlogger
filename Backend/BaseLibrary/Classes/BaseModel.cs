namespace BaseLibrary.Classes;

/// <summary>
/// Базовая модель.
/// </summary>
public abstract class BaseModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// Дата обновления.
    /// </summary>
    public DateTime ModifiedOn { get; set; }
}