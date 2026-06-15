using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Interfaces.Repositories;
using SMB.APPLICATION.Interfaces.Services;
using SMB.DOMAIN.Entities;

namespace SMB.APPLICATION.Services;

public class CatalogService(ICatalogRepository catalogRepository) : ICatalogService
{
    public async Task<List<LanguageResponse>> GetAllLanguages()
    {
        var language = await catalogRepository.GetLanguageByActiveStatus();

        return language.Select(l => new LanguageResponse()
        {
            Id = l.Id,
            Code = l.Code,
            Name = l.Name
        }).ToList();
    }
}