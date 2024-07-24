using BlogManagement.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace BlogManagement.Controllers
{
    /// <summary>
    /// Controller for managing blog entries.
    /// Created by Priyanshu Maurya.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly JsonFileService _jsonFileService;
        private readonly string UserName = "Priyanshu Maurya";
        public BlogsController(JsonFileService jsonFileService)
        {
            _jsonFileService = jsonFileService;
        }

        /// <summary>
        /// Gets the list of all blog entries.
        /// </summary>
        /// <returns>List of BlogModel</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<BlogModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<BlogModel>>> Get()
        {
            try
            {
                var blogs = await _jsonFileService.ReadAsync();
                return Ok(blogs);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while retrieving blogs.");
            }
        }

        /// <summary>
        /// Gets a specific blog entry by ID.
        /// </summary>
        /// <param name="id">Blog ID</param>
        /// <returns>BlogModel</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BlogModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<BlogModel>> Get(int id)
        {
            try
            {
                var blog = await _jsonFileService.ReadAsync(id);
                if (blog == null)
                {
                    return NotFound("Blog not found.");
                }
                return Ok(blog);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the blog.");
            }
        }

        /// <summary>
        /// Creates a new blog entry.
        /// </summary>
        /// <param name="blog">BlogModel</param>
        /// <returns>Created BlogModel</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BlogModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<BlogModel>> Post(BlogModel blog)
        {
            if (blog == null)
            {
                return BadRequest("Blog data is required.");
            }

            try
            {
                blog.DateCreated = DateTime.Now;
                blog.UserName = UserName;
                var createdBlog = await _jsonFileService.CreateAsync(blog);
                return CreatedAtAction(nameof(Get), new { id = createdBlog.BlogId }, createdBlog);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while creating the blog.");
            }
        }

        /// <summary>
        /// Updates an existing blog entry.
        /// </summary>
        /// <param name="id">Blog ID</param>
        /// <param name="blog">BlogModel</param>
        /// <returns>Updated BlogModel</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BlogModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<BlogModel>> Put(int id, BlogModel blog)
        {
            if (id != blog.BlogId)
            {
                return BadRequest("Blog ID mismatch.");
            }

            try
            {
                var updatedBlog = await _jsonFileService.UpdateAsync(blog);
                if (updatedBlog == null)
                {
                    return NotFound("Blog not found.");
                }
                return Ok(updatedBlog);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while updating the blog.");
            }
        }

        /// <summary>
        /// Deletes a blog entry by ID.
        /// </summary>
        /// <param name="id">Blog ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _jsonFileService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound("Blog not found.");
                }
                return NoContent();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error occurred while deleting the blog.");
            }
        }
    }
}

