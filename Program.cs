using System.Text;
using Figgle;
using Octokit;
using static System.Console;

const string product = "GAUDI";
// Banner
Console.ForegroundColor = ConsoleColor.Blue;
Write(FiggleFonts.Slant.Render($"----- {product} -----"));
Console.ForegroundColor = ConsoleColor.Red;
WriteLine(FiggleFonts.Slant.Render(" GitHub Audit Tool"));
Console.ForegroundColor = ConsoleColor.Blue;
WriteLine(FiggleFonts.Slant.Render("----------------"));
Console.ResetColor();
//GitHub Authentication
WriteLine("Insert your GitHub Username:");
var username = ReadLine();
WriteLine("Insert your github token:");
Console.ForegroundColor = ConsoleColor.Black;
var token = ReadLine();
Console.ResetColor();
var client = new GitHubClient(new ProductHeaderValue(product));
var tokenAuth = new Credentials(token);
client.Credentials = tokenAuth;
WriteLine($"\n========================================= AUDIT USER {username} =========================================>\n");
try{
    var user = await client.User.Get(username);
    var apiInfo = client.GetLastApiInfo();
    var rateLimit = apiInfo?.RateLimit;
    var howManyRequestsPerHour = rateLimit?.Limit;
    var howManyRequestsLeft = rateLimit?.Remaining;
    var whenDoesTheLimitReset = rateLimit?.Reset.LocalDateTime;
    //USER'S INFO
    StringBuilder userData = new StringBuilder($"\nPROFILE: {user.HtmlUrl}\n");
    userData.Append($"CREATED AT (local date-time): {user.CreatedAt.LocalDateTime}\n");
    userData.Append($"SUBSCRIPTION PLAN: {user.Plan.Name}\n");
    userData.Append($"AVATAR: {user.AvatarUrl}\n");
    userData.Append($"FOLLOWERS: {user.Followers}\n");
    userData.Append($"PUBLIC REPOS: {user.PublicRepos}\n");
    userData.Append($"PRIVATE REPOS: {user.OwnedPrivateRepos}\n");
    userData.Append($"PUBLIC GISTS: {user.PublicGists}\n");
    userData.Append($"PRIVATE GISTS: {user.PrivateGists}");
    StringBuilder infoApi = new StringBuilder($"API REQUESTS HOURLY LIMIT: {howManyRequestsPerHour}\n");
    infoApi.Append($"API REQUESTS LEFT: {howManyRequestsLeft}\n");
    infoApi.Append($"NEXT API LIMIT RESET (local date-time): {whenDoesTheLimitReset}\n");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(userData.ToString());
    WriteLine(infoApi.ToString());
    Console.ResetColor();
    //REPO AUDIT
    WriteLine("################################ CHECK USER'S REPO ################################\n");
    var repositories = await client.Repository.GetAllForCurrent();
    bool repoWarning = false;
    foreach (var repo in repositories){
        var commit = await client.Repository.Commit.Get(username, repo.Name, "HEAD");
        var isCommitVerified = commit.Commit.Verification.Verified;
        if (isCommitVerified){
            repoWarning = true;
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine($"- Warning: [{repo.HtmlUrl}] has unverified commit: [{commit.Sha}]");
            Console.ResetColor();
        }
    }
    if(repoWarning){
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine($"\n Remediation: add GPG to your GitHub Account and sign all commits with it! 👍\n");
        Console.ResetColor();
    }
    WriteLine("###################################################################################\n");
    //ISSUES AUDIT
    WriteLine("############################### CHECK USER'S ISSUES ###############################\n");
    var issues = await client.Issue.GetAllForCurrent();
    int issuesNumber = issues.Count();
    if(issuesNumber>0){
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine($"- You have {issuesNumber} issues, work on them!\n");
        Console.ResetColor();
    } else {
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine($"- You have 0 issue!, keep up the good work! 💪\n");
        Console.ResetColor();
    }
    WriteLine("###################################################################################\n");
}
catch (Exception ex){
    Console.ForegroundColor = ConsoleColor.Red;
    WriteLine($"ERROR IN EXECUTION: \n{ex}\n");
    Console.ResetColor();
}