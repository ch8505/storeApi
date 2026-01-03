using ChineseAuction.Api.Dtos;

namespace ChineseAuction.Api.Services
{
    public interface IGiftService
    {
        Task<int> AddToDonorAsync(int donorId, GiftCreateUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<GiftAdminDto>> GetAllForAdminAsync();
        Task<IEnumerable<GiftDto>> GetAllForBuyersAsync();
        Task<IEnumerable<GiftDto>> GetAllSortedByCategoryAsync();
        Task<IEnumerable<GiftDto>> GetAllSortedByPriceAsync(bool ascending);
        Task<GiftDetailDto?> GetByIdAsync(int id);
        Task<IEnumerable<GiftDto>> SearchAsync(string? name, string? donor, int? minPurchasers);
        Task<bool> UpdateAsync(int id, GiftCreateUpdateDto dto);
    }
}