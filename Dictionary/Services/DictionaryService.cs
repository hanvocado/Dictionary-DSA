using System;
using Dictionary.Models;

namespace Dictionary.Services;

public interface IDictionaryService
{
    public Word? LookUp(string? key);
}

public class DictionaryService : IDictionaryService
{
    private IWebHostEnvironment _envi;
    private HashTable Dictionary = new();
    public DictionaryService(IWebHostEnvironment envi) {
        _envi = envi;
        CreateDictionary();
    }
    public Word? LookUp(string? searchStr)
    {
        if (String.IsNullOrEmpty(searchStr))
            return null;
        return Dictionary.GetWord(searchStr);
    }

    private void CreateDictionary() {
        string filePath = Path.Combine(_envi.WebRootPath, "EnglishDictionary.txt");
        Console.WriteLine(filePath);
        using (var reader = new StreamReader(filePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                // Split the line into parts
                string[] parts = line.Split(' ');

                // Check if the line contains a word definition
                if (parts.Length >= 3)
                {
                    // Extract the word, part of speech, and meaning
                    string keyword = parts[0];
                    string partOfSpeech = parts[1];
                    string meaning = string.Join(" ", parts.Skip(2));

                    // Create a Word instance and add it to the list
                    Word word = new Word(keyword, partOfSpeech, meaning);

                    Dictionary.Insert(word);
                }
            }
        }
    }
}