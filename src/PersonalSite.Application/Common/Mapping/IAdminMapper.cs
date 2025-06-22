namespace PersonalSite.Application.Common.Mapping;

public interface IAdminMapper<TEntity, TAdminDto>
{
    TAdminDto MapToAdminDto(TEntity entity);
    List<TAdminDto> MapToAdminDtoList(IEnumerable<TEntity> entities);
}