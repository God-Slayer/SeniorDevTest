using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using ShoppingListApi.Interfaces;
using ShoppingListApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingListApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private IMinioClient _minioClient;
        private readonly IShoppingListProvider _shoppingListProvider;

        public ShoppingListController(IShoppingListProvider shoppingListProvider, IMinioClient minioClient)
        {
            _shoppingListProvider = shoppingListProvider;
            _minioClient = minioClient;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            try
            {
                var response = await _shoppingListProvider.GetShoppingList(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] ShoppingList item)
        {
            try
            {
                var response = await _shoppingListProvider.AddItem(item);

                if (response.Equals("Failed")) { return Ok(new { Success = false, }); }

                return Ok(new
                {
                    Data = response,
                    Success = true,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Incomplete code for uploading to Minio
        //[HttpPost("ImageUpload")]
        //public async Task<IActionResult> Post([FromForm] ShoppingList item)
        //{
        //    try
        //    {
        //        // Upload the image to Minio bucket
        //        var putObjectArgs = new PutObjectArgs()
        //        .WithBucket("my-bucket")
        //        .WithObject(item.Image.FileName)
        //        .WithFileName(item.Image.FileName)
        //        .WithContentType(item.Image.ContentType);

        // await _minioClient.PutObjectAsync(putObjectArgs);

        //        // Return a success response
        //        return Ok(new { message = "Image uploaded successfully" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex.Message}");
        //    }
        //}

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] ShoppingList item)
        {
            try
            {
                var response = await _shoppingListProvider.EditItem(item);

                return Ok(new
                {
                    Success = response.Equals("Success")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{index}")]
        public async Task<IActionResult> Delete(int index)
        {
            try
            {
                var response = await _shoppingListProvider.DeleteItem(index);

                return Ok(new
                {
                    Success = response.Equals("Success")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}