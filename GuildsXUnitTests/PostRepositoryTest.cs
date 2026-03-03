using GuidlsMVC.Entities;
using GuidlsMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildsXUnitTests
{
    public class PostRepositoryTest : TestBase
    {
        private readonly PostRepository _repo;
        public PostRepositoryTest()
        {
            _repo = new PostRepository(Configuration);
        }

        [Fact]
        public async Task Create_ShouldReturn_NewId()
        {
            // Arrange
            var post = new Post
            {
                Content = "Test Product",
                Likes = 99
            };

            // Act
            var id = await _repo.CreateAsync(post);

            // Assert
            Assert.True(id > 0);
        }
    }
}
