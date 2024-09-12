using System.Diagnostics;

namespace SpCoreMiner.Services
{
    public class RfidService
    {
        private readonly string _pythonPath;
        private readonly string _readScriptPath;
        private readonly string _writeScriptPath;

        public RfidService(string pythonPath, string readScriptPath, string writeScriptPath)
        {
            _pythonPath = pythonPath;
            _readScriptPath = readScriptPath;
            _writeScriptPath = writeScriptPath;
        }

        public string ReadCard()
        {
            return ExecutePythonScript(_readScriptPath);
        }

        public string WriteCard(string text)
        {
            return ExecutePythonScript(_writeScriptPath, text);
        }

        private string ExecutePythonScript(string scriptPath, string arguments = "")
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = _pythonPath,
                    Arguments = $"{scriptPath} {arguments}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    string result = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();

                    if (!string.IsNullOrEmpty(errors))
                    {
                        throw new Exception(errors);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}