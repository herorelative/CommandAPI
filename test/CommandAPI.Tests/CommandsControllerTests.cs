using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests:IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;
        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg=>cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }
        public void Dispose(){
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if(num>0){
                commands.Add(new Command{
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
        // Test 1.1 – Check 200 OK HTTP Response(Empty DB)
        [Fact]
        public void GetCommandItmes_ReturnsZeroItems_WhenDbIsEmpty()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetAllCommands()).Returns(GetCommands(0));
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        // Test 1.2 – Check Single Resource Returned
        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetAllCommands()).Returns(GetCommands(1));
            var controller =  new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            var okResult = result.Result as OkObjectResult;

            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }
        // Test 1.3 – Check 200 OK HTTP Response
        [Fact]
        public void GetAllCommands_Returns200Ok_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 1.4 – Check the Correct Object Type Returned
        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        // Test 2.1 – Check 404 Not Found HTTP Response
        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // Test 2.2 – Check 200 OK HTTP Response
        [Fact]
        public void GetCommandByID_Returns200OK_WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command
            {
                Id=1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // Test 2.3 – Check the Correct Object Type Returned
        [Fact]
        public void GetCommandByID_ReturnsCorrectType_WhenValidIDProvided()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command{
                Id=1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //Test 3.1 Check If the Correct Object Type Is Returned
        [Fact]
        public void CreatedCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command{
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            //Assert
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        // Test 3.2 Check 201 HTTP Response
        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command{
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.CreateCommand(new CommandCreateDto{});

            //Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        // Test 4.1 Check 204 HTTP Response
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=> repo.GetCommandById(1)).Returns(new Command{
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);
            
            //Act
            var result = controller.UpdateCommand(1, new CommandUpdateDto{});

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        // Test 4.2 Check 404 HTTP Response
        [Fact]
        public void UpdateCommand_Retruns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.UpdateCommand(0,new CommandUpdateDto{});
            
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test 5.1 Check 404 HTTP Response
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(0)).Returns(()=>null);
            var controller = new CommandsController(mockRepo.Object,mapper);
            //Act
            var result = controller.PartialCommandUpdate(0,new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto>{});
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test 6.1 Check for 204 No Content HTTP Response
        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(1)).Returns(new Command{
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object,mapper);

            //Act
            var result = controller.DeleteCommand(1);
            
            //Assert
            Assert.IsType<NoContentResult>(result);
        }
        
        // Test 6.2 Check for 404 Not Found HTTP Response
        [Fact]
        public void DeleteCommand_Returns_404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //Arrange
            mockRepo.Setup(repo=>repo.GetCommandById(0)).Returns(()=>null);
            var controller = new CommandsController(mockRepo.Object,mapper);
            
            //Act
            var result = controller.DeleteCommand(0);
            
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}