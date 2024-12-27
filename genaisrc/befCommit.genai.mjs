script({
    title: "git commit message",
    description: "Generate a commit message for all staged changes",
    //xmodel: "openai:gpt-4o",
    model:"ollama:phi3.5"
})

// Check for staged changes and stage all changes if none are staged
const diff = await git.diff({
    staged: false,
    askStageOnEmpty: true,
    llmify:true
})

if (!diff) cancel("no staged changes")
console.log(diff)

let choice
let message
//do {
    // generate a conventional commit message (https://www.conventionalcommits.org/en/v1.0.0/)
const res = await runPrompt((_) => {
    _.def("GIT_DIFF", diff, { maxTokens: 20000, language: "diff" })
    _.$`Generate a git conventional commit message for the changes in GIT_DIFF.
    - do NOT add quotes
    - detail for each file the modification writing maximum 10 characters
    - do NOT use gitmojis`
})
    // ... handle response and user choices
//} while (choice !== "commit")
