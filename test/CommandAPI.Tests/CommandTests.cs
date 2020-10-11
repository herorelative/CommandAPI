using System;
using CommandAPI.Models;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;
        public CommandTests()
        {
            testCommand = new Command{
                HowTo = "Do something awesome",
                Platform = "Some Platform",
                CommandLine = "Some command line"
            };
        }
        public void Dispose(){
            testCommand = null;
        }
        [Fact]
        public void CanChangeHowTo(){
            //Arrange

            //Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests",testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform(){
            //Arrange

            //Act
            testCommand.Platform = "Extended Unit Test";

            //Assert
            Assert.Equal("Extended Unit Test",testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine(){
            //Arrange
        
            //Act
            testCommand.CommandLine = "dotnet build";

            //Assert
            Assert.Equal("dotnet build",testCommand.CommandLine);
        }
    }
}