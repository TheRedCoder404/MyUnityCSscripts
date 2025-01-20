using Newtonsoft.Json;

class Programm
{
    static void Main(string[] args)
    {
        bool running = true;
        Data data = new Data();

        data.Load();

        // Title of the Program
        Console.WriteLine("-----------------------------");
        Console.WriteLine("|-----C# Script Grabber-----|");
        Console.WriteLine("-----------------------------");


        while (running) // Main loop
        {
            Console.WriteLine("\nWhat do you want to do?");

            switch (Console.ReadLine()) // Checking what was input
            {
                case "newGamesDir": // To change the directory which is check for scripts
                    string newDir = ScriptGrabber.SetNewGamesDirectory(1);
                    if (newDir != "")
                    {
                        data.curGamesDir = newDir;
                        Console.WriteLine("Games directory successfully changed");
                    }
                    else
                    {
                        Console.WriteLine("Games directory was not changed");
                    }
                    break;

                case "newTargetDir": // To change the directory in which the scripts should be copied to
                    string newDir2 = ScriptGrabber.SetNewGamesDirectory(2);
                    if (newDir2 != "")
                    {
                        data.curTargetDir = newDir2;
                        Console.WriteLine("Target directory successfully changed");
                    }
                    else
                    {
                        Console.WriteLine("Target directory was not changed");
                    }
                    break;

                case "updateScripts": // To update the scripts found the Games directory
                    ScriptGrabber.UpdateScripts(data);
                    break;

                case "listDirs": // To list all known Game directories
                    if (data.curGameDirs.Count > 0)
                    {
                        for (int i = 0; i < data.curGameDirs.Count; i++)
                        {
                            Console.WriteLine(data.curGameDirs[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nThere are currently no directories to display");
                    }
                    break;

                case "listNames": // To list all known Game Names
                    if (data.curGameNames.Count > 0)
                    {
                        Console.WriteLine();
                        for (int i = 0; i < data.curGameNames.Count; i++)
                        {
                            Console.WriteLine(data.curGameNames[i]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nThere are currently no Games to display");
                    }
                    break;

                case "list":
                    Console.WriteLine();
                    foreach (ScriptsOfDir scripts in data.scriptLocations)
                    {
                        Console.WriteLine();
                        Console.WriteLine(scripts.name);
                        foreach (string script in scripts.scripts)
                        {
                            string[] scriptFolders = script.Split("\\");
                            Console.WriteLine("- " + scriptFolders[scriptFolders.Length - 1]);
                        }
                    }
                    break;

                case "help":
                    Console.WriteLine("Commands currently available:");
                    Console.WriteLine("newGamesDir, newTargetDir, updateScripts, listDirs, listNames, help, exit");
                    break;

                case "save":
                    data.Save();
                    break;

                case "load":
                    data.Load();
                    break;

                case "clear":
                    data.Clear();
                    break;

                case "exit": // To end the Main loop and stop the Program
                    running = false;
                    break;

                default: // To have something displayed if the input is wrong
                    Console.WriteLine("Invalid Input");
                    break;
            }
        }
    }
}

class ScriptGrabber
{
    public static string SetNewGamesDirectory(int mode) // To change the directory of either the Games or the Target
    {
        bool active = true;
        string? newDir = "";
        string dirName = "";

        if (mode == 1) // Either display Games or Target depending on the mode
        {
            dirName = "Games";
        }
        else if (mode == 2)
        {
            dirName = "Target";
        }

        Console.WriteLine($"\nWhich directory is the new {dirName} directory?");

        while (active) // Loop to change the directory until the user is satisfied
        {
            string? possibNewDir = Console.ReadLine();

            if (Directory.Exists(possibNewDir))
            {
                Console.WriteLine($"\"{possibNewDir}\" should be the new {dirName} directory, right? (To confirm type \"yes\" or \"y\")");
                string? input = Console.ReadLine();

                if (input == "yes" || input == "y") // Check if the user satisfied with the chosen directory
                {
                    active = false;
                    newDir = possibNewDir;
                }
                else
                {
                    Console.WriteLine("Which directory is the new directory instead? (type \"exit\" if you changed your mind)");
                }
            }
            else if (possibNewDir == "exit") // Escape if the user got here by mistake
            {
                newDir = "";
            }
            else
            {
                Console.WriteLine("Incorrect directory. The given directory doesn't exist. Make sure there are no spelling mistakes.");
                Console.WriteLine("Which directory is the new directory instead? (type \"exit\" if you changed your mind)");
            }
        }
        return newDir;
    }

    public static void UpdateScripts(Data data) // To update the changed scripts
    {
        UpdateThisScript(data);
        UpdateDirectories(data);
        GetScripts(data);
        CopyScripts(data);
        data.Save();
        Console.WriteLine();
    }

    private static void UpdateThisScript(Data data)
    {
        string newPath = data.curTargetDir + $"\\{data.dirFromThisScript}";
        string[] folders = data.scriptFromThis.Split("\\");

        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }

        File.Copy(data.scriptFromThis, newPath + "\\" + folders[folders.Length - 1], true);
    }

    private static void CopyScripts(Data data)
    {
        Console.WriteLine();
        foreach (ScriptsOfDir scriptsOfThisDir in data.scriptLocations)
        {
            foreach (string scriptPath in scriptsOfThisDir.scripts)
            {
                string path = data.curTargetDir + "\\" + scriptsOfThisDir.name;
                string[] folder = scriptPath.Split('\\');
                string scriptName = folder[folder.Length - 1];
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string newPath = path + "\\" + scriptName;

                if (scriptsOfThisDir.compData.addedDirs.Contains(scriptPath) || scriptsOfThisDir.compData.unchangedDirs.Contains(scriptPath))
                {
                    File.Copy(scriptPath, newPath, true);
                }
                else if (scriptsOfThisDir.compData.removedDirs.Contains(scriptPath))
                {
                    File.Delete(scriptPath);
                }
            }

            Console.WriteLine($"Scripts from {scriptsOfThisDir.name} successfully updated");
        }

        Console.WriteLine("\nAll scripts successfully updated");
    }

    private static void GetScripts(Data data)
    {
        foreach (string dir in data.curGameDirs)
        {
            List<string> files = new List<string>();
            List<string> scriptsOfThisDir = new List<string>();
            List<string> tempScriptpath = new List<string>();
            CompareData compData;
            ScriptsOfDir curScript;
            string scriptName = "";

            string[] subDirs = dir.Split("\\");
            scriptName = subDirs[subDirs.Length - 1];
            string scriptPath = Path.Combine(dir, scriptName);
            scriptPath = scriptPath + "\\Assets\\Scripts";
            files = Directory.GetFiles(scriptPath, "*.cs").ToList<string>();

            curScript = new ScriptsOfDir();
            curScript.name = scriptName;

            foreach (ScriptsOfDir scriptsInDir in data.scriptLocations)
            { 
                if (scriptsInDir.name == scriptName) 
                { 
                    curScript = scriptsInDir;
                    scriptsOfThisDir = scriptsInDir.scripts; 
                } 
            }

            if (!data.scriptLocations.Contains(curScript))
            {
                data.scriptLocations.Add(curScript);
            }
            
            compData = CompareDirLists(files, scriptsOfThisDir);
            curScript.compData = compData;
            Console.WriteLine();

            CheckResult(compData, 1, data, scriptName, curScript, subDirs);
        }
    }

    public static void UpdateGameNames(Data data) // To update the list of the Game names
    {
        List<string> names = new List<string>();
        string[] sepPath;

        for (int i = 0; i < data.curGameDirs.Count; i++)
        {
            sepPath = data.curGameDirs[i].Split('\\'); // Splitting the string of the path and only giving the last "word" of the array because that is the folder name of the project
            names.Add(sepPath[sepPath.Length - 1]);
        }

        data.curGameNames = names;
    }

    private static void UpdateDirectories(Data data) // Checks for the directories and changes them accoringly
    {
        bool active = true;
        CompareData compData;
        List<string> tempDirList = new List<string>();

        while (active) // Looping to make sure that the Target directory and the Games directory are present before checking the directories in the Games directory
        {
            if (data.curTargetDir != null && data.curTargetDir != "" && data.curGamesDir != null && data.curGamesDir != "")
            {
                tempDirList = Directory.GetDirectories(data.curGamesDir).ToList<string>();
                active = false;
            }
            else if (data.curGamesDir == "") // Setting a Games directory of none is set
            {
                Console.WriteLine("\nCurrently there is no Games directory set, therefore you first need to set on.");
                string newDir2 = ScriptGrabber.SetNewGamesDirectory(1);
                if (newDir2 != "")
                {
                    data.curGamesDir = newDir2;
                    Console.WriteLine("Games directory successfully changed");
                }
                else
                {
                    Console.WriteLine("Games directory was not changed");
                }
            }
            else if (data.curTargetDir == "") // Setting a Target directory of none is set
            {
                Console.WriteLine("\nCurrently there is no Target directory set, therefore you first need to set on.");
                string newDir2 = ScriptGrabber.SetNewGamesDirectory(2);
                if (newDir2 != "")
                {
                    data.curTargetDir = newDir2;
                    Console.WriteLine("Target directory successfully changed");
                }
                else
                {
                    Console.WriteLine("Target directory was not changed");
                }
            }
        }

        compData = CompareDirLists(tempDirList, data.curGameDirs); // Comparing the new and the old list of directories to find differences and to find the right next move

        Console.WriteLine();
        CheckResult(compData, 0, data);

        Console.WriteLine();
        UpdateGameNames(data); // Update the Game names because the list probably got changed
    }

    private static void CheckResult(CompareData compData, int mode, Data data, string scriptName = "", ScriptsOfDir curScript = null, string[] subDirs = null)
    {
        bool active;
        string case0 = "";
        string case1 = "";
        switch (mode)
        {
            case 0: // Directories
                case0 = "directories";
                case1 = "directories";
                break;

            case 1: // Scrpts
                case0 = "scripts";
                case1 = $"scripts in {scriptName}";
                break;
        }

        switch (compData.result)
        {
            case 0: // If there is no change
                Console.WriteLine($"There are no changes to the number of {case1}");
                break;

            case 1: // if there were scripts/directories added and none got removed
                Console.WriteLine($"There are {compData.addedDirs.Count} new {case1}. What do you want to do? (\"add\", \"list\", \"dismiss\")");
                active = true;
                while (active)
                {
                    switch (Console.ReadLine()) // Checking fot the right input
                    {
                        case "add": // Add the new scripts/directories to the real list
                            foreach (string dir in compData.addedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Add(dir); } else{ curScript.scripts.Add(dir); }
                            }
                            active = false;
                            break;

                        case "dismiss": // Make no changes to the real list
                            active = false;
                            break;

                        case "list": // List all new scripts/directories
                            Console.WriteLine();
                            Console.WriteLine($"All new {case1}:");
                            foreach (string dir in compData.addedDirs)
                            {
                                string[] dirs = dir.Split("\\");
                                Console.WriteLine(dirs[dirs.Length - 1]);
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid input. Please type ether \"add\", \"list\" or \"dismiss\"");
                            break;
                    }
                }
                break;

            case 2: // If there were scripts/directories removed and none added
                Console.WriteLine($"There were {compData.removedDirs.Count} {case0} removed. What do you want to do? (\"remove\", \"list\", \"dismiss\")");
                active = true;
                while (active)
                {
                    switch (Console.ReadLine()) // Checking for the right input
                    {
                        case "remove": // Remove the now lost scripts/directories from the real list
                            foreach (string dir in compData.removedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Remove(dir); } else { curScript.scripts.Remove(dir); }
                            }
                            active = false;
                            break;

                        case "dismiss": // Make no changes to the real list
                            active = false;
                            break;

                        case "list": // List all removed scripts/directories
                            Console.WriteLine();
                            Console.WriteLine($"All removed {case1}:");
                            foreach (string dir in compData.removedDirs)
                            {
                                string[] dirs = dir.Split("\\");
                                Console.WriteLine(dirs[dirs.Length - 1]);
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid input. Please type ether \"remove\", \"list\" or \"dismiss\"");
                            break;
                    }
                }
                break;

            case 3: // if there were scripts/directories added and removed
                Console.WriteLine($"There were {compData.addedDirs.Count} {case0} added and {compData.removedDirs.Count} removed. What do you want to do? (\"add\", \"remove\", \"sync\", \"list\", \"dismiss\")");
                active = true;
                while (active)
                {
                    switch (Console.ReadLine()) // Checking for the right input
                    {
                        case "add": // Add the new scripts/directories to the real list
                            foreach (string dir in compData.addedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Add(dir); } else { curScript.scripts.Add(dir); }
                            }
                            active = false;
                            break;

                        case "remove": // Remove the now lost scripts/directories from the real list
                            foreach (string dir in compData.removedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Remove(dir); } else { curScript.scripts.Remove(dir); }
                            }
                            active = false;
                            break;

                        case "sync": // Add the new scripts/directories as well as remove the old
                            foreach (string dir in compData.addedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Add(dir); } else { curScript.scripts.Add(dir); }
                            }

                            foreach (string dir in compData.removedDirs)
                            {
                                if (mode == 0) { data.curGameDirs.Remove(dir); } else { curScript.scripts.Remove(dir); }
                            }
                            active = false;
                            break;

                        case "list": // List all new and old scripts/directories
                            Console.WriteLine();
                            Console.WriteLine($"All new (+) and removed (-) {case0}:");
                            foreach (string dir in compData.addedDirs)
                            {
                                string[] dirs = dir.Split("\\");
                                Console.WriteLine("+ " + dirs[dirs.Length - 1]);
                            }

                            foreach (string dir in compData.removedDirs)
                            {
                                string[] dirs = dir.Split('\\');
                                Console.WriteLine("- " + dirs[dirs.Length - 1]);
                            }
                            break;

                        case "dismiss": // Make no changes to the real list
                            active = false;
                            break;

                        default:
                            Console.WriteLine("Invalid input. Please type ether \"add\", \"remove\", \"sync\", \"list\" or \"dismiss\"");
                            break;
                    }
                }
                break;
        }
    }

    public static CompareData CompareDirLists(List<string> tempDirList, List<string> curDirList) // A function to compare the new to the old list
    {
        CompareData data = new CompareData(); // Making an object out of a specific class to save the changes

        for (int i = 0; i < tempDirList.Count; i++) // Checking for new directories as well as unchanged ones
        {
            if (curDirList.Contains(tempDirList[i])) { data.unchangedDirs.Add(tempDirList[i]); } else { data.addedDirs.Add(tempDirList[i]); }
        }

        for (int i = 0; i < curDirList.Count; i++) // Checking for deleted directories as well as unchanged ones
        {
            if (tempDirList.Contains(curDirList[i])) { if (!data.unchangedDirs.Contains(curDirList[i])) { data.unchangedDirs.Add(curDirList[i]); } } else { data.removedDirs.Add(curDirList[i]); }
        }

        if (data.addedDirs.Count == 0 && data.removedDirs.Count == 0) // No changes to the number of directories
        {
            data.result = 0;
        }
        else if (data.addedDirs.Count > 0 && data.removedDirs.Count == 0) // New directories have been added, none have been removed
        {
            data.result = 1;
        }
        else if (data.addedDirs.Count == 0 && data.removedDirs.Count > 0) // Old directories have been removed, none habe been added
        {
            data.result = 2;
        }
        else if (data.addedDirs.Count > 0 && data.removedDirs.Count > 0) // New directories have been adden and old have been removed
        {
            data.result = 3;
        }

        return data;
    }
}

class Data // Class to store everything important
{
    public string? curGamesDir = ""; // Where the Games that need to be checked are
    public string? curTargetDir = ""; // Where the Scripts should be
    public string dirFromThisScript = ""; // The directory name where the code of this Program is should be stored (assigned manually)
    public string scriptFromThis = ""; // The location of the code (assigned manually)
    public List<string> curGameDirs = new List<string>(); // All Game directories currently known
    public List<string> curGameNames = new List<string>(); // All Game names currently known
    public List<ScriptsOfDir> scriptLocations = new List<ScriptsOfDir>(); // All Scripts currently known

    private readonly string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ScriptGrabber\\ScriptGrabber.json");

    public void Save() // To save the current data
    {
        if (!File.Exists(savePath))
        {
            File.Create(savePath);
        }

        List<Data> data = new List<Data>();
        data.Add(new Data()
        {
            curGamesDir = curGamesDir,
            curTargetDir = curTargetDir,
            dirFromThisScript = dirFromThisScript,
            scriptFromThis = scriptFromThis,
            curGameDirs = curGameDirs,
            curGameNames = curGameNames,
            scriptLocations = scriptLocations
        });

        string json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    public void Load() // To load the last data
    {
        if (!File.Exists(savePath))
        {
            Console.WriteLine("There is no save File to load from\n");
        }
        else
        {
            using StreamReader reader = new(savePath);
            var json = reader.ReadToEnd();
            List<Data> data = JsonConvert.DeserializeObject<List<Data>>(json);

            curGamesDir = data[0].curGamesDir;
            curTargetDir = data[0].curTargetDir;
            dirFromThisScript = data[0].dirFromThisScript;
            scriptFromThis = data[0].scriptFromThis;
            curGameDirs = data[0].curGameDirs;
            curGameNames = data[0].curGameNames;
            scriptLocations = data[0].scriptLocations;
        }
    }

    public void Clear()
    {
        curGamesDir = "";
        curTargetDir = "";
        curGameDirs = new List<string>();
        curGameNames = new List<string>();
        scriptLocations = new List<ScriptsOfDir>();
    }
}

class CompareData // A class to save the data from the CompareDirLists function
{
    public int result = 0; // Which case is true
    public List<string> addedDirs = new List<string>(); // Number of added directories
    public List<string> removedDirs = new List<string>(); // Number of removed directories
    public List<string> unchangedDirs = new List<string>(); // Number of unchanged directories
}

class ScriptsOfDir
{
    public string name = "";
    public List<string> scripts = new List<string>();
    public CompareData compData = new CompareData();
}
