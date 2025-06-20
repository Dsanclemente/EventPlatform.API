using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EventPlatform.API.Controllers;
using EventPlatform.API.Data;
using EventPlatform.API.Models;

namespace EventPlatform.API.Tests.Controllers;

[TestClass]
public class EventsControllerTests
{
    private EventsController _controller = null!;
    private ApplicationDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _controller = new EventsController(_context);

        // Seed test data
        SeedTestData();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    private void SeedTestData()
    {
        var testEvents = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Test Conference",
                DateTime = DateTime.Now.AddDays(7),
                Location = "Test Location",
                Description = "Test Description",
                Status = EventStatus.Upcoming,
                CreatedAt = DateTime.Now
            },
            new Event
            {
                Id = 2,
                Title = "Another Event",
                DateTime = DateTime.Now.AddDays(14),
                Location = "Another Location",
                Description = "Another Description",
                Status = EventStatus.Attending,
                CreatedAt = DateTime.Now
            }
        };

        _context.Events.AddRange(testEvents);
        _context.SaveChanges();
    }

    [TestMethod]
    public async Task GetEvents_ReturnsAllEvents()
    {
        // Act
        var result = await _controller.GetEvents(null, null, null, null);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var events = okResult.Value as IEnumerable<Event>;
        Assert.IsNotNull(events);
        Assert.AreEqual(2, events.Count());
    }

    [TestMethod]
    public async Task GetEvents_WithTitleFilter_ReturnsFilteredEvents()
    {
        // Act
        var result = await _controller.GetEvents("Conference", null, null, null);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var events = okResult.Value as IEnumerable<Event>;
        Assert.IsNotNull(events);
        Assert.AreEqual(1, events.Count());
        Assert.AreEqual("Test Conference", events.First().Title);
    }

    [TestMethod]
    public async Task GetEvent_WithValidId_ReturnsEvent()
    {
        // Act
        var result = await _controller.GetEvent(1);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var eventObj = okResult.Value as Event;
        Assert.IsNotNull(eventObj);
        Assert.AreEqual("Test Conference", eventObj.Title);
    }

    [TestMethod]
    public async Task GetEvent_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.GetEvent(999);

        // Assert
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
    }

    [TestMethod]
    public async Task CreateEvent_WithValidData_ReturnsCreatedEvent()
    {
        // Arrange
        var eventRequest = new CreateEventRequest
        {
            Title = "New Test Event",
            DateTime = DateTime.Now.AddDays(30),
            Location = "New Test Location",
            Description = "New Test Description",
            Status = EventStatus.Upcoming
        };

        // Act
        var result = await _controller.CreateEvent(eventRequest);

        // Assert
        Assert.IsNotNull(result);
        var createdResult = result.Result as ObjectResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual(201, createdResult.StatusCode);
        
        var response = createdResult.Value as ApiResponse<Event>;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
        Assert.AreEqual("New Test Event", response.Data?.Title);
    }

    [TestMethod]
    public async Task CreateEvent_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var invalidEventRequest = new CreateEventRequest
        {
            Title = "", // Invalid: empty title
            DateTime = DateTime.Now.AddDays(30),
            Location = "Test Location",
            Status = EventStatus.Upcoming
        };

        // Act
        var result = await _controller.CreateEvent(invalidEventRequest);

        // Assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
    }

    [TestMethod]
    public async Task UpdateEvent_WithValidData_ReturnsUpdatedEvent()
    {
        // Arrange
        var updateEventRequest = new UpdateEventRequest
        {
            Id = 1,
            Title = "Updated Test Conference",
            DateTime = DateTime.Now.AddDays(7),
            Location = "Updated Test Location",
            Description = "Updated Test Description",
            Status = EventStatus.Attending
        };

        // Act
        var result = await _controller.UpdateEvent(1, updateEventRequest);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var response = okResult.Value as ApiResponse;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
    }

    [TestMethod]
    public async Task UpdateEventStatus_WithValidStatus_ReturnsUpdatedEvent()
    {
        // Arrange
        var statusRequest = new UpdateStatusRequest
        {
            Status = EventStatus.Attending
        };

        // Act
        var result = await _controller.UpdateEventStatus(1, statusRequest);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var response = okResult.Value as StatusUpdateResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(EventStatus.Attending, response.Status);
    }

    [TestMethod]
    public async Task DeleteEvent_WithValidId_ReturnsSuccess()
    {
        // Act
        var result = await _controller.DeleteEvent(1);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var response = okResult.Value as DeleteResponse;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
    }

    [TestMethod]
    public async Task DeleteEvent_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var result = await _controller.DeleteEvent(999);

        // Assert
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
    }

    [TestMethod]
    public void GenerateDescription_WithValidTopic_ReturnsDescription()
    {
        // Arrange
        var request = new GenerateDescriptionRequest
        {
            Topic = "Technology Conference"
        };

        // Act
        var result = _controller.GenerateDescription(request);

        // Assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        
        var response = okResult.Value as ApiResponse<GenerateDescriptionResponse>;
        Assert.IsNotNull(response);
        Assert.IsTrue(response.Success);
        Assert.IsFalse(string.IsNullOrEmpty(response.Data?.Description));
    }
} 