namespace ZeroWaste.Data.Static
{
    public static class FormFileUtilities
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}