﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PostService.Domain.Models;

namespace PostService.Infrastructure.Context;

/// <summary>
/// Контекст БД приложения.
/// </summary>
/// <param name="options">Настройки.</param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PostCategory> PostCategories => Set<PostCategory>();
     
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}