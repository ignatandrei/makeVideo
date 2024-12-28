script({
    title: "git commit message",
    description: "Generate a commit message for all staged changes",
    //xmodel: "openai:gpt-4o",
    model:"ollama:phi3.5",
    system: ["system"],
})

//https://microsoft.github.io/genaiscript/guides/auto-git-commit-message/
//https://github.com/microsoft/genaiscript/blob/main/packages/sample/genaisrc/samples/gcm.genai.mts


//let { stdout } = await host.exec("git", ["diff", "--cached"])
// Check for staged changes and stage all changes if none are staged
const diff = await git.diff({
    staged: true,
    askStageOnEmpty: true,
    llmify:false
})

if (!diff) cancel("no staged changes")
console.log(diff)

// chunk if case of massive diff
const chunks = await tokenizers.chunk(diff, { chunkSize: 10000 })
if (chunks.length > 1)
    console.log(`staged changes chunked into ${chunks.length} parts`)
//- <date> should be the today date in the format YYYY-MM-DD
const dateAll = new Date().toISOString().split("T")
const date = dateAll[0]
const time = dateAll[1]
const commonMessage =`        
        
        <type>(<file>) :  <description>

        - ignore ALL files that are ending in mjs
        - ignore ALL files that contains genai
        - <file> should be the file path relative to the repository root
        - <type> can be one of the following: feat, fix, docs, style, refactor, perf, test, build, ci, chore, revert
        - <description> is a short, imperative present-tense description of the change        
        - do NOT use markdown syntax
        - do NOT add quotes or code blocks
        - do NOT use gitmoji        
        - keep it short, 1 line only, maximum 50 characters
        - follow the conventional commit spec at https://www.conventionalcommits.org/en/v1.0.0/#specification
        - do NOT confuse delete lines starting with '-' and add lines starting with '+'
        - do NOT respond anything else than the commit message
        - keep it short, 1 line only, maximum 50 characters
        
 `
let choice
let message
do {
    // Generate a conventional commit message based on the staged changes diff
    message = ""
    for (const chunk of chunks) {
        const res = await runPrompt(
            (_) => {
                _.def("GIT_DIFF", chunk, {
                    maxTokens: 10000,
                    language: "diff",
                    detectPromptInjection: "available",
                })
                _.$`Generate a git conventional commit message that summarizes the changes in GIT_DIFF.
                    ${commonMessage}
        `
            },
            {
                model: "large", // Specifies the LLM model to use for message generation
                label: "generate commit message", // Label for the prompt task
                system: [
                    "system.assistant",
                    "system.safety_jailbreak",
                    "system.safety_harmful_content",
                    "system.safety_validate_harmful_content",
                ],
            }
        )
        if (res.error) throw res.error
        message += res.text + "\n"
    }

    // since we've concatenated the chunks, let's compress it back into a single sentence again
    if (chunks.length > 1) {
        const res =
            await prompt`Generate a git conventional commit message that summarizes the COMMIT_MESSAGES.
                ${commonMessage}

        COMMIT_MESSAGES:
        ${message}
        `.options({
                model: "large",
                label: "summarize chunk commit messages",
                system: [
                    "system.assistant",
                    "system.safety_jailbreak",
                    "system.safety_harmful_content",
                    "system.safety_validate_harmful_content",
                ],
            })
        if (res.error) throw res.error
        message = res.text
    }

    message = message?.trim()
    if (!message) {
        console.log(
            "No commit message generated, did you configure the LLM model?"
        )
        break
    }

    // Prompt user to accept, edit, or regenerate the commit message
    choice = await host.select(message, [
        {
            value: "commit",
            description: "accept message and commit",
        },
        {
            value: "edit",
            description: "edit message and commit",
        },
        {
            value: "regenerate",
            description: "regenerate message",
        },
        {
            "value": "cancel",
            "description": "exit without committing"
        }
    ])
    if(choice === "cancel") {
        cancel("User cancelled the commit");
    }
    // Handle user's choice for commit message
    if (choice === "edit") {
        message = await host.input("Edit commit message", {
            required: true,
        })
        choice = "commit"
    }
    // If user chooses to commit, execute the git commit and optionally push changes
    if (choice === "commit" && message) {
        console.log("Committing changes with the following message:");
        console.log(message);
        
        console.log(await git.exec(["commit","-m",`generated on ${date} : ${time} `, "-m", message]))
        // if (await host.confirm("Push changes?", { default: true }))
        //     console.log(await git.exec("push"))
        // break
    }
} while (choice !== "commit")