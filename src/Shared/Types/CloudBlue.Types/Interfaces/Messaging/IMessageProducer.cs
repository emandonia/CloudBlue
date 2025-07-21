namespace CloudBlue.Domain.Interfaces.Messaging;

public interface IMessageProducer
{
	void SendMessage<T>(T message);
}