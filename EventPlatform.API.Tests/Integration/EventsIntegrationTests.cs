using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EventPlatform.API.Data;
using EventPlatform.API.Models;
using EventPlatform.API.Controllers;

namespace EventPlatform.API.Tests.Integration;

[TestClass]
public class EventsIntegrationTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Replace the database context with in-memory database
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    // Create a new service provider
                    var serviceProvider = services.BuildServiceProvider();

                    // Create a scope to obtain a reference to the database context
                    using var scope = serviceProvider.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                    // Ensure the database is created
                    db.Database.EnsureCreated();

                    // Seed test data
                    SeedTestData(db);
                });
            });

        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        var testEvents = new List<Event>
        {
            new Event
            {
                Id = 1,
                Title = "Integration Test Conference",
                DateTime = DateTime.Now.AddDays(7),
                Location = "Integration Test Location",
                Description = "Integration Test Description",
                Status = EventStatus.Upcoming,
                CreatedAt = DateTime.Now
            },
            new Event
            {
                Id = 2,
                Title = "Another Integration Event",
                DateTime = DateTime.Now.AddDays(14),
                Location = "Another Integration Location",
                Description = "Another Integration Description",
                Status = EventStatus.Attending,
                CreatedAt = DateTime.Now
            }
        };

        context.Events.AddRange(testEvents);
        context.SaveChanges();
    }

    [TestMethod]
    public async Task GetEvents_ReturnsAllEvents()
    {
        // Act
        var response = await _client.GetAsync("/api/events");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var events = JsonSerializer.Deserialize<IEnumerable<Event>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(events);
        Assert.AreEqual(2, events.Count());
    }

    [TestMethod]
    public async Task GetEvents_WithTitleFilter_ReturnsFilteredEvents()
    {
        // Act
        var response = await _client.GetAsync("/api/events?title=Conference");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var events = JsonSerializer.Deserialize<IEnumerable<Event>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(events);
        Assert.AreEqual(1, events.Count());
        Assert.AreEqual("Integration Test Conference", events.First().Title);
    }

    [TestMethod]
    public async Task GetEvent_WithValidId_ReturnsEvent()
    {
        // Act
        var response = await _client.GetAsync("/api/events/1");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var content = await response.Content.ReadAsStringAsync();
        var eventObj = JsonSerializer.Deserialize<Event>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(eventObj);
        Assert.AreEqual("Integration Test Conference", eventObj.Title);
    }

    [TestMethod]
    public async Task GetEvent_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/events/999");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task CreateEvent_WithValidData_ReturnsCreatedEvent()
    {
        // Arrange
        var eventRequest = new CreateEventRequest
        {
            Title = "New Integration Test Event",
            DateTime = DateTime.Now.AddDays(30),
            Location = "New Integration Test Location",
            Description = "New Integration Test Description",
            Status = EventStatus.Upcoming
        };

        var json = JsonSerializer.Serialize(eventRequest);
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PostAsync("/api/events", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<Event>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.AreEqual("New Integration Test Event", result.Data?.Title);
    }

    [TestMethod]
    public async Task UpdateEvent_WithValidData_ReturnsUpdatedEvent()
    {
        // Arrange
        var updateEventRequest = new UpdateEventRequest
        {
            Id = 1,
            Title = "Updated Integration Test Conference",
            DateTime = DateTime.Now.AddDays(7),
            Location = "Updated Integration Test Location",
            Description = "Updated Integration Test Description",
            Status = EventStatus.Attending
        };

        var json = JsonSerializer.Serialize(updateEventRequest);
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PutAsync("/api/events/1", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }

    [TestMethod]
    public async Task UpdateEventStatus_WithValidStatus_ReturnsUpdatedEvent()
    {
        // Arrange
        var statusRequest = new UpdateStatusRequest
        {
            Status = EventStatus.Attending
        };

        var json = JsonSerializer.Serialize(statusRequest);
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PatchAsync("/api/events/1/status", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StatusUpdateResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.AreEqual(EventStatus.Attending, result.Status);
    }

    [TestMethod]
    public async Task DeleteEvent_WithValidId_ReturnsSuccess()
    {
        // Act
        var response = await _client.DeleteAsync("/api/events/1");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DeleteResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
    }

    [TestMethod]
    public async Task DeleteEvent_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/events/999");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GenerateDescription_WithValidTopic_ReturnsDescription()
    {
        // Arrange
        var request = new GenerateDescriptionRequest
        {
            Topic = "Technology Conference"
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        // Act
        var response = await _client.PostAsync("/api/events/generate-description", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<GenerateDescriptionResponse>>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Success);
        Assert.IsFalse(string.IsNullOrEmpty(result.Data));
    }
} 