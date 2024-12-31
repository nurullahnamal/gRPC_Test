using Grpc.Core;
using GrpcMessageService;

namespace GrpcService.Services
{
	public class MessageService:Message.MessageBase
	{
		public override async Task<MessageResponse> sendMessage(MessageRequest request, ServerCallContext context)
		{
			Console.WriteLine($"Message : {request.Message} : {request.Name}");

			return new MessageResponse
			{
				Message = "Test Message Baslatrılıyor ..."
			};
		}

	}
}
