using CompanyReadyApi.DTOs;
using CompanyReadyApi.Models;
using CompanyReadyApi.Repositories;
using CompanyReadyApi.Services;
using Moq;
using Xunit;

namespace CompanyReadyApi.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _service = new ProductService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 50000 },
                new Product { Id = 2, Name = "Mouse", Price = 500 }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Mouse", result[1].Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenMissing_ReturnsNull()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Product?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_TrimsNameAndReturnsCreatedProduct()
        {
            var dto = new CreateProductDto
            {
                Name = " Keyboard ",
                Price = 1500
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product p) =>
                {
                    p.Id = 10;
                    return p;
                });

            var result = await _service.CreateAsync(dto);

            Assert.Equal(10, result.Id);
            Assert.Equal("Keyboard", result.Name);
            Assert.Equal(1500m, result.Price);
        }

        [Fact]
        public async Task UpdateAsync_WhenMissing_ReturnsFalse()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((Product?)null);

            var dto = new UpdateProductDto
            {
                Name = "New Product",
                Price = 100
            };

            var result = await _service.UpdateAsync(99, dto);

            Assert.False(result);
            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsRepositoryResult()
        {
            _repositoryMock
                .Setup(r => r.DeleteAsync(1))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _repositoryMock.Verify(
                r => r.DeleteAsync(1), Times.Once);
        }
    }
}