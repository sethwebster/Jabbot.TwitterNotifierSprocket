using System.Collections.Generic;
using System.IO;

namespace Jabbot.TwitterNotifierSprocket
{
    public static class BotExtensions
    {
        public static void PrivateReplyFile(this Bot input, string who, string FileName)
        {
            using (var reader = new StreamReader(FileName))
            {
                while (!reader.EndOfStream)
                {
                    input.PrivateReply(who, reader.ReadLine());
                }
            }
        }

        public static void PrivateReply(this Bot input, string who, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                input.PrivateReply(who, line);
            }
        }
    }
}
