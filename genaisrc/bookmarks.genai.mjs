import fs from 'fs';
const args = process.argv.slice(2);
const modelArgIndex = args.indexOf("--model");
const modelName = modelArgIndex !== -1 ? args[modelArgIndex + 1] : "ollama:gemma2:27b";

var bookmarks = fs.readFileSync("C:\\Users\\ignat\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Bookmarks")
var bookmarksJson = JSON.parse(bookmarks);
var obj=bookmarksJson["roots"]["bookmark_bar"]["children"]
//obj=bookmarksJson["roots"]
// for (const key in obj) {
//     if (Object.prototype.hasOwnProperty.call(obj, key)) {
//         const element = obj[key];
//         console.log(key)        
//     }
// }
var data= "";
var nr =0;
// console.log(obj)
obj.forEach(element => {
    if(element["name"].indexOf("2024")== 0)
        if (Object.prototype.hasOwnProperty.call(element, "children")) {
            //  console.log(element["name"])
            element["children"].forEach(bookmark => {
                //console.log(bookmark["name"])
                data+=   bookmark["name"]+"\r\n";
                nr++;
            });
    }
});

console.log(nr);

let message1=[
`Please analyze the following and write max 10 lines of  summarizing the interests of the person that have seen those :
${data}
`]


message1 =[
    `Please analyze the following and write max 10 categories based on the input . For each category, put max 3 examples

    ${data}
    `] 
 const res =
            await prompt(message1).options({
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