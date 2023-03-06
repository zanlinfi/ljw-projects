namespace Test.Controller
{
    using EFCore.Db;
    using EntityClass;
    using FebSystem.Controllers;
    using IRepository;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Security.Cryptography.X509Certificates;

    [TestFixture]
    public class Tests
    {
        private Mock<ILogger<BooksEFController>> mockLogger;
        private Mock<IMemoryCache> mockMemoryCache;
        private Mock<EFProvider> mockEFProvider;
        private IReadOnlyList<Book> books;
        private BooksEFController mockController;
        private Mock<IBookEFRepository> mockRepository;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IBookEFRepository>();
            mockLogger = new Mock<ILogger<BooksEFController>>();
            mockEFProvider= new Mock<EFProvider>();
            mockMemoryCache= new Mock<IMemoryCache>();
            mockController = new BooksEFController(mockLogger.Object, mockEFProvider.Object, mockMemoryCache.Object);
            books = new List<Book>()
            {
            new Book() { Id = 1, Title = "tt1", Price = 13.5, AuthorName = "person1"},
            new Book() { Id = 2, Title = "tt2", Price = 13.5, AuthorName = "person2" },
            new Book() { Id = 3, Title = "tt3", Price = 13.5, AuthorName = "person3" }
            };
            
        }

        [Test]
        public async Task GetAll_Ok()
        {
            // arrange
            mockEFProvider.Setup(m=> m.Books).Returns(mockRepository.Object);
            mockEFProvider.Setup(p => p.Books.GetAllAsync()).Returns((Task<IReadOnlyList<Book>>)books);
            mockMemoryCache.Setup(m => m.GetOrCreate(It.IsAny<string>(), (e) => books)).Returns(books);
            // act
            //var book = await mockController.GetAll();
            // assert
            //Assert.IsNotEmpty(book.ToString());
        }
    }
}