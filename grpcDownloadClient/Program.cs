using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using grpcFileTransportClient;

var channel = GrpcChannel.ForAddress("http://localhost:5166");
var client = new FileService.FileServiceClient(channel);

string downloadPath = @"G:\Varol maksatoglu\Desktop";
if (!Directory.Exists(downloadPath))
{
	Directory.CreateDirectory(downloadPath);
}

var fileInfo = new grpcFileTransportClient.FileInfo
{
	FileExtension = ".mp4", // İndirilecek dosyanın uzantısı
	FileName = "video" // İndirilecek dosyanın adı
};

FileStream fileStream = null;
var request = client.FileDownload(fileInfo);


CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

decimal chunkSize = 0;
int count = 0;

while (await request.ResponseStream.MoveNext(cancellationTokenSource.Token))
{
	if (count++ == 0)
	{
		string filePath = Path.Combine(downloadPath, $"{request.ResponseStream.Current.Info.FileName}{request.ResponseStream.Current.Info.FileExtension}");
		fileStream = new FileStream(filePath, FileMode.Create);
		fileStream.SetLength(request.ResponseStream.Current.FileSize);
	}
	var buffer = request.ResponseStream.Current.Buffer.ToByteArray();
	await fileStream.WriteAsync(buffer, 0, request.ResponseStream.Current.ReadedByte);
	Console.WriteLine($"Download Progress: {Math.Round((chunkSize += request.ResponseStream.Current.ReadedByte) * 100 / request.ResponseStream.Current.FileSize)}%");
}




Console.WriteLine("Download completed!");
await fileStream.DisposeAsync();
fileStream.Close();
Console.ReadLine();
