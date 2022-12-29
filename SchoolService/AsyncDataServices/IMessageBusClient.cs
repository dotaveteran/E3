using SchoolService.Dtos;

namespace SchoolService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewShkola(ShkolaPublishedDto shkolaPublishedDto);
    }
}