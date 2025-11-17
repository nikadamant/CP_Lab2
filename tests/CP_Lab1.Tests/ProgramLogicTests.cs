using CP_Lab1;
using Xunit;
using System;
using System.Linq;

namespace CP_Lab1.Tests
{
    public class ProgramLogicTests
    {
        public ProgramLogicTests()
        {
            Program.ClearTasksForTesting();
        }

        // ----------------------------------------------------------------------
        // “≈—“» ƒŒƒ¿¬¿ÕÕﬂ (AddTask)
        // ----------------------------------------------------------------------

        [Fact]
        public void AddTask_Successfully_Adds_Task_And_Increments_Id()
        {
            // Arrange
            DateTime dueDate = DateTime.Today.AddDays(10);

            // Act
            var task1 = Program.AddTask("À1", " œ", dueDate);
            var task2 = Program.AddTask(" –", "¡ƒ", dueDate);

            // Assert
            var tasks = Program.GetTasksForTesting();
            Assert.Equal(2, tasks.Count);
            Assert.Equal(1, task1.Id);
            Assert.Equal(2, task2.Id);
            Assert.Equal("À1", tasks[0].Title);
            Assert.Equal(TaskStatus.Planned, tasks[0].Status);
        }

        [Theory]
        [InlineData("", " ÛÒ", "Title or Course cannot be empty.")]
        [InlineData("Õ‡Á‚‡", "", "Title or Course cannot be empty.")]
        public void AddTask_Throws_Exception_On_Empty_Input(string title, string course, string expectedMessage)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                Program.AddTask(title, course, DateTime.Today.AddDays(1))
            );
            Assert.Contains(expectedMessage, ex.Message);
            Assert.Empty(Program.GetTasksForTesting());
        }

        // ----------------------------------------------------------------------
        // “≈—“» «Ã≤Õ» —“¿“”—” (ToggleStatus)
        // ----------------------------------------------------------------------

        [Fact]
        public void ToggleStatus_Cycles_Planned_To_InProgress_Correctly()
        {
            // Arrange
            Program.AddTask("Task A", "C1", DateTime.Today); // Id=1
            var task = Program.GetTasksForTesting()[0];
            task.Status = TaskStatus.Planned;

            // Act
            bool success = Program.ToggleStatus(1);

            // Assert
            Assert.True(success);
            Assert.Equal(TaskStatus.InProgress, task.Status);
        }

        [Fact]
        public void ToggleStatus_Cycles_Completed_To_Planned_Correctly()
        {
            // Arrange
            Program.AddTask("Task B", "C2", DateTime.Today); // Id=1
            var task = Program.GetTasksForTesting()[0];
            task.Status = TaskStatus.Completed;

            // Act
            Program.ToggleStatus(1);

            // Assert
            Assert.Equal(TaskStatus.Planned, task.Status);
        }

        [Fact]
        public void ToggleStatus_Returns_False_For_NonExistent_Id()
        {
            // Act
            bool success = Program.ToggleStatus(999);

            // Assert
            Assert.False(success);
        }

        // ----------------------------------------------------------------------
        // “≈—“» ¬»ƒ¿À≈ÕÕﬂ (DeleteTask)
        // ----------------------------------------------------------------------

        [Fact]
        public void DeleteTask_Removes_Existing_Task_Returns_True()
        {
            // Arrange
            Program.AddTask("Task to Delete", "C1", DateTime.Today); // Id=1
            Program.AddTask("Task to Keep", "C2", DateTime.Today);   // Id=2

            // Act
            bool success = Program.DeleteTask(1);

            // Assert
            var tasks = Program.GetTasksForTesting();
            Assert.True(success);
            Assert.Single(tasks);
            Assert.DoesNotContain(tasks, t => t.Id == 1);
            Assert.Contains(tasks, t => t.Id == 2);
        }

        [Fact]
        public void DeleteTask_Returns_False_For_NonExistent_Id()
        {
            // Arrange
            Program.AddTask("Only Task", "C1", DateTime.Today); // Id=1

            // Act
            bool success = Program.DeleteTask(999);

            // Assert
            Assert.False(success);
            Assert.Single(Program.GetTasksForTesting());
        }
    }
}