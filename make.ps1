param (
	[Parameter(Position=0,Mandatory=1)] [string] $task
)

function GetCurrentPath {
    Split-Path $script:MyInvocation.MyCommand.Path
}

function GetTempPath {
    (GetCurrentPath) + "\temp"
}

function GetOutputPath {
    (GetCurrentPath) + "\output"
}

function GetBuildPath {
    (GetCurrentPath) + "\build"
}

function CreateTempIfNeeded {
    if ((Test-Path (GetTempPath)) -eq $false) {
        New-Item -Path (GetTempPath) -ItemType Directory | Write-Debug
    }
}

function CreateOutputIfNeeded {
    if ((Test-Path (GetOutputPath)) -eq $false) {
        New-Item -Path (GetOutputPath) -ItemType Directory | Write-Debug
    }
}

function ImportMsBuildModuleIfNeeded {
    Import-Module ((GetBuildPath) + "\Invoke-MsBuild.psm1")
}

function BuildBinaries {
    ImportMsBuildModuleIfNeeded
    CreateOutputIfNeeded

    Invoke-MsBuild -Path ".\src\Solution.2013.Express.sln" `
                   -BuildLogDirectoryPath ((GetTempPath) + "\") `
                   -Params "/target:Clean;Build /property:Configuration=Release;BuildInParallel=true;DebugSymbols=false;OutputPath=$((GetOutputPath) + "\");IntermediateOutputPath=$((GetTempPath) + "\") /verbosity:minimal /maxcpucount"
}

if ($task -eq "build") {
    CreateTempIfNeeded
    BuildBinaries
}
