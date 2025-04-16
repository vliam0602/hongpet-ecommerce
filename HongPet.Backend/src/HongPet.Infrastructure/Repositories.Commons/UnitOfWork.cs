﻿using Domain.Entities.Commons;
using HongPet.Domain.Repositories.Abstraction.Commons;
using HongPet.Domain.Repositories.Abstractions.Commons;
using HongPet.Infrastructure.Database;

namespace HongPet.Infrastructure.Repositories.Commons;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the repository for the specified entity type.
    /// If the repository does not already exist, it is created and added to the dictionary.
    /// Inject another repositories if need specificed logic code in repo.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of IGenericRepository for the specified entity type.</returns>
    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var repository))
        {
            return (IGenericRepository<TEntity>)repository;
        }

        var newRepository = new GenericRepository<TEntity>(_context);
        _repositories[typeof(TEntity)] = newRepository;
        return newRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
