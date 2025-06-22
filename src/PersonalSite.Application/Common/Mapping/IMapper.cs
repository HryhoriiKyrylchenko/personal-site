namespace PersonalSite.Application.Common.Mapping;

public interface IMapper<TEntity, TDto>
{
    TDto MapToDto(TEntity entity);
    List<TDto> MapToDtoList(IEnumerable<TEntity> entities);
}