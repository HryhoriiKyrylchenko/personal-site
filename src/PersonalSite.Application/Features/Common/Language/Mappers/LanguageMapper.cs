namespace PersonalSite.Application.Features.Common.Language.Mappers;

public static class LanguageMapper
{
    public static LanguageDto MapToDto(Domain.Entities.Common.Language entity)
    {
        return new LanguageDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name
        };
    }

    public static List<LanguageDto> MapToDtoList(IEnumerable<Domain.Entities.Common.Language> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}