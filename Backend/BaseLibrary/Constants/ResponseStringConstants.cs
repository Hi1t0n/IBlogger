namespace BaseLibrary.Constants;

/// <summary>
/// Шаблоны строк ответа.
/// </summary>
public static class ResponseStringConstants
{
    /// <summary>
    /// Сущность не найдена.
    /// {0} - nameof(EntityType), {1} - Атрибут nameof(AttributeName), {2} - значение атрибута.
    /// </summary>
    public static readonly string NotFoundResponseStringTemplate = "{0} c {1}: {2} не найден";
    
    /// <summary>
    /// При добавлении сущности произошла ошибка.
    /// {0} - nameof(EntityType).
    /// </summary>
    public static readonly string AddingErrorResponseStringTemplate = "При добавлении {0} что-то пошло не так";
}