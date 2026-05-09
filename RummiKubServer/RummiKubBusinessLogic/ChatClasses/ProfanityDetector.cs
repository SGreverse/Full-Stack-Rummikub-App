using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RummiKubClasses.ChatObjects
{
    public static class ProfanityDetector
    {
        private static readonly string ApiUrl = "https://www.purgomalum.com/service/containsprofanity?text=";

        public static async Task<string> TranslateToFamilyFriendslyText(string original_string)
        {
            string[] words = original_string.Split(' ');
            var tasks = new List<Task<(string Word, bool ContainsProfanity)>>();

            foreach (string word in words)
            {
                tasks.Add(CheckProfanityWithWord(word));
            }

            var results = await Task.WhenAll(tasks);

            string translated_string = "";
            foreach (var result in results)
            {
                if (result.ContainsProfanity)
                {
                    translated_string += new string('*', result.Word.Length);
                }
                else
                {
                    translated_string += result.Word;
                }
                translated_string += " ";
            }

            return translated_string.Trim();
        }

        private static async Task<(string Word, bool ContainsProfanity)> CheckProfanityWithWord(string word)
        {
            bool containsProfanity = await CheckProfanity(word).ConfigureAwait(false);
            return (word, containsProfanity);
        } 

        private static async Task<bool> CheckProfanity(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUrl = $"{ApiUrl}{Uri.EscapeDataString(text)}";
                HttpResponseMessage response = await client.GetAsync(requestUrl).ConfigureAwait(false);
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return result.Trim().ToLower() == "true"; 
            }
        }

    }
}
