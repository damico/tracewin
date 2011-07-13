using System.Collections.Generic;
using System.IO;
using System;

public static class FileHelper
{
    public static List<string> GetLogged(string resultPath)
    {
        List<string> result = null;

        try
        {
            string[] lines = System.IO.File.ReadAllLines(@resultPath);
            result = new List<string>();
            foreach (string line in lines)
            {
                result.Add(line);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }


    public static void WriteLine2File(string line, string path)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, true))
        {
            file.WriteLine(line);
        } 
    }

    public static List<string> GetFilesRecursive(string b)
    {
        
        List<string> result = new List<string>();

        Stack<string> stack = new Stack<string>();

        stack.Push(b);

        while (stack.Count > 0)
        {

            string dir = stack.Pop();

            try
            {

                result.AddRange(Directory.GetFiles(dir, "*.*"));

                foreach (string dn in Directory.GetDirectories(dir))
                {
                    stack.Push(dn);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        return result;
    }

    public static void WriteLog(String logLine) 
    {
        StreamWriter vWriter = new StreamWriter(@"c:\windows\temp\tracewin.log", true);
        vWriter.WriteLine(DateTime.Now.ToString()+": "+logLine);
        vWriter.Flush();
        vWriter.Close();
    }
}