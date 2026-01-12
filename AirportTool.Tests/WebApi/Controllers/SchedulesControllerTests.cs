using AirportManagement.WebApi.Controllers;
using AirportTool.Application.DTOs.FlightSchedule;
using AirportTool.Application.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;

namespace AirportTool.Tests.Controllers
{
    public class SchedulesControllerTests
    {
        private readonly Mock<IFlightScheduleService> _mockService = new();
        private readonly SchedulesController _controller;

        public SchedulesControllerTests()
        {
            _controller = new SchedulesController(_mockService.Object);
        }

        private IFormFile CreateFakeFile(string content, string fileName = "fake.json")
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            return new FormFile(stream, 0, stream.Length, "file", fileName);
        }

        [Fact]
        public async Task GetById_ExistingSchedule_ReturnsOk()
        {
            // Arrange
            var scheduleId = 1;
            var scheduleDto = new FlightScheduleReadDto { Id = scheduleId };

            _mockService.Setup(s => s.GetScheduleByIdAsync(scheduleId)).ReturnsAsync(scheduleDto);

            // Act
            var result = await _controller.GetById(scheduleId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(scheduleDto, actionResult.Value);
        }

        [Fact]
        public async Task GetById_NonExistingSchedule_ReturnsNotFound()
        {
            // Arrange
            var scheduleId = 99;

            _mockService.Setup(s => s.GetScheduleByIdAsync(scheduleId)).ReturnsAsync((FlightScheduleReadDto)null);

            // Act
            var result = await _controller.GetById(scheduleId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var dto = new FlightScheduleCreateDto { FlightId = 1 };
            var createdDto = new FlightScheduleReadDto { Id = 1 };

            _mockService.Setup(s => s.CreateScheduleAsync(dto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(createdDto, actionResult.Value);
            Assert.Equal(nameof(_controller.GetById), actionResult.ActionName);
        }

        [Fact]
        public async Task Import_AllRowsValid_Returns201()
        {
            // Arrange
            var file = CreateFakeFile("valid content");
            var dto = new UploadFileDto { File = file };
            var resultDto = new ImportResultDto
            {
                Total = 2,
                Created = 2,
                Updated = 0,
                Errors = new List<ImportErrorDto>()
            };

            _mockService.Setup(s => s.ImportSchedulesFromFileAsync(file)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.Import(dto);

            // Assert
            var actionResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(resultDto, actionResult.Value);
        }

        [Fact]
        public async Task Import_SomeRowsFail_Returns207()
        {
            // Arrange
            var file = CreateFakeFile("some invalid content");
            var dto = new UploadFileDto { File = file };
            var resultDto = new ImportResultDto
            {
                Total = 3,
                Created = 1,
                Updated = 1,
                Errors = new List<ImportErrorDto>
                {
                    new ImportErrorDto { Row = 3, Message = "Invalid row" }
                }
            };

            _mockService.Setup(s => s.ImportSchedulesFromFileAsync(file)).ReturnsAsync(resultDto);

            // Act
            var result = await _controller.Import(dto);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(207, actionResult.StatusCode);
            Assert.Equal(resultDto, actionResult.Value);
        }

        [Fact]
        public async Task Import_FileNull_ReturnsBadRequest()
        {
            // Arrange
            var dto = new UploadFileDto { File = null };

            // Act
            var result = await _controller.Import(dto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file provided.", actionResult.Value);
        }
    }
}
