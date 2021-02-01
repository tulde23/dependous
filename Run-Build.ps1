<#
.Synopsis
	Builds a .NET Core project and published it to our internal NuGet gallery

.Description
	Builds a .NET Core project and published it to our internal NuGet gallery

.Parameter $projectName
	The project name
.Parameter $testProjectName
    The test project name
.Parameter $incrementalBuildNumber
	The build number coming from CI
.Parameter $consul
    If 1, uses consul for build config
.Parameter $runTests
    If 1, executes unit tests
.Parameter $test
	Tests the configuration
.Example
	Run-Build -projectName "web-core" -incrementalBuildNumber 10
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(

    [Parameter(HelpMessage = "The project name. ")]
    [string] $projectName,
    [string] $testProjectName = $projectName,
    [Parameter(HelpMessage = "The current build number from CI")]
    [int] $incrementalBuildNumber,
    [bool] $consul = $true,
    [bool] $runTests = $true,
    [Parameter(HelpMessage = "Tests the build config without making any changes")]
    [bool] $test = $false

)

function logger() {
    param
    (
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string]$msg,
        [string]$BackgroundColor = "DarkBlue",
        [string]$ForegroundColor = "Magenta"
    )

    Write-Host -ForegroundColor $ForegroundColor $msg;
}


function SetBuildNumber() {
    param
    (
        [Parameter(Position = 0, ValueFromPipeline = $true)]
        [string]$projectName,
        [int]$buildNumber = 0,
        [bool]$test = $false
    )

    $path = "./src/$projectName/$projectName.csproj"
    $packageJson = [xml](Get-Content $path)
    $versionPrefix = $packageJson.Project.PropertyGroup.VersionPrefix;
    $tokens = $versionPrefix.Split(".");
    $versionNumber = $tokens[2]
    $sm = $versionNumber.Split('-');
    if ( $sm.Length -gt 1) {
        $versionNumber = ($sm[0] -as [int] )
    }
    else {
        $versionNumber = ($versionNumber -as [int])
    }
    if ( $test -eq $false) {
        Write-Host "BuildNumber:$buildNumber, VersionNumber: $versionNumber"
    }
    if ( $buildNumber -le $versionNumber) {
        $tokens[2] = $versionNumber + 1
    }
    else {
        $tokens[2] = $buildNumber
    }
    $packageJson.Project.PropertyGroup.VersionPrefix = [string]::Join(".", $tokens);
    logger "Set Build Number To: $([string]::Join(".", $tokens))"
    if ( $test -eq $false) {
        $packageJson.Save(( gi $path ).FullName)
    }
}
function RunTests() {
    if ( $runTests) {
        $testPath = "./tests/$testProjectName"
        push-location $testPath
        logger "Executing UnitTests $testPath"
        dotnet restore
        $testResult = dotnet test --no-build | Out-String
        Write-Host "$testResult"
        pop-location
        if ( $testResult -like '*Failed: 0*') {
            logger "Tests Passed" -ForegroundColor "Green"
            return $true;
        }
        else {
            logger "Tests Failed" -ForegroundColor "Red"
            return $false;
        }
    }
    else {
        return $true
    }
}

$buildInfo = getBuildConfiguration
$endpoint = $buildInfo.nuget
$apiKey = $buildInfo.apiKey
$sourceNugetExe = $buildInfo.nugetExe
$targetNugetExe = ".\nuget.exe"
$fileExists = Test-Path $targetNugetExe
$projectName = "SportsConnect.Dependous"
$projects = ( "SportsConnect.Dependous", "SportsConnect.Dependous.Autofac")

logger "Starting Build With Following Options:"
logger "NuGet Api: $endpoint`nApiKey: $apiKey`nNugetExe:$sourceNugetExe`n"

if ( $fileExists -eq $False) {
    Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
    Set-Alias nuget $targetNugetExe -Scope Global
}
else {
    Set-Alias nuget $targetNugetExe -Scope Global
}

foreach( $project in $projects){
$fileExists = Test-Path "./src/$project/bin/Release"
if ( $fileExists -eq $True) {
    push-location "./src/$project/bin/Release"
    remove-item *.nupkg
    pop-location
}
}
#SetBuildNumber -projectName $projectName -buildNumber $incrementalBuildNumber -test $test

dotnet restore
dotnet build --configuration Release
foreach( $project in $projects){
push-location "./src/$project"
dotnet pack --configuration Release --no-build

push-location './bin/Release'
$packageName = Get-Item -path ".\*" -filter "*.nupkg" | Select-Object FullName -first 1
$packageName = $packageName.FullName
Pop-location
Pop-location
#now run the UTs.  Don't publish anything if they fail
#$testResult = RunTests

    write-host 'Publishing ' $packageName
   nuget push $packageName $apiKey  -source $endpoint
}