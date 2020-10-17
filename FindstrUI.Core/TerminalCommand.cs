using System.Diagnostics;

namespace FindstrUI.Core
{
    public class TerminalCommand
    {
        /// <summary>
        /// Executes a shell command and get the result.
        /// </summary>
        /// <param name="command">string command</param>
        /// <returns>string, as output of the command.</returns>
        public static string GetResult(string command)
        {
            var procStartInfo = GetProcessStartInfo(command);

            // Now we create a process, assign its ProcessStartInfo and start it
            Process proc = new Process() { StartInfo = procStartInfo };
            proc.Start();

            // Return the output into a string
            return proc.StandardOutput.ReadToEnd();
        }

        // todo: Read result from stream
        // Get every result seperately.

        private static ProcessStartInfo GetProcessStartInfo(string command)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                // The following commands are needed to redirect the standard output. 
                //This means that it will be redirected to the Process.StandardOutput StreamReader.
                RedirectStandardOutput = true,
                UseShellExecute = false,
                // Do not create the black window.
                CreateNoWindow = true
            };

            return procStartInfo;
        }
    }
}
