using Moq;

using TheRaceTrace.Services;

namespace TheRaceTrace.Tests
{
    public class ViewModelTests
    {
        [Fact]
        public void Command_Is_Rate_Limited()
        {
            Mock<IErgastService> mockErgast = new();
            Mock<IChartService> mockChart = new();
            ViewModel viewModel = new(mockErgast.Object, mockChart.Object);

            Assert.True(viewModel.GetRaceTraceCommand.CanExecute(null));

            // TODO: async void is bad for unit tests, should use async Task
            // Bonus: don't wait for the full 20 seconds
            viewModel.GetRaceTraceCommand.Execute(null);
            Assert.False(viewModel.GetRaceTraceCommand.CanExecute(null));
        }
    }
}