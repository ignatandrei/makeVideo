const args = process.argv.slice(2);
const modelArgIndex = args.indexOf("--model");
const modelName = modelArgIndex !== -1 ? args[modelArgIndex + 1] : "ollama:gemma2:27b";

console.log("Model name: " + modelName);

const res =
            await prompt
                `Analyze the following blog post BLOGPOST about what I have used and make it better . Put it HTML markup. NO Markdown.

                BLOGPOST:
            
                Operating systems
            
            Windows 11
            
            MacOS  - good for managers, for .NET programmers a little bit difficult
            
            Browsers
            
            Chrome for extensions
            
            Edge for faster browsing
            
            IDE
            
            Visual Studio for C# 
            
            VSCode for all others
            
            
            
            Social Media
            
            Facebook account
            
            Linkedin account
            
            
            
            Hosting and Source Control
            
            GitHub and Azure 
            
            Reading
            
            O’Reilly 
            
            Movies
            
            Disney and Netflix
            
            
            
            
            
                
                `
            .options({
    model: modelName,//"large",
    label: "summarize chunk commit messages",
    system: [
        "system.assistant",
        "system.safety_jailbreak",
        "system.safety_harmful_content",
        "system.safety_validate_harmful_content",
    ],
})
if (res.error) throw res.error
console.log( res.text)