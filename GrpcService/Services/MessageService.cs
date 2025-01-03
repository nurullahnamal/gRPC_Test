﻿using Grpc.Core;
using GrpcMessageService;

namespace GrpcService.Services
{
	public class MessageService : Message.MessageBase
	{
		//unary 
		/*
		public override async Task<MessageResponse> sendMessage(MessageRequest request, ServerCallContext context)
		{
			Console.WriteLine($"Message : {request.Message} : {request.Name}");

			return new MessageResponse
			{
				Message = "Test Message Baslatrılıyor ..."
			};
		}
		*/

		//Server Streaming
		/*
		 * public override async Task  sendMessage(MessageRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
		{
			Console.WriteLine($"Message :  {request.Message} : {request.Name}");

			for (int i = 0; i < 10; i++)
			{
				await Task.Delay(1000);
				await responseStream.WriteAsync(new MessageResponse
				{
					Message="Merhaba " +i
				});
			}
		}*/
		/*
		public override async Task<MessageResponse> sendMessage(IAsyncStreamReader<MessageRequest> requestStream, ServerCallContext context)
		{


			while (await requestStream.MoveNext(context.CancellationToken))
			{
				Console.WriteLine($"Message :  {requestStream.Current.Message} : {requestStream.Current.Name}");
			}

			return new MessageResponse
			{
				Message = "Veri Alınmıştır"
			};
		}*/

		//Bi-directional

		/*
		public override async Task sendMessage(IAsyncStreamReader<MessageRequest> requestStream, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
		{
			var task1 = Task.Run(async () =>

			{
				while (await requestStream.MoveNext(context.CancellationToken))
				{
					Console.WriteLine($"Message :  { requestStream.Current.Message} | : { requestStream.Current.Name }");

				}
			});

			for (int i = 0; i < 10; i++)
			{
				await Task.Delay(1000);
				await responseStream.WriteAsync(new MessageResponse() { Message = "Mesaj" + i });
			}
			await task1;
		}*/


	}
}
