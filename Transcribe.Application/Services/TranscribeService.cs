using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using Eqn.Core.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Transcribe.Application.Services;

public class TranscribeService : ITranscribeService, ITransientDependency
{
    private readonly IAmazonS3 _s3Client;
    private readonly IAmazonTranscribeService _transcribeService;
    private readonly HttpClient _httpClient;

    public TranscribeService(IAmazonS3 s3Client,
        IAmazonTranscribeService transcribeService, HttpClient httpClient)
    {
        _s3Client = s3Client;
        _transcribeService = transcribeService;
        _httpClient = httpClient;
    }

    public async Task<string> StartTranscriptionAsync(IFormFile file, string token)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(file));
            ArgumentException.ThrowIfNullOrEmpty(nameof(token));

            var fileName = RenameFile(file);

            var request = new PutObjectRequest
            {
                BucketName = "acibadem-bucket",
                Key = fileName,
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            var putResponse = await _s3Client.PutObjectAsync(request);
            if (putResponse.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception("File upload failed");

            var transcribeRequest = new StartTranscriptionJobRequest
            {
                LanguageCode = "tr-TR",
                MediaFormat = MediaFormat.Mp3,
                Media = new Media
                {
                    MediaFileUri = "https://s3.amazonaws.com/acibadem-bucket/" + fileName
                },
                IdentifyLanguage = false,
                TranscriptionJobName = Guid.NewGuid().ToString(),
            };
            var response = "";
            var tReq = await _transcribeService.StartTranscriptionJobAsync(transcribeRequest);
            var transcribeJob = tReq.TranscriptionJob;

            if (transcribeJob.TranscriptionJobStatus == TranscriptionJobStatus.FAILED)
                throw new Exception("Transcription failed");

            while (!TranscriptionJobStatus.FAILED.Equals(transcribeJob.TranscriptionJobStatus) &&
                   !TranscriptionJobStatus.COMPLETED.Equals(transcribeJob.TranscriptionJobStatus))
            {
                var gt = await GetTranscribeJobAsync(transcribeJob.TranscriptionJobName);
                transcribeJob = gt.TranscriptionJob;
                await Task.Delay(1000);
                Console.WriteLine($"Transcription job status: {transcribeJob.TranscriptionJobStatus}");
                Console.WriteLine("Waiting for job to complete...");
            }

            if (!transcribeJob.TranscriptionJobStatus.Equals(TranscriptionJobStatus.COMPLETED))
                throw new Exception("Transcription failed because of an error" + transcribeJob.FailureReason);

            var stringRequest = await _httpClient.GetAsync(transcribeJob.Transcript.TranscriptFileUri);
            var stringResponse = await stringRequest.Content.ReadAsStringAsync();
            response = stringResponse;

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception("Transcription failed because of an error" + e.Message);
        }
    }

    private async Task<GetTranscriptionJobResponse> GetTranscribeJobAsync(string jobName)
    {
        var request = new GetTranscriptionJobRequest
        {
            TranscriptionJobName = jobName
        };
        var response = await _transcribeService.GetTranscriptionJobAsync(request);
        return response;
    }

    private string RenameFile(IFormFile file, string newName = "")
    {
        if (string.IsNullOrEmpty(newName))
            newName = file.FileName;
        var fileExtension = Path.GetExtension(file.FileName);
        newName = $"{DateTime.Now:yyyyMMddHHmmssfff}{fileExtension}";
        return newName;
    }
}