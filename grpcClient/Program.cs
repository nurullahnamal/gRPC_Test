using Grpc.Core;
using Grpc.Net.Client;
using GrpcMessageClient;
using GrpcService;

var channel = GrpcChannel.ForAddress("http://localhost:5166");
var messageClient = new Message.MessageClient(channel);

//unary
/*
MessageResponse response= await messageClient.sendMessageAsync(new MessageRequest
{
	Message = "Selam",
	Name = "Nurullah"

});
Console.WriteLine(response.ToString());

var greetClient=new Greeter.GreeterClient(channel);

HelloReply result= await greetClient.SayHelloAsync(new HelloRequest { Name = "Nurullah" });

Console.WriteLine(result.ToString());*/


// Server Streaming
/*
var response = messageClient.sendMessage(new MessageRequest
{
	Message = "Merhaba",
	Name = "Nurullah"

});


CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
	Console.WriteLine(response.ResponseStream.Current.Message);
}*/

// Client Streaming
/*
var request = messageClient.sendMessage();

for (int i = 0; i < 10; i++)
{
	await Task.Delay(1000);
	await request.RequestStream.WriteAsync(new MessageRequest
	{
		Message = "Mesaj" + i,
		Name = "Nurullah"
	});
}
// stream datanın sonlandıgı bildirelim
await request.RequestStream.CompleteAsync();
Console.WriteLine((await request.ResponseAsync).Message);
*/

// Bi-directional
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

var request = messageClient.sendMessage();

var task1=  Task.Run(async () =>
{
	for (int i = 0; i < 10; i++)
	{
		await Task.Delay(1000);
		await request.RequestStream.WriteAsync(new MessageRequest { Message = "Merhaba", Name = "Nurullah" + i });
	}
});

while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
	Console.WriteLine(request.ResponseStream.Current.Message);

}
await task1;
await request.RequestStream.CompleteAsync();


