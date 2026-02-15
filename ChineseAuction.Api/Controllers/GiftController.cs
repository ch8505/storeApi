using ChineseAuction.Api.Dtos;
using ChineseAuction.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChineseAuction.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _service;

        public GiftController(IGiftService service)
        {
            _service = service;
        }

        //מחזיר את כל המתנות הזמינות לרוכשים 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiftDetailDto>>> GetAll()
            => Ok(await _service.GetAllForBuyersAsync());

        //מיון מתנות לפי מחיר כרטיס
        [HttpGet("sort-by-price")]
        public async Task<ActionResult<IEnumerable<GiftDetailDto>>> GetByPrice([FromQuery] bool asc = true)
            => Ok(await _service.GetAllSortedByPriceAsync(asc));

        //מיון מתנות לפי קטגוריה
        [HttpGet("sort-by-category")]
        public async Task<ActionResult<IEnumerable<GiftDetailDto>>> GetByCategory()
            => Ok(await _service.GetAllSortedByCategoryAsync());

        //מחזיר מתנה לפי Id עם כל הפרטים
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GiftDetailDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        //חיפוש מתנות לפי שם, תורם או מינימום רכישות
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> Search([FromQuery] string? name, [FromQuery] string? donor, [FromQuery] int? min)
        {
            var results = await _service.SearchAsync(name, donor, min);
            return Ok(results);
        }
        // --- גישה למנהלים בלבד (חובה טוקן עם Role=Admin) ---

        //צפייה מנהלית מורחבת כולל נתוני מכירות ותורמים
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GiftAdminDto>>> GetAllForAdmin()
            => Ok(await _service.GetAllForAdminAsync());

        //// יצירת מתנה חדשה
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult<int>> Create([FromBody] GiftCreateUpdateDto dto)
        //{
        //    var id = await _service.CreateAsync(dto);
        //    return CreatedAtAction(nameof(GetById), new { id }, id);
        //}


        // הוספת מתנה חדשה לתורם קיים
        [HttpPost("admin/add-to-donor/{donorId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> AddToDonor(int donorId, [FromBody] GiftCreateUpdateDto dto)
        {
            var id = await _service.AddToDonorAsync(donorId, dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // עדכון מתנה קיימת
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GiftCreateUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            return success ? NoContent() : NotFound();
        }

        // מחיקת מתנה לפי Id
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }

}