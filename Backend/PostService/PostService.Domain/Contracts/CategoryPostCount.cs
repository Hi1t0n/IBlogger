namespace PostService.Domain.Contracts;

/// <summary>
/// DTO для ответа с количеством постов у каждой категории.
/// </summary>
/// <param name="CategoryName">Название категории.</param>
/// <param name="PostsCount">Количество постов.</param>
public record CategoryPostCount(string CategoryName, int PostsCount);