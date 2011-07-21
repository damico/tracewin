using System.Collections.Generic;
using System.IO;
using System;
using System.Net;

public static class FileHelper
{
    public static string GenResultFileName() {

        string resultFileName = null;

        
        string strHostName = Dns.GetHostName();
        Console.WriteLine("Local Machine's Host Name: " + strHostName);

        IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        IPAddress[] addr = ipEntry.AddressList;

        for (int i = 0; i < addr.Length; i++)
        {
            string e = addr[i].ToString();
            if(!e.Contains("127.0.0.1") && !e.Contains("0.0.0.0")) resultFileName = e;
            break;
        }

        resultFileName = resultFileName.Replace(":","");
        resultFileName = resultFileName.Replace("%", "");

        resultFileName = "tracewin_"+ resultFileName+"_" + System.Environment.MachineName;

       

        return resultFileName;
    
    }

    public static List<string> GetLogged(string resultPath)
    {
        List<string> result = null;

        

        try
        {
            //WriteLog("===> " + resultPath + "\\" + GenResultFileName());
            string[] lines = System.IO.File.ReadAllLines(@resultPath + "\\" + GenResultFileName()+".csv");
            result = new List<string>();
            foreach (string line in lines)
            {
                result.Add(line);
            }
        }
        catch (Exception e)
        {
            WriteLog("ERROR: " + e.Message);
        }
        return result;
    }


    public static void WriteLine2File(string line, string path)
    {
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path + "\\" + GenResultFileName()+".csv", true))
        {
            //WriteLog("---> "+ path + "\\" + GenResultFileName());
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

                result.AddRange(Directory.GetFiles(dir, "*.exe"));

                foreach (string dn in Directory.GetDirectories(dir))
                {
                    stack.Push(dn);
                }
            }
            catch(Exception e)
            {
                WriteLog("ERROR: "+e.Message);
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

    public static string ExtractFileName(string path) 
    {
        string e = path.ToLower();
        char[] ce = e.ToCharArray();

        for (int i = ce.Length - 1; i >= 0; i--)
        {
            if (ce[i] == '\\')
            {
                e = e.Substring(i + 1, (e.Length - i)-1);
                break;
            }
        }
        return e;
    }
}