using Artemis.Data;
using Artemis.Data.Repositories;
using Artemis.Model.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.API.Tests.Repository
{
    public class CatRepositoryTests
    {
        private ApplicationDBContext _context;
        private CatRepository _catRepository;

        List<CatEntity> _catList;

        public CatRepositoryTests()
        {
            if (_catList == null)
                SetData();
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDBContext(options);
            _catRepository = new CatRepository(_context);

            if (_catList == null)
                SetData();
        }

        [TearDown]
        public void Dispose()
        {
            if (_catList != null)
            {
                _context.Dispose();
                _context = null;
                _catList = null;
            }
        }

        private void SetData()
        {
            _catList = new List<CatEntity>
            {
                new CatEntity {
                    Id = 1,
                    CatId = "cat1",
                    Height = 600,
                    Width= 600,
                    Tags = new List<TagEntity> {
                        new TagEntity {
                            Id = 1,
                            Name = "Intelligent"
                        } ,
                        new TagEntity {
                            Id = 2,
                            Name = "Magnifique"
                        }
                    }
                },
                 new CatEntity {
                    Id = 2,
                    CatId = "cat2",
                    Height = 600,
                    Width= 600,
                    Tags = new List<TagEntity> {
                        new TagEntity {
                            Id = 3,
                            Name = "Cat Fatal"
                        }
                    }
                },
                 new CatEntity {
                    Id = 3,
                    CatId = "cat3",
                    Height = 600,
                    Width= 600,
                    Tags = new List<TagEntity> {
                        new TagEntity {
                            Id = 4,
                            Name = "Superb"
                        }
                    }
                },
            };
        }

        [Test]
        public async Task GetCatsWithPagingAsync_ShouldReturnCats()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;

            // Act
            await _catRepository.AddCatsRangeAsync(_catList.Take(1));
            await _context.SaveChangesAsync();
            var cats = _catRepository.GetCatsWithPaging(page, pageSize, string.Empty);

            // Assert
            Assert.NotNull(cats);
            Assert.That(1 == cats.Count());
        }

        [Test]
        public async Task GetCatsWithPagingAsync_ByTagName_ShouldReturnCats()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var tagName = "Magnifique";


            // Act
            // by adding 1 item we expect to get 1 back
            await _catRepository.AddCatsRangeAsync(_catList.Take(1));
            await _context.SaveChangesAsync();
            var cats = _catRepository.GetCatsWithPaging(page, pageSize, tagName);

            // Assert
            Assert.NotNull(cats);
            Assert.That(1 == cats.Count());
        }

        [TestCase(0, 0, "")]
        [TestCase(0, 0, "Get")]
        [TestCase(1, 0, "")]
        [TestCase(0, 200, "")]
        [TestCase(1, 1, "NotExistingTag")]
        public async Task GetCatsWithPagingAsync_ShouldReturnEmpty(int page, int pageSize, string tagName)
        {
            // Arrange

            //Act
            await _catRepository.AddCatsRangeAsync(_catList);
            await _context.SaveChangesAsync();
            var cats = _catRepository.GetCatsWithPaging(page, pageSize, tagName);

            // Assert
            Assert.IsEmpty(cats);
        }
    }
}