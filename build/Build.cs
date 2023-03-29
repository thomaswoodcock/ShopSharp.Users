using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    On = new[] { GitHubActionsTrigger.Push },
    FetchDepth = 0)]
class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [GitVersion] readonly GitVersion GitVersion;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    Target Clean => _ => _
        .Description("Cleans all projects")
        .Executes(() =>
        {
            DotNetClean(_ => _
                .SetProject(Solution)
                .SetConfiguration(Configuration));
        });

    Target Restore => _ => _
        .Description("Restores all project dependencies")
        .After(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .Description("Builds all projects")
        .DependsOn(Clean, Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    public static int Main() => Execute<Build>(x => x.Compile);
}
